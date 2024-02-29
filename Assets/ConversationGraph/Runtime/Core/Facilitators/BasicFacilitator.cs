using System.Collections.Generic;
using ConversationGraph.Runtime.Core.Base;
using ConversationGraph.Runtime.Foundation;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace ConversationGraph.Runtime.Core.Facilitators
{
    public class BasicFacilitator : BaseFacilitator
    {
        public override async void StartConversation
        (
            string startId,
            TextMeshProUGUI titleText,
            TextMeshProUGUI speakerText,
            TextMeshProUGUI messageText,
            IReadOnlyDictionary<string, ConversationData> dataDic,
            IReadOnlyDictionary<string, string> propertyDic
            )
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
            in TextMeshProUGUI messageText, in IReadOnlyDictionary<string, ConversationData> dataDic, in ConversationPropertyAsset propertyAsset)
        {
            StartConversation(startId, titleText, speakerText, messageText, dataDic, propertyAsset.PropertiesDictionary);
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
