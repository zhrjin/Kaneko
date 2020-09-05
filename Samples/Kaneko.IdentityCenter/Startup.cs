using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Kaneko.IdentityCenter.Configurations;
using Kaneko.IdentityCenter.Data;
using Kaneko.IdentityCenter.Entities;
using Kaneko.IdentityCenter.Service;
using Kaneko.IdentityCenter.Validator;
using Kaneko.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IdentityServer4.EntityFramework.Stores;
using Kaneko.Core.Users;

namespace Kaneko.IdentityCenter
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment Env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConsul(Configuration);

            string connectionString = Configuration.GetConnectionString("DefaultAspNetAccountConnection");
            services.AddDbContext<AspNetAccountDbContext>(u => u.UseSqlServer(connectionString));
            services.AddIdentity<ApplicationUser, IdentityRole>(o =>
            {
                o.User.RequireUniqueEmail = true;
                o.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireUppercase = false;
                o.Password.RequiredLength = 5;
            })
                .AddEntityFrameworkStores<AspNetAccountDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory<ApplicationUser>>();
            services.AddScoped<IProfileService, KanekoProfileService>();
            services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.EmitStaticAudienceClaim = true;
                })
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseSqlServer(connectionString);
                    };
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseSqlServer(connectionString);
                    };

                    options.EnableTokenCleanup = true;//允许对Token的清理
                })
                .AddPersistedGrantStore<PersistedGrantStore>()
                .AddConfigurationStoreCache()
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<KanekoProfileService>()
                .AddResourceOwnerValidator<KanekoResourceOwnerPasswordValidator<ApplicationUser>>()
                .AddSigningCredential(new X509Certificate2(Path.Combine(Env.ContentRootPath,
                    Configuration["Certificates:CerPath"]),
                    Configuration["Certificates:Password"]))
                ;

            services.AddControllers();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddLog4Net();
            app.UseIdentityServer();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseConsul();

            //Task.Run(() => InitDatabase(app));

            //InitializeDatabase(app);
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                context.Database.EnsureCreated();

                var sql = context.Database.GenerateCreateScript();

                context.ApiResources.RemoveRange(context.ApiResources);

                //if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                context.Clients.RemoveRange(context.Clients);
              
                //if (!context.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                context.IdentityResources.RemoveRange(context.IdentityResources);

                //if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                context.ApiScopes.RemoveRange(context.ApiScopes);

                //if (!context.ApiScopes.Any())
                {
                    foreach (var resource in Config.GetApiScopes())
                    {
                        context.ApiScopes.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

               

            }
        }

        private void InitDatabase(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var ctx = scope.ServiceProvider.GetService<AspNetAccountDbContext>();
            ctx.Database.EnsureCreated();

            using var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            if (!userManager.Users.Any())
            {
                foreach (var user in DatabaseIniter.GetUsers())
                {
                    _ = userManager.CreateAsync(user, UserConsts.DefaultUserPassword).Result;
                }
            }

            InitializeDatabase(app);
        }
    }

    public static class DatabaseIniter
    {
        public static List<ApplicationUser> GetUsers()
        {
            var result = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "zhrjin",
                    IsDeleted = false,
                    Email = "zhrjin@163.com",
                    PasswordHash = "123456".Sha256(),
                    EmailConfirmed = true,
                    PhoneNumber = "18100185078",
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = true,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                }
            };
            return result;
        }
    }
}
