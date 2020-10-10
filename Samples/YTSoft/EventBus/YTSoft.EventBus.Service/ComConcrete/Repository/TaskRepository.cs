using Dapper;
using Kaneko.Core.Contract;
using Kaneko.Dapper.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Threading.Tasks;

namespace YTSoft.EventBus.Service.ComConcrete.Repository
{
    public class TaskRepository : BaseRepository<IDomainObject>
    {
        public TaskRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public override Func<string> TableNameFunc => () =>
        {
            var tableName = $"{GetMainTableName()}";
            return tableName;
        };

        public async Task<bool> ExecuteSqlAsync(string sql, object param = null)
        {
            int nRet = await Execute(async connecdtion =>
            {
                var task = await connecdtion.ExecuteAsync(sql, param);
                return task;
            });

            return nRet > 0;
        }
    }
}
