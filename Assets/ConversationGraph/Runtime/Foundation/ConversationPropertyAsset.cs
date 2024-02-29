using System.Collections.Generic;
using UnityEngine;

namespace ConversationGraph.Runtime.Foundation
{
    [CreateAssetMenu]
    public class ConversationPropertyAsset : ScriptableObject
    {
        public Dictionary<string, string> PropertiesDictionary => _propertiesDic;
        [SerializeField] private SerializedDictionary<string, string> _propertiesDic = new SerializedDictionary<string, string>
        {
            {"Player", "Roto"},
            {"CirKit", "SAKITO"}
        };
    }

    public static class DictionaryExtension
    {
        public static void RenameKey<TKey, TValue>(this IDictionary<TKey, TValue> dic,
            TKey previousKey, TKey newKey)
        {
            if(previousKey is null) return;
            
            if (dic.TryGetValue(previousKey, out var value) && value is not null)
            {
                dic.Remove(previousKey);
                dic[newKey] = value;   
            }
        }
    }
}
