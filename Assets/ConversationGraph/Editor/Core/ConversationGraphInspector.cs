using System;
using System.Linq;
using ConversationGraph.Editor.Foundation;
using ConversationGraph.Editor.Foundation.Nodes;
using ConversationGraph.Editor.Foundation.Nodes.ConversationNode;
using ConversationGraph.Editor.Foundation.Nodes.KeyNodes;
using ConversationGraph.Editor.Foundation.Nodes.LogicNodes;
using ConversationGraph.Runtime.Core.Interfaces;
using Cysharp.Threading.Tasks;
using Unity.AppUI.UI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using TextField = Unity.AppUI.UI.TextField;

namespace ConversationGraph.Editor.Core
{
    public class ConversationGraphInspector : EditorWindow
    {
        private const string SpeakerUIDocumentGuid = "eca362e8131f79d45801f2ec6085117b";
        private const string NarratorUIDocumentGuid = "4807ace79a615444199b94d1e67337c5";
        private const string StartUIDocumentGuid = "813588f557bf22b4693716ad76477b70";
        private const string EndUIDocumentGuid = "3130ce218a52aa440aae8d27541b8ce5";
        private const string SelectUIDocumentGuid = "ae6321a0c6aa2af408520ffa5dae5c24";
        private const string ScriptableDocumentGuid = "5264841ee0e405c40b7bf90d52bddf5f";
        private const string ScriptableBranchDocumentGuid = "ab840144c686f084f9a76cf378ff6317";

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

        public ConversationGraphAsset ConversationGraphAsset { get; set; }
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
                case SpeakerNode speaker:
                    ShowSpeakerInspector(speaker);
                    break;
                case NarratorNode narrator:
                    ShowNarratorInspector(narrator);
                    break;
                case StartNode start:
                    ShowStartInspector(start);
                    break;
                case EndNode end:
                    ShowEndInspector(end);
                    break;
                case SelectNode select:
                    ShowSelectInspector(select);
                    break;
                case ScriptableNode scriptable:
                    ShowScriptableInspector(scriptable);
                    break;
                case ScriptableBranchNode scriptableBranch:
                    ShowScriptableBranchInspector(scriptableBranch);
                    break;
            }
        }

        private void ResetInspector()
        {
            rootVisualElement.Clear();
            _selectedElement = (-1, null);
        }

        private void ShowSpeakerInspector(SpeakerNode node)
        {
            var speakerUI = 
                ConversationGraphEditorUtility.CreateElementByGuid(SpeakerUIDocumentGuid);

            var textField = speakerUI.Q<TextField>();
            textField.value = node.MessageData.Speaker;
            textField.RegisterValueChangedCallback(e => SpeakerChangeEvent(e, node));
            
            // TODO: Method化
            // Set up ScrollView
            var scrollView = speakerUI.Q<ScrollView>();
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
            var stepper = speakerUI.Q<Stepper>();
            stepper.RegisterValueChangedCallback(i => StepperChangeEvent(i.newValue, node, scrollView));
            
            // Set up Animation
            var animationDropdown = speakerUI.Q<Dropdown>();
            var typeList = ConversationGraphEditorUtility.GetSubClassesByInterface<ITextAnimation>().ToList();
            animationDropdown.bindItem = (item, i) =>
            {
                item.label = typeList[i].Name;
            };
            animationDropdown.sourceItems = typeList;
            animationDropdown.RegisterValueChangedCallback(e =>
            {
                var i = e.newValue.First();
                node.MessageData.AnimationData = (ITextAnimation)Activator.CreateInstance(typeList[i]);
                Modifier();
            });
            if (node.MessageData.AnimationData is not null)
            {
                animationDropdown.selectedIndex = 
                    typeList.FindIndex(x => x == node.MessageData.AnimationData.GetType());
            }
            
            rootVisualElement.Add(speakerUI);
        }
        private void ShowNarratorInspector(NarratorNode node)
        {
            var narratorUI = 
                ConversationGraphEditorUtility.CreateElementByGuid(NarratorUIDocumentGuid);

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
            
            // Set up Animation
            var animationDropdown = narratorUI.Q<Dropdown>();
            var typeList = ConversationGraphEditorUtility.GetSubClassesByInterface<ITextAnimation>().ToList();
            animationDropdown.bindItem = (item, i) =>
            {
                item.label = typeList[i].Name;
            };
            animationDropdown.sourceItems = typeList;
            animationDropdown.RegisterValueChangedCallback(e =>
            {
                var i = e.newValue.First();
                node.MessageData.AnimationData = (ITextAnimation)Activator.CreateInstance(typeList[i]);
                Modifier();
            });
            if (node.MessageData.AnimationData is not null)
            {
                animationDropdown.selectedIndex = 
                    typeList.FindIndex(x => x == node.MessageData.AnimationData.GetType());
            }
            rootVisualElement.Add(narratorUI);
        }
        private void ShowStartInspector(StartNode node)
        {
            // MainContainerをテンプレートからコピー
            var startUI =
                ConversationGraphEditorUtility.CreateElementByGuid(StartUIDocumentGuid);
            startUI.Q<TextField>().RegisterValueChangedCallback(e =>
            {
                node.StartData.Title = e.newValue;
                Modifier();
            });
            
            rootVisualElement.Add(startUI);
        }
        private void ShowEndInspector(in EndNode node)
        {
            // MainContainerをテンプレートからコピー
            var endUI =
                ConversationGraphEditorUtility.CreateElementByGuid(EndUIDocumentGuid);
            
            rootVisualElement.Add(endUI);
        }

        private void ShowSelectInspector(SelectNode node)
        {
            var selectUI =
                ConversationGraphEditorUtility.CreateElementByGuid(SelectUIDocumentGuid);
            
            var listView = selectUI.Q<ListView>();
            listView.makeItem += () =>
            {
                Modifier();
                var textField = new TextField
                {
                    size = Size.L
                };
                return textField;
            };
            listView.bindItem += (element, index) =>
            {
                var textField = element.Q<TextField>();
                textField.value = node.SelectData.SelectTexts[index];
                textField.RegisterValueChangedCallback(e =>
                {
                    node.SelectData.SelectTexts[index] = e.newValue;
                    node.RefreshNode();
                });
            };
            listView.itemsSource = node.SelectData.SelectTexts;

            listView.itemsAdded += _ => node.RefreshNode();
            listView.itemsRemoved += _ => node.RefreshNode();
            listView.selectedIndicesChanged += _ => node.RefreshNode();
            
            rootVisualElement.Add(selectUI);
        }

        private void ShowScriptableInspector(ScriptableNode node)
        {
            var scriptableUI = ConversationGraphEditorUtility.CreateElementByGuid(ScriptableDocumentGuid);
            var dropdown = scriptableUI.Q<Dropdown>();
            var typeList = ConversationGraphEditorUtility.GetSubClassesByInterface<IScriptableConversation>().ToList();
            dropdown.bindItem = (item, index) =>
            {
                item.label = typeList[index].Name;
            };
            dropdown.sourceItems = typeList;
            dropdown.RegisterValueChangedCallback(e =>
            {
                var index = e.newValue.First();
                node.SetScript((IScriptableConversation)Activator.CreateInstance(typeList[index]));
                Modifier();
            });

            if (node.ScriptableData.ScriptableConversation is not null)
            {
                dropdown.selectedIndex = 
                    typeList.FindIndex(x => x == node.ScriptableData.ScriptableConversation.GetType());
            }
            
            rootVisualElement.Add(scriptableUI);
        }

        private void ShowScriptableBranchInspector(ScriptableBranchNode node)
        {
            var scriptableBranchUI = ConversationGraphEditorUtility.CreateElementByGuid(ScriptableBranchDocumentGuid);
            var dropdown = scriptableBranchUI.Q<Dropdown>();
            var typeList = ConversationGraphEditorUtility.GetSubClassesByInterface<IScriptableBranch>().ToList();
            dropdown.bindItem = (item, index) =>
            {
                item.label = typeList[index].Name;
            };
            dropdown.sourceItems = typeList;
            dropdown.RegisterValueChangedCallback(e =>
            {
                var index = e.newValue.First();
                node.SetScript((IScriptableBranch)Activator.CreateInstance(typeList[index]));
                Modifier();
            });

            if (node.ScriptableBranchData.ScriptableBranch is not null)
            {
                dropdown.selectedIndex = 
                    typeList.FindIndex(x => x == node.ScriptableBranchData.ScriptableBranch.GetType());
            }
            
            rootVisualElement.Add(scriptableBranchUI);
        }
            
        private void StepperChangeEvent(in int i, MessageNode node, in VisualElement view)
        {
            // Add
            if (i == 1)
            {
                node.MessageData.MessageList.Add("");
                var textArea = CreateNewTextArea(node);
                var index = node.MessageData.MessageList.Count - 1;
                textArea.RegisterCallback<ClickEvent>(_ => OnSelectTextArea(textArea, index));
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
            Modifier();
        }

        private void SpeakerChangeEvent(in ChangeEvent<string> e, in SpeakerNode node)
        {
            node.MessageData.Speaker = e.newValue;
            node.ChangeSpeakerName(e.newValue);
            Modifier();
        }
        private void MessageChangeEvent(in ChangeEvent<string> e, in MessageNode node, in TextArea textArea)
        {
            node.MessageData.MessageList[_selectedElement.index] = e.newValue;
            node.RefreshListView();
            Modifier();
        }

        private TextArea CreateNewTextArea(in MessageNode node, in string message)
        {
            var textArea = CreateNewTextArea(node);
            textArea.value = message;
            Modifier();
            
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
            textArea.RegisterValueChangedCallback(e => MessageChangeEvent(e, node, textArea));

            Modifier();
            
            return textArea;
        }

        private async void Modifier()
        {
            await UniTask.WaitUntil(() => ConversationGraphAsset is not null);
            ConversationGraphAsset.IsModified = true;
        }

        private void OnSelectTextArea(in VisualElement element, in int index)
        {
            _selectedElement.element = element;
            _selectedElement.index = index;
        }
    }
}