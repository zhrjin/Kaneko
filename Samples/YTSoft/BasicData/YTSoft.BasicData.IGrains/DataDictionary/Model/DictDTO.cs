using Kaneko.Core.Contract;
using Kaneko.Core.Data;
using Kaneko.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace YTSoft.BasicData.IGrains.DataDictionary.Model
{
    public class DictDTO : BaseDTO<long>
    {
        /// <summary>
        /// 父ID
        /// </summary>
        public int Parentid { set; get; }

        /// <summary>
        /// 字典类型
        /// </summary>
        public string DataType { set; get; }

        /// <summary>
        /// 字典编号
        /// </summary>
        public string DataCode { set; get; }

        /// <summary>
        /// 字典值
        /// </summary>
        public string DataValue { set; get; }

        /// <summary>
        /// 描述
        /// </summary>
        public string DataDesc { set; get; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int SortNo { set; get; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public string Firm { set; get; }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(DataType))
                yield return new ValidationResult("字典类型不能为空！", new[] { nameof(DataType) });

            if (string.IsNullOrWhiteSpace(DataCode))
                yield return new ValidationResult("字典编号不能为空！", new[] { nameof(DataCode) });

            if (string.IsNullOrWhiteSpace(DataValue))
                yield return new ValidationResult("字典值不能为空！", new[] { nameof(DataValue) });
        }

        /// <summary>
        /// 搜索表达式
        /// </summary>
        /// <returns></returns>
        public Expression<Func<DictDO, bool>> GetExpression()
        {
            Expression<Func<DictDO, bool>> expression = oo => oo.IsDel == 0;

            if (!string.IsNullOrEmpty(DataType))
                expression = expression.And(oo => oo.DataType == DataType.Trim());

            return expression;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        public override OrderByField[] GetOrder()
        {
            var list = new List<OrderByField>()
            {
               OrderByField.Create<DictDO>(nameof(DictDO.SortNo), FieldSortType.Asc)
            };
            return list.ToArray();
        }
    }
}
