// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-06-27 17:26
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-06-27 20:30
// ***********************************************************************
//  <copyright file="AnonymousFieldGeneratorModel.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

#endregion

namespace DomainCommonExtensions.Models
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A data Model for the anonymous field generator.
    /// </summary>
    /// =================================================================================================
    internal class AnonymousFieldGeneratorModel
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the type of the field.
        /// </summary>
        /// <value>
        ///     The type of the field.
        /// </value>
        /// =================================================================================================
        public Type FieldType { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the name of the field.
        /// </summary>
        /// <value>
        ///     The name of the field.
        /// </value>
        /// =================================================================================================
        public string FieldName { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="AnonymousFieldGeneratorModel"/> class.
        /// </summary>
        /// <param name="fieldType">The type of the field.</param>
        /// <param name="fieldName">The name of the field.</param>
        /// =================================================================================================
        public AnonymousFieldGeneratorModel(Type fieldType, string fieldName)
        {
            FieldType = fieldType;
            FieldName = fieldName;
        }
    }
}