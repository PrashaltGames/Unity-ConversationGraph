using System;
using ConversationGraph.Editor.Core;
using ConversationGraph.Runtime.Core;
using ConversationGraph.Runtime.Foundation;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ConversationGraph.Editor.Foundation.Nodes
{
    public class SubGraphNode : BaseNode
    {
        public ConversationGraphAsset SubGraph
        {
            get => _subGraph;
            set
            {
                _subGraph = value;
                SubGraphData.AssetGuid = ConversationUtility.GetGuidByInstanceID(value.GetInstanceID());
                title = $"{_subGraph.name} (Sub Graph)";
            }
        }
        public SubGraphData SubGraphData => Data as SubGraphData;

        private ConversationGraphAsset _subGraph;
        public SubGraphNode()
        {
            AddInputPort("Input", Port.Capacity.Single, typeof(float));
            AddOutputPort("Output", Port.Capacity.Single, typeof(float));

            Data = new SubGraphData();
        }

        public override void Initialize(string id, Rect rect, string json, Action<BaseNode> onSelect)
        {
            base.Initialize(id, rect, json, onSelect);
            
            // Setup to item list
            if (!string.IsNullOrEmpty(json))
            {
                var data = JsonUtility.FromJson<SubGraphData>(json);
                var asset = ConversationGraphEditorUtility.GetConversationGraphAssetByGuid(data.AssetGuid);
                SubGraph = asset;
            }
        }
    }
}