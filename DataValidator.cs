using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pioneer
{
    public class DataValidator
    {
        private List<LogInfo> List_LogInfo = new List<LogInfo>();
        private int minSize = 128;
        private int maxSize = 512;
        private int logListSize;
        private Func<int, long> currentValueGetter;
        private Dictionary<int, long> _cachedDic = new Dictionary<int, long>();
        private HashSet<int> idCollection = new HashSet<int>();
        public event Action DetectAbuse;

        public DataValidator(Func<int, long> valueGetter)
        {
            currentValueGetter = valueGetter;
            Resize();
        }

        public void Initialize(Dictionary<int, long> initItems)
        {
            foreach (var item in initItems)
            {
                var key = item.Key;
                var amount = item.Value;
                AddChangeLog(key, amount);
            }
        }

        public void AddChangeLog(int id, long value)
        {
            var log = new LogInfo(id, value);
            List_LogInfo.Add(log);
            bool newId = idCollection.Add(id);
            if (newId)
            {
                Resize();
            }

            if (List_LogInfo.Count >= logListSize)
            {
                Validate();
            }
        }

        private void Resize()
        {
            int idCount = idCollection.Count;
            logListSize = Mathf.Clamp(idCount * 30, minSize, maxSize);
        }

        public bool Validate()
        {
            _cachedDic.Clear();
            foreach (var log in List_LogInfo)
            {
                var id = log.identifier;
                var value = log.num_long;
                if (_cachedDic.ContainsKey(log.identifier) == false)
                {
                    _cachedDic.Add(id, value);
                }
                else
                {
                    _cachedDic[id] += value;
                }
            }

            List_LogInfo.Clear();
            foreach (var item in _cachedDic)
            {
                var current = currentValueGetter.Invoke(item.Key);
                if (current != item.Value)
                {
                    DetectAbuse?.Invoke();
                    return false;
                }

                if (item.Value == 0)
                {
                    idCollection.Remove(item.Key);
                    continue;
                }

                var log = new LogInfo(item.Key, item.Value);
                List_LogInfo.Add(log);
            }

            Resize();
            return true;
        }

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(List_LogInfo);
        }
    }
}
