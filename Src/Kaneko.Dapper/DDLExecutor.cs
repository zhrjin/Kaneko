﻿using Kaneko.Dapper.Repository;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Kaneko.Dapper
{
    public class DDLExecutor : IDDLExecutor
    {
        readonly IServiceProvider ServiceProvider;
        public DDLExecutor(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// 自动生成表结构
        /// </summary>
        /// <param name="grainAssembly"></param>
        /// <returns></returns>
        public Task AutoAlterDbSchema(Assembly grainAssembly)
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
                        exec.DDLExecutor();
                    }
                }
                catch { }
            }

            return Task.CompletedTask;
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
