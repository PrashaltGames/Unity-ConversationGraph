using System.Collections.Generic;
using ConversationGraph.Runtime.Core.Interfaces;
using ConversationGraph.Runtime.Foundation;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace ConversationGraph.Runtime.Core.Facilitators
{
    public class BasicFacilitator : BaseFacilitator
    {
        public override async void StartConversation
        (string startId,
            TextMeshProUGUI titleText,
            TextMeshProUGUI speakerText,
            TextMeshProUGUI messageText,
            IReadOnlyDictionary<string, ConversationData> dataDic)
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
                        await OnMessage(speakerText, messageText, messageData);
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

        public override void AfterMessage(TextMeshProUGUI text)
        {
            
        }

        public override void BeforeMessage(TextMeshProUGUI text)
        {
            
        }
        
        public override async UniTask OnMessage(TextMeshProUGUI speakerText, TextMeshProUGUI messageText, MessageData data)
        {
            if (string.IsNullOrEmpty(data.Speaker))
            {
                OnNarrator?.Invoke();
            }
            else
            {
                OnSpeaker?.Invoke();
                speakerText.text = data.Speaker;
            }
            
            foreach (var message in data.MessageList)
            {
                messageText.SetText(message);
                await ReadingMessage();
            }
        }

        public override void OnStart(StartData data)
        {
            OnConversationStart?.Invoke();
        }

        public override void OnEnd(EndData data)
        {
            OnConversationEnd?.Invoke();
        }

        public override async UniTask ReadingMessage()
        {
            await UniTask.WaitForSeconds(3);
        }
    }
}
