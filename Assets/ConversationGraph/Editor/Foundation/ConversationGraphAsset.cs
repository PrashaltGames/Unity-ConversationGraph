using System;
using System.Collections.Generic;
using System.Linq;
using ConversationGraph.Editor.Foundation.Nodes.KeyNodes;
using ConversationGraph.Runtime.Foundation;
using ConversationGraph.Runtime.Foundation.Interfaces;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ConversationGraph.Editor.Foundation
{
    [CreateAssetMenu(menuName = "ConversationGraph")]
    public class ConversationGraphAsset : ScriptableObject
    {
        /// <summary>
        /// All nodes in this asset.
        /// </summary>
        public IReadOnlyList<NodeData> Nodes => _nodes;
        
        /// <summary>
        /// All edges in this asset.
        /// </summary>
        public IReadOnlyList<EdgeData> Edges => _edges;

        public SerializeReferenceDictionary<string, IScriptableEvent> ScriptableConversationDictionary 
            => _scriptableConversationDictionary;

        public SerializeReferenceDictionary<string, IScriptableBranch> ScriptableBranchDictionary
            => _scriptableBranchDictionary;

        public SerializedDictionary<string, PlayableAsset> PlayableAssetsDictionary
            => _playableAssetsDictionary;

        public SerializedDictionary<string, ConversationGraphAsset> SubGraphAssetDictionary
            => _subGraphAssetDictionary;
        public Action OnIsModified { get; set; }
        
        /// <summary>
        /// The first node in this asset.
        /// </summary>
        public NodeData StartNode => 
            _nodes.Find(x => x.TypeName == typeof(StartNode).FullName);
        
        /// <summary>
        /// Whether the asset has been modified.
        /// </summary>
        public bool IsModified
        {
            get => _isModified;
            set
            {
                _isModified = value;
                OnIsModified?.Invoke();
            }
        }

        public ConversationAsset SubAsset
        {
            get => _subAsset;
            set => _subAsset = value;
        }

        [SerializeField] private List<NodeData> _nodes;
        [SerializeField] private List<EdgeData> _edges;

        [SerializeField] private SerializeReferenceDictionary<string, IScriptableEvent> _scriptableConversationDictionary;
        [SerializeField] private SerializeReferenceDictionary<string, IScriptableBranch> _scriptableBranchDictionary;
        [SerializeField] private SerializedDictionary<string, ConversationGraphAsset> _subGraphAssetDictionary;
        [SerializeField] private SerializedDictionary<string, PlayableAsset> _playableAssetsDictionary;
        [SerializeField] private ConversationAsset _subAsset;

        private bool _isModified;

        /// <summary>
        /// Save the node to this asset.
        /// </summary>
        /// <param name="nodeData">Node to save.</param>
        public void SaveNode(NodeData nodeData)
        {
            var index = _nodes.FindIndex(x => x.Id == nodeData.Id);

            if (index is -1)
            {
                _nodes.Add(nodeData);
            }
            else
            {
                _nodes[index] = nodeData;
            }
        }
        
        /// <summary>
        /// Remove the node from this asset.
        /// </summary>
        /// <param name="nodeData">Node to remove.</param>
        /// <returns>Whether the node has been removed.</returns>
        public bool RemoveNode(NodeData nodeData)
        {
            var index = _nodes.FindIndex(x => x.Id == nodeData.Id);

            if (index is -1)
            {
                return false;
            }

            _nodes.RemoveAt(index);
            return true;
        }
        /// <summary>
        /// Save the edge in this asset.
        /// </summary>
        /// <param name="edgeData"></param>
        public void SaveEdge(EdgeData edgeData)
        {
            var index = _edges.FindIndex(x => x.Id == edgeData.Id);

            if (index == -1)
            {
                _edges.Add(edgeData);
            }
            else
            {
                _edges[index] = edgeData;
            }
        }
        /// <summary>
        /// Remove the edge from this asset.
        /// </summary>
        /// <param name="edgeData">Edge Asset</param>
        /// <returns>Whether the edge has been removed.</returns>
        public bool RemoveEdge(EdgeData edgeData)
        {
            var index = _edges.FindIndex(x => x.Id == edgeData.Id);

            if (index == -1)
            {
                return false;
            }

            _edges.RemoveAt(index);
            return true;
        }
        /// <summary>
        /// Clear the nodes list.
        /// </summary>
        public void ClearNodes()
        {
            _nodes.Clear();
        }
        /// <summary>
        /// Clear the edges list.
        /// </summary>
        public void ClearEdges()
        {
            _edges.Clear();
        }
        /// <summary>
        /// Find the node from this asset.
        /// </summary>
        /// <param name="nodeId">The node's guid</param>
        /// <returns>The found node</returns>
        public NodeData FindNode(string nodeId)
        {
            if (nodeId is "" || nodeId is null) return null;

            NodeData result;
            if(nodeId.Contains(":"))
            {
                result = _nodes.Find(x =>
                    x.Id == nodeId.Split(":")[1]);
            }
            else
            {
                result = _nodes.Find(x => x.Id == nodeId);
            }
            return result;
        }
        
        public List<NodeData> GetNextNode(NodeData nodeData)
        {
            var edges = Edges.Where(x => x.BaseNodeId.Split(":")[0] == nodeData.Id);
            List<NodeData> result = new();
            foreach(var edge in edges)
            {
                var nextNode = Nodes.First(x => x.Id == edge.TargetNodeId.Split(":")[0]);
                result.Add(nextNode);
            }

            return result;
        }
    }
}
