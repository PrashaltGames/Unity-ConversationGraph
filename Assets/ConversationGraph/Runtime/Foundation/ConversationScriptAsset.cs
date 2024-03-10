using ConversationGraph.Runtime.Core.Interfaces;
using UnityEngine;

namespace ConversationGraph.Runtime.Foundation
{
    [CreateAssetMenu]
    public class ConversationScriptAsset : ScriptableObject
    {
        public string Guid => _guid;
        public IScriptableConversation ScriptableConversation => _scriptableConversation;
        [SerializeReference] private IScriptableConversation _scriptableConversation;
        [SerializeField] private string _guid = System.Guid.NewGuid().ToString();
    }
}