﻿using ConversationGraph.Editor.Foundation;
using ConversationGraph.Editor.Foundation.Nodes;
using ConversationGraph.Editor.Foundation.Nodes.ConversationNode;
using ConversationGraph.Editor.Foundation.Nodes.KeyNodes;
using Unity.AppUI.UI;
using UnityEditor;
using UnityEngine.UIElements;

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
            ResetInspector();
            
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

        private void ResetInspector()
        {
            rootVisualElement.Clear();
            _selectedElement = (-1, null);
        }
        private void ShowNarratorInspector(NarratorNode node)
        {
            var narratorUI = 
                ConversationGraphEditorUtility.CreateElementFromGuid(NarratorUIDocumentGuid);

            // Set up ScrollView
            var scrollView = narratorUI.Q<ScrollView>();
            int index = 0;
            foreach (var message in node.MessageData.MessageList)
            {
                var textArea = CreateNewTextArea(node, message);
                var i = index;
                textArea.RegisterCallback<ClickEvent>(_ => OnSelectTextArea(textArea, i));
                
                scrollView.Add(textArea);

                index++;
            }
            
            // Set up Stepper
            var stepper = narratorUI.Q<Stepper>();
            stepper.RegisterValueChangedCallback(i => StepperChangeEvent(i.newValue, node, scrollView));
            
            rootVisualElement.Add(narratorUI);
        }

        private void StepperChangeEvent(in int i, MessageNode node, in VisualElement view)
        {
            // Add
            if (i == 1)
            {
                node.MessageData.MessageList.Add("");
                var textArea = CreateNewTextArea(node);
                var index = node.MessageData.MessageList.Count - 1;
                textArea.RegisterCallback<ClickEvent>(e => OnSelectTextArea(textArea, index));
                view.Add(textArea);
            }
            // Remove
            else
            {
                if (_selectedElement.element is null || _selectedElement.index > node.MessageData.MessageList.Count 
                                                     || node.MessageData.MessageList.Count == 1)
                {
                    return;
                }
                view.Remove(_selectedElement.element);
                node.MessageData.MessageList.RemoveAt(_selectedElement.index);
                node.RefreshListView();
                
                _selectedElement.element = null;
            }
        }

        private void TextChangeEvent(in ChangeEvent<string> e, in MessageNode node)
        {
            node.MessageData.MessageList[_selectedElement.index] = e.newValue;
            node.RefreshListView();
        }

        private TextArea CreateNewTextArea(in MessageNode node, in string message)
        {
            var textArea = CreateNewTextArea(node);
            textArea.value = message;
            
            return textArea;
        }
        private TextArea CreateNewTextArea(MessageNode node)
        {
            var textArea = new TextArea();
            textArea.style.marginBottom = 2;
            textArea.style.marginTop = 2;
            textArea.style.marginRight = 2;
            textArea.style.marginLeft = 2;
            textArea.style.width = Length.Percent(100);
            textArea.RegisterValueChangedCallback(e => TextChangeEvent(e, node));

            return textArea;
        }

        private void OnSelectTextArea(in VisualElement element, in int index)
        {
            _selectedElement.element = element;
            _selectedElement.index = index;
        }
        private void ShowStartInspector(in StartNode node)
        {
            // MainContainerをテンプレートからコピー
            var startUI =
                ConversationGraphEditorUtility.CreateElementFromGuid(StartUIDocumentGuid);
            
            rootVisualElement.Add(startUI);
        }

        private void ShowEndInspector(in EndNode node)
        {
            // MainContainerをテンプレートからコピー
            var endUI =
                ConversationGraphEditorUtility.CreateElementFromGuid(EndUIDocumentGuid);
            
            rootVisualElement.Add(endUI);
        }
    }
}