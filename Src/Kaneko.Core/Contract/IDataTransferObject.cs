using Kaneko.Core.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kaneko.Core.Contract
{
    /// <summary>
    /// 数据传输对象DTO抽象实现类
    /// </summary>
    public abstract class BaseDTO : IDataTransferObject
    {
        /// <summary>
        /// 页标
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页笔数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName{ get; set; }

        /// <summary>
        /// 版本号,乐观锁
        /// </summary>
        public int Version { set; get; }

        /// <summary>
        /// 排序字段
        /// </summary>
        /// <returns></returns>
        public virtual OrderByField[] GetOrder()
        {
            return null;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }

    /// <summary>
    /// 数据传输对象DTO
    /// </summary>
    public interface IDataTransferObject : IValidatableObject
    {
        /// <summary>
        /// 页标
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// 每页笔数
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        /// <returns></returns>
        OrderByField[] GetOrder();
    }

}
