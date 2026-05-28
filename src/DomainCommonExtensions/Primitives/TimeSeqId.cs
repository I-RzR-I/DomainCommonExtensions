// ***********************************************************************
//  Assembly          : RzR.Shared.Extensions.DomainCommonExtensions
//  Author            : RzR
//  Created           : 28-05-2026 20:05
// 
//  Last Modified By : RzR
//  Last Modified On : 28-05-2026 20:55
//  ***********************************************************************
//  <copyright file="TimeSeqId.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

#endregion

namespace RzR.Extensions.Domain.Primitives
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A time-ordered, lexicographically sortable 48-character unique identifiers.
    /// </summary>
    /// =================================================================================================
    public static class TimeSeqId
    {
        /*
         * [0] -> 4 digits year yyyy
         * [1] -> 4 digits month and day MMdd
         * [2] -> 9 digits HHmmssfff
         * [3] -> 4 digits sequence for uniqueness
         * [4] -> 16 characters(random) { 0 - 9A - F}
         * [5] -> 6 characters, hash of the previous block {0-9A-Z}
         */

        [ThreadStatic]
        private static RandomNumberGenerator _threadRng;
        private static RandomNumberGenerator Rng => _threadRng ??= RandomNumberGenerator.Create();

        private const int MAX_SEQUENCE = 9999;
        private static long _lastTimestamp;
        private static int _sequence;
        private static readonly object Lock = new();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generates a new time-ordered, lexicographically sortable unique identifier.
        ///     The result is safe for concurrent use and monotonically ordered within a single process.
        /// </summary>
        /// <returns>
        ///     A 48-character string in the format yyyy-MMdd-HHmmssfff-SSSS-RRRRRRRRRRRRRRRR-HHHHHH
        ///     where SSSS is a per-millisecond sequence, RRRRRRRRRRRRRRRR is 64 bits of CSPRNG entropy,
        ///     and HHHHHH is a SHA-256-derived integrity checksum.
        /// </returns>
        /// =================================================================================================
        public static string Generate()
        {
            DateTime current = default;
            int sequenceNumber = 0;
            long waitUntil = 0;

            while (true)
            {
                if (waitUntil > 0)
                {
                    while (DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond < waitUntil)
                        Thread.Sleep(1);
                    waitUntil = 0;
                }

                var acquired = false;

                lock (Lock)
                {
                    current = DateTime.UtcNow;
                    var currentTimestamp = current.Ticks / TimeSpan.TicksPerMillisecond;

                    if (currentTimestamp <= _lastTimestamp)
                    {
                        _sequence++;

                        if (_sequence > MAX_SEQUENCE)
                        {
                            // Sequence exhausted for this millisecond: release the lock
                            // immediately and wait for the clock to advance outside it.
                            waitUntil = _lastTimestamp + 1;
                        }
                        else
                        {
                            current = new DateTime(_lastTimestamp * TimeSpan.TicksPerMillisecond, DateTimeKind.Utc);
                            acquired = true;
                        }
                    }
                    else
                    {
                        _lastTimestamp = currentTimestamp;
                        _sequence = 0;
                        acquired = true;
                    }

                    if (acquired)
                        sequenceNumber = _sequence;
                }

                if (acquired) break;
            }

            var block0 = current.ToString("yyyy");
            var block1 = current.ToString("MMdd");
            var block2 = current.ToString("HHmmssfff");
            var block3 = sequenceNumber.ToString("D4");
            var block4 = GenerateSecureRandomHex(16);

            var combinedBlocks = $"{block0}-{block1}-{block2}-{block3}-{block4}";
            var block5 = GenerateHash(combinedBlocks, 6);

            return $"{block0}-{block1}-{block2}-{block3}-{block4}-{block5}";
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generates a cryptographically secure random hex string of the specified length.
        /// </summary>
        /// <param name="length">The number of hex characters to generate.</param>
        /// <returns>
        ///     A random uppercase hex string of the requested length using characters { 0-9, A-F }.
        /// </returns>
        /// =================================================================================================
        private static string GenerateSecureRandomHex(int length)
        {
            var bytes = new byte[length];
            Rng.GetBytes(bytes);

            const string hexChars = "0123456789ABCDEF";
            var result = new StringBuilder(length);

            foreach (var b in bytes) 
                result.Append(hexChars[b % 16]);

            return result.ToString();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Computes a SHA-256-based alphanumeric checksum of <paramref name="input"/>,
        ///     truncated to <paramref name="length"/> characters.
        ///     Uses rejection sampling to guarantee a uniform distribution over { 0-9, A-Z }.
        /// </summary>
        /// <param name="input">The string to hash.</param>
        /// <param name="length">The desired output length in characters.</param>
        /// <returns>
        ///     An alphanumeric string of exactly <paramref name="length"/> characters derived from the SHA-256 digest.
        /// </returns>
        /// =================================================================================================
        private static string GenerateHash(string input, int length)
        {
            using (var sha = SHA256.Create())
            {
                var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                const string alphanumeric = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                var unbiasedCeiling = 256 - 256 % alphanumeric.Length;
                var result = new StringBuilder(length);
                var byteIdx = 0;

                while (result.Length < length)
                {
                    if (byteIdx >= hashBytes.Length)
                    {
                        hashBytes = sha.ComputeHash(hashBytes);
                        byteIdx = 0;
                    }

                    var b = hashBytes[byteIdx++];
                    if (b < unbiasedCeiling)
                        result.Append(alphanumeric[b % alphanumeric.Length]);
                }

                return result.ToString();
            }
        }

    }
}