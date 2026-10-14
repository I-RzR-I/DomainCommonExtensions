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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using RzR.Extensions.Domain.Collections;
using RzR.Extensions.Domain.Internal.AnonymousSelect;

#endregion

namespace RzR.Extensions.Domain.Linq
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
        /// <param name="orderByMember">Order member: a public instance property, matched exactly first and then ignoring case</param>
        /// <param name="direction">Order direction</param>
        /// <returns></returns>
        /// <typeparam name="T">Query type</typeparam>
        /// <remarks>Use the overload with an allow-list for caller-supplied field names.</remarks>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="orderByMember"/> is null, empty, white space or not a public instance property.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="direction"/> is not a defined <see cref="OrderType"/> value.
        /// </exception>
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string orderByMember,
            OrderType direction)
        {
            if (!Enum.IsDefined(typeof(OrderType), direction))
                throw new ArgumentOutOfRangeException(nameof(direction), direction, "The order direction is not a defined OrderType value.");

            var queryElementTypeParam = Expression.Parameter(typeof(T));

            var property = ExpressionHelper.ResolveProperty(typeof(T), orderByMember, nameof(orderByMember));
            var memberAccess = Expression.Property(queryElementTypeParam, property);

            var keySelector = Expression.Lambda(memberAccess, queryElementTypeParam);

            var orderBy = Expression.Call(
                typeof(Queryable),
                direction == OrderType.Asc ? "OrderBy" : "OrderByDescending",
                new[] { typeof(T), memberAccess.Type },
                query.Expression,
                Expression.Quote(keySelector));

            return query.Provider.CreateQuery<T>(orderBy);
        }

        /// <summary>
        ///     Dynamic order by a member that must be in an allow-list.
        /// </summary>
        /// <remarks>
        ///     The member name is checked against the allow-list before the query is built: a name that
        ///     matches no allowed name, even ignoring case, is rejected without inspecting <typeparamref name="T"/>
        ///     ; a name that resolves to a property whose exact name is not allowed is also rejected.
        ///     The allow-list spelling is canonical: a requested name is mapped to the allow-list entry
        ///     that matches exactly, otherwise to the only entry that matches ignoring case (several
        ///     such entries reject it), and only that entry is resolved. With the allow-list {"Name"},
        ///     "NAME" and "nAmE" resolve via "Name" to the Name property, never to a NAME sibling. 
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="query"/> or <paramref name="allowedFields"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="direction"/> is not a defined <see cref="OrderType"/> value.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="allowedFields"/> contains a null, empty or white-space entry,
        ///     or
        ///     <paramref name="orderByMember"/> is null, empty, white space, not allowed or not a public
        ///     instance
        ///     property.
        /// </exception>
        /// <typeparam name="T">Query type.</typeparam>
        /// <param name="query">Source.</param>
        /// <param name="orderByMember">
        ///     Order member: a public instance property, matched exactly first and then ignoring case.
        /// </param>
        /// <param name="direction">Order direction.</param>
        /// <param name="allowedFields">
        ///     The property names that may be used. The resolved property name must be in the list with
        ///     an exact (ordinal) match.
        /// </param>
        /// <returns>
        ///     The ordered query.
        /// </returns>
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string orderByMember,
            OrderType direction, IEnumerable<string> allowedFields)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            if (!Enum.IsDefined(typeof(OrderType), direction))
                throw new ArgumentOutOfRangeException(nameof(direction), direction, "The order direction is not a defined OrderType value.");

            var propertyName = AllowedFieldsResolver.ResolveAllowedField(typeof(T), orderByMember, allowedFields,
                nameof(orderByMember), nameof(allowedFields));

            return query.OrderByDynamic(propertyName, direction);
        }
    }
}
