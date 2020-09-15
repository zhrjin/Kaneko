namespace Kaneko.Core.Configuration
{
    public class RedisConfig
    {
        public bool Enable { get; set; } = false;
        public string HostName { get; set; }
        public int Port { get; set; }
        public string InstanceName { get; set; }
        public string Password { get; set; }

        public bool UseJson { get; set; } = false;

        public int DatabaseNumber { get; set; } = 1;
    }
}
