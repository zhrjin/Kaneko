using Kaneko.Core.Contract;
using System;
using System.Linq.Expressions;

namespace MSDemo.IGrains.Entity
{
    public class TestDTO : BaseDTO
    {
        public string UserId { set; get; }

        public Expression<Func<TestDO, bool>> GetExpression()
        {
            Expression<Func<TestDO, bool>> expression = oo => oo.CreateBy == "1111";

            //if (!string.IsNullOrEmpty(TaskCode))
            //    expression = expression.And(oo => oo.TaskCode.Contains(TaskCode));

            return expression;
        }
    }
}
