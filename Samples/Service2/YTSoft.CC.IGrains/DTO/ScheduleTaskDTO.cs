using Kaneko.Core.Contract;
using Kaneko.Core.Data;
using Kaneko.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace YTSoft.CC.IGrains.Entity
{
    public class ScheduleTaskDTO : BaseDTO<long>
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public string TaskCode { set; get; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { set; get; }

        /// <summary>
        /// 生产线编号
        /// </summary>
        public string LineCode { set; get; }

        /// <summary>
        /// 状态
        /// </summary>
        public TaskState TaskState { set; get; }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(TaskCode))
                yield return new ValidationResult("任务编号不能为空！", new[] { nameof(TaskCode) });
        }

        /// <summary>
        /// 搜索表达式
        /// </summary>
        /// <returns></returns>
        public Expression<Func<ScheduleTaskDO, bool>> GetExpression()
        {
            Expression<Func<ScheduleTaskDO, bool>> expression = oo => oo.IsDel == 0;

            if (!string.IsNullOrEmpty(TaskCode))
                expression = expression.And(oo => oo.TaskCode.Contains(TaskCode));

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
               OrderByField.Create<ScheduleTaskDO>(nameof(ScheduleTaskDO.LineCode), FieldSortType.Asc)
            };
            return list.ToArray();
        }
    }
}
