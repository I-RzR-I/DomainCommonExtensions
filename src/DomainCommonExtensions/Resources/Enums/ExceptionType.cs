// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-08-05 22:21
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-08-05 22:21
// ***********************************************************************
//  <copyright file="ExceptionType.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace DomainCommonExtensions.Resources.Enums
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Values that represent exception types.
    /// </summary>
    /// =================================================================================================
    public enum ExceptionType
    {
        /// <summary>
        ///     An enum constant representing the exception option.
        /// </summary>
        Exception,

        /// <summary>
        ///     An enum constant representing the argument exception option.
        /// </summary>
        ArgumentException,

        /// <summary>
        ///     An enum constant representing the argument null exception option.
        /// </summary>
        ArgumentNullException,

        /// <summary>
        ///     An enum constant representing the argument out of range exception option.
        /// </summary>
        ArgumentOutOfRangeException,

        /// <summary>
        ///     An enum constant representing the divide by zero exception option.
        /// </summary>
        DivideByZeroException,

        /// <summary>
        ///     An enum constant representing the file not found exception option.
        /// </summary>
        FileNotFoundException,

        /// <summary>
        ///     An enum constant representing the format exception option.
        /// </summary>
        FormatException,

        /// <summary>
        ///     An enum constant representing the index out of range exception option.
        /// </summary>
        IndexOutOfRangeException,

        /// <summary>
        ///     An enum constant representing the invalid cast exception option.
        /// </summary>
        InvalidCastException,

        /// <summary>
        ///     An enum constant representing the invalid operation exception option.
        /// </summary>
        InvalidOperationException,

        /// <summary>
        ///     An enum constant representing the missing member exception option.
        /// </summary>
        MissingMemberException,

        /// <summary>
        ///     An enum constant representing the key not found exception option.
        /// </summary>
        KeyNotFoundException,

        /// <summary>
        ///     An enum constant representing the not supported exception option.
        /// </summary>
        NotSupportedException,

        /// <summary>
        ///     An enum constant representing the null reference exception option.
        /// </summary>
        NullReferenceException,

        /// <summary>
        ///     An enum constant representing the overflow exception option.
        /// </summary>
        OverflowException,

        /// <summary>
        ///     An enum constant representing the out of memory exception option.
        /// </summary>
        OutOfMemoryException,

        /// <summary>
        ///     An enum constant representing the stack overflow exception option.
        /// </summary>
        StackOverflowException,

        /// <summary>
        ///     An enum constant representing the timeout exception option.
        /// </summary>
        TimeoutException
    }
}