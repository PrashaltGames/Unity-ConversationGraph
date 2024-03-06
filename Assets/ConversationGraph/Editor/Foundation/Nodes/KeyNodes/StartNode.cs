using ConversationGraph.Runtime.Foundation;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConversationGraph.Editor.Foundation.Nodes.KeyNodes
{
    public sealed class StartNode : BaseNode
    {
        public StartData StartData => Data as StartData;
        public StartNode()
        {
            title = "Start";
            
            AddOutputPort("Start", Port.Capacity.Single, typeof(float));

            Data = new StartData();
        }

        public override string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
