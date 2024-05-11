using System;
using System.Collections.Generic;
using ConversationGraph.Runtime.Foundation.Interfaces;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

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
        public ITextAnimation AnimationData
        {
            get => _animation;
            set => _animation = value;
        }
        
        [SerializeField] private List<string> _messageList = new();
        [SerializeField] private string _speaker;
        [SerializeReference] private ITextAnimation _animation;
    }

    [Serializable]
    public class StartData : ConversationData
    {
        public string Title
        {
            get => _title;
            set => _title = value;
        } 
        [SerializeField] private string _title;
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
    public class ScriptableEventData : ConversationData
    {
        public IScriptableEvent ScriptableEvent { get; set; }
        public string Guid
        {
            get => _guid;
            set => _guid = value;
        }
        [SerializeField] private string _guid;
    }

    [Serializable]
    public class ScriptableBranchData : ConversationData
    {
        public IScriptableBranch ScriptableBranch { get; set; }
        public string Guid
        {
            get => _guid;
            set => _guid = value;
        }
        [SerializeField] private string _guid;
    }

    [Serializable]
    public class SubGraphData : ConversationData
    {
        public ConversationAsset SubgraphAsset { get; set; }
        public string Guid
        {
            get => _guid;
            set => _guid = value;
        }
        [SerializeField] private string _guid;
    }

    [Serializable]
    public class TimelineData : ConversationData
    {
        public TimelineAsset TimelineAsset
        {
            get => _timelineAsset;
            set
            {
                _timelineAsset = value;
                var assetPath = AssetDatabase.GetAssetPath(value);
                _assetGuid = AssetDatabase.GUIDFromAssetPath(assetPath).ToString();
            }
        }
        public string AssetGuid => _assetGuid;

        private TimelineAsset _timelineAsset;
        [SerializeField] private string _assetGuid;
    }
}
