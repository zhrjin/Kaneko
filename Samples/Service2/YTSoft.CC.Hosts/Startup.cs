using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Kaneko.Hosts.Extensions;
using System;
using SkyApm.Tracing;

namespace YTSoft.CC.Hosts
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
            //services.AddSingleton<ISamplingInterceptor, CustomSamplingInterceptor>();

            services.AddKaneko(typeof(Startup).Assembly, Configuration,
                corsConfig: (corsConfig) =>
                {
                    string[] origins = Configuration["AllowedOrigins"].Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    corsConfig.AddPolicy(Configuration["ServiceName"],
                        builder => builder.WithOrigins(origins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                    );
                },
                mvcConfig: (mvcConfig) =>
                {
                    mvcConfig.Filters.Add<GlobalExceptionFilter>();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseKaneko(Configuration);
        }
    }
}
