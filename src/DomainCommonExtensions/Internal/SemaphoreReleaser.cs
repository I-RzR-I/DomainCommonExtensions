// ***********************************************************************
//  Assembly          : RzR.Shared.Extensions.DomainCommonExtensions
//  Author            : RzR
//  Created On        : 2026-10-10 22:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-10-14 09:13
//  ***********************************************************************
//  <copyright file="SemaphoreReleaser.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using System;
using System.Threading;

#endregion

namespace RzR.Extensions.Domain.Internal
{
#if NET45_OR_GREATER || NET || NETSTANDARD1_0_OR_GREATER

    /// <summary>
    ///     Releases an entered <see cref="SemaphoreSlim" /> exactly once.
    /// </summary>
    internal sealed class SemaphoreReleaser : IDisposable
    {
        private SemaphoreSlim _semaphore;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SemaphoreReleaser" /> class.
        /// </summary>
        /// <param name="semaphore">The semaphore the caller has entered.</param>
        internal SemaphoreReleaser(SemaphoreSlim semaphore)
        {
            _semaphore = semaphore;
        }

        /// <summary>
        ///     Releases the semaphore on the first call; later and concurrent calls do nothing.
        /// </summary>
        public void Dispose()
        {
            Interlocked.Exchange(ref _semaphore, null)?.Release();
        }
    }
#endif
}