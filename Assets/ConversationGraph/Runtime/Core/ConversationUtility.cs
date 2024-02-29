using System;
using ConversationGraph.Runtime.Foundation;
using UnityEngine;

namespace ConversationGraph.Runtime.Core
{
    public static class ConversationUtility
    {
        public static ConversationData JsonToConversationData(ConversationSaveData data)
        {
            return data.TypeName switch
            {
               "ConversationGraph.Runtime.Foundation.StartData"
                   => JsonUtility.FromJson<StartData>(data.Json),
               "ConversationGraph.Runtime.Foundation.EndData"
                   => JsonUtility.FromJson<EndData>(data.Json),
               "ConversationGraph.Runtime.Foundation.MessageData"
                   => JsonUtility.FromJson<MessageData>(data.Json),
               _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
