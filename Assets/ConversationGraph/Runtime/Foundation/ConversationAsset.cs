using System;
using UnityEngine;

namespace ConversationGraph.Runtime.Foundation
{
    public class ConversationAsset : ScriptableObject
    {
        public SerializedDictionary<string, ConversationSaveData> ConversationSaveData
        {
            get => _conversationSaveData;
            set => _conversationSaveData = value;
        }

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
