using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pioneer.DB
{
    public abstract class DbContextBase : IDbSetCache
    {
        public IDbSetCache DbSetCache => this;

        public IDbBridge DbBridge => _dbBridge;
        private IDbBridge _dbBridge;

        public ChangeDetector ChangeDetector { get; set; }

        private Dictionary<DbSetSource, IDbSet> _sets = new Dictionary<DbSetSource, IDbSet>();

        public DbContextBase() : this(new DbContextBuildOptions()) { }


        public DbContextBase()
            : this(new LocalDbBridge(this.GetType().Name))
        {
        }

        public DbContextBase(DbContextBuildOptions options)
        {
            _dbBridge = options.DbBridge;

            ChangeDetector = new ChangeDetector(this);
            new DbSetInitializer().InitializeSets(this);

            OnConfiguring(options);
        }

        public async UniTask LoadAll()
        {
            var loaded = await DbBridge.Load(_sets.Keys);
            foreach (var dbSet in _sets.Values)
            {
                dbSet.Clear();
            }

            Debug.Assert(loaded != null);
            foreach (var data in loaded)
            {
                _sets.FirstOrDefault(x => x.Key.Name == data.source.Name);
                var dbSet = GetOrAddSet(data.source);
                dbSet.CopyFrom(data.data);
            }

            ChangeDetector.Clear();
        }

        public async UniTask<int> SaveChanges(bool force = false)
        {
            IEnumerable<DbSetSource> targetSourceKeys;
            if (force)
            {
                targetSourceKeys = _sets.Keys;
            }
            else
            {
                ChangeDetector.DetectChanges();

                targetSourceKeys = _sets.Keys
                    .Where(source => ChangeDetector.IsDirty(source));
            }

            List<(DbSetSource source, IDbSet data)> sources = new List<(DbSetSource source, IDbSet data)>();
            foreach (var source in targetSourceKeys)
            {
                sources.Add((source, _sets[source]));
            }

            await DbBridge.Save(sources);
            return targetSourceKeys.Count();
        }

        public IDbSet GetOrAddSet(DbSetSource source)
        {
            if (!_sets.TryGetValue(source, out var value))
            {
                value = Activator.CreateInstance(source.DbSetType) as IDbSet;
                this.GetType().GetProperty(source.Name).SetValue(this, value);

                _sets[source] = value;
            }

            return value;
        }

        public Dictionary<DbSetSource, IDbSet> GetDbSets()
        {
            return _sets;
        }

        protected virtual void OnConfiguring(DbContextBuildOptions options) { }
    }
}
