using System;
using System.Data;
using System.Threading.Tasks;

namespace Kaneko.Dapper.Contract
{
    /// <summary>
    /// 通用的接口
    /// </summary>
    public interface IPropertyAssist
    {
        /// <summary>
        /// 数据库key
        /// </summary>
        string DbStoreKey { get; set; }

        /// <summary>
        /// 事务
        /// </summary>
        IDbTransaction Transaction { get; set; }

        /// <summary>
        /// 是否在事务中
        /// </summary>
        bool InTransaction { get; }

        /// <summary>
        /// 表名生成方法
        /// </summary>
        Func<string> TableNameFunc { get; set; }

        /// <summary>
        /// 创建表的sql语句
        /// 参数：表名
        /// </summary>
        Func<string, string> CreateScriptFunc { get; set; }

        /// <summary>
        /// 连接方法创建
        /// </summary>
        Func<bool, string> ConnectionFunc { get; set; }

        /// <summary>
        /// 打开连接
        /// </summary>
        /// <returns></returns>
        IDbConnection OpenConnection(bool isMaster = false, bool ignoreTransaction = false);

        /// <summary>
        /// 执行的sql脚本
        /// </summary>
        string ExecuteScript { get; set; }

        /// <summary>
        /// 自动生存表结构
        /// </summary>
        /// <returns></returns>
        Task<bool> DDLExecutor();
    }
}
