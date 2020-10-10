using Kaneko.Core.DependencyInjection;
using Kaneko.Dapper.Contract;
using Kaneko.Dapper.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using YTSoft.BasicData.IGrains.PbCodeMaterialManager.Domain;

namespace YTSoft.BasicData.Grains.PbCodeMaterialManager.Repository
{
    public interface IPbCodeMaterialRepository : IBaseRepository<PbCodeMaterialDO>
    {
    }

    [ServiceDescriptor(typeof(IPbCodeMaterialRepository), ServiceLifetime.Transient)]
    public class PbBasicFirmRepository : BaseRepository<PbCodeMaterialDO>, IPbCodeMaterialRepository
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
