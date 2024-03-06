using System;
using System.Collections.Generic;
using ConversationGraph.Runtime.Foundation;
using UnityEditor;
using UnityEngine;

namespace ConversationGraph.Runtime.Core
{
    public static class ConversationUtility
    {
        public static bool ShouldNext { get; set; }
        public static bool WaitForInput { get; set; }
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
               "ConversationGraph.Runtime.Foundation.SelectData"
                   => JsonUtility.FromJson<SelectData>(data.Json),
               "ConversationGraph.Runtime.Foundation.ScriptableData"
                   => JsonUtility.FromJson<ScriptableData>(data.Json),
               "ConversationGraph.Runtime.Foundation.SubGraphData"
                   => JsonUtility.FromJson<SubGraphData>(data.Json),
               _ => throw new ArgumentOutOfRangeException()
            };
        }
        public static string GetGuidByInstanceID(int instanceId)
        {
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(instanceId, out var guid, out long _);
            return guid;
        }
        public static IEnumerable<ConversationScriptAsset> GetScriptableAssets(ScriptableObject parentAsset)
        {
            foreach (var asset in 
                     AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(parentAsset))) 
            {
                if (AssetDatabase.IsSubAsset(asset) && asset is ConversationScriptAsset conversationScriptAsset)
                {
                    yield return conversationScriptAsset;
                }
            }
        }

        public static ConversationAsset GetConversationAssetByGuid(string guid)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);

            foreach (var asset in assets)
            {
                if (asset is ConversationAsset conversationAsset)
                {
                    return conversationAsset;
                }
            }

            return null;
        }
    }
}
