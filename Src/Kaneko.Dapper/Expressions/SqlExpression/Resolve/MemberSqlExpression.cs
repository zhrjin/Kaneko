﻿using Kaneko.Dapper.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Kaneko.Dapper.Expressions
{
    class MemberSqlExpression : BaseSqlExpression<MemberExpression>
    {
        protected override SqlGenerate Select(MemberExpression expression, SqlGenerate sqlGenerate)
        {
            if (IsEnumerable(expression))
            {
                var result = SqlExpressionCompiler.Evaluate(expression);
                var fields = (result as IEnumerable).Flatten();
                if (fields?.Count > 0)
                    sqlGenerate.SelectFields.AddRange(fields.Select(field => field.ToString().ParamSql(sqlGenerate)));
                else
                    sqlGenerate.SelectFields = new List<string>() { "*" };

                return sqlGenerate;
            }

            string fieldName = expression.Member.GetFieldName();
            if (string.IsNullOrEmpty(fieldName)) { return sqlGenerate; }

            sqlGenerate.SelectFields.Add(fieldName.ParamSql(sqlGenerate));
            return sqlGenerate;
        }

        protected override SqlGenerate Where(MemberExpression expression, SqlGenerate sqlGenerate)
        {
            if (expression.Expression != null)
            {
                if (expression.Member.DeclaringType.IsNullableType())
                {
                    if (expression.Member.Name == "Value") //Can't use C# 6 yet: nameof(Nullable<bool>.Value)
                    {
                        SqlExpressionProvider.Where(expression.Expression, sqlGenerate);
                        return sqlGenerate;
                    }
                    if (expression.Member.Name == "HasValue")
                    {
                        var doesNotEqualNull = Expression.MakeBinary(ExpressionType.NotEqual, expression.Expression, Expression.Constant(null));
                        SqlExpressionProvider.Where(doesNotEqualNull, sqlGenerate);
                        return sqlGenerate;
                    }
                    throw new ArgumentException($"Expression '{expression}' accesses unsupported property '{expression.Member}' of Nullable<T>");
                }

                if (expression.IsParameterOrConvertAccess())
                {
                    string fieldName = expression.Member.GetFieldName();
                    if (string.IsNullOrEmpty(fieldName)) { return sqlGenerate; }

                    sqlGenerate += $" {fieldName.ParamSql(sqlGenerate)}";
                    return sqlGenerate;
                }
            }

            var val = SqlExpressionCompiler.Evaluate(expression);
            sqlGenerate.AddDbParameter(val);
            return sqlGenerate;

        }

        protected override SqlGenerate In(MemberExpression expression, SqlGenerate sqlGenerate)
        {
            var result = SqlExpressionCompiler.Evaluate(expression);
            var inArgs = (result as IEnumerable).Flatten();
            sqlGenerate.AddDbParameter(inArgs);
            return sqlGenerate;
        }

        protected override SqlGenerate OrderBy(MemberExpression expression, SqlGenerate sqlGenerate)
        {
            string fieldName = expression.Member.GetFieldName();
            if (string.IsNullOrEmpty(fieldName)) { return sqlGenerate; }

            sqlGenerate += fieldName.ParamSql(sqlGenerate);
            return sqlGenerate;
        }

        protected override SqlGenerate Update(MemberExpression expression, SqlGenerate sqlGenerate)
        {
            var obj = SqlExpressionCompiler.Evaluate(expression);
            if (obj == null)
                throw new ArgumentException($"Expression '{expression}' accesses unsupported property '{expression.Member}' of Nullable<T>");

            if (obj.GetType().IsValueType)
                throw new ArgumentException($"Expression '{expression}' accesses unsupported valuetype");

            if (obj is IDictionary dictionary)
            {
                foreach (string key in dictionary.Keys)
                {
                    sqlGenerate += $"{key.ParamSql(sqlGenerate)} = ";
                    sqlGenerate.AddDbParameter(dictionary[key]);
                    sqlGenerate += ",";
                }
            }
            else
            {
                var pis = obj.GetType().GetProperties();
                foreach (var p in pis)
                {
                    string fieldName = p.GetFieldName();
                    if (string.IsNullOrEmpty(fieldName)) { continue; }

                    sqlGenerate += $"{fieldName.ParamSql(sqlGenerate)} = ";
                    sqlGenerate.AddDbParameter(p.GetValue(obj));
                    sqlGenerate += ",";
                }
            }

            if (sqlGenerate[sqlGenerate.Length - 1] == ',')
                sqlGenerate.Sql.Remove(sqlGenerate.Length - 1, 1);

            return sqlGenerate;
        }

        protected override SqlGenerate Max(MemberExpression expression, SqlGenerate sqlGenerate)
        {
            string fieldName = expression.Member.GetFieldName();
            if (string.IsNullOrEmpty(fieldName)) { return sqlGenerate; }

            sqlGenerate.Sql.AppendFormat("select max({0}) from {1}", fieldName.ParamSql(sqlGenerate), sqlGenerate.TableName);
            return sqlGenerate;
        }

        protected override SqlGenerate Min(MemberExpression expression, SqlGenerate sqlGenerate)
        {
            string fieldName = expression.Member.GetFieldName();
            if (string.IsNullOrEmpty(fieldName)) { return sqlGenerate; }

            sqlGenerate.Sql.AppendFormat("select min({0}) from {1}", fieldName.ParamSql(sqlGenerate), sqlGenerate.TableName);
            return sqlGenerate;
        }

        protected override SqlGenerate Avg(MemberExpression expression, SqlGenerate sqlGenerate)
        {
            string fieldName = expression.Member.GetFieldName();
            if (string.IsNullOrEmpty(fieldName)) { return sqlGenerate; }

            sqlGenerate.Sql.AppendFormat("select avg({0}) from {1}", fieldName.ParamSql(sqlGenerate), sqlGenerate.TableName);
            return sqlGenerate;
        }

        protected override SqlGenerate Count(MemberExpression expression, SqlGenerate sqlGenerate)
        {
            string fieldName = expression.Member.GetFieldName();
            if (string.IsNullOrEmpty(fieldName)) { return sqlGenerate; }

            sqlGenerate.Sql.AppendFormat("select count({0}) from {1}", fieldName.ParamSql(sqlGenerate), sqlGenerate.TableName);
            return sqlGenerate;
        }

        protected override SqlGenerate Sum(MemberExpression expression, SqlGenerate sqlGenerate)
        {
            string fieldName = expression.Member.GetFieldName();
            if (string.IsNullOrEmpty(fieldName)) { return sqlGenerate; }

            sqlGenerate.Sql.AppendFormat("select sum({0}) from {1}", fieldName.ParamSql(sqlGenerate), sqlGenerate.TableName);
            return sqlGenerate;
        }

        /// <summary>
        /// 是否是集合方法
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        internal static bool IsEnumerable(MemberExpression m)
        {
            return m.Type.IsOrHasGenericInterfaceTypeOf(typeof(IEnumerable<>))
                && m.Type != typeof(string);
        }
    }
}