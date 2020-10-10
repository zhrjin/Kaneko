using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YTSoft.EventBus.Hosts.Core;
using MongoDB.Driver;
using Kaneko.Core.Configuration;
using DotNetCore.CAP.Dashboard.NodeDiscovery;
using Kaneko.Server.AutoMapper;
using Kaneko.Core.DependencyInjection;
using AutoMapper;
using YTSoft.EventBus.Service.ComConcrete;
using Microsoft.OpenApi.Models;
using Kaneko.Hosts.Controller;
using YTSoft.EventBus.Service.Contracts;

namespace YTSoft.EventBus.Hosts
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            KanekoOptions kanekoConfig = Configuration.Get<KanekoOptions>();

            services.AddControllers(mvcConfig =>
            {
                mvcConfig.Filters.Add<GlobalExceptionFilter>();
            })
            .AddApplicationPart(typeof(Startup).Assembly)
            .AddApplicationPart(typeof(CapController).Assembly);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = kanekoConfig.ServiceName,
                    Version = "v1"
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
            });

            services.AddSingleton<IMongoClient>(new MongoClient(kanekoConfig.MongoDB.ConnectionString));
            services.AddCap(x =>
            {
                x.UseMongoDB(configure =>
                {
                    configure.DatabaseConnection = kanekoConfig.MongoDB.ConnectionString;
                    configure.DatabaseName = kanekoConfig.MongoDB.DatabaseName + "_cap";
                });
                x.UseRabbitMQ(configure =>
                {
                    configure.HostName = kanekoConfig.RabbitMQ.HostName;
                    configure.UserName = kanekoConfig.RabbitMQ.UserName;
                    configure.Password = kanekoConfig.RabbitMQ.Password;
                    configure.Port = kanekoConfig.RabbitMQ.Port;
                    configure.VirtualHost = kanekoConfig.RabbitMQ.VirtualHost;
                });

                x.UseDashboard();

                if (kanekoConfig.Cap.ServiceDiscovery.Enable)
                {
                    x.UseDiscovery(d =>
                    {
                        d.DiscoveryServerHostName = kanekoConfig.Consul.HostName;
                        d.DiscoveryServerPort = kanekoConfig.Consul.Port;
                        d.CurrentNodeHostName = kanekoConfig.CurrentNodeHostName;
                        d.CurrentNodePort = kanekoConfig.CurrentNodePort;
                        d.NodeId = $"cap_{kanekoConfig.ServiceName}_{kanekoConfig.CurrentNodeHostName}:{kanekoConfig.CurrentNodePort}";
                        d.NodeName = "cap_" + kanekoConfig.ClusterId;
                        d.MatchPath = kanekoConfig.Cap.ServiceDiscovery.HealthPath;
                    });
                }
            });

            //对象转换注入
            services.AddSingleton<Kaneko.Server.AutoMapper.IObjectMapper, AutoMapperObjectMapper>();

            //自动注入接口
            services.Scan(scan =>
            {
                scan.FromAssemblies(typeof(ITaskListSyncService).Assembly).AddClasses().UsingAttributes();
            });

            //AutoMapper 注入
            services.AddAutoMapper(typeof(ITaskListSyncService).Assembly);

            services.AddHttpClient();
            services.AddHttpClient(ConfigKeys.HttpClientName, configureClient =>
            {
                configureClient.BaseAddress = new System.Uri(Configuration[ConfigKeys.SYSDOMAIN_GATEWAYURL]);
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = $"{kanekoConfig.Redis.HostName}:{kanekoConfig.Redis.Port}";
                options.InstanceName = kanekoConfig.Redis.InstanceName;
                if (!string.IsNullOrEmpty(kanekoConfig.Redis.Password))
                {
                    options.ConfigurationOptions.Password = kanekoConfig.Redis.Password;
                }
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string serviceName = Configuration["ServiceName"];

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", serviceName);
                options.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
