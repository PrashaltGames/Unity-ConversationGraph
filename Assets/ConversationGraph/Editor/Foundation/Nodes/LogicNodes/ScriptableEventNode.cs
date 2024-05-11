using System;
using ConversationGraph.Runtime.Foundation;
using ConversationGraph.Runtime.Foundation.Interfaces;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ConversationGraph.Editor.Foundation.Nodes.LogicNodes
{
    [Serializable]
    public class ScriptableEventNode : BaseNode
    {
        public ScriptableEventData ScriptableEventData => Data as ScriptableEventData;

        public ScriptableEventNode()
        {
            title = "None (Scriptable Event)";
            
            AddInputPort("Input", Port.Capacity.Single, typeof(float));
            AddOutputPort("Output", Port.Capacity.Single, typeof(float));

            Data = new ScriptableEventData();
        }

        public override void Initialize(string id, Rect rect, string json, Action<BaseNode> onSelect)
        {
            base.Initialize(id, rect, json, onSelect);
            
            // Setup to item list
            if (!string.IsNullOrEmpty(json))
            {
                var data = JsonUtility.FromJson<ScriptableEventData>(json);
                ScriptableEventData.Guid = data.Guid;
            }
        }

        public void SetScript(IScriptableEvent scriptableEvent)
        {
            ScriptableEventData.ScriptableEvent = scriptableEvent;
            if (ScriptableEventData.ScriptableEvent is not null)
            {
                title = $"{ScriptableEventData.ScriptableEvent.GetType().Name} (Scriptable Node)";   
            }
        }
    }
}