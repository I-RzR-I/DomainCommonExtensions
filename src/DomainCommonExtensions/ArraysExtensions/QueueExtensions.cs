// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-12-08 22:09
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-12-08 22:11
// ***********************************************************************
//  <copyright file="QueueExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using DomainCommonExtensions.DataTypeExtensions;

#endregion

namespace DomainCommonExtensions.ArraysExtensions
{
    /// <summary>
    ///     Queue extensions
    /// </summary>
    public static class QueueExtensions
    {
        /// <summary>
        ///     Pop from the queue the next item or returns the default value if the queue is empty
        /// </summary>
        /// <param name="queue">Required. Input source</param>
        /// <param name="ifEmpty">Optional. The default value is default.</param>
        /// <returns></returns>
        /// <typeparam name="TItem"></typeparam>
        /// <remarks></remarks>
        public static TItem DequeueOrDefault<TItem>(this Queue<TItem> queue, TItem ifEmpty = default)
        {
            return queue.Count.IsGreaterThanZero() ? queue.Dequeue() : ifEmpty;
        }
    }
}