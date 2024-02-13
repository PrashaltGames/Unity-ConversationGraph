using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ConversationGraph.Editor.Foundation.Nodes.KeyNodes
{
    public sealed class StartNode : BaseNode
    {
        public StartNode()
        {
            title = "Start";
            
            AddOutputPort("Start", Port.Capacity.Single, typeof(float));
        }
    }
}
