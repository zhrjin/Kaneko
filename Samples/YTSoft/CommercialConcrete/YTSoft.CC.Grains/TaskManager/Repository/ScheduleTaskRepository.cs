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
using YTSoft.CC.IGrains.TaskManager.Domain;

namespace YTSoft.CC.Grains.TaskManager.Repository
{
    public interface IScheduleTaskRepository : IBaseRepository<ScheduleTaskDO>
    {
    }

    [ServiceDescriptor(typeof(IScheduleTaskRepository), ServiceLifetime.Transient)]
    public class ScheduleTaskRepository : BaseRepository<ScheduleTaskDO>, IScheduleTaskRepository
    {
        public ScheduleTaskRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public override Func<string> TableNameFunc => () =>
        {
            var tableName = $"{GetMainTableName()}";
            return tableName;
        };

        public async Task<List<string>> OtherSqlAsync()
        {
            var tableName = GetTableName();
            var sql = $"select distinct([UserName]) from [{tableName}]";
            return await Execute(async connecdtion =>
            {
                var task = await connecdtion.QueryAsync<string>(sql);
                return task.ToList();
            });
        }
    }
}
