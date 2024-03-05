using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ConversationGraph.Runtime.Core.Components;
using ConversationGraph.Runtime.Foundation;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConversationGraph.Runtime.Core.Base
{
    public abstract class BaseFacilitator
    {
        // for user event
        public Action OnConversationStart;
        public Action OnConversationEnd;
        public Action OnNarrator;
        public Action OnSpeaker;
        public Action OnShowSelectButtons;
        public Action OnSelected;
        public abstract void StartConversation(ConversationSystem conversationSystem);
        
        public abstract void AfterMessage(in TextMeshProUGUI text);
        public abstract void BeforeMessage(in TextMeshProUGUI text);
        public abstract UniTask OnMessage(TextMeshProUGUI speakerText, TextMeshProUGUI messageText, MessageData data,
            IReadOnlyDictionary<string, string> propertyDic, IReadingWaiter readingWaiter);
        public abstract void OnStart(in StartData data);
        public abstract void OnEnd(in EndData data);
        public abstract UniTask<int> OnSelect(SelectData data, Transform parent, Button prefab);
        public abstract void OnScriptable(in ScriptableData data);
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
        protected Dictionary<string, ConversationData> GetConversationDicFromSaveDataDic(ConversationAsset asset)
        {
            var resultDic = new Dictionary<string, ConversationData>();
            foreach (var saveData in 
                     asset.ConversationSaveData)
            {
                resultDic.Add(saveData.Key, ConversationUtility.JsonToConversationData(saveData.Value));
            }
            
            return resultDic;
        }
    }
}
