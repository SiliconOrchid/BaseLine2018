namespace BaseLine2018.Common.Models.Configuration
{
    public class FeatureSwitchesConfig
    {
        public bool EnableRepositoryMemoryCache { get; set; }
        public bool EnableDatabaseAutoMigration { get; set; }
        public bool EnableDatabaseAutoSeeding { get; set; }
        public bool EnableHoldingPage { get; set; }
        
    }
}
