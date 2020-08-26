using System;
using System.Data;
using System.Threading.Tasks;
using Kaneko.Dapper.Contract;
using Microsoft.Extensions.Configuration;

namespace Kaneko.Dapper.Repository
{
    /// <summary>
    /// DbRespository 抽象类
    /// </summary>
    public abstract class PropertyAssist : IPropertyAssist
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="configuration">配置注入</param>
        /// <param name="dbStoreKey">数据库前缀</param>
        public PropertyAssist(IConfiguration configuration, string dbStoreKey = "")
        {
            _configuration = configuration;
            DbStoreKey = dbStoreKey;
        }

        /// <summary>
        /// 数据库名key
        /// </summary>
        public string DbStoreKey { get; set; }

        /// <summary>
        /// 事务
        /// </summary>
        public virtual IDbTransaction Transaction { get; set; }

        /// <summary>
        /// 是否在事务中
        /// </summary>
        public virtual bool InTransaction
        {
            get
            {
                return Transaction?.Connection != null;
            }
        }

        /// <summary>
        /// 打开连接 已赋值 connection 属性
        /// </summary>
        /// <returns></returns>
        public virtual IDbConnection OpenConnection(bool isMaster = false, bool ignoreTransaction = false)
        {
            IDbConnection connection;

            if (ignoreTransaction || !InTransaction)
                connection = new  DataContext.DataContext(_configuration, isMaster, DbStoreKey, ConnectionFunc).DbConnection;
            else
                connection = Transaction.Connection;


            if (connection == null)
                throw new Exception("数据库连接创建失败，请检查连接字符串是否正确...");

            if (connection.State != ConnectionState.Open)
                connection.Open();

            return connection;
        } 
       
        /// <summary>
        /// 数据库连接方法
        /// </summary>
        public virtual Func<bool, string> ConnectionFunc { get; set; }

        /// <summary>
        /// 表名方法
        /// </summary>
        public virtual Func<string> TableNameFunc { get; set; }

        /// <summary>
        /// 创建表的脚本
        /// </summary>
        public virtual Func<string, string> CreateScriptFunc { get; set; }

        /// <summary>
        /// 执行的sql脚本
        /// </summary>
        public virtual string ExecuteScript { get; set; }

        /// <summary>
        /// 自动生存表结构
        /// </summary>
        /// <returns></returns>
        public abstract Task<bool> DDLExecutor();
    }
}
