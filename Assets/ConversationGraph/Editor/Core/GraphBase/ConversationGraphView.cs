using System;
using System.Collections.Generic;
using System.Linq;
using ConversationGraph.Editor.Foundation;
using ConversationGraph.Editor.Foundation.Nodes;
using ConversationGraph.Editor.Foundation.Nodes.ConversationNode;
using ConversationGraph.Editor.Foundation.Nodes.KeyNodes;
using ConversationGraph.Editor.Foundation.Nodes.LogicNodes;
using ConversationGraph.Editor.Foundation.Nodes.TimelineNodes;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConversationGraph.Editor.Core.GraphBase
{
    public class ConversationGraphView : GraphView
    {
        private ConversationGraphWindow _window;
        public ConversationGraphView(ConversationGraphWindow window)
        {
            _window = window;
            
            // Set size
            this.StretchToParentSize();
            
            // Set Controls
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            // Add right menu.
            var menuWindowProvider = ScriptableObject.CreateInstance<SearchMenuWindowProvider>();
            menuWindowProvider.Initialize(this, window);
            nodeCreationRequest += ctx =>
            {
                SearchWindow.Open(new SearchWindowContext(ctx.screenMousePosition), menuWindowProvider);
            };
            
            if (window.Asset is null || window.Asset.Nodes.Count <= 0)
            {
                var startNode = new StartNode();
                AddElement(startNode);
                var endNode = new EndNode();
                AddElement(endNode);

                startNode.Initialize(null, 
                    new Rect(100f, 150f, default, default), "", window.ShowInspector);
                endNode.Initialize(null, 
                    new Rect(800f, 150f, default, default), "", window.ShowInspector);
            }
            else
            {
                ShowNodesFromAsset(window.Asset);
                ShowEdgeFromAsset(window.Asset);
            }
            
            //グラフビューの変更を検知する
            graphViewChanged += OnGraphViewChanged;
        }
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            compatiblePorts.AddRange(ports.ToList().Where(port =>
            {
                // 同じノードには繋げない
                if (startPort.node == port.node)
                    return false;

                // Input同士、Output同士は繋げない
                if (port.direction == startPort.direction)
                    return false;

                //ポートの型が継承されていなければ繋げない
                if(!startPort.portType.IsSubclassOf(port.portType) && startPort.portType != port.portType)
                    return false;

                return true;
            }));

            return compatiblePorts;
        }
        
        private void ShowNodesFromAsset(ConversationGraphAsset asset)
        {
            foreach(var nodeData in asset.Nodes)
            {
                var t = Type.GetType(nodeData.TypeName);
                if (t is null) continue;

                var instance = Activator.CreateInstance(t) as BaseNode;
                if (instance is null) continue;

                AddElement(instance);
                instance.Initialize(nodeData.Id, nodeData.Rect, nodeData.Json, _window.ShowInspector);

                if (instance is ScriptableEventNode scriptableNode)
                {
                    var scriptableConversation = asset.ScriptableConversationDictionary[scriptableNode.ScriptableEventData.Guid];
                    scriptableNode.SetScript(scriptableConversation);
                }
                else if (instance is ScriptableBranchNode scriptableBranchNode)
                {
                    var scriptableBranch = asset.ScriptableBranchDictionary[scriptableBranchNode.ScriptableBranchData.Guid];
                    scriptableBranchNode.SetScript(scriptableBranch);
                }
                else if (instance is SubGraphNode subGraphNode)
                {
                    var subGraphAsset = asset.SubGraphAssetDictionary[subGraphNode.SubGraphData.Guid];
                    subGraphNode.SetSubGraphAsset(subGraphAsset);
                }
            }
        }
        private async void ShowEdgeFromAsset(ConversationGraphAsset asset)
        {
            await UniTask.Delay(10);
            foreach (var edgeData in asset.Edges)
            {
                // Split Id and Count.
                var baseNodeGuidWithCount = edgeData.BaseNodeId.Split(":");
                var targetNodeGuidWithCount = edgeData.TargetNodeId.Split(":");
                
                // Get node from Id.
                var baseNode = nodes.Select(x => x as BaseNode)
                    .FirstOrDefault(x => x?.Id == baseNodeGuidWithCount[0]);
                var targetNode = nodes.Select(x => x as BaseNode)
                    .FirstOrDefault(x => x?.Id == targetNodeGuidWithCount[0]);
                
                if (baseNode is null || targetNode is null) return;
                
                var input = targetNode.inputContainer.Children().Where(x => x is Port).ElementAt(targetNodeGuidWithCount.Length == 1 ? 0 : int.Parse(targetNodeGuidWithCount[1])) as Port;
                var output = baseNode.outputContainer.Children().Where(x => x is Port).ElementAt(baseNodeGuidWithCount.Length == 1 ? 0 : int.Parse(baseNodeGuidWithCount[1])) as Port;

                var edge = new Edge { input = input, output = output };
                edge.input?.Connect(edge);
                edge.output?.Connect(edge);
                Add(edge);
            }
            
        }
        class SearchMenuWindowProvider : ScriptableObject, ISearchWindowProvider
        {
            private ConversationGraphView _graphView;
            private ConversationGraphWindow _window;

            public void Initialize(ConversationGraphView graphView, ConversationGraphWindow editorWindow)
            {
                _graphView = graphView;
                _window = editorWindow;
            }

            List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
            {
                var entries = new List<SearchTreeEntry>
                {
                    new SearchTreeGroupEntry(new GUIContent("Create Node")),

                    new SearchTreeGroupEntry(new GUIContent("Basic")) { level = 1 },
                    new (new GUIContent(nameof(NarratorNode))) { level = 2, userData = typeof(NarratorNode) }, 
                    new (new GUIContent(nameof(SpeakerNode))) { level = 2, userData = typeof(SpeakerNode) }, 
                    new (new GUIContent(nameof(SelectNode))) { level = 2, userData = typeof(SelectNode) },
                    
                    new SearchTreeGroupEntry(new GUIContent("Scriptable")) { level = 1 },
                    new (new GUIContent(nameof(ScriptableEventNode))) {level = 2, userData = typeof(ScriptableEventNode)},
                    new (new GUIContent(nameof(ScriptableBranchNode))) {level = 2, userData = typeof(ScriptableBranchNode)},
                    
                    new SearchTreeGroupEntry(new GUIContent("Other")) {level = 1},
                    new (new GUIContent(nameof(TimelineNode))) {level = 2, userData = typeof(TimelineNode)}
                };

                return entries;
            }

            bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
            {
                var type = searchTreeEntry.userData as Type;
                var node = Activator.CreateInstance(type) as Node;
                
                // マウスの位置にノードを追加
                var worldMousePosition = _window.rootVisualElement.ChangeCoordinatesTo(_window.rootVisualElement.parent, context.screenMousePosition - _window.position.position);
                var localMousePosition = _graphView.contentViewContainer.WorldToLocal(worldMousePosition);
                var nodePosition = new Rect(localMousePosition, new Vector2(100, 100));
                node.SetPosition(nodePosition);

                if (node is BaseNode baseNode)
                {
                    baseNode.Initialize(baseNode.Id, nodePosition, "", _window.ShowInspector);
                }
                _graphView.AddElement(node);
                return true;
            }
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange e)
        {
            _window.Asset.IsModified = true;
            return e;
        }

        public bool DropSubGraph()
        {
            if (!contentRect.Contains(Event.current.mousePosition))
            {
                return false;
            }

            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            var eventType = Event.current.type;
            if (eventType != EventType.DragExited)
            {
                return false;
            }
            
            DragAndDrop.AcceptDrag();
            Event.current.Use();

            var subGraphReferences = 
                DragAndDrop.objectReferences.OfType<ConversationGraphAsset>().ToList();
            if (subGraphReferences.Count <= 0)
            {
                return false;
            }

            if (subGraphReferences[0].GetInstanceID() == _window.Asset.GetInstanceID())
            {
                return false;
            }

            var asset = subGraphReferences[0];
            var subGraphNode = new SubGraphNode();
            subGraphNode.SubGraphAsset = asset;
            
            var worldMousePosition = _window.rootVisualElement.ChangeCoordinatesTo(_window.rootVisualElement.parent, GUIUtility.GUIToScreenPoint(Event.current.mousePosition) - _window.position.position);
            var localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
            var nodePosition = new Rect(localMousePosition, new Vector2(100, 100));

            subGraphNode.Initialize(subGraphNode.Id, nodePosition, "", _window.ShowInspector);
            
            AddElement(subGraphNode);

            return true;
        }
    }
}
