namespace Pioneer.DB
{
    [System.Serializable]
    public class DbSetSingle<TEntity> : IDbSet, IJson where TEntity : class, new()
    {
        public TEntity Entity { get; set; } = new TEntity();

        public void Clear()
        {
            Entity = new TEntity();
        }

        public void CopyFrom(IDbSet other)
        {
            if(other is DbSetSingle<TEntity> otherDbSet == false)
            {
                Debug.LogError("Fail Merge - different type");
                return;
            }

            Entity = otherDbSet.Entity;
        }

        public void FromJson(string json)
        {
            Entity = Newtonsoft.Json.JsonConvert.DeserializeObject<TEntity>(json);
        }

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(Entity);
        }

        public void Update([JetBrains.Annotations.NotNull] TEntity entity)
        {
            Entity = entity;
        }
    }
}
