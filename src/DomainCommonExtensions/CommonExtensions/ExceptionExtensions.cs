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

using System;
using System.Text;
using DomainCommonExtensions.DataTypeExtensions;

namespace DomainCommonExtensions.CommonExtensions
{
    /// <summary>
    ///     Exception extensions
    /// </summary>
    /// <remarks></remarks>
    public static class ExceptionExtensions
    {
        /// <summary>
        ///     Get full exception details
        /// </summary>
        /// <param name="ex">Required. Exception</param>
        /// <param name="showCallStack">Optional. The default value is true.If set to <see langword="true" />, then ; otherwise, .</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetFullError(this Exception ex, bool showCallStack = true)
        {
            if (ex.IsNull()) return string.Empty;
            var result = new StringBuilder();

            do
            {
                if (!showCallStack)
                {
                    if (ex.Message != "An error occurred while updating the entries. See the inner exception for details.")
                        result.AppendLine(ex.Message);
                }
                else
                    result.AppendLine(ex.Message);
                
                if (showCallStack)
                    result.Append(ex.StackTrace);

                ex = ex.InnerException;
            }
            while (ex != null);

            return result?.ToString().TrimIfNotNull();
        }
    }
}