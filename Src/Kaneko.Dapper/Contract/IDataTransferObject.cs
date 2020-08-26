using Kaneko.Dapper.Params;
using System;
using System.Linq.Expressions;

namespace Kaneko.Dapper.Contract
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
        /// 查询表达式
        /// </summary>
        /// <returns></returns>
        public virtual Expression<Func<IDomainObject, bool>> GetExpression()
        {
            return null;
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        /// <returns></returns>
        public virtual OrderByField[] GetOrder()
        {
            return null;
        }
    }

    /// <summary>
    /// 数据传输对象DTO
    /// </summary>
    public interface IDataTransferObject
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
        /// 查询表达式
        /// </summary>
        /// <returns></returns>
        Expression<Func<IDomainObject, bool>> GetExpression();

        /// <summary>
        /// 排序字段
        /// </summary>
        /// <returns></returns>
        OrderByField[] GetOrder();
    }

}
