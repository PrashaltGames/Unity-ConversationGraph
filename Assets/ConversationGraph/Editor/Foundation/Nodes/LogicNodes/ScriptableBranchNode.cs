using System;
using ConversationGraph.Runtime.Core.Interfaces;
using ConversationGraph.Runtime.Foundation;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ConversationGraph.Editor.Foundation.Nodes.LogicNodes
{
    public class ScriptableBranchNode : BaseNode
    {
        public ScriptableBranchData ScriptableBranchData => Data as ScriptableBranchData;

        public ScriptableBranchNode()
        {
            title = "None (Scriptable Branch Node)";
            
            AddInputPort("Input", Port.Capacity.Single, typeof(float));
            AddOutputPort("Output", Port.Capacity.Single, typeof(float));

            Data = new ScriptableBranchData();
        }

        public override void Initialize(string id, Rect rect, string json, Action<BaseNode> onSelect)
        {
            base.Initialize(id, rect, json, onSelect);
            
            if (!string.IsNullOrEmpty(json))
            {
                var data = JsonUtility.FromJson<ScriptableBranchData>(json);
                ScriptableBranchData.Guid = data.Guid;
            }
        }

        public void SetScript(IScriptableBranch scriptableBranch)
        {
            ScriptableBranchData.ScriptableBranch = scriptableBranch;
            if (ScriptableBranchData.ScriptableBranch is not null)
            {
                title = $"{ScriptableBranchData.ScriptableBranch.GetType().Name} (Scriptable Branch Node)";   
            }

            outputContainer.Clear();
            for (var i = 0; i < scriptableBranch.BranchCount; i++)
            {
                AddOutputPort(i.ToString(), Port.Capacity.Single, typeof(float));
            }
        }
    }
}