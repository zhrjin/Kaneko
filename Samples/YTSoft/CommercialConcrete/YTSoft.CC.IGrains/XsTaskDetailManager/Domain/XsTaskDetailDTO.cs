using Kaneko.Core.Contract;
using Kaneko.Core.Data;
using Kaneko.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;


namespace YTSoft.CC.IGrains.XsTaskDetailManager.Domain
{
    public class XsTaskDetailDTO : BaseDTO<string>
    {
        public string XTD_TaskID { set; get; }

        public string XTD_Auxiliary { set; get; }

        public decimal? XTD_Number { get; set; }

        public decimal? XTD_Price { get; set; }

        public decimal? XTD_Total { get; set; }

        public int? XTD_Order { get; set; }

        public string XTD_Remark { get; set; }

        public string XTD_Firm { get; set; }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(XTD_TaskID))
                yield return new ValidationResult("任务单编号不能为空！", new[] { nameof(XTD_TaskID) });
        }

        /// <summary>
        /// 搜索表达式
        /// </summary>
        /// <returns></returns>
        public Expression<Func<XsTaskDetailDO, bool>> GetExpression()
        {
            Expression<Func<XsTaskDetailDO, bool>> expression = oo => oo.IsDel == 0;

            if (!string.IsNullOrEmpty(XTD_TaskID))
                expression = expression.And(oo => oo.XTD_TaskID.Contains(XTD_TaskID));

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
               OrderByField.Create<XsTaskDetailDO>(nameof(XsTaskDetailDO.CreateDate), FieldSortType.Desc)
            };
            return list.ToArray();
        }
    }
}
