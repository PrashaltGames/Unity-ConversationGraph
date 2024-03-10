using UnityEditor;
using UnityEngine;

namespace ConversationGraph.Editor.Foundation
{
    public static class EditorExtensions
    {
        private const string templateGuid = "5eea233c2f50d5242a3c4fa0fb36e30b";
        
        [MenuItem("GameObject/ConversationGraph/Template")]
        public static void CreateTemplate()
        {
            var prefabAssetPath = AssetDatabase.GUIDToAssetPath(templateGuid);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabAssetPath);
            Object.Instantiate(prefab);
        }        
    }
}