using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConversationGraph.Editor.Foundation.Nodes.ConversationNode
{
    [Serializable]
    public abstract class ConversationNode : BaseNode
    {
        public List<string> MessageList { get; private set; } = new();
        private const string UIDocumentGuid = "62b599624b359384ea2c322e81eab23a";
        public ConversationNode()
        {
            AddInputPort("Input", Port.Capacity.Single, typeof(float));
            
            //MainContainerをテンプレートからコピー
            var assetPath = AssetDatabase.GUIDToAssetPath(UIDocumentGuid);
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(assetPath);
            var defaultContainer = visualTree.Instantiate();

                        
            MessageList.Add("aaa");
            MessageList.Add("bbb");
            
            var listView = defaultContainer.Q<ListView>();
            listView.itemsSource = MessageList;
            listView.makeItem += CreateMessageElement;
            listView.bindItem += SetMessage;
            
            mainContainer.Add(defaultContainer);
        }

        private void SetMessage(VisualElement visualElement, int index)
        {
            visualElement.Q<Label>().text = MessageList[index];
        }

        private VisualElement CreateMessageElement()
        {
            var label = new Label();
            label.style.paddingBottom = 3;
            label.style.paddingTop = 3;
            return label;
        }
        public override string ToJson()
        {
            base.ToJson();

            return JsonUtility.ToJson(this);
        }
    }
}
