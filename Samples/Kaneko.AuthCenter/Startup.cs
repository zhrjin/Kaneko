using System.Linq;
using Kaneko.Core.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Kaneko.AuthCenter
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
            services.AddConsul(Configuration);

            //1. �ͻ���ģʽ
            //services.AddIdentityServer()
            //      .AddDeveloperSigningCredential()    //����Tokenǩ����Ҫ�Ĺ�Կ��˽Կ,�洢��bin��tempkey.rsa(��������Ҫ����ʵ֤�飬�˴���ΪAddSigningCredential)
            //      .AddInMemoryApiResources(ClientsConfig.GetApiResources())  //�洢��Ҫ����api��Դ
            //      .AddInMemoryApiScopes(ClientsConfig.GetApiScopes())        //����api��Χ 4.x�汾�������õ�
            //      .AddInMemoryClients(ClientsConfig.GetClients()); //�洢�ͻ���ģʽ(����Щ�ͻ��˿�����)


            //2. �û�������ģʽ
            services.AddIdentityServer()
                  .AddDeveloperSigningCredential()    //����Tokenǩ����Ҫ�Ĺ�Կ��˽Կ,�洢��bin��tempkey.rsa(��������Ҫ����ʵ֤�飬�˴���ΪAddSigningCredential)
                  .AddInMemoryApiResources(PasswordConfig.GetApiResources())  //�洢��Ҫ����api��Դ
                  .AddInMemoryClients(PasswordConfig.GetClients()) //�洢�ͻ���ģʽ(����Щ�ͻ��˿�����)
                  .AddInMemoryApiScopes(ClientsConfig.GetApiScopes())        //����api��Χ 4.x�汾�������õ�
                  .AddTestUsers(PasswordConfig.GetUsers().ToList());  //�洢��Щ�û���������Է��� (�û�������ģʽ)

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            //1.����IdentityServe4
            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseConsul();
        }
    }
}
