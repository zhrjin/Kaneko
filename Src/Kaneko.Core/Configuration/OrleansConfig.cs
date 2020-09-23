using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Kaneko.Core.Configuration
{
    public class OrleansConfig
    {
        public int SiloGatewayPort { get; set; }

        public int SiloNetworkingPort { get; set; }

        public string MetricsTableWriteInterval { get; set; } = "00:00:30";

        public DashboardConfig Dashboard { get; set; } = new DashboardConfig(); // We need initialization, else will be null, and no default will be available
        public double DefaultGrainAgeLimitInMins { get; set; } = 5;

        public double DefaultReminderGrainAgeLimitInMins { get; set; }

        public IDictionary<string, GrainAgeLimitConfig> GrainAgeLimits { get; set; } = new ConcurrentDictionary<string, GrainAgeLimitConfig>();

        public List<OrleansClientConfig> Clients { get; set; } = new List<OrleansClientConfig>();
    }
}
