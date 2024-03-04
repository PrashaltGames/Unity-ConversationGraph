using System.Collections.Generic;
using ConversationGraph.Runtime.Foundation;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConversationGraph.Runtime.Core.Interfaces
{
    public interface IFacilitator
    {
        public void StartConversation(string startId, TextMeshProUGUI titleText, TextMeshProUGUI speakerText,
            TextMeshProUGUI messageText, IReadOnlyDictionary<string, ConversationData> dataDic,
            IReadOnlyDictionary<string, string> propertyDic, Transform selectParent, Button selectPrefab);
        public void StartConversation
        (in string startId, in TextMeshProUGUI titleText, in TextMeshProUGUI speakerText, in TextMeshProUGUI messageText, in IReadOnlyDictionary<string, ConversationData> dataDic, in ConversationPropertyAsset propertyAsset, in Transform selectParent, in Button selectPrefab);
        public void AfterMessage(in TextMeshProUGUI text);
        public void BeforeMessage(in TextMeshProUGUI text);
        public UniTask OnMessage(TextMeshProUGUI speakerText, TextMeshProUGUI messageText, MessageData data, IReadOnlyDictionary<string, string> propertyDic);
        public void OnStart(in StartData data);
        public void OnEnd(in EndData data);
        public UniTask<string> OnSelect(SelectData data, Transform parent, Button prefab);
    }
}