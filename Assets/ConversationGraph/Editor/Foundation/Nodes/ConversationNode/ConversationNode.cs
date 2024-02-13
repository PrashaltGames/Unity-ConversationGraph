using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ConversationGraph.Editor.Foundation.Nodes.ConversationNode
{
    [Serializable]
    public abstract class ConversationNode : BaseNode
    {
        [SerializeField] protected List<string> MessageList { get; private set; }

        public ConversationNode()
        {
            AddInputPort("Input", Port.Capacity.Single, typeof(float));
        }

        public override string ToJson()
        {
            base.ToJson();

            return JsonUtility.ToJson(this);
        }
    }
}
