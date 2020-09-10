﻿using Kaneko.Dapper.Extensions;
using System.Linq.Expressions;
namespace Kaneko.Dapper.Expressions
{
    class NewSqlExpression : BaseSqlExpression<NewExpression>
    {
        protected override SqlGenerate Update(NewExpression expression, SqlGenerate sqlGenerate)
        {
            for (int i = 0; i < expression.Members.Count; i++)
            {
                var m = expression.Members[i];
                sqlGenerate += $"{m.GetFieldName().ParamSql(sqlGenerate)} = ";

                var val = SqlExpressionCompiler.Evaluate(expression.Arguments[i]);
                sqlGenerate.AddDbParameter(val);
                sqlGenerate += ",";
            }
            if (sqlGenerate[^1] == ',')
                sqlGenerate.Sql.Remove(sqlGenerate.Length - 1, 1);
            
            return sqlGenerate;
        }

        protected override SqlGenerate Select(NewExpression expression, SqlGenerate sqlGenerate)
        {
            foreach (Expression item in expression.Arguments)
            {
                SqlExpressionProvider.Select(item, sqlGenerate);
            }
            return sqlGenerate;
        }

        protected override SqlGenerate OrderBy(NewExpression expression, SqlGenerate sqlGenerate)
        {
            foreach (Expression item in expression.Arguments)
            {
                SqlExpressionProvider.OrderBy(item, sqlGenerate);
                sqlGenerate += ",";
            }
            if (sqlGenerate[^1] == ',')
                sqlGenerate.Sql.Remove(sqlGenerate.Length - 1, 1);

            return sqlGenerate;
        }
    }
}
