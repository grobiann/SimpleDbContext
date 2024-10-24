using Pioneer.DB;

namespace PGD
{
    public class DbContext_PGD : DbContextBase
    {
        public DbSetSingle<IngameSaveData> Ingame { get; set; }
        public DbSetSingle<UserData> User { get; set; }
        public DbSetSingle<UserSettings> Settings { get; set; }

        public DbDictionary<CurrencyData> Currency { get; set; }
        public DbDictionary<ProductData> Product { get; set; }
        public DbDictionary<SkillData> Skill { get; set; }
        public DbDictionary<EquipmentData> Equipment { get; set; }
        public DbDictionary<StoneData> Stone { get; set; }
        public DbDictionary<StageData> Stage { get; set; }
        public DbDictionary<MailData> Mail { get; set; }
        public DbDictionary<EquipmentStoneRelation> EquipmentStoneRelation { get; set; }

        public DbContext_PGD(DbContextBuildOptions options) : base(options)
        {
        }
    }
}