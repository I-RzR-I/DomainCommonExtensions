// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="LinqExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Linq;
using System.Linq.Expressions;
using DomainCommonExtensions.Resources.Enums;

#endregion

namespace DomainCommonExtensions.ArraysExtensions
{
    /// <summary>
    ///     Linq extensions
    /// </summary>
    /// <remarks></remarks>
    public static class LinqExtensions
    {
        /// <summary>
        ///     Func to expression
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Expression<Func<TObject, TResult>> ToExpression<TObject, TResult>(
            this Func<TObject, TResult> func)
        {
            return x => func(x);
        }

        /// <summary>
        ///     Func to object
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Expression<Func<TObject>> ToExpression<TObject>(this Func<TObject> func)
        {
            return Expression.Lambda<Func<TObject>>(Expression.Call(func.Method));
        }

        /// <summary>
        ///     To func
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Func<TObject, TResult> ToFunc<TObject, TResult>(
            this Expression<Func<TObject, TResult>> expression)
        {
            return expression.Compile();
        }

        /// <summary>
        ///     Get value or default
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Func<TObject, bool> GetValueOrDefault<TObject>(this Func<TObject, bool> func)
        {
            return func ?? (x => true);
        }

        /// <summary>
        ///     Where clause with condition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="condition"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition,
            Expression<Func<T, bool>> whereClause)
        {
            return condition ? query.Where(whereClause) : query;
        }

        /// <summary>
        ///     Dynamic order
        /// </summary>
        /// <param name="query">Source</param>
        /// <param name="orderByMember">Order member</param>
        /// <param name="direction">Order direction</param>
        /// <returns></returns>
        /// <typeparam name="T">Query type</typeparam>
        /// <remarks></remarks>
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string orderByMember,
            OrderType direction)
        {
            var queryElementTypeParam = Expression.Parameter(typeof(T));

            var memberAccess = Expression.PropertyOrField(queryElementTypeParam, orderByMember);

            var keySelector = Expression.Lambda(memberAccess, queryElementTypeParam);

            var orderBy = Expression.Call(
                typeof(Queryable),
                direction == OrderType.Asc ? "OrderBy" : "OrderByDescending",
                new[] { typeof(T), memberAccess.Type },
                query.Expression,
                Expression.Quote(keySelector));

            return query.Provider.CreateQuery<T>(orderBy);
        }
    }
}