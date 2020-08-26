using System;
using System.Text.RegularExpressions;
using Kaneko.Dapper.Enums;
using Microsoft.Extensions.Configuration;

namespace Kaneko.Dapper.DataContext
{
    /// <summary>
    /// 连接配置信息获取
    /// 1. master / secondary
    /// 2. xx.master / xx.secondary
    /// </summary>
    public class DataSettings
    {
        static readonly string _connNmeOfMaster = "master";
        static readonly string _connNameOfSecondary = "secondary";
        static readonly string _connNameOfPoint = ".";
        static string _connNameOfPrefix = "";
     
        /// <summary>
        /// 主库
        /// </summary>
        private string Master
        {
            get { return $"{_connNameOfPrefix}{_connNmeOfMaster}"; }
        }
        /// <summary>
        /// 从库
        /// </summary>
        private string Secondary
        {
            get
            {
                return $"{_connNameOfPrefix}{_connNameOfSecondary}";
            }
        }
     
        static readonly object lockHelper = new object();
        static volatile DataSettings _Default;
        /// <summary>
        /// 单例模式
        /// </summary>
        static public DataSettings Default
        {
            get
            {
                if (_Default == null)
                {
                    lock (lockHelper)
                    {
                        _Default ??= new DataSettings();
                    }
                }
                return _Default;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        private DataSettings()
        {
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="isMaster"></param>
        /// <param name="dbStoreKey"></param>
        /// <param name="connectionFunc"></param>
        /// <returns></returns>
        public (string, DatabaseType) Get(IConfiguration configuration, bool isMaster, string dbStoreKey, Func<bool, string> connectionFunc = null)
        {
            string connectionString;
            if (connectionFunc != null)
            {
                connectionString = connectionFunc.Invoke(isMaster);
                return ResolveConnectionString(connectionString);
            }

            if (configuration == null)
            {
                configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
            }

            var connectionKey = GetKey(isMaster, dbStoreKey);
            connectionString = configuration.GetConnectionString(connectionKey);
            if (string.IsNullOrEmpty(connectionString) && !isMaster)
            {
                // 从库转主库
                connectionKey = GetKey(true, dbStoreKey);
                connectionString = configuration.GetConnectionString(connectionKey);
            }

            return ResolveConnectionString(connectionString);
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private (string, DatabaseType) ResolveConnectionString(string connectionString, string param = "DbType")
        {
            var dbTypeRegex = new Regex($@"(^|;){param}=(?<dbtype>[A-Za-z]+)(;|$)");
            var m = dbTypeRegex.Match(connectionString);
            var dbTypeString = m?.Groups["dbtype"].Value;
            var parseResult = Enum.TryParse(dbTypeString, out DatabaseType dbType);
            if (!parseResult)
                dbType = DatabaseType.MySql;

            connectionString = Regex.Replace(connectionString, $@"{param}=([A-Za-z]+)(;|$)", "");
            return (connectionString, dbType);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="isMaster"></param>
        /// <param name="store">不能包含点</param>
        /// <returns></returns>
        private string GetKey(bool isMaster = false, string store = "")
        {
            _connNameOfPrefix = string.IsNullOrEmpty(store) ? "" : $"{store}{_connNameOfPoint}";
            string connName;
            if (isMaster)
                connName = Master;
            else
                connName = Secondary;

            return connName;
        }
    }
}
