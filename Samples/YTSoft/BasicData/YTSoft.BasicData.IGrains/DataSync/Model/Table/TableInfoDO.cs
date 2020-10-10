using AutoMapper;
using Kaneko.Core.Attributes;
using Kaneko.Core.Contract;
namespace YTSoft.BasicData.IGrains.DataSync.Model
{
    /// <summary>
    /// 表配置
    /// </summary>
    [KanekoTable(name: "bd_sync_table")]
    [AutoMap(typeof(TableInfoDTO))]
    public class TableInfoDO : SqlServerBaseDO<long>
    {
        /// <summary>
        /// 组别
        /// </summary>
        [KanekoColumn(Name = "group_code", ColumnDefinition = "varchar(40) not null")]
        public string GroupCode { set; get; }

        /// <summary>
        /// 系统ID
        /// </summary>
        [KanekoColumn(Name = "system_id", ColumnDefinition = "bigint not null")]
        public long SystemId { set; get; }

        /// <summary>
        /// 表名称
        /// </summary>
        [KanekoColumn(Name = "table_name", ColumnDefinition = "varchar(30) not null")]
        public string TableName { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        [KanekoColumn(Name = "desc", ColumnDefinition = "nvarchar(255) null")]
        public string Desc { set; get; }
    }
}
