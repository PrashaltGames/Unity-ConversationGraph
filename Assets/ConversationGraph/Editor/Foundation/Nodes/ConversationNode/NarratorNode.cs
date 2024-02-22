using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace ConversationGraph.Editor.Foundation.Nodes.ConversationNode
{
    public class NarratorNode : MessageNode
    {
        private const string UIDocumentGuid = "62b599624b359384ea2c322e81eab23a";
        public NarratorNode() : base()
        {
            title = "Narrator";

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
    }
}
