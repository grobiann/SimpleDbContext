using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Pioneer.DB
{
    public class ChangeDetector
    {
        private Dictionary<DbSetSource, bool> _dirty = new Dictionary<DbSetSource, bool>();
        private Dictionary<DbSetSource, string> _snapShot = new Dictionary<DbSetSource, string>();
        private readonly IDbSetCache _dbSetCache;

        public ChangeDetector(IDbSetCache dbSet)
        {
            _dbSetCache = dbSet;
        }

        public void SetDirty(DbSetSource source, bool dirty)
        {
            _dirty[source] = dirty;
        }

        public bool IsDirty(DbSetSource source)
        {
            if (_dirty.TryGetValue(source, out bool result))
                return result;

            return true;
        }

        public virtual void DetectChanges()
        {
            var sets = _dbSetCache.GetDbSets();
            foreach (var set in sets)
            {
                if (_snapShot.ContainsKey(set.Key) == false)
                {
                    SetDirty(set.Key, true);
                    _snapShot[set.Key] = ToSnapshot(set.Value);
                    continue;
                }

                if (IsDirty(set.Key))
                    continue;

                var prev = _snapShot[set.Key];
                var current = ToSnapshot(set.Value);
                if (string.Equals(prev, current))
                {
                    SetDirty(set.Key, true);

                    _snapShot[set.Key] = current;
                }   
            }
        }

        private string ToSnapshot(IDbSet dbSet)
        {
            return JsonUtility.ToJson(dbSet);
        }

        public void Clear()
        {
            _snapShot.Clear();
            _dirty.Clear();
        }
    }
}
