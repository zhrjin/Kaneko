using Kaneko.Server.SkyAPM.CAP.Diagnostics;
using Kaneko.Server.SkyAPM.MongoDB.Diagnostic;
using Kaneko.Server.SkyAPM.Orleans.Diagnostic;
using Microsoft.Extensions.DependencyInjection;
using SkyApm;
using SkyApm.AspNetCore.Diagnostics;
using SkyApm.Diagnostics.AspNetCore.Handlers;
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
            services.AddSingleton<ISamplingInterceptor, IgnoreSamplingInterceptor>();
            //services.AddSingleton<ITracingDiagnosticProcessor, MongoTracingDiagnosticProcessor>();
            services.AddSingleton<ITracingDiagnosticProcessor, CapTracingDiagnosticProcessor>();
            services.AddSingleton<ITracingDiagnosticProcessor, KanekoDiagnosticProcessor>();

            services.AddSingleton<ITracingDiagnosticProcessor, HostingTracingDiagnosticProcessor>();
            services.AddSingleton<IHostingDiagnosticHandler, DefaultHostingDiagnosticHandler>();
            services.AddSingleton<IHostingDiagnosticHandler, GrpcHostingDiagnosticHandler>();

            return services;
        }
    }
}
