using Kaneko.Core.Attributes;
using System;
namespace Kaneko.Core.Contract
{
    /// <summary>
    /// 领域实体DO抽象实现类 带修改人中文名称
    /// </summary>
    /// <typeparam name="PrimaryKey"></typeparam>
    public abstract class SqlServerWithOperatorBaseDO<PrimaryKey> : SqlServerBaseDO<PrimaryKey>
    {
        /// <summary>
        /// 创建人姓名
        /// </summary>
        [KanekoColumn(Name = "create_by_name", ColumnDefinition = "nvarchar(36) null")]
        public virtual string CreateByName { set; get; }

        /// <summary>
        /// 修改人姓名
        /// </summary>
        [KanekoColumn(Name = "modity_by_name", ColumnDefinition = "nvarchar(36) null")]
        public virtual string ModityByName { set; get; }
    }

    /// <summary>
    /// 领域实体DO抽象实现类
    /// </summary>
    public abstract class SqlServerBaseDO<PrimaryKey> : IDomainObject
    {
        /// <summary>
        /// 主键
        /// </summary>
        [KanekoId]
        [KanekoColumn(Name = "id", ColumnDefinition = "bigint not null primary key")]
        public virtual PrimaryKey Id { set; get; }

        /// <summary>
        /// 创建人
        /// </summary>
        [KanekoColumn(Name = "create_by", ColumnDefinition = "varchar(36) null")]
        public virtual string CreateBy { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [KanekoColumn(Name = "create_date", ColumnDefinition = "datetime null")]
        public virtual DateTime CreateDate { set; get; }

        /// <summary>
        /// 修改人
        /// </summary>
        [KanekoColumn(Name = "modity_by", ColumnDefinition = "varchar(36) null")]
        public virtual string ModityBy { set; get; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [KanekoColumn(Name = "modity_date", ColumnDefinition = "datetime null")]
        public virtual DateTime ModityDate { set; get; }

        /// <summary>
        /// 版本号,乐观锁
        /// </summary>
        [KanekoColumn(Name = "version", ColumnDefinition = "int not null default 1")]
        public virtual int Version { set; get; }

        /// <summary>
        /// 是否删除 1-已删除
        /// </summary>
        [KanekoColumn(Name = "is_del", ColumnDefinition = "tinyint not null default 1")]
        public virtual int IsDel { set; get; }
    }

    /// <summary>
    /// 用作泛型约束，表示继承自该接口的为领域实体DO
    /// </summary>
    public interface IDomainObject
    {
    }
}
