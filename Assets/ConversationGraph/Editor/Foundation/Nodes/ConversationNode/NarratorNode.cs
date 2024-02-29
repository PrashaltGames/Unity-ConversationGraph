using ConversationGraph.Editor.Core;
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
            var defaultContainer = 
                ConversationGraphEditorUtility.CreateElementFromGuid(UIDocumentGuid);
            
            ListView = defaultContainer.Q<ListView>();
            ListView.makeItem += CreateMessageElement;
            ListView.bindItem += SetMessage;
            
            mainContainer.Add(defaultContainer);
            
            AddOutputPort("Output", Port.Capacity.Single, typeof(float));
        }
    }
}
