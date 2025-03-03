// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="GuidExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using DomainCommonExtensions.CommonExtensions;

#endregion

namespace DomainCommonExtensions.DataTypeExtensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Guid extensions.
    /// </summary>
    /// =================================================================================================
    public static class GuidExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Parse string to Guid.
        /// </summary>
        /// <param name="source">.</param>
        /// <returns>
        ///     A Guid?
        /// </returns>
        /// =================================================================================================
        public static Guid? TryToGuid(this string source)
        {
            if (source.IsNullOrEmpty()) return null;
            try
            {
                return Guid.Parse(source);
            }
            catch
            {
                return null;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Guid to int 32.
        /// </summary>
        /// <param name="uuid">.</param>
        /// <returns>
        ///     Uuid as an int.
        /// </returns>
        /// =================================================================================================
        public static int ToInt32(this Guid uuid)
        {
            var gb = uuid.ToByteArray();

            return BitConverter.ToInt32(gb, 0);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Guid to long.
        /// </summary>
        /// <param name="uuid">.</param>
        /// <returns>
        ///     Uuid as a long.
        /// </returns>
        /// =================================================================================================
        public static long ToLong(this Guid uuid)
        {
            var gb = uuid.ToByteArray();

            return BitConverter.ToInt64(gb, 0);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Guid? extension method that query if 'source' is empty.
        /// </summary>
        /// <param name="source">Source Guid value to be checked.</param>
        /// <returns>
        ///     True if empty, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsEmpty(this Guid? source) => source.IsNull() || source == Guid.Empty;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Guid? extension method that query if 'source' is empty.
        /// </summary>
        /// <param name="source">Source Guid value to be checked.</param>
        /// <returns>
        ///     True if empty, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsEmpty(this Guid source) => source == Guid.Empty;
    }
}