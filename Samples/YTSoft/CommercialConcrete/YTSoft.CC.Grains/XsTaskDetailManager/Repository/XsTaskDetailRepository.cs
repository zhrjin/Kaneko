﻿using Dapper;
using Kaneko.Core.DependencyInjection;
using Kaneko.Dapper.Contract;
using Kaneko.Dapper.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTSoft.CC.IGrains.XsTaskDetailManager.Domain;
namespace YTSoft.CC.Grains.XsTaskDetailManager.Repository
{
    public interface IXsTaskDetailRepository : IBaseRepository<XsTaskDetailDO>
    {
    }

    [ServiceDescriptor(typeof(IXsTaskDetailRepository), ServiceLifetime.Transient)]
    public class XsTaskDetailRepository : BaseRepository<XsTaskDetailDO>, IXsTaskDetailRepository
    {
        public XsTaskDetailRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public override Func<string> TableNameFunc => () =>
        {
            var tableName = $"{GetMainTableName()}";
            return tableName;
        };

        public async Task<List<string>> OtherSqlAsync()
        {
            var tableName = GetTableName();
            var sql = $"select distinct([UserName]) from [{tableName}]";
            return await Execute(async connecdtion =>
            {
                var task = await connecdtion.QueryAsync<string>(sql);
                return task.ToList();
            });
        }
    }
}
