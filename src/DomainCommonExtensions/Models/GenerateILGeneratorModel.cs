// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-06-27 17:16
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-06-27 20:31
// ***********************************************************************
//  <copyright file="GenerateILGeneratorModel.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Reflection.Emit;

// ReSharper disable InconsistentNaming
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

#endregion

namespace DomainCommonExtensions.Models
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A data Model for the generate il generator.
    /// </summary>
    /// =================================================================================================
    internal class GenerateILGeneratorModel
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the il generator.
        /// </summary>
        /// <value>
        ///     The il generator.
        /// </value>
        /// =================================================================================================
        public ILGenerator IlGenerator { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the method builder.
        /// </summary>
        /// <value>
        ///     The method builder.
        /// </value>
        /// =================================================================================================
        public MethodBuilder MethodBuilder { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="GenerateILGeneratorModel"/> class.
        /// </summary>
        /// <param name="ilGenerator">The il generator.</param>
        /// <param name="methodBuilder">The method builder.</param>
        /// =================================================================================================
        public GenerateILGeneratorModel(ILGenerator ilGenerator, MethodBuilder methodBuilder)
        {
            IlGenerator = ilGenerator;
            MethodBuilder = methodBuilder;
        }
    }
}