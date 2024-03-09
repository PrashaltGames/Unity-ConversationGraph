using System;
using ConversationGraph.Runtime.Core.Interfaces;
using UnityEngine;

namespace ConversationGraph.Runtime.Foundation
{
    public class ConversationAsset : ScriptableObject
    {
        public SerializedDictionary<string, ConversationSaveData> ConversationSaveData 
            => _conversationSaveData;

        public SerializeReferenceDictionary<string, IScriptableConversation> ScriptableConversationDictionary 
            => _scriptableConversations;

        public string Title
        {
            get => _title;
            set => _title = value;
        }

        public string StartId
        {
            get => _startId;
            set => _startId = value;
        }
        
        [SerializeField] private SerializedDictionary<string, ConversationSaveData> _conversationSaveData = new();
        [SerializeField] private SerializeReferenceDictionary<string, IScriptableConversation> _scriptableConversations = new();
        [SerializeField] private string _title;
        [SerializeField] private string _startId;
    }
    [Serializable]
    public class ConversationSaveData
    {
        public string TypeName => _typeName;
        public string Json => _json;
        [SerializeField] private string _typeName;
        [SerializeField] private string _json;

        public ConversationSaveData(string typeName, string json)
        {
            _typeName = typeName;
            _json = json;
        }
    }
}
