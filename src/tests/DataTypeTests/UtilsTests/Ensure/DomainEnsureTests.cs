// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2025-08-05 19:27
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-08-05 20:05
// ***********************************************************************
//  <copyright file="DomainEnsureTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using DomainCommonExtensions.Utilities.Ensure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ExpressionIsAlwaysNull

#endregion

namespace DataTypeTests.UtilsTests.Ensure
{
    [TestClass]
    public class DomainEnsureTests
    {
        [TestMethod]
        public void IsNotNull_Null_Object_Throw_Test()
        {
            object sourceData = null;

            Assert.ThrowsException<ArgumentNullException>(
                () => DomainEnsure.IsNotNull(sourceData, nameof(sourceData)));
        }

        [TestMethod]
        public void IsNotNull_Object_Pass_Test()
        {
            object sourceData = 1;

            DomainEnsure.IsNotNull(sourceData, nameof(sourceData));

            Assert.AreEqual(1, sourceData);
        }

        [TestMethod]
        public void IsNotNullAll_Null_Objects_Throw_Test()
        {
            object sourceData = null;
            object sourceData1 = 1;

            Assert.ThrowsException<ArgumentNullException>(
                () => DomainEnsure.IsNotNullAll("Exception message", sourceData, sourceData1));
        }

        [TestMethod]
        public void IsNotNullAll_Objects_Pass_Test()
        {
            object sourceData = 1;
            object sourceData1 = 2;

            DomainEnsure.IsNotNullAll("Exception message", sourceData, sourceData1);

            Assert.AreEqual(1, sourceData);
            Assert.AreEqual(2, sourceData1);
        }

        [TestMethod]
        public void IsNotNull_WithMessage_Null_Object_Throw_Test()
        {
            object sourceData = null;

            Assert.ThrowsException<ArgumentNullException>(
                () => DomainEnsure.IsNotNull(sourceData, nameof(sourceData), "Exception message"));
        }

        [TestMethod]
        public void IsNotNull_WithMessage_Object_Pass_Test()
        {
            object sourceData = 1;

            DomainEnsure.IsNotNull(sourceData, nameof(sourceData), "Exception message");

            Assert.AreEqual(1, sourceData);
        }

        [TestMethod]
        public void IsNotNull_WithMessage_Null_Object_Exception_Throw_Test()
        {
            object sourceData = null;

            Assert.ThrowsException<ArgumentNullException>(
                () => DomainEnsure.IsNotNull(sourceData, nameof(sourceData), new Exception("Exception message")));
        }

        [TestMethod]
        public void IsNotNull_WithMessage_Object_Exception_Pass_Test()
        {
            object sourceData = 1;

            DomainEnsure.IsNotNull(sourceData, nameof(sourceData), new Exception("Exception message"));

            Assert.AreEqual(1, sourceData);
        }

        [TestMethod]
        public void IsNotNullOrEmpty_WithMessage_Null_Object_Exception_Throw_Test()
        {
            string sourceData = null;

            Assert.ThrowsException<ArgumentNullException>(
                () => DomainEnsure.IsNotNullOrEmpty(sourceData, nameof(sourceData), "Exception message"));
        }

        [TestMethod]
        public void IsNotNullOrEmpty_WithMessage_Object_Exception_Pass_Test()
        {
            var sourceData = "test";

            DomainEnsure.IsNotNullOrEmpty(sourceData, nameof(sourceData), "Exception message");

            Assert.AreEqual("test", sourceData);
        }

        [TestMethod]
        public void ThrowArgumentExceptionIfFuncIsFalse_ThrowException()
        {
            Func<bool> func = () => false;
            
            Assert.ThrowsException<ArgumentException>(
                () => DomainEnsure.ThrowArgumentExceptionIfFuncIsFalse(func, nameof(func), "Exception message"));
        }

        [TestMethod]
        public void ThrowArgumentExceptionIfFuncIsFalse_Pass()
        {
            Func<bool> func = () => true;

            DomainEnsure.ThrowArgumentExceptionIfFuncIsFalse(func, nameof(func), "Exception message");

            Assert.IsTrue(func.Invoke());
        }
    }
}