using Kaneko.Core.DependencyInjection;
using Kaneko.Dapper.Contract;
using Kaneko.Dapper.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using YTSoft.BasicData.IGrains.DataDictionary.Model;

namespace YTSoft.BasicData.Grains.DataDictionary.Repository
{
    public interface IDictRepository : IBaseRepository<DictDO>
    {
    }

    [ServiceDescriptor(typeof(IDictRepository), ServiceLifetime.Transient)]
    public class DictRepository : BaseRepository<DictDO>, IDictRepository
    {
        public DictRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public override Func<string> TableNameFunc => () =>
        {
            var tableName = $"{GetMainTableName()}";
            return tableName;
        };
    }
}
