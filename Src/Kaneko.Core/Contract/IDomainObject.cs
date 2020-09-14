using Kaneko.Core.Attributes;
using System;
namespace Kaneko.Core.Contract
{
    /// <summary>
    /// 领域实体DO抽象实现类
    /// </summary>
    public abstract class BaseDO : IDomainObject
    {
        /// <summary>
        /// 创建人
        /// </summary>
        [KanekoColumn(Name = "create_by", ColumnDefinition = "varchar(20) null")]
        public string CreateBy { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [KanekoColumn(Name = "create_date", ColumnDefinition = "datetime null")]
        public DateTime CreateDate { set; get; }

        /// <summary>
        /// 修改人
        /// </summary>
        [KanekoColumn(Name = "modity_by", ColumnDefinition = "varchar(20) null")]
        public string ModityBy { set; get; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [KanekoColumn(Name = "modity_date", ColumnDefinition = "datetime null")]
        public DateTime ModityDate { set; get; }

        /// <summary>
        /// 版本号,乐观锁
        /// </summary>
        [KanekoColumn(Name = "version", ColumnDefinition = "int null")]
        public int Version { set; get; }

        /// <summary>
        /// 是否删除 1-已删除
        /// </summary>
        [KanekoColumn(Name = "is_del", ColumnDefinition = "int null")]
        public int IsDel { set; get; }
    }

    /// <summary>
    /// 用作泛型约束，表示继承自该接口的为领域实体DO
    /// </summary>
    public interface IDomainObject
    {
    }
}
