// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-17 22:48
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-21 19:02
// ***********************************************************************
//  <copyright file="PredicateBuilderExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Linq.Expressions;

#endregion

namespace RzR.Extensions.Domain.Linq
{
    /// <summary>
    ///     PredicateBuilder extensions
    /// </summary>
    /// <remarks></remarks>
    public static class PredicateBuilderExtensions
    {
        /// <summary>
        ///     Build true predicate
        /// </summary>
        /// <returns></returns>
        /// <typeparam name="T"></typeparam>
        /// <remarks></remarks>
        public static Expression<Func<T, bool>> True<T>()
        {
            return f => true;
        }

        /// <summary>
        ///     Build false predicate
        /// </summary>
        /// <returns></returns>
        /// <typeparam name="T"></typeparam>
        /// <remarks></remarks>
        public static Expression<Func<T, bool>> False<T>()
        {
            return f => false;
        }

        /// <summary>
        ///     Build 'OR' predicate for expression.
        /// </summary>
        /// <remarks>
        ///     The parameter of <paramref name="expr2"/> is replaced with the parameter of <paramref name="expr1"/>, 
        ///     so the result is a single lambda (OrElse) without an invocation expression, which some
        ///     providers (e.g. EF6) cannot translate.
        /// </remarks>
        /// <typeparam name="T">.</typeparam>
        /// <param name="expr1">Current expression.</param>
        /// <param name="expr2">.</param>
        /// <returns>
        ///     An expression that evaluates to a Func&lt;T,bool&gt;
        /// </returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            return Expression.Lambda<Func<T, bool>>
                (Expression.OrElse(expr1.Body, ReplaceParameter(expr2, expr1)), expr1.Parameters);
        }

        /// <summary>
        ///     Build 'AND' predicate for expression
        /// </summary>
        /// <param name="expr1">Current expression</param>
        /// <param name="expr2"></param>
        /// <returns></returns>
        /// <typeparam name="T"></typeparam>
        /// <remarks>
        ///     The parameter of <paramref name="expr2"/> is replaced with the parameter of <paramref name="expr1"/>,
        ///     so the result is a single lambda (AndAlso) without an invocation expression, which some providers (e.g. EF6) cannot translate.
        /// </remarks>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            return Expression.Lambda<Func<T, bool>>
                (Expression.AndAlso(expr1.Body, ReplaceParameter(expr2, expr1)), expr1.Parameters);
        }

        /// <summary>
        ///     Get the body of the source expression bound to the parameter of the target expression.
        /// </summary>
        /// <param name="source">Expression whose body is rewritten.</param>
        /// <param name="target">Expression whose parameter is used.</param>
        /// <returns>The rewritten body of <paramref name="source"/>.</returns>
        /// <typeparam name="T">Type of the parameter.</typeparam>
        private static Expression ReplaceParameter<T>(Expression<Func<T, bool>> source,
            Expression<Func<T, bool>> target)
        {
            if (source == null)
                throw new ArgumentNullException("expression");

            return new ParameterReplacer(source.Parameters[0], target.Parameters[0]).Visit(source.Body);
        }

        /// <summary>
        ///     Expression visitor that replaces one parameter instance with another.
        /// </summary>
        private sealed class ParameterReplacer : ExpressionVisitor
        {
            /// <summary>
            ///     (Immutable) the parameter to replace.
            /// </summary>
            private readonly ParameterExpression _source;

            /// <summary>
            ///     (Immutable) the replacement parameter.
            /// </summary>
            private readonly ParameterExpression _target;

            /// <summary>
            ///     Initializes a new instance of the <see cref="ParameterReplacer"/> class.
            /// </summary>
            /// <param name="source">The parameter to replace.</param>
            /// <param name="target">The replacement parameter.</param>
            public ParameterReplacer(ParameterExpression source, ParameterExpression target)
            {
                _source = source;
                _target = target;
            }

            /// <summary>
            ///     Replace the source parameter with the target parameter.
            /// </summary>
            /// <param name="node">The parameter expression to visit.</param>
            /// <returns>The target parameter when <paramref name="node"/> is the source parameter; otherwise the node.</returns>
            protected override Expression VisitParameter(ParameterExpression node)
            {
                return ReferenceEquals(node, _source) ? _target : base.VisitParameter(node);
            }
        }
    }
}
