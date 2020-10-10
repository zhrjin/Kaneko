using Kaneko.Core.DependencyInjection;
using Kaneko.Dapper.Contract;
using Kaneko.Dapper.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using YTSoft.CC.IGrains.MaterialManager.Domain;

namespace YTSoft.CC.Grains.MaterialManager.Repository
{
    public interface IMaterialRepository : IBaseRepository<MaterialDO>
    {
    }

    [ServiceDescriptor(typeof(IMaterialRepository), ServiceLifetime.Transient)]
    public class MaterialRepository : BaseRepository<MaterialDO>, IMaterialRepository
    {
        public MaterialRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public override Func<string> TableNameFunc => () =>
        {
            var tableName = $"{GetMainTableName()}";
            return tableName;
        };
    }
}
