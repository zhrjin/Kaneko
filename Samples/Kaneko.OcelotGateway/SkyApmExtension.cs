using Kaneko.OcelotGateway;
using Microsoft.Extensions.DependencyInjection;
using SkyApm.AspNetCore.Diagnostics;
using SkyApm.Tracing;

namespace Kaneko.Server.SkyAPM
{
    /// <summary>
    /// 
    /// </summary>
    public static class SkyApmExtension
    {
        /// <summary>
        /// SkyAPM
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection UseSkyApm(this IServiceCollection services)
        {
            services.AddSkyAPM(ext => ext.AddAspNetCoreHosting());
            services.AddSingleton<ISamplingInterceptor, IgnoreSamplingInterceptor>();
            return services;
        }
    }
}
