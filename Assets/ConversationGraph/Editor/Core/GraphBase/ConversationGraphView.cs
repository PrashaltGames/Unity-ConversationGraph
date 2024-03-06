using System;
using System.Collections.Generic;
using System.Linq;
using ConversationGraph.Editor.Foundation;
using ConversationGraph.Editor.Foundation.Nodes;
using ConversationGraph.Editor.Foundation.Nodes.ConversationNode;
using ConversationGraph.Editor.Foundation.Nodes.KeyNodes;
using ConversationGraph.Editor.Foundation.Nodes.LogicNodes;
using Cysharp.Threading.Tasks;
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
                
                // else if(instance is SubGraphNode subGraphNode)
                // {
                //     var obj = JsonUtility.FromJson<SubGraphNode>(nodeData.json);
                //     subGraphNode.SetSubGraphAsset(obj.subGraph);
                // }

                AddElement(instance);
                instance.Initialize(nodeData.Id, nodeData.Rect, nodeData.Json, _window.ShowInspector);
            }
        }
        private async void ShowEdgeFromAsset(ConversationGraphAsset asset)
        {
            await UniTask.Delay(10);
            foreach (var edgeData in asset.Edges)
            {
                var baseNodeGuidWithCount = edgeData.BaseNodeId.Split(":");
                var targetNodeGuidWithCount = edgeData.TargetNodeId.Split(":");
                
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
            private ConversationGraphWindow _editorWindow;

            public void Initialize(ConversationGraphView graphView, ConversationGraphWindow editorWindow)
            {
                _graphView = graphView;
                _editorWindow = editorWindow;
            }

            List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
            {
                var entries = new List<SearchTreeEntry>
                {
                    new SearchTreeGroupEntry(new GUIContent("Create Node")),

                    new SearchTreeGroupEntry(new GUIContent("Conversation")) { level = 1 },
                    new (new GUIContent(nameof(NarratorNode))) { level = 2, userData = typeof(NarratorNode) }, 
                    new (new GUIContent(nameof(SpeakerNode))) { level = 2, userData = typeof(SpeakerNode) }, 
        //
                    new SearchTreeGroupEntry(new GUIContent("Logic")) { level = 1 },
                    new (new GUIContent(nameof(SelectNode))) { level = 2, userData = typeof(SelectNode) },
                    new (new GUIContent(nameof(ScriptableNode))) {level = 2, userData = typeof(ScriptableNode)}
        //
        //             new SearchTreeEntry(new GUIContent(nameof(BranchNode))) { level = 2, userData = typeof(BranchNode)},				
        //
                };

                return entries;
            }

            bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
            {
                var type = searchTreeEntry.userData as Type;
                var node = Activator.CreateInstance(type) as Node;
                
                // マウスの位置にノードを追加
                var worldMousePosition = _editorWindow.rootVisualElement.ChangeCoordinatesTo(_editorWindow.rootVisualElement.parent, context.screenMousePosition - _editorWindow.position.position);
                var localMousePosition = _graphView.contentViewContainer.WorldToLocal(worldMousePosition);
                var nodePosition = new Rect(localMousePosition, new Vector2(100, 100));
                node.SetPosition(nodePosition);

                if (node is BaseNode baseNode)
                {
                    baseNode.Initialize(baseNode.Id, nodePosition, "", _editorWindow.ShowInspector);
                }
                _graphView.AddElement(node);
                return true;
            }
        }
    }
}
