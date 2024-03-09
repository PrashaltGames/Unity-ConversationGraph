using System;
using System.Collections.Generic;
using UnityEngine;

namespace ConversationGraph.Runtime.Foundation
{
    [Serializable]
    public class SerializeReferenceDictionary<TKey, TValue> : Dictionary<TKey, TValue> , ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> _keys = new();
        [SerializeReference] private List<TValue> _values = new();
        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();

            var e = GetEnumerator();

            while (e.MoveNext())
            {
                _keys.Add(e.Current.Key);
                _values.Add(e.Current.Value);
            }
            
            e.Dispose();
        }

        public void OnAfterDeserialize()
        {
            Clear();

            var count = _keys.Count <= _values.Count ? _keys.Count : _values.Count;
            for (int i = 0; i < count; ++i)
            {
                this[_keys[i]] = _values[i];
            }
        }
    }
}