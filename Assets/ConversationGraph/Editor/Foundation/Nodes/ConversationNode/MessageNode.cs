using System;
using ConversationGraph.Runtime.Core.Animation;
using ConversationGraph.Runtime.Core.Interfaces;
using ConversationGraph.Runtime.Foundation;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConversationGraph.Editor.Foundation.Nodes.ConversationNode
{
    [Serializable]
    public class MessageNode : BaseNode
    {
        public MessageData MessageData => Data as MessageData;
        
        protected ListView ListView;
        public MessageNode()
        {
            AddInputPort("Input", Port.Capacity.Single, typeof(float));

            Data = new MessageData();
        }

        protected void SetMessage(VisualElement visualElement, int index)
        {
            visualElement.Q<Label>().text = MessageData.MessageList[index];
        }

        protected VisualElement CreateMessageElement()
        {
            var label = new Label
            {
                style =
                {
                    paddingBottom = 3,
                    paddingTop = 3
                }
            };
            return label;
        }
        public override string ToJson()
        {
            base.ToJson();

            if (MessageData.AnimationData is null)
            {
                MessageData.AnimationData = new DefaultAnimation();
            }
            return JsonUtility.ToJson(this);
        }
        
        public override void Initialize(string id, Rect rect, string json, Action<BaseNode> onSelect)
        {
            base.Initialize(id, rect, json, onSelect);

            // Setup to item list
            if (string.IsNullOrEmpty(json))
            {
                MessageData.MessageList.Add("Hello, World!");
                RefreshListView();
            }
            else
            {
                var data = JsonUtility.FromJson<MessageData>(json);
                MessageData.Speaker = data.Speaker;
                MessageData.MessageList = data.MessageList;
                MessageData.AnimationData = data.AnimationData ?? new DefaultAnimation();
            }
            
            ListView.itemsSource = MessageData.MessageList;
        }
        public void RefreshListView()
        {
            ListView.RefreshItems();
        }
    }
}
