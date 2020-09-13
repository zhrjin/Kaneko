using Kaneko.Core.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace MSDemo.IGrains.Entity
{
    public class TestDTO : BaseDTO
    {
        public Expression<Func<TestDO, bool>> GetExpression()
        {
            Expression<Func<TestDO, bool>> expression = oo => oo.CreateBy == "1111";

            //if (!string.IsNullOrEmpty(TaskCode))
            //    expression = expression.And(oo => oo.TaskCode.Contains(TaskCode));

            return expression;
        }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(UserId))
                yield return new ValidationResult("用户id不能为空！", new[] { nameof(UserId) });

            if (string.IsNullOrWhiteSpace(UserName))
                yield return new ValidationResult("用户名称不能为空！", new[] { nameof(UserName) });
        }
    }
}
