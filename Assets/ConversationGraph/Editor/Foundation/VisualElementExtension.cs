using UnityEditor;
using UnityEngine.UIElements;

namespace ConversationGraph.Editor.Foundation
{
    public static class VisualElementExtension
    {
        private const string TSSGuid = "dc39b1949c0d08c4b93d17de7fb085d0";
        
        public static VisualElement ToAppUIElement(this VisualElement root)
        {
            var tssAsset = AssetDatabase.GUIDToAssetPath(TSSGuid);
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<ThemeStyleSheet>(tssAsset));
            root.AddToClassList("appui--editor-dark");

            return root;
        }
    }
}