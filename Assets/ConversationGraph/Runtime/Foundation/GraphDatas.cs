using System;
using UnityEngine;

namespace ConversationGraph.Runtime.Foundation
{
    [Serializable]
    public class NodeData
    {
        public string Id { get; set; }
        public string TypeName { get; set; }
        public Rect Rect { get; set; }
        public string Json { get; set; }
    }
    [Serializable]
    public class EdgeData
    {
        public string Id;
        public string BaseNodeId;
        public string TargetNodeId;

        public EdgeData(string id, string baseNodeId, string targetNodeId)
        {
            if(id == null || id == "")
            {
                this.Id = System.Guid.NewGuid().ToString("N");
            }
            else
            {
                this.Id = id;
            }

            BaseNodeId = baseNodeId;
            TargetNodeId = targetNodeId;
        }
    }
}
