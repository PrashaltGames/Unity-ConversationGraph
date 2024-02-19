using System;
using ConversationGraph.Editor.Foundation;
using ConversationGraph.Editor.Foundation.Nodes;
using ConversationGraph.Editor.Foundation.Nodes.ConversationNode;
using ConversationGraph.Editor.Foundation.Nodes.KeyNodes;
using Unity.AppUI.Editor;
using Unity.AppUI.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using TextField = UnityEngine.UIElements.TextField;

namespace ConversationGraph.Editor.Core
{
    public class ConversationGraphInspector : EditorWindow
    {
        private const string NarratorUIDocumentGuid = "4807ace79a615444199b94d1e67337c5";
        private const string StartUIDocumentGuid = "813588f557bf22b4693716ad76477b70";
        private const string EndUIDocumentGuid = "3130ce218a52aa440aae8d27541b8ce5";

        private const string TSSGuid = "dc39b1949c0d08c4b93d17de7fb085d0";

        private (int index, VisualElement element) _selectedElement;
        
        /// <summary>
        /// Selected Node
        /// </summary>
        public BaseNode SelectedNode
        {
            get => _selectedNode;
            set
            {
                _selectedNode = value;
                OnChangeSelectNode();
            }
        }
        private BaseNode _selectedNode;

        private void OnEnable()
        {
            var tssAsset = AssetDatabase.GUIDToAssetPath(TSSGuid);
            rootVisualElement.styleSheets.Add(AssetDatabase.LoadAssetAtPath<ThemeStyleSheet>(tssAsset));
            rootVisualElement.AddToClassList("appui--editor-dark"); 
        }

        private void OnChangeSelectNode()
        {
            CreateGUI();
        }

        private void CreateGUI()
        {
            rootVisualElement.Clear();
            
            switch (_selectedNode)
            {
                case NarratorNode narrator:
                    ShowNarratorInspector(narrator);
                    break;
                case StartNode start:
                    ShowStartInspector(start);
                    break;
                case EndNode end:
                    ShowEndInspector(end);
                    break;
            }
        }

        private void ShowNarratorInspector(NarratorNode node)
        {
            var narratorUI = 
                ConversationGraphEditorUtility.CreateElementFromGuid(NarratorUIDocumentGuid);

            // Set up ScrollView
            var scrollView = narratorUI.Q<ScrollView>();
            int index = 0;
            foreach (var message in node.MessageList)
            {
                var textArea = CreateNewTextArea(message);
                var i = index;
                textArea.RegisterCallback<ClickEvent>(_ => OnSelectTextArea(textArea, i));
                
                scrollView.Add(textArea);
            }
            
            // Set up Stepper
            var stepper = narratorUI.Q<Stepper>();
            stepper.RegisterValueChangedCallback(i => StepperChangeEvent(i.newValue, node, scrollView));
            
            rootVisualElement.Add(narratorUI);
        }

        private void StepperChangeEvent(int i, ConversationNode node, VisualElement view)
        {
            // Add
            if (i == 1)
            {
                node.MessageList.Add("");
                var textArea = CreateNewTextArea();
                textArea.RegisterCallback<ClickEvent>(e => OnSelectTextArea(textArea, node.MessageList.Count - 1));
                view.Add(textArea);
            }
            // Remove
            else
            {
                if (_selectedElement.element is null || _selectedElement.index > node.MessageList.Count)
                {
                    return;
                }
                view.Remove(_selectedElement.element);
                node.MessageList.RemoveAt(_selectedElement.index);

                _selectedElement.element = null;
            }
        }

        private TextArea CreateNewTextArea(string message)
        {
            var textArea = CreateNewTextArea();
            textArea.value = message;
            
            return textArea;
        }
        private TextArea CreateNewTextArea()
        {
            var textArea = new TextArea();
            textArea.style.marginBottom = 2;
            textArea.style.marginTop = 2;
            textArea.style.marginRight = 2;
            textArea.style.marginLeft = 2;
            textArea.style.width = Length.Percent(100);

            return textArea;
        }

        private void OnSelectTextArea(VisualElement element, int index)
        {
            _selectedElement.element = element;
            _selectedElement.index = index;
        }
        private void ShowStartInspector(StartNode node)
        {
            // MainContainerをテンプレートからコピー
            var startUI =
                ConversationGraphEditorUtility.CreateElementFromGuid(StartUIDocumentGuid);
            
            rootVisualElement.Add(startUI);
        }

        private void ShowEndInspector(EndNode node)
        {
            // MainContainerをテンプレートからコピー
            var endUI =
                ConversationGraphEditorUtility.CreateElementFromGuid(EndUIDocumentGuid);
            
            rootVisualElement.Add(endUI);
        }
    }
}