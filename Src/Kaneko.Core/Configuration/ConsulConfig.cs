namespace Kaneko.Core.Configuration
{
    public class ConsulConfig
    {
        public bool Enable { get; set; } = true;
        public string HostName { get; set; }
        public int Port { get; set; }
        public string HealthCheckRelativeUri { get; set; } = "api/Consul/health";
    }
}
