using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;
using Ocelot.Cache.CacheManager;

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

            //ע��ID4У��
            services.AddAuthentication("Bearer")
                    .AddIdentityServerAuthentication("ApoOneServiceKey", option =>      //����GoodsServiceKey��Ocelot�����ļ��е�AuthenticationProviderKey��Ӧ,�Ӷ����а���֤
                    {
                        option.Authority = "http://192.168.0.103:12345";  //����������127.0.0.1����ôͨ��ID4��������ȡtoken��ʱ�򣬾ͱ���д127.0.0.1������дlocalhost.   
                        option.ApiName = "ApoOneService";             //�����ӦID4��������GetApiResources���õ�apiName,�˴��������д����
                        option.RequireHttpsMetadata = false;
                    })
                    .AddIdentityServerAuthentication("ApoTwoServiceKey", option =>
                    {
                        option.Authority = "http://192.168.0.103:12345";
                        option.ApiName = "ApoTwoService";
                        option.RequireHttpsMetadata = false;
                    });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOcelot().Wait();

            //app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
