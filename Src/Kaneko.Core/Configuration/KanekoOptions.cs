using System;
using System.Collections.Generic;

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

        public string Urls { get; set; }

        public OrleansConfig Orleans { get; set; } = new OrleansConfig();

        public ConsulConfig Consul { get; set; } = new ConsulConfig();

        public MongoDBConfig MongoDB { get; set; } = new MongoDBConfig();

        public RabbitMQConfig RabbitMQ { get; set; } = new RabbitMQConfig();

        public CapConfig Cap { get; set; } = new CapConfig();

        public RedisConfig Redis { get; set; } = new RedisConfig();

        public OrmConfig Orm { get; set; } = new OrmConfig();

    }
}
