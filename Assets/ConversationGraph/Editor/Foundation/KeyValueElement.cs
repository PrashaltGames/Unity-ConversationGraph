using System;
using System.Collections.Generic;
using ConversationGraph.Editor.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConversationGraph.Editor.Foundation
{
    public class KeyValueElement : VisualElement
    {
        public Action<VisualElement> OnClick;
        private const string uiDocumentGuid = "054b52553af9bc34399813e52db02360";
        public KeyValuePair<string, string> Pair { get; private set; }
        
        public new class UxmlFactory : UxmlFactory<KeyValueElement> { }

        public KeyValueElement()
        {
            var element = ConversationGraphEditorUtility.CreateElementFromGuid(uiDocumentGuid);
            element.RegisterCallback<ClickEvent>(_ => OnClick?.Invoke(this));
            Add(element);
        }

        public void Init(KeyValuePair<string, string> pair)
        {
            Pair = pair;
        }
    }
}