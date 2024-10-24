namespace Pioneer.DB
{
    public interface IHasPK
    {
        long PK { get; set; }
    }

    public interface IJson
    {
        string ToJson();
        void FromJson(string json);
    }
}
