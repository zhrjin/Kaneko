﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Orleans.Statistics;
using System.Collections.Generic;
using Orleans.Providers;
using AutoMapper;
using Kaneko.Core.Extensions;
using Kaneko.Server.AutoMapper;
using Kaneko.Dapper;
using Kaneko.Core.DependencyInjection;
using MongoDB.Driver;
using DotNetCore.CAP.Dashboard.NodeDiscovery;
using Kaneko.Core.Configuration;
using Kaneko.Server.Orleans.HostServices;
using Kaneko.Server.Orleans.Grains;
using Microsoft.AspNetCore.Hosting;
using Kaneko.Server.SkyAPM;
using Kaneko.Server.Orleans.Client;

namespace Kaneko.Hosts.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class OrleansHostBuilderExtensions
    {
        static KanekoOptions OrleansConfig;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="grainAssembly"></param>
        /// <param name="autoMapperAssembly"></param>
        /// <returns></returns>
        public static IHostBuilder AddOrleans(this IHostBuilder hostBuilder, Assembly grainAssembly, Assembly autoMapperAssembly)
        {
            hostBuilder.ConfigureServices(serviceCollection =>
            {
                serviceCollection.AddHostedService<ConsulCleanupService>();
            });

            hostBuilder.UseOrleans((context, siloBuilder) =>
            {
                OrleansConfig = context.Configuration.Get<KanekoOptions>();

                siloBuilder
                .ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(grainAssembly).WithReferences();
                    parts.AddKanekoParts();
                })
                .Configure<SiloOptions>(options => options.SiloName = OrleansConfig.ServiceName)
                .Configure<HostOptions>(options => options.ShutdownTimeout = TimeSpan.FromSeconds(30))
                .UseLinuxEnvironmentStatistics()
                .UsePerfCounterEnvironmentStatistics();

                if (OrleansConfig.Orleans.Dashboard.Enable)
                {
                    siloBuilder.UseDashboard(o =>
                    {
                        o.Port = OrleansConfig.Orleans.Dashboard.SiloDashboardPort;
                        o.CounterUpdateIntervalMs = (int)TimeSpan.Parse(OrleansConfig.Orleans.Dashboard.WriteInterval).TotalMilliseconds;
                        o.HideTrace = OrleansConfig.Orleans.Dashboard.HideTrace;
                        o.Username = OrleansConfig.Orleans.Dashboard.UserName;
                        o.Password = OrleansConfig.Orleans.Dashboard.Password;
                        o.BasePath = OrleansConfig.Orleans.Dashboard.BasePath;
                    });
                }

                SetGrainCollectionOptions(siloBuilder, grainAssembly);

                siloBuilder.Configure<ClusterMembershipOptions>(options =>
                {
                    options.IAmAliveTablePublishTimeout = TimeSpan.FromMinutes(1.0);//失活的节点默认忽略时间
                });

                //siloBuilder.Configure<PerformanceTuningOptions>(options =>
                //{
                //    options.DefaultConnectionLimit = ServicePointManager.DefaultConnectionLimit;
                //});
                siloBuilder.Configure<SchedulingOptions>(options =>
                {
                    options.PerformDeadlockDetection = true;
                    options.AllowCallChainReentrancy = true;
                    options.MaxActiveThreads = Process.GetCurrentProcess().ProcessorAffinityList().Count();
                })
                .Configure<MessagingOptions>(options =>
                {
                    options.ResponseTimeout = TimeSpan.FromMinutes(5);
                })
                .Configure<SiloMessagingOptions>(options =>
                {
                    options.ResponseTimeout = TimeSpan.FromMinutes(5);
                });

                SetReminder(siloBuilder);
                SetStorage(siloBuilder);
                SetSiloSource(siloBuilder);
                SetConfigureServices(siloBuilder, grainAssembly, autoMapperAssembly);

                siloBuilder.Configure<StatisticsOptions>(o =>
                {
                    o.LogWriteInterval = TimeSpan.FromMinutes(10);
                    o.PerfCountersWriteInterval = TimeSpan.Parse(OrleansConfig.Orleans.MetricsTableWriteInterval);
                });
            });

            return hostBuilder;
        }

        private static void SetReminder(ISiloBuilder silo)
        {
            if (OrleansConfig.MongoDB.Enable)
            {
                silo.UseMongoDBClient(OrleansConfig.MongoDB.ConnectionString)
                  .UseMongoDBReminders(options =>
                  {
                      options.DatabaseName = OrleansConfig.MongoDB.DatabaseName + "_reminder";
                      options.CreateShardKeyForCosmos = OrleansConfig.MongoDB.CreateShardKeyForCosmos;
                  });
            }
        }

        private static void SetStorage(ISiloBuilder silo)
        {
            if (OrleansConfig.MongoDB.Enable)
            {
                silo.AddMongoDBGrainStorageAsDefault(op =>
                {
                    op.Configure(ooop =>
                    {
                        ooop.DatabaseName = OrleansConfig.MongoDB.DatabaseName + "_defaultgrain";
                        ooop.CreateShardKeyForCosmos = OrleansConfig.MongoDB.CreateShardKeyForCosmos;
                        ooop.ConfigureJsonSerializerSettings = settings =>
                        {
                            settings.NullValueHandling = NullValueHandling.Include;
                            settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                            settings.DefaultValueHandling = DefaultValueHandling.Populate;
                        };
                    });

                });

                silo.AddMongoDBGrainStorage(GrainStorageKey.MongoDBStore, op =>
                {
                    op.Configure(ooop =>
                    {
                        ooop.DatabaseName = OrleansConfig.MongoDB.DatabaseName + "_grain";
                        ooop.CreateShardKeyForCosmos = OrleansConfig.MongoDB.CreateShardKeyForCosmos;
                        ooop.ConfigureJsonSerializerSettings = settings =>
                        {
                            settings.NullValueHandling = NullValueHandling.Include;
                            settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                            settings.DefaultValueHandling = DefaultValueHandling.Populate;
                        };
                    });

                });
            }
            else
            {
                silo.AddMemoryGrainStorage(GrainStorageKey.MongoDBStore);
            }

            if (OrleansConfig.Redis.Enable)
            {
                silo.AddRedisGrainStorage(GrainStorageKey.RedisStore, optionsBuilder => optionsBuilder.Configure(options =>
                {
                    options.DataConnectionString = $"{OrleansConfig.Redis.HostName}:{OrleansConfig.Redis.Port}";
                    options.UseJson = OrleansConfig.Redis.UseJson;
                    options.DatabaseNumber = OrleansConfig.Redis.DatabaseNumber;
                }));
            }
            else
            {
                silo.AddMemoryGrainStorage(GrainStorageKey.RedisStore);
            }

        }

        private static void SetSiloSource(ISiloBuilder silo)
        {
            if (OrleansConfig.Consul.Enable)
            {
                silo.UseConsulClustering(options => { options.Address = new Uri($"http://{OrleansConfig.Consul.HostName}:{OrleansConfig.Consul.Port}"); })
                    .ConfigureEndpoints(siloPort: OrleansConfig.Orleans.SiloNetworkingPort, gatewayPort: OrleansConfig.Orleans.SiloGatewayPort);
            }
            else
            {
                silo.UseLocalhostClustering();
            }

            silo.Configure<ClusterOptions>(options =>
            {
                options.ClusterId = OrleansConfig.ClusterId;
                options.ServiceId = OrleansConfig.ServiceId;
            });
        }

        private static void SetGrainCollectionOptions(ISiloBuilder silo, Assembly assembly)
        {
            silo.Configure<GrainCollectionOptions>(options =>
            {
                options.CollectionAge = TimeSpan.FromMinutes(OrleansConfig.Orleans.DefaultGrainAgeLimitInMins);
                double grainAgeLimitInMins = OrleansConfig.Orleans.DefaultReminderGrainAgeLimitInMins;
                if (grainAgeLimitInMins > 0)
                {
                    var assemblyList = GetStateGrainTypesFromAssembly(assembly);
                    if (assemblyList != null)
                    {
                        foreach (var grainAgeLimitConfig in assemblyList)
                        {
                            try
                            {
                                double timeSpan = grainAgeLimitInMins;//+ RandomHelper.GetRandomNumber(1, 120);
                                options.ClassSpecificCollectionAge.Add(grainAgeLimitConfig.FullName,
                                    TimeSpan.FromMinutes(timeSpan));
                            }
                            catch (Exception e)
                            {
                                throw new ArgumentException(
                                    $"Assigning Age Limit on {grainAgeLimitConfig} has failed, because {grainAgeLimitConfig} is an invalid type\n{e.Message}");
                            }
                        }
                    }
                }
            });
        }

        private static void SetConfigureServices(ISiloBuilder silo, Assembly grainAssembly, Assembly autoMapperAssembly)
        {
            silo.ConfigureServices((context, services) =>
            {
                //SkyAPM
                services.UseSkyApm();

                services.AddHttpClient();

                if (OrleansConfig.Redis.Enable)
                {
                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = $"{OrleansConfig.Redis.HostName}:{OrleansConfig.Redis.Port}";
                        options.InstanceName = OrleansConfig.Redis.InstanceName;
                        if (!string.IsNullOrEmpty(OrleansConfig.Redis.Password))
                        {
                            options.ConfigurationOptions.Password = OrleansConfig.Redis.Password;
                        }
                    });
                }

                //cap
                if (OrleansConfig.Cap.Enable)
                {
                    services.AddSingleton<IMongoClient>(new MongoClient(OrleansConfig.MongoDB.ConnectionString));
                    services.AddCap(x =>
                    {
                        x.UseMongoDB(configure =>
                        {
                            configure.DatabaseConnection = OrleansConfig.MongoDB.ConnectionString;
                            configure.DatabaseName = OrleansConfig.MongoDB.DatabaseName + "_cap";
                        });
                        x.UseRabbitMQ(configure =>
                        {
                            configure.HostName = OrleansConfig.RabbitMQ.HostName;
                            configure.UserName = OrleansConfig.RabbitMQ.UserName;
                            configure.Password = OrleansConfig.RabbitMQ.Password;
                            configure.Port = OrleansConfig.RabbitMQ.Port;
                            configure.VirtualHost = OrleansConfig.RabbitMQ.VirtualHost;
                        });

                        x.UseDashboard(option => option.PathMatch = "/CapDashboard");

                        if (OrleansConfig.Cap.ServiceDiscovery.Enable)
                        {
                            x.UseDiscovery(d =>
                            {
                                d.DiscoveryServerHostName = OrleansConfig.Consul.HostName;
                                d.DiscoveryServerPort = OrleansConfig.Consul.Port;
                                d.CurrentNodeHostName = OrleansConfig.CurrentNodeHostName;
                                d.CurrentNodePort = OrleansConfig.CurrentNodePort;
                                d.NodeId = $"cap_{OrleansConfig.ServiceName}_{OrleansConfig.CurrentNodeHostName}:{OrleansConfig.CurrentNodePort}";
                                d.NodeName = "cap_" + OrleansConfig.ClusterId;
                                d.MatchPath = OrleansConfig.Cap.ServiceDiscovery.HealthPath;
                            });
                        }
                    });
                }

                //对象转换注入
                services.AddSingleton<Server.AutoMapper.IObjectMapper, AutoMapperObjectMapper>();

                //自动注入接口
                services.Scan(scan =>
                {
                    scan.FromAssemblies(grainAssembly).AddClasses().UsingAttributes();
                });

                //AutoMapper 注入
                services.AddAutoMapper(autoMapperAssembly);

                //添加多客户端
                AddOrleansMultiClient(services);

                //自动更新表结构
                if (OrleansConfig.Orm.DDLAutoUpdate)
                {
                    services.AddTransient<IDDLExecutor, DDLExecutor>();
                    Startup.Register(async serviceProvider =>
                    {
                        var exec = serviceProvider.GetService<IDDLExecutor>();
                        await exec.AutoAlterDbSchema(grainAssembly);
                    });
                }
            }).AddStartupTask<SiloStartupTask>();
        }

        private static void AddOrleansMultiClient(IServiceCollection services)
        {
            try
            {
                //添加多客户端
                if (OrleansConfig.Orleans.Clients.Count > 0)
                {
                    services.AddOrleansMultiClient(build =>
                    {
                        foreach (var clusterConfig in OrleansConfig.Orleans.Clients)
                        {
                            int assCount = clusterConfig.ServiceAssembly.Length;
                            Assembly[] assemblys = new Assembly[assCount];
                            for (int i = 0; i < assCount; i++)
                            {
                                assemblys[i] = Assembly.Load(clusterConfig.ServiceAssembly[i]);
                            }

                            build.AddClient(opt =>
                            {
                                opt.ClusterId = clusterConfig.ClusterId;
                                opt.ServiceId = clusterConfig.ServiceId;
                                opt.SetServiceAssembly(assemblys);
                                opt.Configure = (b =>
                                {
                                    b.UseConsulClustering(gatewayOptions =>
                                    {
                                        gatewayOptions.Address = new Uri($"http://{clusterConfig.Consul.HostName}:{clusterConfig.Consul.Port}");
                                    });
                                });
                            });
                        }
                    });
                }
                else
                {
                    //注入一个空对象，避免构造引用IOrleansClient报错
                    services.AddSingleton<IOrleansClient, NullOrleansClient>();
                }
            }
            catch { }
        }

        private static List<Type> GetStateGrainTypesFromAssembly(Assembly a)
        {
            return a.GetExportedTypes()
                 .Where(x => x.IsClass && !x.IsAbstract)
                 .Where(x => x.GetInterfaces().Any(i => typeof(IGrain).IsAssignableFrom(i)))
                 .Where(o =>
                 {
                     return IsStorageProviderAttribute(System.Attribute.GetCustomAttributes(o, true));
                 })
                 .ToList();
        }

        private static bool IsStorageProviderAttribute(Attribute[] o)
        {
            foreach (Attribute a in o)
            {
                if (a is StorageProviderAttribute)
                    return true;
            }
            return false;
        }
    }
}
