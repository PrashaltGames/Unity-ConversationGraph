using System;
using ConversationGraph.Runtime.Foundation;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConversationGraph.Editor.Foundation.Nodes.ConversationNode
{
    [Serializable]
    public class MessageNode : BaseNode
    {
        public MessageData MessageData => Data as MessageData;
        
        private const string UIDocumentGuid = "62b599624b359384ea2c322e81eab23a";
        private ListView _listView;
        public MessageNode()
        {
            AddInputPort("Input", Port.Capacity.Single, typeof(float));
            
            //MainContainerをテンプレートからコピー
            var assetPath = AssetDatabase.GUIDToAssetPath(UIDocumentGuid);
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(assetPath);
            var defaultContainer = visualTree.Instantiate();
            
            _listView = defaultContainer.Q<ListView>();
            _listView.makeItem += CreateMessageElement;
            _listView.bindItem += SetMessage;
            
            mainContainer.Add(defaultContainer);

            Data = new MessageData();
        }

        private void SetMessage(VisualElement visualElement, int index)
        {
            visualElement.Q<Label>().text = MessageData.MessageList[index];
        }

        private VisualElement CreateMessageElement()
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
                MessageData.MessageList = data.MessageList;   
            }
            
            _listView.itemsSource = MessageData.MessageList;
        }
        public void RefreshListView()
        {
            _listView.RefreshItems();
        }
    }
}
