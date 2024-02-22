using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConversationGraph.Editor.Foundation.Nodes.ConversationNode
{
    public class SpeakerNode : MessageNode
    {
        private const string UIDocumentGuid = "62b599624b359384ea2c322e81eab23a";

        private Label _speakerName;
        public SpeakerNode() : base()
        {
            title = "Speaker";

            _speakerName = new Label(MessageData.Speaker);
            mainContainer.Add(_speakerName);
            
            //MainContainerをテンプレートからコピー
            var assetPath = AssetDatabase.GUIDToAssetPath(UIDocumentGuid);
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(assetPath);
            var defaultContainer = visualTree.Instantiate();
            
            _listView = defaultContainer.Q<ListView>();
            _listView.makeItem += CreateMessageElement;
            _listView.bindItem += SetMessage;
            
            mainContainer.Add(defaultContainer);
            
            AddOutputPort("Output", Port.Capacity.Single, typeof(float));
        }

        public void ChangeSpeakerName(string speakerName)
        {
            _speakerName.text = speakerName;
        }

        public override void Initialize(string id, Rect rect, string json, Action<BaseNode> onSelect)
        {
            base.Initialize(id, rect, json, onSelect);
            
            _speakerName.text = MessageData.Speaker;
        }
    }
}