using System;
using System.Linq;
using ConversationGraph.Runtime.Foundation;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ConversationGraph.Editor.Foundation.Nodes
{
    public class SubGraphNode : BaseNode
    {
        public ConversationGraphAsset SubGraphAsset
        {
            get => _subGraphAsset;
            set
            {
                _subGraphAsset = value;
                title = $"{_subGraphAsset.name} (Sub Graph)";
            }
        }
        public SubGraphData SubGraphData => Data as SubGraphData;

        private ConversationGraphAsset _subGraphAsset;
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
                SubGraphData.Guid = data.Guid;
            }
        }

        public void SetSubGraphAsset(ConversationGraphAsset subGraphAsset)
        {
            var assetPath = AssetDatabase.GetAssetPath(subGraphAsset.GetInstanceID());
            SubGraphAsset = subGraphAsset;
            SubGraphData.SubgraphAsset = AssetDatabase.LoadAllAssetsAtPath(assetPath)
                .FirstOrDefault(x => x.GetType() == typeof(ConversationAsset)) as ConversationAsset;
            
            if (SubGraphData.SubgraphAsset is not null)
            {
                title = $"{SubGraphData.SubgraphAsset.name} (SubGraphAsset Node)";   
            }
        }
    }
}