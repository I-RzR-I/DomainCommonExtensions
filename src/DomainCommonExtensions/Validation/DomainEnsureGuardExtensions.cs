// ***********************************************************************
//  Assembly          : RzR.Shared.Extensions.DomainCommonExtensions
//  Author            : RzR
//  Created On        : 2026-10-14 22:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-10-14 10:55
//  ***********************************************************************
//  <copyright file="DomainEnsureGuardExtensions.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using System;
using System.Collections.Generic;
using System.Linq;
using RzR.Extensions.Domain.Internal.Attributes;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Text;

#endregion

namespace RzR.Extensions.Domain.Validation
{
    /// <summary>
    ///     Argument guard extensions for strings, sequences and <see cref="Guid" /> values.
    /// </summary>
    public static class DomainEnsureGuardExtensions
    {
        /// <summary>
        ///     A string extension method that throws if the value is null, empty or consists only of
        ///     white-space characters.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="value" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="value" /> is empty or consists only of white-space characters.
        /// </exception>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">
        ///     (Optional) Name of the parameter; defaults to the caller's argument expression.
        /// </param>
        public static void ThrowIfNullOrWhiteSpace(this string value,
            [CallerArgumentExpression("value")] string paramName = null)
        {
            if (value.IsNull())
                throw new ArgumentNullException(paramName);
            if (value.IsMissing())
                throw new ArgumentException("Value cannot be empty or consist only of white-space characters.",
                    paramName);
        }

        /// <summary>
        ///     An IEnumerable&lt;T&gt; extension method that throws if the sequence is null or has no
        ///     elements.
        /// </summary>
        /// <remarks>
        ///     A lazy sequence is started to read at most one element, so its side effects up to the
        ///     first element run once here and again when the caller enumerates it. A string is a
        ///     sequence of characters here; to reject blank strings use
        ///     <see cref="ThrowIfNullOrWhiteSpace(string, string)" />.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="value" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="value" /> has no elements.
        /// </exception>
        /// <typeparam name="T">The type of the elements of the sequence.</typeparam>
        /// <param name="value">The sequence to check.</param>
        /// <param name="paramName">
        ///     (Optional) Name of the parameter; defaults to the caller's argument expression.
        /// </param>
        public static void ThrowIfNullOrEmptyCollection<T>(this IEnumerable<T> value,
            [CallerArgumentExpression("value")] string paramName = null)
        {
            if (value.IsNull())
                throw new ArgumentNullException(paramName);
            var collection = value as ICollection<T>;
            if (collection != null ? collection.Count == 0 : !value.Any())
                throw new ArgumentException("Collection cannot be empty.", paramName);
        }

        /// <summary>
        ///     A Guid extension method that throws if the value is <see cref="Guid.Empty" />.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="value" /> equals <see cref="Guid.Empty" />.
        /// </exception>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">
        ///     (Optional) Name of the parameter; defaults to the caller's argument expression.
        /// </param>
        public static void ThrowIfEmpty(this Guid value, [CallerArgumentExpression("value")] string paramName = null)
        {
            if (value.IsEmpty())
                throw new ArgumentException("Value cannot be an empty GUID.", paramName);
        }
    }
}