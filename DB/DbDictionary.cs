using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pioneer.DB
{
    public class DbDictionary<TKey, TValue> : IDbSet, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable,
        IDictionary<TKey, TValue>
    {
        public Dictionary<TKey, TValue> Entities => _innerDictionary;

        public ICollection<TKey> Keys => _innerDictionary.Keys;
        public ICollection<TValue> Values => _innerDictionary.Values;
        public int Count => _innerDictionary.Count;

        protected Dictionary<TKey, TValue> _innerDictionary = new Dictionary<TKey, TValue>();

        public bool IsReadOnly => false;

        public TValue this[TKey key]
        {
            get { return _innerDictionary[key]; }
            set
            {
                _innerDictionary[key] = value;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _innerDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _innerDictionary.GetEnumerator();
        }

        public virtual void CopyFrom(IDbSet other)
        {
            if (!(other is DbDictionary<TKey, TValue> otherSet))
            {
                Debug.LogError("Fail Merge - different type");
                return;
            }

            _innerDictionary = otherSet._innerDictionary;
        }

        public virtual void Add(KeyValuePair<TKey, TValue> item)
        {
            _innerDictionary.Add(item.Key, item.Value);
        }

        public virtual bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _innerDictionary.Contains(item);
        }

        public virtual bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _innerDictionary.Remove(item.Key);
        }

        public virtual void Add(TKey key, TValue value)
        {
            _innerDictionary.Add(key, value);
        }

        public virtual bool ContainsKey(TKey key)
        {
            return _innerDictionary.ContainsKey(key);
        }

        public virtual bool Remove(TKey key)
        {
            return _innerDictionary.Remove(key);
        }

        public virtual bool TryGetValue(TKey key, out TValue value)
        {
            return _innerDictionary.TryGetValue(key, out value);
        }

        public virtual void Clear()
        {
            _innerDictionary.Clear();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)_innerDictionary).CopyTo(array, arrayIndex);
        }
    }

    public class DbDictionary<TEntity> : DbDictionary<long, TEntity>, IJson where TEntity : class, IHasPK, new()
    {
        private long maxPk;
        private readonly bool _autoIncrement;

        public DbDictionary()
        {
            var attr = (AutoIncrementKey[])(typeof(TEntity).GetCustomAttributes(typeof(AutoIncrementKey), false));
            _autoIncrement = attr.Length > 0;
        }

        public TEntity GetOrCreate(long key)
        {
            if (TryGetValue(key, out TEntity entity) == false)
            {
                entity = new TEntity();
                entity.PK = key;
                _innerDictionary.Add(key, entity);
            }

            return entity;
        }

        public void Add(TEntity entity)
        {
            if (_autoIncrement && IsKeyAssigned(entity.PK) == false)
            {
                maxPk++;
                entity.PK = maxPk;
            }
            _innerDictionary.Add(entity.PK, entity);
        }

        public override void Add(KeyValuePair<long, TEntity> item)
        {
            if (_autoIncrement && IsKeyAssigned(item.Key) == false)
            {
                maxPk++;
                item = new KeyValuePair<long, TEntity>(maxPk, item.Value);
            }
            base.Add(item);
        }

        public override void Add(long key, TEntity value)
        {
            if (_autoIncrement && IsKeyAssigned(key) == false)
            {
                maxPk++;
                key = maxPk;
            }

            base.Add(key, value);
        }

        public override void CopyFrom(IDbSet other)
        {
            if (!(other is DbDictionary<TEntity> otherSet))
            {
                Debug.LogError("Fail Merge - different type");
                return;
            }

            _innerDictionary = otherSet._innerDictionary;
            if (_innerDictionary.Count > 0)
            {
                maxPk = _innerDictionary.Keys.Max();
            }
        }

        private bool IsKeyAssigned(long key)
        {
            return key > 0;
        }

        public string ToJson()
        {
            List<TEntity> list = Values.ToList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }

        public void FromJson(string json)
        {
            _innerDictionary.Clear();
            var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TEntity>>(json);
            foreach (var item in list)
            {
                _innerDictionary.Add(item.PK, item);
            }
        }
    }
}
