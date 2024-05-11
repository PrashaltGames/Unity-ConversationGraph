using System;
using ConversationGraph.Runtime.Foundation.Interfaces;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ConversationGraph.Runtime.Foundation
{
    public class ConversationAsset : ScriptableObject
    {
        public SerializedDictionary<string, ConversationSaveData> ConversationSaveData 
            => _conversationSaveData;

        public SerializedDictionary<string, ConversationAsset> SubGraphAssetDictionary
            => _subGraphAssetDictionary;

        public SerializeReferenceDictionary<string, IScriptableEvent> ScriptableConversationDictionary 
            => _scriptableConversations;

        public SerializeReferenceDictionary<string, IScriptableBranch> ScriptableBranchDictionary
            => _scriptableBranches;
        
        public SerializedDictionary<string, PlayableAsset> TimelineAssetsDictionary
            => _timelineAssetsDictionary;

        public ConversationPropertyAsset ConversationPropertyAsset
            => _propertyAsset;

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

        [SerializeField] private ConversationPropertyAsset _propertyAsset;
        [SerializeField] private SerializedDictionary<string, ConversationSaveData> _conversationSaveData = new();
        [SerializeField] private SerializeReferenceDictionary<string, IScriptableEvent> _scriptableConversations = new();
        [SerializeField] private SerializeReferenceDictionary<string, IScriptableBranch> _scriptableBranches = new();
        [SerializeField] private SerializedDictionary<string, ConversationAsset> _subGraphAssetDictionary = new();
        [SerializeField] private SerializedDictionary<string, PlayableAsset> _timelineAssetsDictionary = new();
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
