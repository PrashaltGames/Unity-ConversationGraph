using System.Collections.Generic;
using ConversationGraph.Editor.Foundation;
using ConversationGraph.Editor.Foundation.Nodes;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;

namespace ConversationGraph.Editor.Core.GraphBase
{
    public class ConversationGraphWindow : EditorWindow
    {
        #region Properties
        
        /// <summary>
        /// Asset for this window.
        /// </summary>
        public ConversationGraphAsset Asset { get; set; }
        
        #endregion

        #region Static_Variables
        /// <summary>
        /// List for All active windows.
        /// </summary>
        private static List<ConversationGraphWindow> _activeWindowList = new();
        
        #endregion
        #region Methods

        public void ShowInspector(BaseNode node)
        {
            var window = GetWindow<ConversationGraphInspector>();
            window.Show();
            window.SelectedNode = node;
        }
        private async void OnEnable()
        {
            rootVisualElement.Clear();
            
            await UniTask.WaitUntil(() => Asset is not null);
            var graphBase = new ConversationGraphView(this);
            rootVisualElement.Add(graphBase);

            var toolBar = new Toolbar();
            var saveButton = new ToolbarButton(OnSave) { text = "Save", name = "save-button" };
            toolBar.Add(saveButton);
            rootVisualElement.Add(toolBar);
        }

        private void OnSave()
        {
            Debug.Log("Save");
        }
        
        #endregion

        #region Static_Methods
        
        [OnOpenAsset()]
        private static bool ShowOpenAsset(int instanceID, int _)
        {
            if (EditorUtility.InstanceIDToObject(instanceID) is ConversationGraphAsset asset)
            {
                if (HasOpenInstances<ConversationGraphWindow>())
                {
                    foreach (var window in _activeWindowList)
                    {
                        if (window.Asset.GetInstanceID() == asset.GetInstanceID())
                        {
                            window.Focus();
                            return false;
                        }
                    }
                }

                CreateNewWindow(asset);
                return true;
            }

            return true;
        }
        
        #endregion

        private void ShowWindow(ConversationGraphAsset convasationGraphAsset)
        {
            Asset = convasationGraphAsset;

            Show();
        }

        #region Utility
        private static void CreateNewWindow(ConversationGraphAsset conversationGraphAsset)
        {
            var newWindow = CreateWindow<ConversationGraphWindow>(typeof(SceneView));

            newWindow.ShowWindow(conversationGraphAsset);
            newWindow.titleContent = new(conversationGraphAsset.name);
            newWindow.Focus();

            _activeWindowList.Add(newWindow);
        }
        #endregion
    }
}
