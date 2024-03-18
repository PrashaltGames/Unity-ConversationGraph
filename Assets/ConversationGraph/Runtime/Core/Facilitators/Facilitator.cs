using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ConversationGraph.Runtime.Core.Interfaces;
using ConversationGraph.Runtime.Foundation;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ConversationGraph.Runtime.Core.Facilitators
{
    public class Facilitator
    {
        private readonly IConversationView _view;
        private readonly IConversationEvents _events;

        private readonly ConversationAsset _asset;
        private readonly Dictionary<string, ConversationData> _conversationDataDic;
        private Dictionary<string, string> _conversationPropertiesDic;

        public Facilitator(
            ConversationAsset asset, 
            IConversationView view,
            IConversationEvents events)
        {
            _asset = asset;
            _conversationDataDic = GetConversationDicFromSaveDataDic(asset.ConversationSaveData);

            _view = view;
            _events = events;
        }
        public async void Facilitate()
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
                    case ScriptableData scriptableData:
                        OnScriptable(scriptableData);
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
                
                // if now data is End Data, finish conversation.
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

        public void AfterMessage()
        {
            
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
                await _view.ChangeMessage(ReflectProperty(message, _asset.ConversationPropertyAsset.PropertiesDictionary));
            }
            
            AfterMessage();
        }

        private void OnStart(in StartData data)
        {
            _view.ChangeTitle(ReflectProperty(data.Title, _conversationPropertiesDic));
        }

        private void OnEnd(in EndData data)
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

        private void OnScriptable(in ScriptableData data)
        {
            data.ScriptableConversation.OnArrival();
        }

        private void OnSubGraph(in SubGraphData data)
        {
            var subGraph = _asset.SubGraphAssetDictionary[data.Guid];
            var facilitator = new Facilitator(subGraph, _view, _events);
            facilitator.Facilitate();
        }
        private string ReflectProperty(string text, in IReadOnlyDictionary<string, string> properties)
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
        protected Dictionary<string, ConversationData> GetConversationDicFromSaveDataDic(IReadOnlyDictionary<string, ConversationSaveData> saveDatas)
        {
            var resultDic = new Dictionary<string, ConversationData>();
            foreach (var saveData in saveDatas)
            {
                resultDic.Add(saveData.Key, ConversationUtility.JsonToConversationData(saveData.Value));
            }
            
            return resultDic;
        }
    }
}
