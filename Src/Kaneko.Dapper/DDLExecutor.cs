using Kaneko.Dapper.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Kaneko.Dapper
{
    public class DDLExecutor : IDDLExecutor
    {
        readonly IServiceProvider ServiceProvider;
        readonly ILogger Logger;

        public DDLExecutor(IServiceProvider serviceProvider, ILogger<DDLExecutor> logger)
        {
            this.ServiceProvider = serviceProvider;
            this.Logger = logger;
        }

        /// <summary>
        /// 自动生成表结构
        /// </summary>
        /// <param name="grainAssembly"></param>
        /// <returns></returns>
        public async Task AutoAlterDbSchema(Assembly grainAssembly)
        {
            var eventType = typeof(PropertyAssist);
            var allType = grainAssembly.GetExportedTypes().Where(t => eventType.IsAssignableFrom(t) && t.IsClass);
            foreach (var type in allType)
            {
                try
                {
                    if (type.Name != "PropertyAssist")
                    {
                        var exec = (PropertyAssist)ServiceProvider.GetService(type);
                        await exec.DDLExecutor(Logger);
                    }
                }
                catch { }
            }
        }
    }

    public interface IDDLExecutor
    {
        /// <summary>
        /// 自动生成表结构
        /// </summary>
        /// <param name="grainAssembly"></param>
        /// <returns></returns>
        Task AutoAlterDbSchema(Assembly grainAssembly);
    }
}
