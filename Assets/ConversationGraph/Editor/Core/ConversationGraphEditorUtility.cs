using System;
using System.Collections.Generic;
using System.Linq;
using ConversationGraph.Editor.Foundation;
using ConversationGraph.Editor.Foundation.Nodes;
using ConversationGraph.Runtime.Foundation;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace ConversationGraph.Editor.Core
{
    public static class ConversationGraphEditorUtility
    {
        public static VisualElement CreateElementByGuid(string guid)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(assetPath);
            var result = visualTree.CloneTree();
            result.style.height = Length.Percent(100);
            return result;
        }
        
        public static T GetAssetByGuid<T>(string guid) where T : Object
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            return asset;
        }
        public static bool CheckPortEmpty(IEnumerable<Port> ports)
        {
            foreach (var port in ports)
            {
                if (!port.connected)
                {
                    return true;
                }
            }
            return false;
        }
        public static NodeData NodeToData(BaseNode node)
        {
            var id = node.Id;
            var rect = node.GetPosition();
            var json = JsonUtility.ToJson(node.Data);
            var type = node.GetType().FullName;

            return new NodeData(id, rect, json, type);
        }
        
        /// <summary>
        /// The edge convert to nodeData for save. 
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        public static EdgeData EdgeToData(Edge edge)
        {
            var targetNode = edge.input.node as BaseNode;
            var baseNode = edge.output.node as BaseNode;

            if (baseNode is null || targetNode is null) return null;
            var edgeData = new EdgeData("", baseNode.Id, targetNode.Id);
            return edgeData;
        }

        public static IEnumerable<Type> GetSubClassesByInterface<T>()
        {
            return TypeCache.GetTypesDerivedFrom<T>().AsEnumerable();
        }
    }
}
