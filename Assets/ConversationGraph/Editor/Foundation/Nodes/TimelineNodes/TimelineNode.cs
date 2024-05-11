#if USE_TIMELINE

using System;
using ConversationGraph.Editor.Core;
using ConversationGraph.Runtime.Foundation;
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

                TimelineData.TimelineAsset = data.TimelineAsset;
                _objectField.SetValueWithoutNotify(TimelineData.TimelineAsset);
            }
        }
    }
}

#endif