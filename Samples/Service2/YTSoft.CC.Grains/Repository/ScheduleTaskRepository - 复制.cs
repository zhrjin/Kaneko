using Dapper;
using Kaneko.Core.DependencyInjection;
using Kaneko.Dapper.Contract;
using Kaneko.Dapper.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.Entity;

namespace YTSoft.CC.Grains.Repository
{
    public interface IScheduleTaskRepository1 : IBaseRepository<ScheduleTaskDO1>
    {
        Task<ScheduleTaskDO> GetModelAsync(string id);
    }

    [ServiceDescriptor(typeof(IScheduleTaskRepository1), ServiceLifetime.Transient)]
    public class ScheduleTaskRepository1 : BaseRepository<ScheduleTaskDO1>, IScheduleTaskRepository1
    {
        public ScheduleTaskRepository1(IConfiguration configuration) : base(configuration)
        {
        }

        public override Func<string> TableNameFunc => () =>
        {
            var tableName = $"{GetMainTableName()}";
            return tableName;
        };

        public async Task<ScheduleTaskDO> GetModelAsync(string id)
        {
            var tableName = GetTableName();
            var sql = $"select * from [{tableName}] where id='{id}'";
            return await Execute(async connecdtion =>
            {
                var task = await connecdtion.QueryFirstAsync<ScheduleTaskDO>(sql);
                return task;
            });
        }
    }
}
