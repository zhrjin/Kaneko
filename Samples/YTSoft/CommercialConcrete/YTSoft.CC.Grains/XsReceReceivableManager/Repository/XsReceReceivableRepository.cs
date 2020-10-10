using Kaneko.Core.DependencyInjection;
using Kaneko.Dapper.Contract;
using Kaneko.Dapper.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using YTSoft.CC.IGrains.XsReceReceivableManager.Domain;

namespace YTSoft.CC.Grains.XsReceReceivableManager.Repository
{
    public interface IXsReceReceivableRepository : IBaseRepository<XsReceReceivableDO>
    {
    }

    [ServiceDescriptor(typeof(IXsReceReceivableRepository), ServiceLifetime.Transient)]
    public class XsLadeBaseRepository : BaseRepository<XsReceReceivableDO>, IXsReceReceivableRepository
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
