using System.Collections.Generic;
using ConversationGraph.Runtime.Foundation;
using Cysharp.Threading.Tasks;
using TMPro;

namespace ConversationGraph.Runtime.Core.Interfaces
{
    public interface IFacilitator
    {
        public void StartConversation(string startId , TextMeshProUGUI titleText, TextMeshProUGUI speakerText, TextMeshProUGUI messageText, IReadOnlyDictionary<string, ConversationData> dataDic);
        public void AfterMessage(TextMeshProUGUI text);
        public void BeforeMessage(TextMeshProUGUI text);
        public UniTask OnMessage(TextMeshProUGUI speakerText, TextMeshProUGUI messageText, MessageData data);
        public void OnStart(StartData data);
        public void OnEnd(EndData data);
        public UniTask ReadingMessage();
    }
}