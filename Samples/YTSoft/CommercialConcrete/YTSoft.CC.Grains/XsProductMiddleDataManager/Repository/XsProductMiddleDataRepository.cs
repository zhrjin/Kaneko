using Kaneko.Core.DependencyInjection;
using Kaneko.Dapper.Contract;
using Kaneko.Dapper.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using YTSoft.CC.IGrains.XsProductMiddleDataManager.Domain;

namespace YTSoft.CC.Grains.XsProductMiddleDataManager.Repository
{
    public interface IXsProductMiddleDataRepository : IBaseRepository<XsProductMiddleDataDO>
    {
    }

    [ServiceDescriptor(typeof(IXsProductMiddleDataRepository), ServiceLifetime.Transient)]
    public class XsProductMiddleDataRepository : BaseRepository<XsProductMiddleDataDO>, IXsProductMiddleDataRepository
    {
        public XsProductMiddleDataRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public override Func<string> TableNameFunc => () =>
        {
            var tableName = $"{GetMainTableName()}";
            return tableName;
        };
    }
}
