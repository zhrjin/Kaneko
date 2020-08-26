using Microsoft.Extensions.Configuration;
using System.IO;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Kaneko.Dapper.Enums;

namespace Kaneko.Dapper.DataContext
{
    /// <summary>
    /// 数据库连接工具类
    /// </summary>
    public class DataContext : IDisposable
    {
        readonly bool _isMaster;
        readonly string _dbStoreKey;
        DbProviderFactory _dbFactory;

        readonly IConfiguration _configuration;
        readonly Func<bool, string> _connectionFunc;

        /// <summary>
        /// 连接对象
        /// </summary>
        public IDbConnection DbConnection { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration">if null 则从appsettings.json中获取</param>
        /// <param name="isMaster">是否从库</param>
        /// <param name="dbStoreKey">存储字符串标识</param>
        /// <param name="connectionFunc">连接字符串Func</param>
        public DataContext(IConfiguration configuration, bool isMaster = false, string dbStoreKey = "", Func<bool, string> connectionFunc = null)
        {
            _configuration = configuration;
            _isMaster = isMaster;
            _dbStoreKey = dbStoreKey;
            _connectionFunc = connectionFunc;

            // 打开连接
            CreateAndOpen();
        }

        /// <summary>
        /// 打开链接
        /// </summary>
        private void CreateAndOpen()
        {
            string connectionString = string.Empty;
            DataSettings settings = DataSettings.Default;

            // 获取连接
            var connectionSetting = settings.Get(_configuration, _isMaster, _dbStoreKey, _connectionFunc);
            connectionString = connectionSetting.Item1;
            _dbFactory = GetFactory(connectionSetting.Item2);

            if (string.IsNullOrEmpty(connectionString))
                throw new Exception($"连接字符串获取为空，请检查Repository是否指定了dbStoreKey以及检查配置文件是否存在");

            DbConnection = _dbFactory.CreateConnection();
            DbConnection.ConnectionString = connectionString;
            if (DbConnection.State != ConnectionState.Open)
                DbConnection.Open();
        }

        /// <summary>
        /// 获取Factory
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        private DbProviderFactory GetFactory(DatabaseType dbType)
        {
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                case DatabaseType.GteSqlServer2012:
                    return SqlClientFactory.Instance;
                case DatabaseType.MySql:
                    return MySqlClientFactory.Instance;
                case DatabaseType.SQLite:
                    AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data"));
                    return Microsoft.Data.Sqlite.SqliteFactory.Instance;
            }
            return null;
        }

        /// <summary>
        /// 垃圾回收
        /// </summary>
        public void Dispose()
        {
            if (DbConnection == null)
                return;
            try
            {
                DbConnection.Dispose();
            }
            catch { }
        }
    }
}
