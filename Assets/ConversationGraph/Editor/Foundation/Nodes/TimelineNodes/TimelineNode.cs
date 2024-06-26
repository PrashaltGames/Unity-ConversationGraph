﻿#if USE_TIMELINE

using System;
using ConversationGraph.Editor.Core;
using ConversationGraph.Runtime.Foundation;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

namespace ConversationGraph.Editor.Foundation.Nodes.TimelineNodes
{
    public class TimelineNode : BaseNode
    {
        public TimelineData TimelineData => Data as TimelineData;
     
        private const string UIDocumentGuid = "b7f2c7a2c5710de48ba1834234f47d70";

        private ObjectField _objectField;
        
        public TimelineNode()
        {
            title = "Timeline Node";
            
            var defaultContainer = 
                ConversationGraphEditorUtility.CreateElementByGuid(UIDocumentGuid);

            _objectField = defaultContainer.Q<ObjectField>();
            _objectField.RegisterValueChangedCallback(e =>
            {
                var timelineAsset = (TimelineAsset)e.newValue;
                TimelineData.TimelineAsset = timelineAsset;

                var path = AssetDatabase.GetAssetPath(timelineAsset);
                TimelineData.AssetGuid = AssetDatabase.GUIDFromAssetPath(path).ToString();
            });
            mainContainer.Add(defaultContainer);
            
            AddInputPort("Input", Port.Capacity.Single, typeof(float));
            AddOutputPort("Output", Port.Capacity.Single, typeof(float));

            Data = new TimelineData();
        }

        public override void Initialize(string id, Rect rect, string json, Action<BaseNode> onSelect)
        {
            base.Initialize(id, rect, json, onSelect);

            // Setup to Node
            if (!string.IsNullOrEmpty(json))
            {
                var data = JsonUtility.FromJson<TimelineData>(json);

                var timelineAsset = ConversationGraphEditorUtility.GetAssetByGuid<TimelineAsset>(data.AssetGuid);
                TimelineData.TimelineAsset = timelineAsset;
                _objectField.SetValueWithoutNotify(timelineAsset);
                
                var path = AssetDatabase.GetAssetPath(timelineAsset);
                TimelineData.AssetGuid = AssetDatabase.GUIDFromAssetPath(path).ToString();
            }
        }
    }
}

#endif