using System.Collections.Generic;
using ConversationGraph.Runtime.Core.Base;
using ConversationGraph.Runtime.Foundation;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConversationGraph.Runtime.Core.Facilitators
{
    public class BasicFacilitator : BaseFacilitator
    {
        public override async void StartConversation
        (string startId,
            TextMeshProUGUI titleText,
            TextMeshProUGUI speakerText,
            TextMeshProUGUI messageText,
            IReadOnlyDictionary<string, ConversationData> dataDic,
            IReadOnlyDictionary<string, string> propertyDic,
            Transform selectParent,
            Button selectPrefab)
        {
            var id = startId;
            var isEnd = false;
            while (true)
            {
                var data = dataDic[id];
                var nextIndex = 0;

                switch (data)
                {
                    case MessageData messageData:
                        BeforeMessage(messageText);
                        await OnMessage(speakerText, messageText, messageData, propertyDic);
                        AfterMessage(messageText);
                        break;
                    case SelectData selectData:
                        id = await OnSelect(selectData, selectParent, selectPrefab);
                        break;
                    case StartData startData:
                        OnStart(startData);
                        break;
                    case EndData endData:
                        OnEnd(endData);
                        isEnd = true;
                        break;
                }
                
                // if now data is End Data, finish conversation.
                if (isEnd)
                {
                    break;   
                }

                if (data.TryGetNextDataIds(out var nextDataIds))
                {
                    id = nextDataIds[nextIndex];
                }
#if UNITY_EDITOR
                else
                {
                    Debug.Log("正しく次のデータが取得できませんでした。");                    
                }
#endif
            }
        }

        public override void StartConversation(in string startId, in TextMeshProUGUI titleText, in TextMeshProUGUI speakerText,
            in TextMeshProUGUI messageText, in IReadOnlyDictionary<string, ConversationData> dataDic, in ConversationPropertyAsset propertyAsset,
            in Transform selectParent, in Button selectPrefab)
        {
            StartConversation(startId, titleText, speakerText, messageText, dataDic, (IReadOnlyDictionary<string, string>)propertyAsset.PropertiesDictionary, selectParent, selectPrefab);
        }

        public override void AfterMessage(in TextMeshProUGUI text)
        {
            
        }

        public override void BeforeMessage(in TextMeshProUGUI text)
        {
            
        }
        
        public override async UniTask OnMessage(TextMeshProUGUI speakerText, TextMeshProUGUI messageText, MessageData data, IReadOnlyDictionary<string, string> propertyDic)
        {
            if (string.IsNullOrEmpty(data.Speaker))
            {
                OnNarrator?.Invoke();
            }
            else
            {
                OnSpeaker?.Invoke();
                speakerText.text = ReflectProperty(data.Speaker, propertyDic);
            }
            
            foreach (var message in data.MessageList)
            {
                messageText.SetText(ReflectProperty(message, propertyDic));
                await WaitReading();
            }
        }
        public override async UniTask<string> OnSelect(SelectData data, Transform parent, Button prefab)
        {
            var textComponent = prefab.GetComponentInChildren<TextMeshProUGUI>();
            var isSelected = false;
            var index = 0;
            
            OnShowSelectButtons?.Invoke();
            foreach (var text in data.SelectTexts)
            {
                textComponent.text = text;
                var obj = Object.Instantiate(prefab, parent);

                var i = index;
                obj.onClick.AddListener(() =>
                {
                    isSelected = true;
                    index = i;
                });
                index++;
            }

            await UniTask.WaitUntil(() => isSelected);
            OnSelected?.Invoke();
            
            return data.NextDataIds[index];
        }

        public override void OnStart(in StartData data)
        {
            OnConversationStart?.Invoke();
        }

        public override void OnEnd(in EndData data)
        {
            OnConversationEnd?.Invoke();
        }

        public override async UniTask WaitReading()
        {
            await UniTask.WaitForSeconds(3);
        }
    }
}
