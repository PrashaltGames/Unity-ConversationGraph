using UnityEditor;
using UnityEngine;

namespace ConversationGraph.Editor.Foundation
{
    public static class EditorExtensions
    {
        private const string templateGuid = "5eea233c2f50d5242a3c4fa0fb36e30b";
        private const string templateWithHistoryGuid = "b44299410e19e064394d5103f8855aeb";
        
        [MenuItem("GameObject/ConversationGraph/Template")]
        public static void CreateTemplate()
        {
            var prefabAssetPath = AssetDatabase.GUIDToAssetPath(templateGuid);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabAssetPath);
            Object.Instantiate(prefab);
        }
        
        [MenuItem("GameObject/ConversationGraph/Template with History")]
        public static void CreateTemplateWithHistory()
        {
            var prefabAssetPath = AssetDatabase.GUIDToAssetPath(templateWithHistoryGuid);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabAssetPath);
            Object.Instantiate(prefab);
        }
    }
}