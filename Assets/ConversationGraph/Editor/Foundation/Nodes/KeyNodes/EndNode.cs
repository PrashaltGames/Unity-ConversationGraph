using ConversationGraph.Runtime.Foundation;
using UnityEditor.Experimental.GraphView;

namespace ConversationGraph.Editor.Foundation.Nodes.KeyNodes
{
    public sealed class EndNode : BaseNode
    {
        public EndNode()
        {
            title = "End";
            
            AddInputPort("End", Port.Capacity.Multi, typeof(float));

            Data = new EndData();
            
            capabilities &= ~Capabilities.Deletable;
        }
    }
}