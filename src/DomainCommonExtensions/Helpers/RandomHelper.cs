// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-02-16 19:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-02-19 18:23
// ***********************************************************************
//  <copyright file="RandomHelper.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Threading;

#endregion

namespace DomainCommonExtensions.Helpers
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A random helper.
    /// </summary>
    /// =================================================================================================
    public class RandomHelper
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the locker.
        /// </summary>
        /// =================================================================================================
        private static readonly Lazy<object> Locker = new Lazy<object>(() => new object(), LazyThreadSafetyMode.ExecutionAndPublication);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the local seed.
        /// </summary>
        /// =================================================================================================
        private readonly Random _localSeed;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the seed.
        /// </summary>
        /// =================================================================================================
        private readonly Random _seed = new Random();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RandomHelper" /> class.
        /// </summary>
        /// =================================================================================================
        private RandomHelper() => _localSeed = _seed ?? new Random();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RandomHelper" /> class.
        /// </summary>
        /// <param name="localSeed">Random seed.</param>
        /// =================================================================================================
        public RandomHelper(Random localSeed) => _localSeed = localSeed;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Random instance.
        /// </summary>
        /// <value>
        ///     The instance.
        /// </value>
        /// =================================================================================================
        public static RandomHelper Instance { get; } = new RandomHelper();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generate new random number.
        /// </summary>
        /// <param name="min">(Optional) Optional. The default value is 0 (inclusive).</param>
        /// <param name="max">(Optional) Optional. The default value is 1 (inclusive).</param>
        /// <returns>
        ///     An int.
        /// </returns>
        /// =================================================================================================
        public int Number(int min = 0, int max = 1)
        {
            lock (Locker.Value)
            {
                if (max < int.MaxValue) return _localSeed.Next(min, max + 1);
                if (min > int.MinValue) return 1 + _localSeed.Next(min - 1, max);

                var num = _localSeed.Next();
                var num2 = _localSeed.Next();
                var num3 = (num >> 8) & 0xFFFF;
                var num4 = (num2 >> 8) & 0xFFFF;

                return (num3 << 16) | num4;
            }
        }
    }
}