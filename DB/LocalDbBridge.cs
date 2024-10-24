using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pioneer.DB
{
    public class LocalDbBridge : IDbBridge
    {
        public class LocalSaveDTO
        {
            public struct Category
            {
                public string CategoryName;
                public string Value;
            }
            public List<Category> categories = new List<Category>();
        }

        public bool Encrypt { get; set; }
        private readonly string _dataPath;

        public LocalDbBridge(string fileName)
        {
            _dataPath = Path.Combine(FileUtil.GetPersistentDataPath(), fileName + ".json");
        }

        public UniTask<IEnumerable<(DbSetSource source, IDbSet data)>> Load(IEnumerable<DbSetSource> sources)
        {
            var result = new List<(DbSetSource source, IDbSet data)>();
            if (File.Exists(_dataPath) == false)
            {
                return new UniTask<IEnumerable<(DbSetSource source, IDbSet data)>>(result);
            }

            try
            {
                string json = FileUtil.ReadAllText(_dataPath, Encrypt);
                if (string.IsNullOrEmpty(json))
                {
                    return new UniTask<IEnumerable<(DbSetSource source, IDbSet data)>>(result);
                }

                var data = JsonConvert.DeserializeObject<LocalSaveDTO>(json);
                foreach (var source in sources)
                {
                    var idx = data.categories.FindIndex(x => x.CategoryName == source.Name);
                    if (idx >= 0)
                    {
                        var categoryData = data.categories[idx].Value;
                        var obj = Deserialize(categoryData, source.DbSetType);
                        result.Add((source, (IDbSet)obj));

                        Debug.Log($"Table '{source.Name}' has been loaded\n{categoryData}");
                    }
                }

                return new UniTask<IEnumerable<(DbSetSource source, IDbSet data)>>(result);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load the file located at '{_dataPath}'\n{e}");
                return new UniTask<IEnumerable<(DbSetSource source, IDbSet data)>>(result);
            }
        }

        public UniTask Save(IEnumerable<(DbSetSource source, IDbSet data)> sources)
        {
            LocalSaveDTO dto = new LocalSaveDTO();
            foreach (var source in sources)
            {
                string name = source.source.Name;
                string js = Serialize(source.data);
                dto.categories.Add(new LocalSaveDTO.Category()
                {
                    CategoryName = name,
                    Value = js
                });
                Debug.Log($"[SaveChanges - {name}] {js}");
            }

            string json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            FileUtil.WriteAllText(_dataPath, json, Encrypt);
            return new UniTask();
        }

        public UniTask Clear()
        {
            if(File.Exists(_dataPath))
            {
                File.Delete(_dataPath);
            }
            return new UniTask();
        }

        public string Serialize(IDbSet dbSet)
        {
            if (dbSet is IJson jsonSerializable)
            {
                string json = jsonSerializable.ToJson();
                // JSON 데이터를 원하는 저장 매체에 저장합니다. 여기서는 콘솔 출력으로 대체합니다.
                return json;
            }
            else
            {
                return JsonConvert.SerializeObject(dbSet);
            }
        }

        public IDbSet Deserialize(string json, Type type)
        {
            if (typeof(IJson).IsAssignableFrom(type))
            {
                IDbSet instance = (IDbSet)Activator.CreateInstance(type);
                if (instance is IJson jsonSerializable)
                {
                    jsonSerializable.FromJson(json);
                    return instance;
                }
            }

            return JsonConvert.DeserializeObject(json, type) as IDbSet;
        }
    }
}
