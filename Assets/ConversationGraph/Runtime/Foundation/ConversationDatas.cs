using System;
using System.Collections.Generic;
using UnityEngine;

namespace ConversationGraph.Runtime.Foundation
{
    [Serializable]
    public class ConversationData
    {
        public List<string> NextDataIds
        {
            get => _nextDataIds; 
            set => _nextDataIds = value;
        }
        [SerializeField] private List<string> _nextDataIds = new();

        /// <summary>
        /// Get next data ids when it have.
        /// </summary>
        /// <param name="nextDataIds">next ids or null</param>
        /// <returns>Whether you have the next data IDs </returns>
        public bool TryGetNextDataIds(out List<string> nextDataIds)
        {
            if (_nextDataIds.Count <= 0)
            {
                nextDataIds = null;
                return false;
            }

            nextDataIds = _nextDataIds;
            return true;
        }
    }

    [Serializable]
    public class MessageData : ConversationData
    {
        public string Speaker
        {
            get => _speaker;
            set => _speaker = value;
        }
        public List<string> MessageList
        {
            get => _messageList; 
            set => _messageList = value;
        }
        [SerializeField] private List<string> _messageList = new();
        [SerializeField] private string _speaker;
    }

    [Serializable]
    public class StartData : ConversationData
    {
        [SerializeField] private string _title;

        public StartData(string title)
        {
            _title = title;
        }
    }
    [Serializable]
    public class EndData : ConversationData
    {
        
    }

    [Serializable]
    public class SelectData : ConversationData
    {
        public List<string> SelectTexts
        {
            get => _selectTexts;
            set => _selectTexts = value;
        }
        [SerializeField] private List<string> _selectTexts = new();
    }

    [Serializable]
    public class ScriptableData : ConversationData
    {
        public ConversationScriptAsset ScriptAsset
        {
            get => _scriptAsset;
            set => _scriptAsset = value;
        }
        [SerializeField] private ConversationScriptAsset _scriptAsset;

        public void Init()
        {
            _scriptAsset = ScriptableObject.CreateInstance<ConversationScriptAsset>();
        }
    }
}
