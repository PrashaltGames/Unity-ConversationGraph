using ConversationGraph.Runtime.Foundation;
using Unity.AppUI.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using TextField = Unity.AppUI.UI.TextField;

namespace ConversationGraph.Editor.Foundation
{
    [CustomEditor(typeof(ConversationPropertyAsset))]
    public class ConversationPropertiesAssetInspector : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset _mainDocument;

        private ConversationPropertyAsset _asset;
        private ScrollView _scrollView;
        private KeyValueElement _selectedElement;

        private const int BorderWidth = 2;

        public override VisualElement CreateInspectorGUI()
        {
            _asset = (ConversationPropertyAsset)target;
            var root = _mainDocument.CloneTree();

            _scrollView = root.Q<ScrollView>(); 
            InitListView(_scrollView);
            InitStepper(root.Q<Stepper>());
            
            return root.ToAppUIElement();
        }

        private void InitListView(in ScrollView scrollView)
        {
            scrollView.Clear();
            int i = 1;
            foreach (var pair in _asset.PropertiesDictionary)
            {
                var item = new KeyValueElement();

                item.OnClick = OnSelect;
                item.Init(pair);
                
                item.Q<Text>("label").text = $"Property {i}";
                
                var keyField = item.Q<TextField>("key");
                keyField.value = pair.Key;
                keyField.RegisterValueChangedCallback(OnKeyChanged);

                var valueField = item.Q<TextField>("value");
                valueField.value = pair.Value;
                valueField.RegisterValueChangedCallback(OnValueChanged);
                
                scrollView.Add(item);
                i++;
            }
        }

        private void OnSelect(VisualElement targetElement)
        {
            foreach (var element in _scrollView.Query<VisualElement>("pair").ToList())
            {
                element.style.borderBottomWidth = 0;
                element.style.borderTopWidth = 0;
                element.style.borderRightWidth = 0;
                element.style.borderLeftWidth = 0;
            }

            var pair = targetElement.Q<VisualElement>("pair");
            pair.style.borderBottomWidth = BorderWidth;
            pair.style.borderTopWidth = BorderWidth;
            pair.style.borderRightWidth = BorderWidth;
            pair.style.borderLeftWidth = BorderWidth;
            
            _selectedElement = targetElement.Q<KeyValueElement>();
        }

        private void InitStepper(in Stepper stepper)
        {
            stepper.RegisterValueChangedCallback(e => OnStepperClick(e.newValue));
        }

        private void OnStepperClick(in int value)
        {
            if (value is 1)
            {
                TryAdd(_asset.PropertiesDictionary.Count + 1);
            }
            else if(value is -1 && _selectedElement is not null && _asset.PropertiesDictionary.Count > 1)
            {
                _asset.PropertiesDictionary.Remove(_selectedElement.Pair.Key);
            }
            InitListView(_scrollView);
            SaveAsset();
        }

        private void TryAdd(int i)
        {
            while (true)
            {
                if (!_asset.PropertiesDictionary.TryAdd($"Item{i}", ""))
                {
                    i += 1;
                    continue;
                }

                break;
            }
        }

        private void OnKeyChanged(ChangeEvent<string> e)
        {
            _asset.PropertiesDictionary.RenameKey(e.previousValue, e.newValue);
            SaveAsset();
        }

        private void OnValueChanged(ChangeEvent<string> e)
        {
            _asset.PropertiesDictionary[_selectedElement.Pair.Key] = e.newValue;
            SaveAsset();
        }

        private void SaveAsset()
        {
            EditorUtility.SetDirty(_asset);
            AssetDatabase.SaveAssets();
        }
    }
}
