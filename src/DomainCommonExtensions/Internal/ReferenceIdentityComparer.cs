// ***********************************************************************
//  Assembly          : RzR.Shared.Extensions.DomainCommonExtensions
//  Author            : RzR
//  Created On        : 2026-10-10 22:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-10-14 12:07
//  ***********************************************************************
//  <copyright file="ReferenceIdentityComparer.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using System.Collections.Generic;
using System.Runtime.CompilerServices;

#endregion

namespace RzR.Extensions.Domain.Internal
{
    /// <summary>
    ///     Compares objects by reference, ignoring any <see cref="object.Equals(object)" /> override.
    /// </summary>
    /// <typeparam name="T">The reference type to compare.</typeparam>
    /// <seealso cref="T:System.Collections.Generic.IEqualityComparer{T}"/>
    internal sealed class ReferenceIdentityComparer<T> : IEqualityComparer<T>
        where T : class
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the shared instance.
        /// </summary>
        /// =================================================================================================
        internal static readonly ReferenceIdentityComparer<T> Instance = new();

        /// <summary>
        ///     Checks whether two references point to the same object.
        /// </summary>
        /// <param name="x">The first reference.</param>
        /// <param name="y">The second reference.</param>
        /// <returns>
        ///     True when both references point to the same object or are both null; otherwise false.
        /// </returns>
        public bool Equals(T x, T y)
        {
            return ReferenceEquals(x, y);
        }

        /// <summary>
        ///     Gets the identity hash code of an object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        ///     The hash code from <see cref="RuntimeHelpers.GetHashCode(object)" />.
        /// </returns>
        public int GetHashCode(T obj)
        {
            return RuntimeHelpers.GetHashCode(obj);
        }
    }
}