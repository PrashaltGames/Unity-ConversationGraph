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
using ConversationGraph.Editor.Foundation.Nodes.LogicNodes;
using ConversationGraph.Editor.Foundation.Nodes.TimelineNodes;
using ConversationGraph.Runtime.Foundation;
using ConversationGraph.Runtime.Foundation.Dummies;
using UnityEngine.Timeline;

namespace ConversationGraph.Editor.Core.GraphBase
{
    public class ConversationGraphWindow : EditorWindow
    {
        #region Properties
        
        /// <summary>
        /// Asset for this window.
        /// </summary>
        public ConversationGraphAsset Asset { get; private set; }

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
            _inspector.ConversationGraphAsset = Asset;
        }
        public void OnGUI()
        {
            _view?.DropSubGraph();
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
            
            Asset.OnIsModified += () =>
            {
                titleContent.text = $"{Asset.name}*";
            };
        }

        private void OnDestroy()
        {
            _inspector?.Close();
            
            if(Asset is null || !Asset.IsModified)
            {
                return;
            }
            var shouldSave = EditorUtility.DisplayDialog($"{Asset.name} - Unsaved Changes Detected", "Do you want to save the changes you made in the Conversation Graph?", "Save", "Discard");
            if(shouldSave)
            {
                OnSave();
            }
        }

        private void OnSave()
        {
            Asset.ClearNodes();
            Asset.ClearEdges();
            Asset.ScriptableConversationDictionary.Clear();
            Asset.ScriptableBranchDictionary.Clear();
            Asset.SubGraphAssetDictionary.Clear();
            Asset.PlayableAssetsDictionary.Clear();
            
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
                if (node is BaseNode baseNode)
                {
                    if (baseNode is ScriptableEventNode scriptableNode)
                    {
                        scriptableNode.ScriptableEventData.Guid = string.IsNullOrEmpty(scriptableNode.ScriptableEventData.Guid) 
                            ? Guid.NewGuid().ToString() : scriptableNode.ScriptableEventData.Guid;
                        scriptableNode.ScriptableEventData.ScriptableEvent ??= new DummyScriptableEvent();
                        Asset.ScriptableConversationDictionary.Add(scriptableNode.ScriptableEventData.Guid, scriptableNode.ScriptableEventData.ScriptableEvent);
                    }
                    else if (baseNode is ScriptableBranchNode scriptableBranchNode)
                    {
                        scriptableBranchNode.ScriptableBranchData.Guid = string.IsNullOrEmpty(scriptableBranchNode.ScriptableBranchData.Guid) 
                            ? Guid.NewGuid().ToString() : scriptableBranchNode.ScriptableBranchData.Guid;
                        scriptableBranchNode.ScriptableBranchData.ScriptableBranch ??= new DummyScriptableBranch();
                        Asset.ScriptableBranchDictionary.Add(scriptableBranchNode.ScriptableBranchData.Guid, scriptableBranchNode.ScriptableBranchData.ScriptableBranch);
                    }
                    else if (baseNode is SubGraphNode subGraphNode)
                    {
                        if(subGraphNode.SubGraphData.SubgraphAsset is null) continue;
                        subGraphNode.SubGraphData.Guid = string.IsNullOrEmpty(subGraphNode.SubGraphData.Guid)
                            ? Guid.NewGuid().ToString() : subGraphNode.SubGraphData.Guid;
                        Asset.SubGraphAssetDictionary.Add(subGraphNode.SubGraphData.Guid, subGraphNode.SubGraphAsset);
                    }
                    else if (baseNode is TimelineNode timelineNode)
                    {
                        if (timelineNode.TimelineData.TimelineAsset is null) continue;
                        Asset.PlayableAssetsDictionary.TryAdd(timelineNode.TimelineData.AssetGuid,
                            timelineNode.TimelineData.TimelineAsset);
                    }
                    var nodeData = ConversationGraphEditorUtility.NodeToData(baseNode);
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
                edgeData.TargetNodeId += $":{inputOptionId}";

                var outputOptionId = 0;
                foreach (var parentChild in edge.output.parent.Children())
                {
                    if (parentChild == edge.output)
                    {
                        break;
                    }
                    outputOptionId++;
                }
                edgeData.BaseNodeId += $":{outputOptionId}";

                Asset.SaveEdge(edgeData);
            }

            // Subassetを追加
            AddConversationSubAsset();
            
            EditorUtility.SetDirty(Asset);
            AssetDatabase.SaveAssets();

            Asset.IsModified = false;
            titleContent.text = titleContent.text.Replace("*", "");
        }

        private void AddConversationSubAsset()
        {
            var subAsset = GetConversationSubAsset();
            if (subAsset is null)
            {
                subAsset = CreateInstance<ConversationAsset>();
                subAsset.name = "Conversation Asset";
                AssetDatabase.AddObjectToAsset(subAsset, Asset);
            }
            
            subAsset.ConversationSaveData.Clear();
            subAsset.ScriptableConversationDictionary.Clear();
            subAsset.ScriptableBranchDictionary.Clear();
            subAsset.SubGraphAssetDictionary.Clear();
            subAsset.TimelineAssetsDictionary.Clear();
            
            var nodes = _view.nodes.ToList();
            var count = nodes.Count;
            for (var i = 0; i < count; i++)
            {
                if (nodes[i] is BaseNode baseNode)
                {
                    baseNode.Data.NextDataIds = GetNextData(baseNode.Id);
                    var dataJson = JsonUtility.ToJson(baseNode.Data);
                    var saveData = new ConversationSaveData(baseNode.Data.GetType().FullName, dataJson);

                    if (baseNode.Data.GetType() == typeof(StartData))
                    {
                        subAsset.StartId = baseNode.Id;
                    }
                    else if (baseNode is ScriptableEventNode scriptableNode)
                    {
                        subAsset.ScriptableConversationDictionary.Add(scriptableNode.ScriptableEventData.Guid, scriptableNode.ScriptableEventData.ScriptableEvent);
                    }
                    else if (baseNode is ScriptableBranchNode scriptableBranchNode)
                    {
                        subAsset.ScriptableBranchDictionary.Add(scriptableBranchNode.ScriptableBranchData.Guid, scriptableBranchNode.ScriptableBranchData.ScriptableBranch);
                    }
                    else if (baseNode is SubGraphNode subGraphNode)
                    {
                        subAsset.SubGraphAssetDictionary.Add(subGraphNode.SubGraphData.Guid, subGraphNode.SubGraphData.SubgraphAsset);    
                    }
                    else if (baseNode is TimelineNode timelineNode)
                    {
                        subAsset.TimelineAssetsDictionary.TryAdd(timelineNode.TimelineData.AssetGuid, timelineNode.TimelineData.TimelineAsset);
                    }

                    subAsset.ConversationSaveData.Add(baseNode.Id, saveData);
                }

                Asset.SubAsset = subAsset;
            }
        }
        private ConversationAsset GetConversationSubAsset()
        {
            foreach (var asset in 
                     AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(Asset))) 
            {
                if (AssetDatabase.IsSubAsset(asset) && asset is ConversationAsset conversationAsset)
                {
                    return conversationAsset;
                }
            }

            return null;
        }

        private List<string> GetNextData(string baseNodeId)
        {
            var list = new List<string>();
            foreach (var edge in _view.edges)
            {
                var edgeData = ConversationGraphEditorUtility.EdgeToData(edge);
                if (edgeData is null) return null;

                if (edgeData.BaseNodeId == baseNodeId)
                {
                    list.AddRange(GetTargetNodeIdFromBaseNodeId(edgeData.TargetNodeId));
                }
            }

            return list;
        }

        private List<string> GetTargetNodeIdFromBaseNodeId(string targetNodeId)
        {
            var list = new List<string>();
            foreach (var node in _view.nodes)
            {
                if (node is BaseNode baseNode && baseNode.Id == targetNodeId)
                {
                    list.Add(baseNode.Id);
                }
            }

            return list;
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

        private void ShowWindow(ConversationGraphAsset conversationGraphAsset)
        {
            Asset = conversationGraphAsset;

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
