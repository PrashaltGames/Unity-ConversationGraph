using System;
using System.Collections.Generic;
using ConversationGraph.Runtime.Foundation;
using Cysharp.Threading.Tasks;
using TMPro;

namespace ConversationGraph.Runtime.Core.Interfaces
{
    public abstract class BaseFacilitator : IFacilitator
    {
        public Action OnConversationStart;
        public Action OnConversationEnd;
        public Action OnNarrator;
        public Action OnSpeaker;
        public abstract void StartConversation(string startId , TextMeshProUGUI titleText, TextMeshProUGUI speakerText, TextMeshProUGUI messageText, IReadOnlyDictionary<string, ConversationData> dataDic);
        public abstract void AfterMessage(TextMeshProUGUI text);
        public abstract void BeforeMessage(TextMeshProUGUI text);
        public abstract UniTask OnMessage(TextMeshProUGUI speakerText, TextMeshProUGUI messageText, MessageData data);
        public abstract void OnStart(StartData data);
        public abstract void OnEnd(EndData data);
        public abstract UniTask ReadingMessage();
    }
}
