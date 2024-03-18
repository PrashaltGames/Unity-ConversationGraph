using ConversationGraph.Runtime.Core.Components;
using ConversationGraph.Runtime.Core.Interfaces;
using UnityEngine;

namespace ConversationGraph.Runtime.Foundation
{
    public class DummyScriptableConversation : IScriptableConversation
    {
        public string Guid { get; set; }
        public void OnArrival()
        {
#if UNITY_EDITOR
            Debug.LogWarning("ScriptableNodeにクラスが設定されていません");
#endif
        }
    }
}