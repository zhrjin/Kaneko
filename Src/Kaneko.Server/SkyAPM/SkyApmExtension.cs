using Kaneko.Server.SkyAPM.CAP.Diagnostics;
using Kaneko.Server.SkyAPM.Orleans.Diagnostic;
using Kaneko.Server.SkyAPM.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using SkyApm;
using SkyApm.AspNetCore.Diagnostics;
using SkyApm.Diagnostics.SqlClient;
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
            //官方Skywalking添加
            services.AddSkyAPM(ext => ext.AddAspNetCoreHosting());

            //添加自定义
            services.AddSingleton<ISamplingInterceptor, IgnoreSamplingInterceptor>();
            //services.AddSingleton<ITracingDiagnosticProcessor, MongoTracingDiagnosticProcessor>();
            services.AddSingleton<ITracingDiagnosticProcessor, CapTracingDiagnosticProcessor>();
            services.AddSingleton<ITracingDiagnosticProcessor, KanekoDiagnosticProcessor>();

            //替换SqlClient 原sql监控无耗时记录
            services.RemoveByImpl<SqlClientTracingDiagnosticProcessor>();
            services.AddSingleton<ITracingDiagnosticProcessor, SqlClientDiagnosticProcessor>();
            return services;
        }

        private static IServiceCollection RemoveByImpl<T>(this IServiceCollection collection)
        {
            for (int num = collection.Count - 1; num >= 0; num--)
            {
                if (collection[num].ImplementationType == typeof(T))
                {
                    collection.RemoveAt(num);
                }
            }
            return collection;
        }

    }
}
