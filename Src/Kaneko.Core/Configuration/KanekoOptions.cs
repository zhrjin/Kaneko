using DotNetCore.CAP;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kaneko.Core.Configuration
{
    public class KanekoOptions : IConfigObject
    {
        public string ServiceName { get; set; }

        public string CurrentNodeHostName
        {
            get
            {
                if (!string.IsNullOrEmpty(Urls))
                {
                    string[] hostUrl = Urls.Replace("https://", "").Replace("http://", "").Split(":", StringSplitOptions.RemoveEmptyEntries);
                    return hostUrl[0];
                }

                return "";
            }
        }

        public int CurrentNodePort
        {
            get
            {
                if (!string.IsNullOrEmpty(Urls))
                {
                    string[] hostUrl = Urls.Replace("https://", "").Replace("http://", "").Split(":", StringSplitOptions.RemoveEmptyEntries);
                    if (hostUrl.Length == 2)
                        return int.Parse(hostUrl[1]);
                }
                return 80;
            }
        }

        public string Urls { get; set; }

        public OrleansConfig Orleans { get; set; } = new OrleansConfig();

        public ConsulConfig Consul { get; set; } = new ConsulConfig();

        public MongoDBConfig MongoDB { get; set; } = new MongoDBConfig();

        public RabbitMQOptions RabbitMQ { get; set; } = new RabbitMQOptions();

        public CapConfig Cap { get; set; } = new CapConfig();

        public string ServiceId
        {
            get
            {
                return ServiceName;
            }
        }

        public string ClusterId
        {
            get
            {
                return ServiceName;
            }
        }
    }

    public class ConsulConfig
    {
        public bool Enable { get; set; } = true;
        public string HostName { get; set; }
        public int Port { get; set; }
        public string HealthCheckRelativeUri { get; set; } = "api/HealthCheck";
    }

    public class GrainAgeLimitConfig
    {
        /// <remarks>
        /// The CollectionAgeLimit must be greater than CollectionQuantum, which is set to 00:01:00 (by default).
        /// https://dotnet.github.io/orleans/Documentation/clusters_and_clients/configuration_guide/activation_garbage_collection.html
        /// See CollectionAgeLimitValidator.cs details.
        /// </remarks>
        [Range(1.001d, double.MaxValue, ErrorMessage = "The GrainAgeLimitInMins " +
                                                       "(CollectionAgeLimit) must be greater than CollectionQuantum, " +
                                                       "which is set to 1 min (by default). The type is double.")]
        public double GrainAgeLimitInMins { get; set; }

        /// <summary>
        /// The full qualified type name to apply grain age limit.
        /// </summary>
        public string GrainType { get; set; }
    }

    public class DashboardConfig
    {
        public string WriteInterval { get; set; } = "00:00:05"; // Recommended, not less than
        public bool HideTrace { get; set; } = true;
        public bool Enable { get; set; } = true;
        public int SiloDashboardPort { get; set; } = 7080;
        public string UserName { get; set; } = "Silo";
        public string Password { get; set; } = "Silo";
    }

    public class MongoDBConfig
    {
        public bool Enable { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

        public bool CreateShardKeyForCosmos { get; set; } = false;
    }

    public class CapConfig
    {
        public bool Enable { get; set; } = true;

        public ServiceDiscoveryConfig ServiceDiscovery { get; set; }
    }

    public class ServiceDiscoveryConfig
    {
        public bool Enable { get; set; } = true;
        public string HealthPath { get; set; }
    }

    public class OrleansConfig
    {
        public int SiloGatewayPort { get; set; }

        public int SiloNetworkingPort { get; set; }

        public string MetricsTableWriteInterval { get; set; } = "00:00:30";

        public DashboardConfig Dashboard { get; set; } = new DashboardConfig(); // We need initialization, else will be null, and no default will be available
        public double DefaultGrainAgeLimitInMins { get; set; } = 5;

        public double DefaultReminderGrainAgeLimitInMins { get; set; } = 10080;//7天

        public IDictionary<string, GrainAgeLimitConfig> GrainAgeLimits { get; set; } = new ConcurrentDictionary<string, GrainAgeLimitConfig>();
    }
}
