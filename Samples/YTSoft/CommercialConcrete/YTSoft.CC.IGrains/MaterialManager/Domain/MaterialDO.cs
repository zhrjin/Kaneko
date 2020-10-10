using AutoMapper;
using Kaneko.Core.Attributes;
using Kaneko.Core.Contract;

namespace YTSoft.CC.IGrains.MaterialManager.Domain
{
    /// <summary>
    /// isAutoUpdate：false 不自动更新表结果
    /// </summary>
    [KanekoTable(name: "t_material", isAutoUpdate: false)]
    public class MaterialDO : IDomainObject, IViewObject
    {
        /// <summary>
        /// 任务ID，主键
        /// </summary>
        [KanekoId]
        [KanekoColumn(Name = "id", ColumnDefinition = "varchar(20) not null primary key")]
        public string Id { set; get; }

        /// <summary>
        /// 物料名称
        /// </summary>
        [KanekoColumn(Name = "item_name", ColumnDefinition = "varchar(255) null")]
        public string ItemName { set; get; }

        /// <summary>
        /// 物料编号
        /// </summary>
        [KanekoColumn(Name = "item_number", ColumnDefinition = "varchar(120) null")]
        public string ItemNumber { set; get; }

        /// <summary>
        /// 规格型号
        /// </summary>
        [KanekoColumn(Name = "item_model", ColumnDefinition = "varchar(255) null")]
        public string ItemModel { set; get; }
    }
}
