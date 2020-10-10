using Kaneko.Core.DependencyInjection;
using Kaneko.Dapper.Contract;
using Kaneko.Dapper.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using YTSoft.BasicData.IGrains.PbBasicFirmManager.Domain;

namespace YTSoft.BasicData.Grains.PbBasicFirmManager.Repository
{
    public interface IPbBasicFirmRepository : IBaseRepository<PbBasicFirmDO>
    {
    }

    [ServiceDescriptor(typeof(IPbBasicFirmRepository), ServiceLifetime.Transient)]
    public class PbBasicFirmRepository : BaseRepository<PbBasicFirmDO>, IPbBasicFirmRepository
    {
        public PbBasicFirmRepository(IConfiguration configuration) : base(configuration, "group")
        {
        }

        public override Func<string> TableNameFunc => () =>
        {
            var tableName = $"{GetMainTableName()}";
            return tableName;
        };
    }
}
