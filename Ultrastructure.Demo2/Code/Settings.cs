using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultrastructure.Demo2.Code
{
    public class Settings : IDictionary<string, string>
    {
        private readonly Dictionary<string, string> _data;

        public Settings(IDictionary<string, string> source) { _data = new Dictionary<string, string>(source); }

        public string this[string key]
        {
            get
            {
                if (_data.ContainsKey(key)) { return _data[key]; }
                throw new Exception(String.Format("Could not find key {0} in settings.", key));
            }

            set
            {
                _data[key] = value;
            }
        }

        public int Count
        {
            get
            {
                return _data.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((IDictionary<string, string>)_data).IsReadOnly;
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                return ((IDictionary<string, string>)_data).Keys;
            }
        }

        public ICollection<string> Values
        {
            get
            {
                return ((IDictionary<string, string>)_data).Values;
            }
        }

        public void Add(KeyValuePair<string, string> item)
        {
            ((IDictionary<string, string>)_data).Add(item);
        }

        public void Add(string key, string value)
        {
            _data.Add(key, value);
        }

        public void Clear()
        {
            _data.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return ((IDictionary<string, string>)_data).Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _data.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            ((IDictionary<string, string>)_data).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return ((IDictionary<string, string>)_data).GetEnumerator();
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            return ((IDictionary<string, string>)_data).Remove(item);
        }

        public bool Remove(string key)
        {
            return _data.Remove(key);
        }

        public bool TryGetValue(string key, out string value)
        {
            return _data.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<string, string>)_data).GetEnumerator();
        }
    }
}