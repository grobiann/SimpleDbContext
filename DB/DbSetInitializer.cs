using System;
using System.Collections.Generic;

namespace Pioneer.DB
{
    public class DbSetInitializer
    {
        public void InitializeSets(IDbSetCache dbSetCache)
        {
            var dbSetSources = FindDbSets(dbSetCache);
            foreach (var source in dbSetSources)
            {
                dbSetCache.GetOrAddSet(source);
            }
        }

        private List<DbSetSource> FindDbSets(IDbSetCache dbSetCache)
        {
            var properties = dbSetCache.GetType().GetProperties();
            var dbSetType = typeof(IDbSet);
            List<DbSetSource> sources = new List<DbSetSource>();
            foreach (var property in properties)
            {
                if (dbSetType.IsAssignableFrom(property.PropertyType) == false)
                    continue;

                sources.Add(new DbSetSource()
                {
                    Name = property.Name,
                    DbSetType = property.PropertyType,
                });
            }
            return sources;
        }
    }
}
