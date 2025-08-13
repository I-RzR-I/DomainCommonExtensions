// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:40
// ***********************************************************************
//  <copyright file="RegularExpressions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

// ReSharper disable InconsistentNaming

namespace DomainCommonExtensions.Resources
{
    /// <summary>
    ///     Regular expressions REGEX
    /// </summary>
    /// <remarks></remarks>
    public struct RegularExpressions
    {
        /// <summary>
        ///     Is only numbers
        /// </summary>
        public const string NUMBER = "^[0-9]*$";

        /// <summary>
        ///     Is guid format
        /// </summary>
        public const string GUID =
            @"^(\{{0,1}([0-9a-fA-F]){8}-{0,1}([0-9a-fA-F]){4}-{0,1}([0-9a-fA-F]){4}-{0,1}([0-9a-fA-F]){4}-{0,1}([0-9a-fA-F]){12}\}{0,1})$";

        /// <summary>
        ///     Is email format
        /// </summary>
        public const string EMAIL =
            @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        /// <summary>
        ///     Is credit card format
        /// </summary>
        public const string CREDIT_CARD =
            @"^(?:4[0-9]{12}(?:[0-9]{3})?|[25][1-7][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$";

        /// <summary>
        ///     URL check regex
        /// </summary>
        public const string URL = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";

        /// <summary>
        ///     IP regex
        /// </summary>
        public const string IP =
            @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";

        /// <summary>
        ///     BASE 64 regex
        /// </summary>
        public const string BASE64 = @"^[a-zA-Z0-9\+/]*={0,3}$";

        /// <summary>
        ///     BASE 32 regex
        /// </summary>
        public const string BASE32 = @"^[A-Z2-7]+(?:={1,6})?$";
    }
}