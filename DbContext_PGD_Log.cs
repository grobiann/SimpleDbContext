using Pioneer;
using System;
using Pioneer.DB;
using UnityEngine;

namespace PGD
{
    public class DbContext_PGD_Log : DbContextBase
    {   
        public DbDictionary<ProductPurchaseLogData> ProductLog { get; set; }

        public DbContext_PGD_Log(DbContextBuildOptions options) : base(options)
        {
        }
    }

    [System.Serializable]
    public class ProductPurchaseLogData : IHasPK
    {
        public Crypt_Long id;
        public Crypt_Int key;
        public Crypt_Int amount;
        public Crypt_Long purchasedTime;

        public long PK { get => id; set => id = value; }
    }
}