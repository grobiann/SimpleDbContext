namespace Pioneer
{
    [System.Serializable]
    public struct LogInfo
    {
        public Crypt_Int identifier;
        public Crypt_Long num_long;

        public LogInfo(int identifier, long num_long)
        {
            this.identifier = identifier;
            this.num_long = num_long;
        }
    }
}
