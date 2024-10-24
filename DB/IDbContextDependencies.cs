using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Pioneer.DB
{
    public interface IDbSetCache
    {
        IDbSet GetOrAddSet(DbSetSource source);
        Dictionary<DbSetSource, IDbSet> GetDbSets();
    }

    public interface IDbBridge
    {
        UniTask<IEnumerable<(DbSetSource source, IDbSet data)>> Load(IEnumerable<DbSetSource> sources);
        UniTask Save(IEnumerable<(DbSetSource source, IDbSet data)> sources);
    }

    public struct DbSetSource
    {
        public Type DbSetType { get; set; }
        public string Name { get; set; }
    }

    public interface IDbSet
    {
        void Clear();
        void CopyFrom(IDbSet other);
    }
}
