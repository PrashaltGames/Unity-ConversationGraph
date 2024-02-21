using ConversationGraph.Runtime.Foundation;
using UnityEditor.Experimental.GraphView;

namespace ConversationGraph.Editor.Foundation.Nodes.KeyNodes
{
    public sealed class EndNode : BaseNode
    {
        public EndNode()
        {
            title = "End";
            
            AddInputPort("End", Port.Capacity.Single, typeof(float));

            Data = new EndData();
        }
    }
}