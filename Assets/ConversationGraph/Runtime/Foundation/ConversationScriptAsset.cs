using ConversationGraph.Runtime.Core.Interfaces;
using UnityEngine;

namespace ConversationGraph.Runtime.Foundation
{
    [CreateAssetMenu]
    public class ConversationScriptAsset : ScriptableObject
    {
        public IScriptableConversation ScriptableConversation => _scriptableConversation;
        [SerializeReference, SubclassSelector] private IScriptableConversation _scriptableConversation;
    }
}