using System;
using System.Collections.Generic;
using UnityEngine;

namespace ConversationGraph.Runtime.Foundation
{
    public class ConversationData
    {
        
    }

    [Serializable]
    public class MessageData : ConversationData
    {
        public List<string> MessageList
        {
            get => _messageList; 
            set => _messageList = value;
        }
        [SerializeField] private List<string> _messageList = new();
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
}
