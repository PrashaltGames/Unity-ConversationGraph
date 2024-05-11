using System;
using System.Collections.Generic;
using ConversationGraph.Runtime.Foundation;
using UnityEngine;

namespace ConversationGraph.Runtime.Core
{
    public static class ConversationUtility
    {
        public static bool ShouldNext { get; set; }
        public static bool WaitForInput { get; set; }
        
        public static Dictionary<string, ConversationData> GetConversationDicFromSaveDataDic(IReadOnlyDictionary<string, ConversationSaveData> saveDatas)
        {
            var resultDic = new Dictionary<string, ConversationData>();
            foreach (var saveData in saveDatas)
            {
                resultDic.Add(saveData.Key, JsonToConversationData(saveData.Value));
            }
            
            return resultDic;
        }
        private static ConversationData JsonToConversationData(ConversationSaveData data)
        {
            return data.TypeName switch
            {
               "ConversationGraph.Runtime.Foundation.StartData"
                   => JsonUtility.FromJson<StartData>(data.Json),
               "ConversationGraph.Runtime.Foundation.EndData"
                   => JsonUtility.FromJson<EndData>(data.Json),
               "ConversationGraph.Runtime.Foundation.MessageData"
                   => JsonUtility.FromJson<MessageData>(data.Json),
               "ConversationGraph.Runtime.Foundation.SelectData"
                   => JsonUtility.FromJson<SelectData>(data.Json),
               "ConversationGraph.Runtime.Foundation.TimelineData"
                   => JsonUtility.FromJson<TimelineData>(data.Json),
               "ConversationGraph.Runtime.Foundation.ScriptableEventData"
                   => JsonUtility.FromJson<ScriptableEventData>(data.Json),
               "ConversationGraph.Runtime.Foundation.SubGraphData"
                   => JsonUtility.FromJson<SubGraphData>(data.Json),
               "ConversationGraph.Runtime.Foundation.ScriptableBranchData"
                   => JsonUtility.FromJson<ScriptableBranchData>(data.Json),
               _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
