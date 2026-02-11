// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2026-01-28 22:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-02-11 20:07
// ***********************************************************************
//  <copyright file="ObservableEnumeratorExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using DomainCommonExtensions.Collections;
using DomainCommonExtensions.Utilities.Ensure;

#endregion

namespace DomainCommonExtensions.ArraysExtensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An observable enumerator extensions.
    /// </summary>
    /// =================================================================================================
    public static class ObservableEnumeratorExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IEnumerable&lt;T&gt; extension method that convert to observable.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="source">The source to act on.</param>
        /// <returns>
        ///     An ObservableEnumerator&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public static ObservableEnumerator<T> WithObservable<T>(
            this IEnumerable<T> source)
        {
            DomainEnsure.IsNotNull(source, nameof(source));

            return new ObservableEnumerator<T>(source);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IEnumerable&lt;T&gt; extension method that convert to observable.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="enumerator">The enumerator to act on.</param>
        /// <returns>
        ///     An ObservableEnumerator&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public static ObservableEnumerator<T> WithObservable<T>(
            this IEnumerator<T> enumerator)
        {
            DomainEnsure.IsNotNull(enumerator, nameof(enumerator));

            return new ObservableEnumerator<T>(enumerator);
        }
    }
}