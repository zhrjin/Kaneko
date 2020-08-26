using Dapper;
using Kaneko.Core.DependencyInjection;
using Kaneko.Dapper.Contract;
using Kaneko.Dapper.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MSDemo.IGrains.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSDemo.Grains.Repository
{
    public interface ITestRepository : IBaseRepository<TestDO>
    {
    }

    [ServiceDescriptor(typeof(ITestRepository), ServiceLifetime.Transient)]
    public class TestRepository : BaseRepository<TestDO>, ITestRepository
    {
        public TestRepository(IConfiguration configuration) : base(configuration)
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
