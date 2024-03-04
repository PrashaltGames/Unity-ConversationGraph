using System;
using ConversationGraph.Editor.Core;
using ConversationGraph.Runtime.Foundation;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConversationGraph.Editor.Foundation.Nodes.LogicNodes
{
    [Serializable]
    public class SelectNode : BaseNode
    {
        public SelectData SelectData => Data as SelectData;
        public SelectNode()
        {
            title = "Select";
            AddInputPort("Input", Port.Capacity.Single, typeof(float));
            Data = new SelectData();
        }

        public override void Initialize(string id, Rect rect, string json, Action<BaseNode> onSelect)
        {
            base.Initialize(id, rect, json, onSelect);
            
            // Setup to item list
            if (string.IsNullOrEmpty(json))
            {
                SelectData.SelectTexts.Add("Yes!");
                SelectData.SelectTexts.Add("No...");
            }
            else
            {
                var data = JsonUtility.FromJson<SelectData>(json);
                SelectData.SelectTexts = data.SelectTexts;
            }
            RefreshNode();
        }
        public void RefreshNode()
        {
            outputContainer.Clear();
            foreach (var selectText in SelectData.SelectTexts)
            {
                AddOutputPort(selectText, Port.Capacity.Single, typeof(float));
            }
        }
    }
}
