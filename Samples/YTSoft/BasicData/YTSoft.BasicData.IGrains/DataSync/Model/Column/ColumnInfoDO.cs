using AutoMapper;
using Kaneko.Core.Attributes;
using Kaneko.Core.Contract;

namespace YTSoft.BasicData.IGrains.DataSync.Model
{
    /// <summary>
    /// 列配置
    /// </summary>
    [KanekoTable(name: "bd_sync_column")]
    [AutoMap(typeof(ColumnInfoDTO))]
    public class ColumnInfoDO : SqlServerBaseDO<long>
    {
        /// <summary>
        /// 表ID
        /// </summary>
        [KanekoColumn(Name = "table_id", ColumnDefinition = "bigint not null")]
        public long TableId { set; get; } 

        /// <summary>
        /// 自身列名称
        /// </summary>
        [KanekoColumn(Name = "self_column", ColumnDefinition = "varchar(30) not null")]
        public string SelfColumn { set; get; }

        /// <summary>
        /// 模型列名称
        /// </summary>
        [KanekoColumn(Name = "model_column", ColumnDefinition = "varchar(30) not null")]
        public string ModelColumn { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        [KanekoColumn(Name = "desc", ColumnDefinition = "nvarchar(255) null")]
        public string Desc { set; get; }

        /// <summary>
        /// 顺序
        /// </summary>
        [KanekoColumn(Name = "sort_no", ColumnDefinition = "int not null default 1")]
        public int SortNo { set; get; }

        /// <summary>
        /// 展示名称
        /// </summary>
        [KanekoColumn(Name = "display_name", ColumnDefinition = "nvarchar(20) null")]
        public string DisplayName { set; get; }

    }
}
