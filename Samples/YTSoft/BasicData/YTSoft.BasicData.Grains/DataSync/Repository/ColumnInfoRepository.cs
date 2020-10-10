using Kaneko.Core.DependencyInjection;
using Kaneko.Dapper.Contract;
using Kaneko.Dapper.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using YTSoft.BasicData.IGrains.DataSync.Model;

namespace YTSoft.BasicData.Grains.DataSync.Repository
{
    public interface IColumnInfoRepository : IBaseRepository<ColumnInfoDO>
    {
    }

    [ServiceDescriptor(typeof(IColumnInfoRepository), ServiceLifetime.Transient)]
    public class ColumnInfoRepository : BaseRepository<ColumnInfoDO>, IColumnInfoRepository
    {
        public ColumnInfoRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public override Func<string> TableNameFunc => () =>
        {
            var tableName = $"{GetMainTableName()}";
            return tableName;
        };
    }
}
