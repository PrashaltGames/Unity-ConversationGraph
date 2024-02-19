using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace ConversationGraph.Editor.Foundation
{
    public static class ConversationGraphEditorUtility
    {
        public const string PackageFilePath = "Assets/ConversationGraph/"
            /*"Packages/com.prashalt.unity.conversationgraph/"*/;

        public static VisualElement CreateElementFromGuid(string guid)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(assetPath);
            var result = visualTree.CloneTree();
            result.style.height = Length.Percent(100);
            return result;
        }
    }   
}
