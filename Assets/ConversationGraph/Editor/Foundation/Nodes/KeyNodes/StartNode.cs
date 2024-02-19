using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConversationGraph.Editor.Foundation.Nodes.KeyNodes
{
    public sealed class StartNode : BaseNode
    {
        public StartNode()
        {
            title = "Start";
            
            AddOutputPort("Start", Port.Capacity.Single, typeof(float));
            
            mainContainer.Add(new Label("Title"));
            mainContainer.Add(new TextField());
        }

        public override string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
