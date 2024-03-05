using System;
using ConversationGraph.Runtime.Foundation;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ConversationGraph.Editor.Foundation.Nodes.LogicNodes
{
    [Serializable]
    public class ScriptableNode : BaseNode
    {
        public ScriptableData ScriptableData => Data as ScriptableData;

        public ScriptableNode()
        {
            title = "None (Scriptable Node)";
            
            AddInputPort("Input", Port.Capacity.Single, typeof(float));
            AddOutputPort("Output", Port.Capacity.Single, typeof(float));

            Data = new ScriptableData();
        }

        public override void Initialize(string id, Rect rect, string json, Action<BaseNode> onSelect)
        {
            base.Initialize(id, rect, json, onSelect);
            
            // Setup to item list
            if (!string.IsNullOrEmpty(json))
            {
                var data = JsonUtility.FromJson<ScriptableData>(json);
                ScriptableData.ScriptAsset = data.ScriptAsset;
                
                OnSelectScript();
            }
        }

        public void OnSelectScript()
        {
            title = $"{ScriptableData.ScriptAsset.ScriptableConversation.GetType().Name} (Scriptable Node)";
        }
    }
}