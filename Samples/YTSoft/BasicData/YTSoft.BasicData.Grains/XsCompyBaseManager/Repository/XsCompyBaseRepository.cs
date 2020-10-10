using Kaneko.Core.DependencyInjection;
using Kaneko.Dapper.Contract;
using Kaneko.Dapper.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using YTSoft.BasicData.IGrains.XsCompyBaseManager.Domain;

namespace YTSoft.BasicData.Grains.XsCompyBaseManager.Repository
{
    public interface IXsCompyBaseRepository : IBaseRepository<XsCompyBaseDO>
    {
    }

    [ServiceDescriptor(typeof(IXsCompyBaseRepository), ServiceLifetime.Transient)]
    public class XsCompyBaseRepository : BaseRepository<XsCompyBaseDO>, IXsCompyBaseRepository
    {
        public XsCompyBaseRepository(IConfiguration configuration) : base(configuration, "group")
        {
        }

        public override Func<string> TableNameFunc => () =>
        {
            var tableName = $"{GetMainTableName()}";
            return tableName;
        };
    }
}
