using Kaneko.Core.DependencyInjection;
using Kaneko.Dapper.Contract;
using Kaneko.Dapper.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using YTSoft.BasicData.IGrains.DataDictionary.Model;

namespace YTSoft.BasicData.Grains.DataSync.Repository
{
    public interface ISystemInfoRepository : IBaseRepository<SystemInfoDO>
    {
    }

    [ServiceDescriptor(typeof(ISystemInfoRepository), ServiceLifetime.Transient)]
    public class SystemInfoRepository : BaseRepository<SystemInfoDO>, ISystemInfoRepository
    {
        public SystemInfoRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public override Func<string> TableNameFunc => () =>
        {
            var tableName = $"{GetMainTableName()}";
            return tableName;
        };
    }
}
