using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ConversationGraph.Runtime.Foundation;
using ConversationGraph.Runtime.Foundation.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ConversationGraph.Runtime.Core.Facilitators
{
    public class Facilitator
    {
        public Action<MessageData> AfterMessageEvent { get; set; }
        
        private readonly IConversationView _view;
        private readonly IConversationEvents _events;

        private readonly ConversationAsset _asset;
        private readonly Dictionary<string, ConversationData> _conversationDataDic;
        private Dictionary<string, string> _conversationPropertiesDic;

        public Facilitator(
            ConversationAsset asset, 
            IConversationView view,
            IConversationEvents events
            )
        {
            _asset = asset;
            _conversationDataDic = ConversationUtility.GetConversationDicFromSaveDataDic(asset.ConversationSaveData);

            _view = view;
            _events = events;
        }
        
        public async UniTask Facilitate()
        {
            var isEnd = false;
            var id = _asset.StartId;
            
            while (true)
            {
                var data = _conversationDataDic[id];
                var nextIndex = 0;

                switch (data)
                {
                    case MessageData messageData:
                        await OnMessage(messageData);
                        break;
                    case SelectData selectData:
                        nextIndex = await OnSelect(selectData);
                        break;
                    case ScriptableEventData scriptableData:
                        OnScriptable(scriptableData);
                        break;
                    case ScriptableBranchData scriptableBranchData:
                        nextIndex = OnScriptableBranch(scriptableBranchData);
                        break;
                    case SubGraphData subGraphData:
                        OnSubGraph(subGraphData);
                        break;
                    case StartData startData:
                        OnStart(startData);
                        break;
                    case EndData endData:
                        OnEnd(endData);
                        isEnd = true;
                        break;
                }
                
                // if now eventData is End Data, finish conversation.
                if (isEnd)
                {
                    break;
                }
                
                if (data.TryGetNextDataIds(out var nextDataIds))
                {
                    id = nextDataIds[nextIndex];
                }
#if UNITY_EDITOR
                else
                {
                    Debug.Log("正しく次のデータが取得できませんでした。");                    
                }
#endif   
            }
        }

        public void AfterMessage(MessageData data)
        {
            AfterMessageEvent?.Invoke(data);
        }

        public void BeforeMessage()
        {
            
        }

        private async UniTask OnMessage(MessageData data)
        {
            BeforeMessage();
            
            if (string.IsNullOrEmpty(data.Speaker))
            {
                _events.OnNarrator();
            }
            else
            {
                _events.OnSpeaker();
                var speakerText = ReflectProperty(data.Speaker, _asset.ConversationPropertyAsset.PropertiesDictionary);
                _view.ChangeSpeaker(speakerText);
            }
            
            foreach (var message in data.MessageList)
            {
                await _view.ChangeMessage(ReflectProperty(message, _asset.ConversationPropertyAsset.PropertiesDictionary), data.AnimationData);
            }
            
            AfterMessage(data);
        }

        private void OnStart(in StartData data)
        {
            _events.OnConversationStart();
            _view.ChangeTitle(ReflectProperty(data.Title, _conversationPropertiesDic));
        }

        private void OnEnd(in EndData _)
        {
            _events.OnConversationEnd();
        }

        private async UniTask<int> OnSelect(SelectData data)
        {
            var index = 0;
            
            foreach (var text in data.SelectTexts)
            {
                var i = index;

                Action selectAction = () =>
                {
                    index = i;
                };

                _view.AddSelect(text, selectAction);
                
                index++;
            }
            await _view.WaitSelect();

            return index;
        }

        private void OnScriptable(in ScriptableEventData eventData)
        {
            _asset.ScriptableConversationDictionary[eventData.Guid].OnArrival();
        }

        private int OnScriptableBranch(in ScriptableBranchData data)
        {
            var result = _asset.ScriptableBranchDictionary[data.Guid].OnArrival();
            return result;
        }

        private void OnSubGraph(in SubGraphData data)
        {
            var subGraph = _asset.SubGraphAssetDictionary[data.Guid];
            var facilitator = new Facilitator(subGraph, _view, _events);
            facilitator.Facilitate().Forget();
        }
        private string ReflectProperty(string text, in IReadOnlyDictionary<string, string> properties)
        {
            if (string.IsNullOrEmpty(text)) return "";
            var matches = new Regex(@"\{(.+?)\}").Matches(text);
            
            foreach (Match propertyNameMatch in matches)
            {
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
    }
}
