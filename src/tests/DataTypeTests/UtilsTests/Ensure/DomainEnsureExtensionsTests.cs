// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2025-08-05 19:28
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-08-05 20:12
// ***********************************************************************
//  <copyright file="DomainEnsureExtensionsTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DomainCommonExtensions.Utilities.Ensure;

// ReSharper disable ExpressionIsAlwaysNull

#endregion

namespace DataTypeTests.UtilsTests.Ensure
{
    [TestClass]
    public class DomainEnsureExtensionsTests
    {
        [TestMethod]
        public void ThrowIfNullTest()
        {
            object obj = null;

            Assert.ThrowsException<Exception>(
                () => obj.ThrowIfNull("Obj is null"), "Obj is null");
        }

        [TestMethod]
        public void ThrowIfArgNullTest()
        {
            object obj = null;

            Assert.ThrowsException<ArgumentNullException>(
                () => obj.ThrowIfArgNull(nameof(obj)), "Value cannot be null. (Parameter 'obj')");
        }
    }
}