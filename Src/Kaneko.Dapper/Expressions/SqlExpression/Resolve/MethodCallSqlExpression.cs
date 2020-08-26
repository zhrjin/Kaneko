﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Kaneko.Dapper.Expressions
{
    class MethodCallSqlExpression : BaseSqlExpression<MethodCallExpression>
    {
        static readonly Dictionary<string, Action<MethodCallExpression, SqlGenerate>> _Methods = new Dictionary<string, Action<MethodCallExpression, SqlGenerate>>
        {
            {"In", In},
            {"Equals", Equals},
            {"Contains", Contains},
            {"StartsWith", StartsWith},
            {"EndsWith", EndsWith}
        };

        private static new void In(MethodCallExpression expression, SqlGenerate sqlGenerate)
        {
            SqlExpressionProvider.Where(expression.Arguments[0], sqlGenerate);
            sqlGenerate += " in ";
            SqlExpressionProvider.In(expression.Arguments[1], sqlGenerate);
        }

        private static void Equals(MethodCallExpression expression, SqlGenerate sqlGenerate)
        {
            SqlExpressionProvider.Where(expression.Object, sqlGenerate);
            sqlGenerate += " = ";
            SqlExpressionProvider.Where(expression.Arguments[0], sqlGenerate);
        }

        private static void Contains(MethodCallExpression expression, SqlGenerate sqlGenerate)
        {
            if (IsStaticArrayMethod(expression))
            {
                DoStaticArrayMethodCall(expression, sqlGenerate);
                return;
            }
            if (IsEnumerableMethod(expression))
            {
                DoEnumerableMethodCall(expression, sqlGenerate);
                return;
            }

            SqlExpressionProvider.Where(expression.Object, sqlGenerate);
            sqlGenerate += " like ";
            var val = SqlExpressionCompiler.Evaluate(expression.Arguments[0]);
            sqlGenerate.AddDbParameter($"%{val}%");
        }

        private static void EndsWith(MethodCallExpression expression, SqlGenerate sqlGenerate)
        {
            SqlExpressionProvider.Where(expression.Object, sqlGenerate);
            SqlExpressionProvider.Where(expression.Arguments[0], sqlGenerate);
            sqlGenerate += " like ";

            var val = SqlExpressionCompiler.Evaluate(expression.Arguments[0]);
            sqlGenerate.AddDbParameter($"%{val}");
        }

        private static void StartsWith(MethodCallExpression expression, SqlGenerate sqlGenerate)
        {
            SqlExpressionProvider.Where(expression.Object, sqlGenerate);
            SqlExpressionProvider.Where(expression.Arguments[0], sqlGenerate);
            sqlGenerate += " like ";

            var val = SqlExpressionCompiler.Evaluate(expression.Arguments[0]);
            sqlGenerate.AddDbParameter($"{val}%");
        }

        protected override SqlGenerate Where(MethodCallExpression expression, SqlGenerate sqlGenerate)
        {
            var key = expression.Method;
            if (key.IsGenericMethod)
                key = key.GetGenericMethodDefinition();

            if (_Methods.TryGetValue(key.Name, out Action<MethodCallExpression, SqlGenerate> action))
            {
                action(expression, sqlGenerate);
                return sqlGenerate;
            }

            throw new NotImplementedException("无法解析方法" + expression.Method);
        }


        /// <summary>
        /// 是否是静态集合方法
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        internal static bool IsStaticArrayMethod(MethodCallExpression m)
        {
            return (m.Object == null
                && m.Arguments.Count == 2);
        }

        /// <summary>
        /// 是否是集合方法
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        internal static bool IsEnumerableMethod(MethodCallExpression m)
        {
            return m.Object != null
                && m.Object.Type.IsOrHasGenericInterfaceTypeOf(typeof(IEnumerable<>))
                && m.Object.Type != typeof(string)
                && m.Arguments.Count == 1;
        }

        internal static void DoEnumerableMethodCall(MethodCallExpression expression, SqlGenerate sqlGenerate)
        {
            SqlExpressionProvider.Where(expression.Arguments[0], sqlGenerate);
            sqlGenerate += " in ";
            SqlExpressionProvider.In(expression.Object, sqlGenerate);
        }

        internal static void DoStaticArrayMethodCall(MethodCallExpression expression, SqlGenerate sqlGenerate)
        {
            SqlExpressionProvider.Where(expression.Arguments[expression.Arguments.Count - 1], sqlGenerate);
            sqlGenerate += " in ";

            var memberExpr = expression.Arguments[0];
            if (memberExpr.NodeType == ExpressionType.MemberAccess)
                memberExpr = expression.Arguments[0] as MemberExpression;

            SqlExpressionProvider.In(memberExpr, sqlGenerate);
        }
    }
}