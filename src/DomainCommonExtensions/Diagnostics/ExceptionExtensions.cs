// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-09-14 20:53
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-09-14 20:53
// ***********************************************************************
//  <copyright file="ExceptionExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Text;
using RzR.Extensions.Domain.Internal;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Text;
using RzR.Extensions.Domain.Validation;

#endregion

namespace RzR.Extensions.Domain.Diagnostics
{
    /// <summary>
    ///     Exception extensions
    /// </summary>
    /// <remarks></remarks>
    public static class ExceptionExtensions
    {
        /// <summary>
        ///     Get full exception details.
        /// </summary>
        /// <remarks>
        ///     The default (<paramref name="showCallStack"/> = <see langword="true" />) includes stack
        ///     traces; do not expose the result to end users. Only the <see cref="Exception.InnerException"/>
        ///     chain is followed, so for an <see cref="AggregateException"/>
        ///     only its first inner exception (and that exception's inner chain) is included; the other
        ///     entries of
        ///     <see cref="AggregateException.InnerExceptions"/> are not.
        ///     When <paramref name="showCallStack"/> is <see langword="false" />, the generic Entity
        ///     Framework message "An error occurred while updating the entries. See the inner exception
        ///     for details." is skipped.
        /// </remarks>
        /// <param name="ex">Required. Exception.</param>
        /// <param name="showCallStack">
        ///     (Optional) Optional. The default value is true. If set to <see langword="true" />, the
        ///     stack traces are included; otherwise, only the messages.
        /// </param>
        /// <returns>
        ///     The full error.
        /// </returns>
        public static string GetFullError(this Exception ex, bool showCallStack = true)
        {
            if (ex.IsNull()) return string.Empty;
            var result = new StringBuilder();

            do
            {
                if (!showCallStack)
                {
                    if (ex?.Message != "An error occurred while updating the entries. See the inner exception for details.")
                        result.AppendLine(ex?.Message);
                }
                else
                    result.AppendLine(ex?.Message);
                
                if (showCallStack)
                    result.Append(ex?.StackTrace);

                ex = ex?.InnerException;
            }
            while (ex.IsNotNull());

            return result.ToString().TrimIfNotNull();
        }

        /// <summary>
        ///     Gets <paramref name="ex"/> and every exception nested inside it, following both
        ///     <see cref="Exception.InnerException"/> and <see cref="AggregateException.InnerExceptions"/>.
        /// </summary>
        /// <remarks>
        ///     The order is deterministic: an exception comes first, followed by everything nested
        ///     inside it, before its next sibling. For an <see cref="AggregateException"/> the nested
        ///     exceptions are the entries of <see cref="AggregateException.InnerExceptions"/>, in their
        ///     order (its <see cref="Exception.InnerException"/> is the first entry); for any other exception it is
        ///     its <see cref="Exception.InnerException"/>.
        ///     Exceptions are compared by reference. An exception reachable along several paths is
        ///     listed once, at its first position, and a cycle in the graph ends the walk along that
        ///     path instead of looping forever. The walk uses an explicit stack, so a deep chain cannot
        ///     overflow the call stack.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="ex"/> is null.
        /// </exception>
        /// <param name="ex">The exception to flatten.</param>
        /// <returns>
        ///     A new list that starts with <paramref name="ex"/> and holds each nested exception once,
        ///     in depth-first pre-order.
        /// </returns>
        public static IEnumerable<Exception> GetAllExceptions(this Exception ex)
        {
            DomainEnsure.IsNotNull(ex, nameof(ex));

            var result = new List<Exception>();
            var visited = new HashSet<Exception>(ReferenceIdentityComparer<Exception>.Instance);
            var pending = new Stack<Exception>();
            pending.Push(ex);

            while (pending.Count > 0)
            {
                var current = pending.Pop();
                if (current == null || !visited.Add(current))
                    continue;

                result.Add(current);

                if (current is AggregateException aggregate)
                {
                    for (var index = aggregate.InnerExceptions.Count - 1; index >= 0; index--)
                        pending.Push(aggregate.InnerExceptions[index]);
                }
                else
                {
                    pending.Push(current.InnerException);
                }
            }

            return result;
        }

        /// <summary>
        ///     Set/add data to the given Exception object.
        /// </summary>
        /// <remarks>
        ///     On .NET Framework, <see cref="Exception.Data"/> accepts only serializable keys and
        ///     serializable (or null) values;
        ///     newer runtimes do not enforce this rule. An existing value under the same key is
        ///     overwritten.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="ex"/> or <paramref name="dataKey"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown on .NET Framework when <paramref name="dataKey"/> is not serializable, or <paramref name="dataValue"/>
        ///     is neither null nor serializable.
        /// </exception>
        /// <param name="ex">Source exception.</param>
        /// <param name="dataKey">Key.</param>
        /// <param name="dataValue">Value.</param>
        /// <returns>
        ///     An Exception.
        /// </returns>
        public static Exception WithData(this Exception ex, object dataKey, object dataValue)
        {
            DomainEnsure.IsNotNull(ex, nameof(ex));
            DomainEnsure.IsNotNull(dataKey, nameof(dataKey));

            if (ex.Data.IsNotNull()) ex.Data[dataKey] = dataValue;

            return ex;
        }
    }
}
