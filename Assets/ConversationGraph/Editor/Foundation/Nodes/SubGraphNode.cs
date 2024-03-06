using UnityEditor.Experimental.GraphView;

namespace ConversationGraph.Editor.Foundation.Nodes
{
    public class SubGraphNode : BaseNode
    {
        public ConversationGraphAsset SubGraph { get; set; }
        public SubGraphNode()
        {
            AddInputPort("Input", Port.Capacity.Single, typeof(float));
            AddOutputPort("Output", Port.Capacity.Single, typeof(float));
        }

        public void SetSubGraphAsset(ConversationGraphAsset subGraph)
        {
            SubGraph = subGraph;
            title = $"{subGraph.name} (Sub Graph)";
        }
    }
}