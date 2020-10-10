using Kaneko.Core.DependencyInjection;
using Kaneko.Dapper.Contract;
using Kaneko.Dapper.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using YTSoft.CC.IGrains.HyFormulaMaterManager.Domain;

namespace YTSoft.CC.Grains.HyFormulaMaterManager.Repository
{
    public interface IHyFormulaMaterRepository : IBaseRepository<HyFormulaMaterDO>
    {
    }

    [ServiceDescriptor(typeof(IHyFormulaMaterRepository), ServiceLifetime.Transient)]
    public class XsLadeBaseRepository : BaseRepository<HyFormulaMaterDO>, IHyFormulaMaterRepository
    {
        public XsLadeBaseRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public override Func<string> TableNameFunc => () =>
        {
            var tableName = $"{GetMainTableName()}";
            return tableName;
        };
    }
}
