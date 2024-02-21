using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace ConversationGraph.Runtime.Foundation
{
    public class ConversationAsset : ScriptableObject
    {
        public List<ConversationSaveData> ConversationSaveData
        {
            get => _conversationSaveData;
            set => _conversationSaveData = value;
        }

        public string Title
        {
            get => _title;
            set => _title = value;
        }
        
        [SerializeField] private List<ConversationSaveData> _conversationSaveData = new();
        [SerializeField] private string _title;
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
