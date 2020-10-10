using Kaneko.Core.DependencyInjection;
using Kaneko.Dapper.Contract;
using Kaneko.Dapper.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using YTSoft.CC.IGrains.XsLadeRimpactManager.Domain;

namespace YTSoft.CC.Grains.XsLadeRimpactManager.Repository
{
    public interface IXsLadeRimpactRepository : IBaseRepository<XsLadeRimpactDO>
    {
    }

    [ServiceDescriptor(typeof(IXsLadeRimpactRepository), ServiceLifetime.Transient)]
    public class XsLadeBaseRepository : BaseRepository<XsLadeRimpactDO>, IXsLadeRimpactRepository
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
