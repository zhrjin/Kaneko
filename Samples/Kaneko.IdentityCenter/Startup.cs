using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Kaneko.IdentityCenter.Configurations;
using Kaneko.IdentityCenter.Data;
using Kaneko.IdentityCenter.Entities;
using Kaneko.IdentityCenter.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AspNetAccountDbContext>(u => u.UseSqlServer(Configuration.GetConnectionString("DefaultAspNetAccountConnection")));
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

                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
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
                })
                .AddConfigurationStoreCache()
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<KanekoProfileService>()
                //.AddDeveloperSigningCredential()
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

            //Task.Run(() => InitDatabase(app));
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                //context.Database.EnsureCreated();
                var sql = context.Database.GenerateCreateScript();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
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
            using var ctx = scope.ServiceProvider.GetService<ApplicationDbContext>();
            ctx.Database.EnsureCreated();

            using var userManager = scope.ServiceProvider.GetService<UserManager<SysUser>>();
            if (!userManager.Users.Any())
            {
                foreach (var user in DatabaseIniter.GetUsers())
                {
                    _ = userManager.CreateAsync(user, Consts.DefaultUserPassword).Result;
                }
            }

            InitializeDatabase(app);
        }
    }

    public static class DatabaseIniter
    {
        public static List<SysUser> GetUsers()
        {
            var result = new List<SysUser>
            {
                new SysUser
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

    public class StartupBak
    {
        public StartupBak(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AspNetAccountDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultAspNetAccountConnection"));
            });
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AspNetAccountDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
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
                })
                .AddAspNetIdentity<ApplicationUser>();

            builder.AddDeveloperSigningCredential();

            //services.AddScoped<ConsentService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    //foreach (var client in Config.Clients)
                    //{
                    //    context.Clients.Add(client.ToEntity());
                    //}
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    //foreach (var resource in Config.IdentityResources)
                    //{
                    //    context.IdentityResources.Add(resource.ToEntity());
                    //}
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    //foreach (var resource in Config.ApiScopes)
                    //{
                    //    context.ApiScopes.Add(resource.ToEntity());
                    //}
                    context.SaveChanges();
                }
            }
        }
    }
}
