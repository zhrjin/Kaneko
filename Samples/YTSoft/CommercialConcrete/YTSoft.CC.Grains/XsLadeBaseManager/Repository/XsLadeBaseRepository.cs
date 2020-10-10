using Kaneko.Core.DependencyInjection;
using Kaneko.Dapper.Contract;
using Kaneko.Dapper.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using YTSoft.CC.IGrains.XsLadeBaseManager.Domain;

namespace YTSoft.CC.Grains.XsLadeBaseManager.Repository
{
    public interface IXsLadeBaseRepository : IBaseRepository<XsLadeBaseDO>
    {
    }

    [ServiceDescriptor(typeof(IXsLadeBaseRepository), ServiceLifetime.Transient)]
    public class XsLadeBaseRepository : BaseRepository<XsLadeBaseDO>, IXsLadeBaseRepository
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
