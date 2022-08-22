// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="ExpressionExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace DomainCommonExtensions.CommonExtensions
{
    /// <summary>
    ///     Expression extensions
    /// </summary>
    /// <remarks></remarks>
    public static class ExpressionExtensions
    {
        /// <summary>
        ///     And also expressions
        /// </summary>
        /// <param name="expr1">Expression 1</param>
        /// <param name="expr2">Expression 2</param>
        /// <returns></returns>
        /// <typeparam name="T">Type</typeparam>
        /// <remarks></remarks>
        public static Expression<Func<T, bool>> AndAlso<T>(
                    this Expression<Func<T, bool>> expr1,
                    Expression<Func<T, bool>> expr2)
        {
            // need to detect whether they use the same
            // parameter instance; if not, they need fixing
            var param = expr1.Parameters[0];
            if (ReferenceEquals(param, expr2.Parameters[0]))
                // simple version
                return Expression.Lambda<Func<T, bool>>(
                    Expression.AndAlso(expr1.Body, expr2.Body), param);

            // otherwise, keep expr1 "as is" and invoke expr2
            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(
                    expr1.Body,
                    Expression.Invoke(expr2, param)), param);
        }


        /// <summary>
        ///     Convert all expressions to 'AND' clause
        /// </summary>
        /// <param name="source">Source expression list</param>
        /// <returns></returns>
        /// <typeparam name="T">Source expression type</typeparam>
        /// <remarks></remarks>
        public static Expression<Func<T, bool>> All<T>(this IEnumerable<Expression<Func<T, bool>>> source)
        {
            return source.Aggregate(PredicateBuilderExtensions.True<T>(),
                (acc, predicate) => acc.And(predicate)
            );
        }

        /// <summary>
        ///     Convert all expressions to 'OR' clause
        /// </summary>
        /// <param name="source">Source expression list</param>
        /// <returns></returns>
        /// <typeparam name="T">Source expression type</typeparam>
        /// <remarks></remarks>
        public static Expression<Func<T, bool>> Any<T>(this IEnumerable<Expression<Func<T, bool>>> source)
        {
            return source.Aggregate(PredicateBuilderExtensions.False<T>(),
                (acc, predicate) => acc.Or(predicate)
            );
        }
    }
}