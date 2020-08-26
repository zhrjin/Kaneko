﻿using System;
using System.Linq.Expressions;
using static Kaneko.Dapper.Expressions.SqlExpressionFingerprint;

namespace Kaneko.Dapper.Expressions
{
    /// <summary>
    /// 参数编译器 
    /// base ServiceStack
    /// </summary>
    public static class SqlExpressionCompiler
    {
        private static readonly ParameterExpression _unusedParameterExpr = Expression.Parameter(typeof(object), "_unused");

        /// <summary>
        /// 编译
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="lambdaExpression"></param>
        /// <returns></returns>
        public static Func<TModel, TValue> Compile<TModel, TValue>(this Expression<Func<TModel, TValue>> lambdaExpression)
        {
            if (lambdaExpression == null)
                throw new ArgumentNullException(nameof(lambdaExpression));

            return ExpressionCompiler.Process(lambdaExpression);
        }
        /// <summary>
        /// 编译
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static object Evaluate(Expression arg)
        {
            if (arg == null)
                throw new ArgumentNullException(nameof(arg));

            var func = Wrap(arg);
            return func(null);
        }

        private static Func<object, object> Wrap(Expression arg)
        {
            var lambdaExpr = Expression.Lambda<Func<object, object>>(Expression.Convert(arg, typeof(object)), _unusedParameterExpr);
            return ExpressionCompiler.Process(lambdaExpr);
        }
    }
}