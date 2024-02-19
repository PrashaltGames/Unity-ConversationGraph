using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ConversationGraph.Editor.Foundation.Nodes
{
    public abstract class BaseNode : Node
    {
        public string Id { get; private set; }
        private Action<BaseNode> _onSelect;
        public BaseNode()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        public BaseNode(string id)
        {
            Id = id;
        }

        public override void OnSelected()
        {
            _onSelect.Invoke(this);
        }

        public virtual string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public virtual void Initialize(string id, Rect rect, string json, Action<BaseNode> onSelect)
        {
            if (string.IsNullOrEmpty(id))
            {
                Id = Guid.NewGuid().ToString();
            }
            else
            {
                Id = id;
            }
            SetPosition(rect);

            _onSelect += onSelect;
        }
        
        #region Utility
        
        /// <summary>
        /// Add Input Port in this node.
        /// </summary>
        /// <param name="portName">Port portName</param>
        /// <param name="capacity">Port capacity</param>
        /// <param name="portType">Port type</param>
        /// <returns>Added Port</returns>
        protected Port AddInputPort(string portName, Port.Capacity capacity, Type portType)
        {
            var result =  Port.Create<Edge>(
                Orientation.Horizontal,
                Direction.Input,
                capacity,
                portType
            );
            result.name = portName;
            
            inputContainer.Add(result);
            return result;
        }
        
        /// <summary>
        /// Add Output Port in this node.
        /// </summary>
        /// <param name="portName">Port portName</param>
        /// <param name="capacity">Port capacity</param>
        /// <param name="portType">Port type</param>
        /// <returns>Added Port</returns>
        protected Port AddOutputPort(string portName, Port.Capacity capacity, Type portType)
        {
            var result =  Port.Create<Edge>(
                Orientation.Horizontal,
                Direction.Output,
                capacity,
                portType
            );
            result.name = portName;
            
            outputContainer.Add(result);
            return result;
        }
        
        #endregion
    }   
}
