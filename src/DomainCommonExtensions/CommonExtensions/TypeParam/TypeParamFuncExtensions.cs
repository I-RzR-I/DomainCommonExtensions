// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2024-02-13 19:30
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-02-13 19:35
// ***********************************************************************
//  <copyright file="TypeParamFuncExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using DomainCommonExtensions.DataTypeExtensions;

#endregion

namespace DomainCommonExtensions.CommonExtensions.TypeParam
{
    /// -------------------------------------------------------------------------------------------------
    /// <content>
    ///     T type extensions.
    /// </content>
    /// =================================================================================================
    public static partial class TExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Return data if not null.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <typeparam name="TInput">Input type.</typeparam>
        /// <typeparam name="TResult">Result type.</typeparam>
        /// <param name="source">Source data.</param>
        /// <param name="func">Function.</param>
        /// <returns>
        ///     A TResult.
        /// </returns>
        /// =================================================================================================
        public static TResult IfNotNull<TInput, TResult>(this TInput source, Func<TInput, TResult> func)
        {
            if (func.IsNull())
                throw new ArgumentNullException(nameof(func));

            return source.IsNotNull()
                ? func(source)
                : default;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Return data if not null.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <typeparam name="TInput">Input type.</typeparam>
        /// <typeparam name="TResult">Result type.</typeparam>
        /// <param name="source">Source data.</param>
        /// <param name="func">Function.</param>
        /// <param name="defaultValue">Default result value.</param>
        /// <returns>
        ///     A TResult.
        /// </returns>
        /// =================================================================================================
        public static TResult IfNotNull<TInput, TResult>(this TInput source, Func<TInput, TResult> func,
            TResult defaultValue)
        {
            if (func.IsNull())
                throw new ArgumentNullException(nameof(func));

            return source.IsNotNull()
                ? func(source)
                : defaultValue;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Return data if null.
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="source">Source data.</param>
        /// <param name="defaultValue">Default result value.</param>
        /// <param name="func">The function.</param>
        /// <returns>
        ///     A TSource.
        /// </returns>
        /// =================================================================================================
        public static TSource IfNull<TSource>(this TSource source, TSource defaultValue, Func<bool> func)
        {
            if (func.IsNull())
                throw new ArgumentNullException(nameof(func));

            if (source.IsNull() && func.Invoke().IsTrue())
                return defaultValue;

            return source;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Return data if not null.
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="source">Source data.</param>
        /// <param name="defaultValue">Default result value.</param>
        /// <param name="func">The function.</param>
        /// <returns>
        ///     A TResult.
        /// </returns>
        /// =================================================================================================
        public static TSource IfNotNull<TSource>(this TSource source, TSource defaultValue, Func<bool> func)
        {
            if (func.IsNull())
                throw new ArgumentNullException(nameof(func));

            if (source.IsNotNull() && func.Invoke().IsTrue())
                return defaultValue;

            return source;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Return data if not null.
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="source">Source data.</param>
        /// <param name="defaultValue">Default result value.</param>
        /// <param name="func">The function.</param>
        /// <param name="isOrOperator">True if is or operator, false if not.</param>
        /// <returns>
        ///     A TResult.
        /// </returns>
        /// =================================================================================================
        public static TSource IfNotNull<TSource>(this TSource source, TSource defaultValue, Func<bool> func, bool isOrOperator)
        {
            if (func.IsNull())
                throw new ArgumentNullException(nameof(func));

            if (isOrOperator.IsTrue())
            {
                if (source.IsNotNull() || func.Invoke().IsTrue())
                    return defaultValue;
            }
            else
            {
                if (source.IsNotNull() && func.Invoke().IsTrue())
                    return defaultValue;
            }

            return source;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Return data if null.
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="source">Source data.</param>
        /// <param name="defaultValue">Default result value.</param>
        /// <param name="func">The function.</param>
        /// <param name="isOrOperator">True if is or operator, false if not.</param>
        /// <param name="funcResult">True to function result.</param>
        /// <returns>
        ///     A TSource.
        /// </returns>
        /// =================================================================================================
        public static TSource IfNull<TSource>(this TSource source, TSource defaultValue, Func<bool> func, bool isOrOperator, bool funcResult)
        {
            if (func.IsNull())
                throw new ArgumentNullException(nameof(func));

            if (isOrOperator.IsTrue())
            {
                if (source.IsNull() || func.Invoke() == funcResult)
                    return defaultValue;
            }
            else
            {
                if (source.IsNull() && func.Invoke() == funcResult)
                    return defaultValue;
            }

            return source;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TSource extension method that if is null or function is true.
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="source">Source data.</param>
        /// <param name="defaultValue">Default result value.</param>
        /// <param name="func">The function.</param>
        /// <returns>
        ///     A TSource.
        /// </returns>
        /// =================================================================================================
        public static TSource IfIsNullOrFuncIsTrue<TSource>(this TSource source, TSource defaultValue, Func<bool> func)
        {
            if (func.IsNull())
                throw new ArgumentNullException(nameof(func));

            if (source.IsNull() || func.Invoke().IsTrue())
                return defaultValue;

            return source;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TSource extension method that if is null and function is true.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="source">Source data.</param>
        /// <param name="defaultValue">Default result value.</param>
        /// <param name="func">The function.</param>
        /// <returns>
        ///     A TSource.
        /// </returns>
        /// =================================================================================================
        public static TSource IfIsNullAndFuncIsTrue<TSource>(this TSource source, TSource defaultValue, Func<bool> func)
        {
            if (func.IsNull())
                throw new ArgumentNullException(nameof(func));

            if (source.IsNull() && func.Invoke().IsTrue())
                return defaultValue;

            return source;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TSource extension method that if function is true.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="source">Source data.</param>
        /// <param name="defaultValue">Default result value.</param>
        /// <param name="func">The function.</param>
        /// <returns>
        ///     A TSource.
        /// </returns>
        /// =================================================================================================
        public static TSource IfFuncIsTrue<TSource>(this TSource source, TSource defaultValue, Func<bool> func)
        {
            if (func.IsNull())
                throw new ArgumentNullException(nameof(func));

            if (func.Invoke().IsTrue())
                return defaultValue;

            return source;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TSource extension method that if function is false.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="source">Source data.</param>
        /// <param name="defaultValue">Default result value.</param>
        /// <param name="func">The function.</param>
        /// <returns>
        ///     A TSource.
        /// </returns>
        /// =================================================================================================
        public static TSource IfFuncIsFalse<TSource>(this TSource source, TSource defaultValue, Func<bool> func)
        {
            if (func.IsNull())
                throw new ArgumentNullException(nameof(func));

            if (func.Invoke().IsFalse())
                return defaultValue;

            return source;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TSource extension method that if function.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="source">Source data.</param>
        /// <param name="defaultValue">Default result value.</param>
        /// <param name="func">The function.</param>
        /// <param name="funcResult">True to function result.</param>
        /// <returns>
        ///     A TSource.
        /// </returns>
        /// =================================================================================================
        public static TSource IfFunc<TSource>(this TSource source, TSource defaultValue, Func<bool> func,
            bool funcResult)
        {
            if (func.IsNull())
                throw new ArgumentNullException(nameof(func));

            if (func.Invoke() == funcResult)
                return defaultValue;

            return source;
        }
    }
}