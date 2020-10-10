using Kaneko.Core.Attributes;
using Kaneko.Core.Contract;
using AutoMapper;

namespace YTSoft.BasicData.IGrains.DataDictionary.Model
{
    [AutoMap(typeof(DictDTO))]
    [KanekoTable(name: "bd_dict")]
    public class DictDO : SqlServerBaseDO<long>
    {
        /// <summary>
        /// 父ID
        /// </summary>
        [KanekoColumn(Name = "parent_id", ColumnDefinition = "int not null default 0")]
        public int Parentid { set; get; }

        /// <summary>
        /// 字典类型
        /// </summary>
        [KanekoColumn(Name = "data_type", ColumnDefinition = "varchar(40) null")]
        public string DataType { set; get; }

        /// <summary>
        /// 字典键
        /// </summary>
        [KanekoColumn(Name = "data_code", ColumnDefinition = "varchar(40) null")]
        public string DataCode { set; get; }

        /// <summary>
        /// 字典值
        /// </summary>
        [KanekoColumn(Name = "data_value", ColumnDefinition = "varchar(255) null")]
        public string DataValue { set; get; }

        /// <summary>
        /// 描述
        /// </summary>
        [KanekoColumn(Name = "data_desc", ColumnDefinition = "varchar(255) null")]
        public string DataDesc { set; get; }

        /// <summary>
        /// 顺序
        /// </summary>
        [KanekoColumn(Name = "sort_no", ColumnDefinition = "int not null default 1")]
        public int SortNo { set; get; }

        /// <summary>
        /// 公司编号
        /// </summary>
        [KanekoColumn(Name = "firm", ColumnDefinition = "varchar(20)  null")]
        public string Firm { set; get; }
    }
}
