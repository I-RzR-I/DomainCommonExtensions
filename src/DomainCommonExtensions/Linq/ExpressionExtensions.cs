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

namespace RzR.Extensions.Domain.Linq
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
        /// <remarks>
        ///     The result is a single lambda (AndAlso) on the parameter of <paramref name="expr1"/>,
        ///     without an invocation expression.
        /// </remarks>
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1,
                    Expression<Func<T, bool>> expr2)
        {
            return expr1.And(expr2);
        }

        /// <summary>
        ///     Convert all expressions to 'AND' clause
        /// </summary>
        /// <param name="source">Source expression list</param>
        /// <returns></returns>
        /// <typeparam name="T">Source expression type</typeparam>
        /// <remarks>
        ///     The result is a single lambda without invocation expressions. An empty source returns an always-true predicate.
        ///     See also <see cref="CombineAnd{T}"/>, which has the same behaviour.
        /// </remarks>
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
        /// <remarks>
        ///     The result is a single lambda without invocation expressions. An empty source returns an always-false predicate.
        ///     See also <see cref="CombineOr{T}"/>, which has the same behaviour and does not compete with Enumerable.Any in overload resolution.
        /// </remarks>
        public static Expression<Func<T, bool>> Any<T>(this IEnumerable<Expression<Func<T, bool>>> source)
        {
            return source.Aggregate(PredicateBuilderExtensions.False<T>(),
                (acc, predicate) => acc.Or(predicate)
            );
        }

        /// <summary>
        ///     Combine all expressions into a single 'AND' (AndAlso) predicate.
        /// </summary>
        /// <remarks>
        ///     Same behaviour as <see cref="All{T}"/>, under a name that does not resemble
        ///     Enumerable.All.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="source"/> is null or contains a null expression.
        /// </exception>
        /// <typeparam name="T">Type of the predicate parameter.</typeparam>
        /// <param name="source">Source expressions to combine.</param>
        /// <returns>
        ///     A single-parameter lambda without invocation expressions. An empty <paramref name="source"/>
        ///     returns an always-true predicate.
        /// </returns>
        public static Expression<Func<T, bool>> CombineAnd<T>(this IEnumerable<Expression<Func<T, bool>>> source)
        {
            return All(source);
        }

        /// <summary>
        ///     Combine all expressions into a single 'OR' (OrElse) predicate.
        /// </summary>
        /// <remarks>
        ///     Same behaviour as <see cref="Any{T}"/>, without competing with Enumerable.Any in overload
        ///     resolution.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="source"/> is null or contains a null expression.
        /// </exception>
        /// <typeparam name="T">Type of the predicate parameter.</typeparam>
        /// <param name="source">Source expressions to combine.</param>
        /// <returns>
        ///     A single-parameter lambda without invocation expressions. An empty <paramref name="source"/>
        ///     returns an always-false predicate.
        /// </returns>
        public static Expression<Func<T, bool>> CombineOr<T>(this IEnumerable<Expression<Func<T, bool>>> source)
        {
            return Any(source);
        }
    }
}
