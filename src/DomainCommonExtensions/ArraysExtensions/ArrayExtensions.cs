// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-12-08 09:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-12-08 09:49
// ***********************************************************************
//  <copyright file="ArrayExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace DomainCommonExtensions.ArraysExtensions
{
    /// <summary>
    ///     Array extensions
    /// </summary>
    /// <remarks></remarks>
    public static class ArrayExtensions
    {
        /// <summary>
        ///     Returns the first index of the given value
        /// </summary>
        /// <param name="array">Input array</param>
        /// <param name="value">Value to search</param>
        /// <returns></returns>
        /// <typeparam name="T">Type of input</typeparam>
        /// <remarks>In case when no found any value result is -1</remarks>
        public static int IndexOf<T>(this T[] array, T value)
        {
            const int index = -1;
            try
            {
                if (array.IsNullOrEmptyEnumerable()) return index;

                for (var i = 0; i < array.Length; i++)
                    if (Equals(array[i], value))
                        return i;
            }
            catch
            {
                return index;
            }

            return index;
        }
    }
}