using Kaneko.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        /// 验证器
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }

    /// <summary>
    /// 用作泛型约束，表示继承自该接口的为领域实体DO
    /// </summary>
    public interface IDomainObject : IValidatableObject
    {
    }
}
