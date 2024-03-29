﻿// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-12-12 23:40
// ***********************************************************************
//  <copyright file="GeneralUtils.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
#if NETSTANDARD2_0_OR_GREATER
using Microsoft.Extensions.Hosting;
#endif

#endregion

namespace DomainCommonExtensions.Utilities
{
    /// <summary>
    ///     Utils methods
    /// </summary>
    /// <remarks></remarks>
    public class GeneralUtils
    {
        /// <summary>
        ///     Check if is debug
        /// </summary>
        public static bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

#if  NET || NETSTANDARD2_0_OR_GREATER
        /// <summary>
        ///     Check if app is in Development mode
        /// </summary>
        /// <returns></returns>
        public static bool IsDevelopment()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return environment == Environments.Development;
        }

        /// <summary>
        ///     Get current env
        /// </summary>
        /// <returns></returns>
        public static string GetEnv()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }
#endif
    }
}