using DotNetCore.CAP;
using DotNetCore.CAP.Dashboard.NodeDiscovery;
using DotNetCore.CAP.MongoDB;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kaneko.Hosts.Configuration
{
    public class OrleansOptions : IConfigObject
    {
        public const string Position = "HostSetting";

        public string MetricsTableWriteInterval { get; set; } = "00:00:30";

        public DashboardConfig Dashboard { get; set; } = new DashboardConfig(); // We need initialization, else will be null, and no default will be available
        public double DefaultGrainAgeLimitInMins { get; set; } = 5;

        public double DefaultReminderGrainAgeLimitInMins { get; set; } = 10080;//7天

        public IDictionary<string, GrainAgeLimitConfig> GrainAgeLimits { get; set; } = new ConcurrentDictionary<string, GrainAgeLimitConfig>();

        public ConsulConfig Consul { get; set; } = new ConsulConfig();

        public MongoDBConfig MongoDB { get; set; } = new MongoDBConfig();

        public RabbitMQConfig RabbitMQ { get; set; } = new RabbitMQConfig();

        public CapConfig Cap { get; set; } = new CapConfig();

        public int SiloGatewayPort { get; set; }

        public int SiloNetworkingPort { get; set; }

        public string ServiceId { get; set; }

        public string ClusterId { get; set; }
    }

    public class ConsulConfig
    {
        public bool Enable { get; set; } = true;
        public string ConnectionString { get; set; }
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

    public class RabbitMQConfig
    {
        public bool Enable { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public int PoolSizePerConnection { get; set; } = 200;
        public int MaxConnection { get; set; } = 20;
        /// <summary>
        /// 消费者批量处理每次处理的最大消息量
        /// </summary>
        public ushort CunsumerMaxBatchSize { get; set; } = 3000;
        /// <summary>
        /// 消费者批量处理每次处理的最大延时
        /// </summary>
        public int CunsumerMaxMillisecondsInterval { get; set; } = 1000;
        public string[] Hosts
        {
            get; set;
        }

        public int Port { get; set; } = 5672;

        public string QueueName { get; set; }

        public List<AmqpTcpEndpoint> EndPoints
        {
            get
            {
                var list = new List<AmqpTcpEndpoint>();
                foreach (var host in Hosts)
                {
                    list.Add(AmqpTcpEndpoint.Parse(host));
                }
                return list;
            }
        }
    }

    public class CapConfig
    {
        public bool Enable { get; set; } = true;

        public MongoDBOptions MongoDB { get; set; }

        public RabbitMQOptions RabbitMQ { get; set; }

        public ServiceDiscoveryConfig ServiceDiscovery { get; set; }
    }

    public class ServiceDiscoveryConfig
    {
        public bool Enable { get; set; } = true;

        public DiscoveryOptions DiscoveryOptions { get; set; }
    }
}
