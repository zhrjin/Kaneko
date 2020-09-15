namespace Kaneko.Core.Configuration
{
    public class ServiceDiscoveryConfig
    {
        public bool Enable { get; set; } = true;
        public string HealthPath { get; set; } = "/api/Cap";
    }
}
