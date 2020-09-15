namespace Kaneko.Core.Configuration
{
    public class MongoDBConfig
    {
        public bool Enable { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

        public bool CreateShardKeyForCosmos { get; set; } = false;
    }
}
