using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;
using Ocelot.Cache.CacheManager;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;

namespace Kaneko.OcelotGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot().AddConsul().AddPolly().AddCacheManager(x => x.WithDictionaryHandle());

            //注册ID4校验
            services.AddAuthentication("Bearer")
                    .AddIdentityServerAuthentication("ApoOneServiceKey", option =>      //这里GoodsServiceKey与Ocelot配置文件中的AuthenticationProviderKey对应,从而进行绑定验证
                    {
                        option.Authority = "http://192.168.0.103:12345";  //这里配置是127.0.0.1，那么通过ID4服务器获取token的时候，就必须写127.0.0.1，不能写localhost.   
                        option.ApiName = "ApoOneService";             //必须对应ID4服务器中GetApiResources配置的apiName,此处不能随便写！！
                        option.RequireHttpsMetadata = false;
                    })
                    .AddIdentityServerAuthentication("ApoTwoServiceKey", option =>
                    {
                        option.Authority = "http://192.168.0.103:12345";
                        option.ApiName = "ApoTwoService";
                        option.RequireHttpsMetadata = false;
                    });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ApiGateway", new OpenApiInfo { Title = "Kaneko.ApiGateway", Version = "v1" });
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var apis = new List<string> { "demo", "UsersApi" };

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                apis.ForEach(m =>
                {
                    options.SwaggerEndpoint($"{m}/swagger/v1/swagger.json", m);
                    options.RoutePrefix = string.Empty;
                });
            });

            app.UseOcelot().Wait();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
