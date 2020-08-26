using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MSDemo.Application;
using Kaneko.Core.Consul;

namespace MSDemo
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
            services.AddConsul(Configuration);

            services.AddControllers()
                    .AddApplicationPart(typeof(TestController).Assembly);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = nameof(MSDemo), Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(nameof(MSDemo),
                    builder =>
                    {
                        builder
                            .WithOrigins(
                                "http://localhost:62653",
                                "http://localhost:62654")
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseConsul();
            app.UseCors(nameof(MSDemo));
            app.UseSwagger(c=> {
                //加上服务名，支持直接在ocelot进行api测试
                string basepath = $"/{nameof(MSDemo)}";
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    OpenApiPaths paths = new OpenApiPaths();
                    foreach (var path in swaggerDoc.Paths)
                    {
                        paths.Add(basepath + path.Key, path.Value);
                    }
                    swaggerDoc.Paths = paths;
                });
            });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", nameof(MSDemo));
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
