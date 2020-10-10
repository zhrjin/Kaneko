using AutoMapper;
using Kaneko.Core.Attributes;
using Kaneko.Core.Contract;
using YTSoft.BasicData.IGrains.DataSync.Model;

namespace YTSoft.BasicData.IGrains.DataDictionary.Model
{
    /// <summary>
    /// 系统配置
    /// </summary>
    [AutoMap(typeof(SystemInfoAllDTO))]
    [KanekoTable(name: "bd_sync_system")]
    public class SystemInfoDO : SqlServerBaseDO<long>
    {
        /// <summary>
        /// 系统名称
        /// </summary>
        [KanekoColumn(Name = "system_name", ColumnDefinition = "nvarchar(255) null")]
        public string SystemName { set; get; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        [KanekoColumn(Name = "db_connection", ColumnDefinition = "varchar(255) null")]
        public string DbConnection { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        [KanekoColumn(Name = "desc", ColumnDefinition = "nvarchar(255) null")]
        public string Desc { set; get; }

        /// <summary>
        /// 公司编号
        /// </summary>
        [KanekoColumn(Name = "firm", ColumnDefinition = "varchar(20) null")]
        public string Firm { set; get; }

        /// <summary>
        /// 产线
        /// </summary>
        [KanekoColumn(Name = "line", ColumnDefinition = "varchar(20) null")]
        public string Line { set; get; }
    }
}
