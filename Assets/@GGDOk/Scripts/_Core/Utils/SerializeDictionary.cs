using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Utils
{
    [Serializable]
    public class SerializeDictionary<TK,TV> : IEnumerable<KeyValuePair<TK,TV>>
    {
        [SerializeField]private List<TK> _keys = new();
        [SerializeField]private List<TV> _values = new();
        
        public void Add(TK key, TV value)
        {
            if (_keys.Contains(key))
                return;
            _keys.Add(key);
            _values.Add(value);
        }

        public int Count => _keys.Count;

        public void Clear()
        {
            _keys.Clear();
            _values.Clear();
        }
        
        public List<TK> Keys => _keys;
        public List<TV> Values => _values;

        public void Remove(TK key)
        {
            int index = _keys.IndexOf(key);
            if (index != -1)
            {
                _keys.RemoveAt(index);
                _values.RemoveAt(index);
            }
        }
        
        public bool ContainsKey(TK key)
        {
           return _keys.Contains(key);
        }
        public bool ContainsValue(TV value)
        {
            return _values.Contains(value);
        }
        public bool TryAdd(TK key, TV value)
        {
            if (_keys.Contains(key))
            {
                return false;
            }
            Add(key, value);
            return true;
        }
        public bool TryGetValue(TK key, out TV value)
        {
            if (_keys.Contains(key))
            {
                value = default;
                return false;
            }
            value = _values[_keys.IndexOf(key)];
            return true;
        }
        public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator()
        {
            for (int i = 0; i < _keys.Count; i++)
            {
                yield return new KeyValuePair<TK, TV>(_keys[i], _values[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < _keys.Count; i++)
            {
                yield return new KeyValuePair<TK, TV>(_keys[i], _values[i]);
            }
        }
    }
}