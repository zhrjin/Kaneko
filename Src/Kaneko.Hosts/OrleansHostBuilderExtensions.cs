using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using Orleans.Statistics;
using System.Collections.Generic;
using Orleans.Providers;
using AutoMapper;
using Kaneko.Hosts.Configuration;
using Kaneko.Core.Extensions;
using Kaneko.Core.Utils;
using Kaneko.Core.AutoMapper;
using Kaneko.Dapper;
using Kaneko.Core.DependencyInjection;

namespace Kaneko.Hosts
{
    public static class OrleansHostBuilderExtensions
    {
        static OrleansOptions OrleansConfig;

        public static IHostBuilder AddOrleans(this IHostBuilder hostBuilder, string siloName, Assembly grainAssembly, Assembly autoMapperAssembly)
        {
            hostBuilder.UseOrleans((context, siloBuilder) =>
            {
                OrleansConfig = context.Configuration.GetSection(OrleansOptions.Position).Get<OrleansOptions>();

                siloBuilder
                .ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(grainAssembly).WithReferences();
                })
                .Configure<SiloOptions>(options => options.SiloName = siloName)
                .Configure<HostOptions>(options => options.ShutdownTimeout = TimeSpan.FromSeconds(30))
                .UseLinuxEnvironmentStatistics()
                .UsePerfCounterEnvironmentStatistics()
                ;

                if (OrleansConfig.Dashboard.Enable)
                {
                    siloBuilder.UseDashboard(o =>
                    {
                        o.Port = OrleansConfig.Dashboard.SiloDashboardPort;
                        o.CounterUpdateIntervalMs = (int)TimeSpan.Parse(OrleansConfig.Dashboard.WriteInterval).TotalMilliseconds;
                        o.HideTrace = OrleansConfig.Dashboard.HideTrace;
                        o.Username = OrleansConfig.Dashboard.UserName;
                        o.Password = OrleansConfig.Dashboard.Password;
                    });
                }

                SetGrainCollectionOptions(siloBuilder, grainAssembly);

                siloBuilder.Configure<PerformanceTuningOptions>(options =>
                {
                    options.DefaultConnectionLimit = ServicePointManager.DefaultConnectionLimit;
                });
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
                SetStream(siloBuilder);
                SetSiloSource(siloBuilder);
                SetConfigureServices(siloBuilder, grainAssembly, autoMapperAssembly);

                siloBuilder.Configure<StatisticsOptions>(o =>
                {
                    o.LogWriteInterval = TimeSpan.FromMinutes(10);
                    o.PerfCountersWriteInterval = TimeSpan.Parse(OrleansConfig.MetricsTableWriteInterval);
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
                      options.DatabaseName = OrleansConfig.MongoDB.DatabaseName;
                      options.CreateShardKeyForCosmos = OrleansConfig.MongoDB.CreateShardKeyForCosmos;
                  });
            }
        }

        private static void SetStorage(ISiloBuilder silo)
        {
            if (OrleansConfig.MongoDB.Enable)
            {
                silo.AddMongoDBGrainStorage("PubSubStore", op =>
                {
                    op.DatabaseName = OrleansConfig.MongoDB.DatabaseName;
                    op.CreateShardKeyForCosmos = OrleansConfig.MongoDB.CreateShardKeyForCosmos;
                    op.ConfigureJsonSerializerSettings = settings =>
                    {
                        settings.NullValueHandling = NullValueHandling.Include;
                        settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                        settings.DefaultValueHandling = DefaultValueHandling.Populate;
                    };
                });
            }
        }

        private static void SetStream(ISiloBuilder silo)
        {
            if (OrleansConfig.RabbitMQ.Enable)
            {
                /*silo.AddRabbitMqStream(EventBusGlobals.StreamProviderNameDefault, configurator =>
                {
                    configurator.ConfigureRabbitMq(
                        hosts: OrleansConfig.RabbitMQ.Hosts,
                        port: OrleansConfig.RabbitMQ.Port,
                        virtualHost: OrleansConfig.RabbitMQ.VirtualHost,
                        user: OrleansConfig.RabbitMQ.UserName,
                        password: OrleansConfig.RabbitMQ.Password,
                        queueName: OrleansConfig.RabbitMQ.QueueName
                    );
                });*/
            }
        }

        private static void SetSiloSource(ISiloBuilder silo)
        {
            silo
            //.UseConsulClustering(options =>
            //{
            //    options.Address = new Uri(OrleansConfig.Consul.ConnectionString);
            //}).ConfigureEndpoints(siloPort: OrleansConfig.SiloNetworkingPort, gatewayPort: OrleansConfig.SiloGatewayPort)

            .UseLocalhostClustering()
            .ConfigureEndpoints(siloPort: OrleansConfig.SiloNetworkingPort, gatewayPort: OrleansConfig.SiloGatewayPort)
            .Configure<ClusterOptions>(options =>
            {
                options.ClusterId = OrleansConfig.ClusterId;
                options.ServiceId = OrleansConfig.ServiceId;
            });
        }

        private static void SetGrainCollectionOptions(ISiloBuilder silo, Assembly assembly)
        {
            silo.Configure<GrainCollectionOptions>(options =>
            {
                options.CollectionAge = TimeSpan.FromMinutes(OrleansConfig.DefaultGrainAgeLimitInMins);
                var assemblyList = GetStateGrainTypesFromAssembly(assembly);
                if (assemblyList != null)
                {
                    double grainAgeLimitInMins = OrleansConfig.DefaultReminderGrainAgeLimitInMins;
                    foreach (var grainAgeLimitConfig in assemblyList)
                    {
                        try
                        {
                            double timeSpan = grainAgeLimitInMins + RandomHelper.GetRandomNumber(1, 120);
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
            });
        }

        private static void SetConfigureServices(ISiloBuilder silo, Assembly grainAssembly, Assembly autoMapperAssembly)
        {
            silo.ConfigureServices((context, servicecollection) =>
            {
                //对象转换注入
                servicecollection.AddSingleton<Core.AutoMapper.IObjectMapper, AutoMapperObjectMapper>();

                //自动注入接口
                servicecollection.Scan(scan => scan.FromAssemblies(grainAssembly).AddClasses().UsingAttributes());

                //AutoMapper 注入
                servicecollection.AddAutoMapper(autoMapperAssembly);

                servicecollection.AddSingleton<IDDLExecutor, DDLExecutor>();
                Startup.Register(async serviceProvider =>
                {
                    var container = serviceProvider.GetService<IDDLExecutor>();
                    await container.AutoAlterDbSchema(grainAssembly);
                });

            }).AddStartupTask<SiloStartupTask>();
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
