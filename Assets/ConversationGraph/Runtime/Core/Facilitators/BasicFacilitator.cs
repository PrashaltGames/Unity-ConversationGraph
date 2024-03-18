using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ConversationGraph.Runtime.Core.Base;
using ConversationGraph.Runtime.Core.Components;
using ConversationGraph.Runtime.Core.Interfaces;
using ConversationGraph.Runtime.Core.ReadingWaiter;
using ConversationGraph.Runtime.Foundation;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConversationGraph.Runtime.Core.Facilitators
{
    // TODO: Actionを使用してSystem側に返すようにする形式にしたい
    // awaitとかをしないといけなくて難しかった…
    public class BasicFacilitator : BaseFacilitator
    {
        public override async void StartConversation
            (ConversationSystem conversationSystem, ConversationAsset asset)
        {
            var id = asset.StartId;
            var isEnd = false;
            var conversationDataDic = GetConversationDicFromSaveDataDic(asset);
            while (true)
            {
                var data = conversationDataDic[id];
                var nextIndex = 0;

                switch (data)
                {
                    case MessageData messageData:
                        BeforeMessage(conversationSystem.MessageText);
                        await OnMessage(conversationSystem.SpeakerText, conversationSystem.MessageText, messageData, conversationSystem.ConversationPropertyAsset.PropertiesDictionary, conversationSystem.ReadingWaiter);
                        AfterMessage(conversationSystem.MessageText);
                        break;
                    case SelectData selectData:
                        nextIndex = await OnSelect(selectData, conversationSystem.SelectParent, conversationSystem.SelectButton);
                        break;
                    case ScriptableData scriptableData:
                        OnScriptable(scriptableData, conversationSystem);
                        break;
                    case SubGraphData subGraphData:
                        OnSubGraph(subGraphData, conversationSystem);
                        break;
                    case StartData startData:
                        OnStart(startData, conversationSystem.TitleText);
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

        private void Conversation()
        {
            
        }

        public override void AfterMessage(in TextMeshProUGUI text)
        {
            
        }

        public override void BeforeMessage(in TextMeshProUGUI text)
        {
            
        }
        
        public override async UniTask OnMessage(TextMeshProUGUI speakerText, TextMeshProUGUI messageText,
            MessageData data, IReadOnlyDictionary<string, string> propertyDic, IReadingWaiter readingWaiter)
        {
            var source = new CancellationTokenSource();
            
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
                
                data.AnimationData.StartAnimation(speakerText, messageText, source.Token);
                await new WaitForClick().WaitReading();
                source.Cancel();
                
                await readingWaiter.WaitReading();
            }
        }
        public override async UniTask<int> OnSelect(SelectData data, Transform parent, Button prefab)
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
            
            return index;
        }

        public override void OnScriptable(ScriptableData data, ConversationSystem system)
        {
            var script = system.ConversationAsset.ScriptableConversationDictionary[data.Guid];
            script.OnArrival(system);
        }

        public override void OnSubGraph(in SubGraphData data, ConversationSystem system)
        {
            var subGraphAsset = system.ConversationAsset.SubGraphAssetDictionary[data.Guid];
            StartConversation(system, subGraphAsset);
        }

        public override void OnStart(in StartData data, in TextMeshProUGUI title)
        {
            title.text = data.Title;
            OnConversationStart?.Invoke();
        }

        public override void OnEnd(in EndData data)
        {
            OnConversationEnd?.Invoke();
        }
    }
}
