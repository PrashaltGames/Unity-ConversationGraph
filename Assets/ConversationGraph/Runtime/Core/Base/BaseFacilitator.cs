using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ConversationGraph.Runtime.Core.Interfaces;
using ConversationGraph.Runtime.Foundation;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConversationGraph.Runtime.Core.Base
{
    public abstract class BaseFacilitator : IFacilitator, IReadingWaiter
    {
        public Action OnConversationStart;
        public Action OnConversationEnd;
        public Action OnNarrator;
        public Action OnSpeaker;
        public Action OnShowSelectButtons;
        public Action OnSelected;
        public abstract void StartConversation(string startId, TextMeshProUGUI titleText, TextMeshProUGUI speakerText,
            TextMeshProUGUI messageText, IReadOnlyDictionary<string, ConversationData> dataDic,
            IReadOnlyDictionary<string, string> propertyDic, Transform selectParent, Button selectPrefab);

        public abstract void StartConversation(in string startId, in TextMeshProUGUI titleText, in TextMeshProUGUI speakerText, in TextMeshProUGUI messageText, in IReadOnlyDictionary<string, ConversationData> dataDic, in ConversationPropertyAsset propertyAsset, in Transform selectParent, in Button selectPrefab);
        
        public abstract void AfterMessage(in TextMeshProUGUI text);
        public abstract void BeforeMessage(in TextMeshProUGUI text);
        public abstract UniTask OnMessage(TextMeshProUGUI speakerText, TextMeshProUGUI messageText, MessageData data, IReadOnlyDictionary<string, string> propertyDic);
        public abstract void OnStart(in StartData data);
        public abstract void OnEnd(in EndData data);
        public abstract UniTask<string> OnSelect(SelectData data, Transform parent, Button prefab);

        protected string ReflectProperty(string text, in IReadOnlyDictionary<string, string> properties)
        {
            if (string.IsNullOrEmpty(text)) return "";
            var matches = new Regex(@"\{(.+?)\}").Matches(text);
            
            foreach (Match propertyNameMatch in matches)
            {
                //正規表現分からないので、ゴリ押す
                var propertyName = propertyNameMatch.Value.Replace("{", "");
                propertyName = propertyName.Replace("}", "");

                var hasProperty = properties.TryGetValue(propertyName, out var value);
                if (!hasProperty)
                {
                    #if UNITY_EDITOR
                    Debug.LogError("Property is missing maybe");
                    #endif
                    continue;
                }
                text = text.Replace($"{{{propertyName}}}", value);
            }
            return text;
        }
        public abstract UniTask WaitReading();
    }
}
