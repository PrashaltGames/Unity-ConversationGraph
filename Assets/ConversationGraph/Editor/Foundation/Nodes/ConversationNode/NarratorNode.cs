using UnityEditor.Experimental.GraphView;

namespace ConversationGraph.Editor.Foundation.Nodes.ConversationNode
{
    public class NarratorNode : MessageNode
    {
        public NarratorNode() : base()
        {
            title = "Narrator";

            AddOutputPort("Output", Port.Capacity.Single, typeof(float));
        }
    }
}
