using Kaneko.Core.DependencyInjection;
using Kaneko.Dapper.Contract;
using Kaneko.Dapper.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using YTSoft.BasicData.IGrains.DataSync.Model;

namespace YTSoft.BasicData.Grains.DataSync.Repository
{
    public interface ITableInfoRepository : IBaseRepository<TableInfoDO>
    {
    }

    [ServiceDescriptor(typeof(ITableInfoRepository), ServiceLifetime.Transient)]
    public class TableInfoRepository : BaseRepository<TableInfoDO>, ITableInfoRepository
    {
        public TableInfoRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public override Func<string> TableNameFunc => () =>
        {
            var tableName = $"{GetMainTableName()}";
            return tableName;
        };
    }
}
