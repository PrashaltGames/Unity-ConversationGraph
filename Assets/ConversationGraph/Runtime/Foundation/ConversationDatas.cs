using System;
using System.Collections.Generic;
using ConversationGraph.Runtime.Core;
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
    public class ScriptableData : ConversationData
    {
        public ConversationScriptAsset ScriptAsset
        {
            get => _scriptAsset;
            set => _scriptAsset = value;
        }

        public string ParentGuid => _parentGuid;
        public string AssetGuid => _assetGuid;
        [NonSerialized] private ConversationScriptAsset _scriptAsset;
        [SerializeField] private string _parentGuid;
        [SerializeField] private string _assetGuid;

#if UNITY_EDITOR
        public void Init(ScriptableObject asset)
        {
            _scriptAsset = ScriptableObject.CreateInstance<ConversationScriptAsset>();
            ScriptAsset.name = ScriptAsset.GetInstanceID().ToString();
            SetGuids(asset);
        }

        public void SetGuids(ScriptableObject asset)
        {
            _parentGuid = ConversationUtility.GetGuidByInstanceID(asset.GetInstanceID());
            _assetGuid = _scriptAsset.Guid;
        }
#endif
    }

    [Serializable]
    public class SubGraphData : ConversationData
    {
        public string AssetGuid
        {
            get => _assetGuid;
            set => _assetGuid = value;
        }
        [SerializeField] private string _assetGuid;
    }
}
