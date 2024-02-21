using System;
using System.Collections.Generic;
using ConversationGraph.Editor.Foundation;
using ConversationGraph.Editor.Foundation.Nodes;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using System.Linq;
using ConversationGraph.Runtime.Foundation;

namespace ConversationGraph.Editor.Core.GraphBase
{
    public class ConversationGraphWindow : EditorWindow
    {
        #region Properties
        
        /// <summary>
        /// Asset for this window.
        /// </summary>
        public ConversationGraphAsset Asset { get; set; }

        private ConversationGraphView _view;
        
        #endregion

        #region Variables

        private ConversationGraphInspector _inspector;

        #endregion
        #region Static_Variables
        /// <summary>
        /// List for All active windows.
        /// </summary>
        private static List<ConversationGraphWindow> _activeWindowList = new();
        
        #endregion
        #region Methods

        public void ShowInspector(BaseNode node)
        {
            _inspector = GetWindow<ConversationGraphInspector>();
            _inspector.ShowUtility();
            _inspector.SelectedNode = node;
        }
        private async void OnEnable()
        {
            rootVisualElement.Clear();
            
            await UniTask.WaitUntil(() => Asset is not null);
            _view = new ConversationGraphView(this);
            rootVisualElement.Add(_view);

            var toolBar = new Toolbar();
            var saveButton = new ToolbarButton(OnSave) { text = "Save", name = "save-button" };
            toolBar.Add(saveButton);
            rootVisualElement.Add(toolBar);
        }

        private void OnDestroy()
        {
            _inspector?.Close();
        }

        private void OnSave()
        {
            Asset.ClearNodes();
            Asset.ClearEdges();
            
            // Nodes
            var isShowedWarning = false;
            foreach (var node in _view.nodes)
            {
                if((ConversationGraphEditorUtility.
                        CheckPortEmpty(node.inputContainer.Children().Select(x => x as Port)) || 
                    ConversationGraphEditorUtility.
                        CheckPortEmpty(node.outputContainer.Children().Select(x => x as Port)))
                   && !isShowedWarning)
                {
                    Debug.LogWarning("ConversationGraph : There are Empty Port!");
                    isShowedWarning = true;
                }
                if (node is BaseNode masterNode)
                {
                    var nodeData = ConversationGraphEditorUtility.NodeToData(masterNode);
                    Asset.SaveNode(nodeData);
                }
            }
            
            // Edges
            Asset.ClearEdges();
            foreach (var edge in _view.edges)
            {
                var edgeData = ConversationGraphEditorUtility.EdgeToData(edge);
                if (edgeData is null) continue;

                var inputOptionId = 0;
                foreach (var parentChild in edge.input.parent.Children())
                {
                    if (parentChild == edge.input)
                    {
                        break;
                    }
                    inputOptionId++;
                }
                edgeData.Id += $":{inputOptionId}";

                var outputOptionId = 0;
                foreach (var parentChild in edge.output.parent.Children())
                {
                    if (parentChild == edge.output)
                    {
                        break;
                    }
                    outputOptionId++;
                }
                edgeData.Id += $":{outputOptionId}";

                Asset.SaveEdge(edgeData);
            }

            // Subassetを追加
            AddNewSubAsset();
            
            EditorUtility.SetDirty(Asset);
            AssetDatabase.SaveAssets();

            Asset.IsModified = false;
            titleContent.text = titleContent.text.Replace("*", "");
        }

        private void AddNewSubAsset()
        {
            RemoveAllSubAsset();

            var subAsset = CreateInstance<ConversationAsset>();
            subAsset.name = "Conversation Asset";
            foreach (var node in _view.nodes)
            {
                if (node is BaseNode baseNode)
                {
                    var dataJson = JsonUtility.ToJson(baseNode.Data);
                    var saveData = new ConversationSaveData(baseNode.Data.GetType().FullName, dataJson);
                    subAsset.ConversationSaveData.Add(saveData);
                }
            }
            AssetDatabase.AddObjectToAsset(subAsset, Asset);
        }
        private void RemoveAllSubAsset()
        {
            foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(Asset))) 
            {
                if (AssetDatabase.IsSubAsset(asset)) 
                {
                    DestroyImmediate(asset, true);
                }
            }
        }
        #endregion

        #region Static_Methods
        
        [OnOpenAsset()]
        private static bool ShowOpenAsset(int instanceID, int _)
        {
            if (EditorUtility.InstanceIDToObject(instanceID) is ConversationGraphAsset asset)
            {
                if (HasOpenInstances<ConversationGraphWindow>())
                {
                    foreach (var window in _activeWindowList)
                    {
                        if (window.Asset.GetInstanceID() == asset.GetInstanceID())
                        {
                            window.Focus();
                            return false;
                        }
                    }
                }

                CreateNewWindow(asset);
                return true;
            }

            return true;
        }
        
        #endregion

        private void ShowWindow(ConversationGraphAsset convasationGraphAsset)
        {
            Asset = convasationGraphAsset;

            Show();
        }

        #region Utility
        private static void CreateNewWindow(ConversationGraphAsset conversationGraphAsset)
        {
            var newWindow = CreateWindow<ConversationGraphWindow>(typeof(SceneView));

            newWindow.ShowWindow(conversationGraphAsset);
            newWindow.titleContent = new(conversationGraphAsset.name);
            newWindow.Focus();

            _activeWindowList.Add(newWindow);
        }
        #endregion
    }
}
