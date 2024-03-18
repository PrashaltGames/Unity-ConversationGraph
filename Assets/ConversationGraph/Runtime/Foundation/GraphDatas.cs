using System;
using UnityEngine;

namespace ConversationGraph.Runtime.Foundation
{
    [Serializable]
    public class NodeData
    {
        public NodeData(string id, Rect rect, string json, string typeName)
        {
            Id = string.IsNullOrEmpty(id) ?
                Guid.NewGuid().ToString("N") : id;
            Rect = rect;
            Json = json;
            TypeName = typeName;
        }

        [field:SerializeField]public string Id { get; private set; }
        [field:SerializeField]public string TypeName { get; private set; }
        [field:SerializeField]public Rect Rect { get; private set; }
        [field:SerializeField]public string Json { get; private set; }
    }
    [Serializable]
    public class EdgeData
    {
        [field:SerializeField]public string Id { get; set; }
        [field:SerializeField]public string BaseNodeId { get; set; }
        [field:SerializeField]public string TargetNodeId { get; set; }

        public EdgeData(string id, string baseNodeId, string targetNodeId)
        {
            Id = string.IsNullOrEmpty(id) ?
                Guid.NewGuid().ToString("N") : id;

            BaseNodeId = baseNodeId;
            TargetNodeId = targetNodeId;
        }
    }
}
