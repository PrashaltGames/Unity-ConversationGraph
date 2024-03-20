using ConversationGraph.Runtime.Core.Interfaces;
using UnityEngine;

namespace ConversationGraph.Runtime.Foundation.Dummies
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