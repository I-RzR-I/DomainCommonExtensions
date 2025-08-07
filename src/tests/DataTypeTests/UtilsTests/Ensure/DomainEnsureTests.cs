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

using DataTypeTests.DataTests;
using DomainCommonExtensions.Resources.Enums;
using DomainCommonExtensions.Utilities.Ensure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

// ReSharper disable EntityNameCapturedOnly.Local
// ReSharper disable RedundantTypeArgumentsOfMethod
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

        [TestMethod]
        public void IsNotNullOrEmptyArgNull_ThrowException()
        {
            string source = null;

            Assert.ThrowsException<ArgumentNullException>(
                () => DomainEnsure.IsNotNullOrEmptyArgNull(source, nameof(source), "Exception message source is null"));
        }

        [TestMethod]
        public void IsNotNullOrEmptyArgNull_Pass()
        {
            string source = "Test";

            DomainEnsure.IsNotNullOrEmptyArgNull(source, nameof(source), "Exception message source is null");

            Assert.IsTrue(source.Equals("Test"));
        }

        [TestMethod]
        public void IsValidEnum_ThrowException()
        {
            ResultEnum? source = null;

            Assert.ThrowsException<ArgumentException>(
                () => DomainEnsure.IsValidEnum<ResultEnum>(source, nameof(source)));
        }

        [TestMethod]
        public void IsValidEnum_Pass()
        {
            var source = ResultEnum.Invalid;

            DomainEnsure.IsValidEnum<ResultEnum>(source, nameof(source));

            Assert.AreEqual(ResultEnum.Invalid, source);
        }

        [TestMethod]
        [DataRow(ExceptionType.Exception)]
        [DataRow(ExceptionType.ArgumentException)]
        [DataRow(ExceptionType.ArgumentNullException)]
        [DataRow(ExceptionType.ArgumentOutOfRangeException)]
        public void ThrowExceptionIfFuncIsFalse_Throw(ExceptionType exceptionType)
        {
            Func<bool> func = () => false;

            switch (exceptionType)
            {
                case ExceptionType.Exception:
                    Assert.ThrowsException<Exception>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsFalse(exceptionType, 
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.ArgumentException:
                    Assert.ThrowsException<ArgumentException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsFalse(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.ArgumentNullException:
                    Assert.ThrowsException<ArgumentNullException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsFalse(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.ArgumentOutOfRangeException:
                    Assert.ThrowsException<ArgumentOutOfRangeException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsFalse(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.DivideByZeroException:
                    Assert.ThrowsException<DivideByZeroException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsFalse(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.FileNotFoundException:
                    Assert.ThrowsException<FileNotFoundException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsFalse(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.FormatException:
                    Assert.ThrowsException<FormatException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsFalse(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.IndexOutOfRangeException:
                    Assert.ThrowsException<IndexOutOfRangeException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsFalse(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.InvalidCastException:
                    Assert.ThrowsException<InvalidCastException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsFalse(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.InvalidOperationException:
                    Assert.ThrowsException<InvalidOperationException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsFalse(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.MissingMemberException:
                    Assert.ThrowsException<MissingMemberException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsFalse(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.KeyNotFoundException:
                    Assert.ThrowsException<KeyNotFoundException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsFalse(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.NotSupportedException:
                    Assert.ThrowsException<NotSupportedException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsFalse(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.NullReferenceException:
                    Assert.ThrowsException<NullReferenceException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsFalse(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.OverflowException:
                    Assert.ThrowsException<OverflowException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsFalse(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.OutOfMemoryException:
                    Assert.ThrowsException<OutOfMemoryException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsFalse(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.StackOverflowException:
                    Assert.ThrowsException<StackOverflowException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsFalse(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.TimeoutException:
                    Assert.ThrowsException<TimeoutException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsFalse(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
            }
        }

        [TestMethod]
        [DataRow(ExceptionType.Exception)]
        [DataRow(ExceptionType.ArgumentException)]
        [DataRow(ExceptionType.ArgumentNullException)]
        [DataRow(ExceptionType.ArgumentOutOfRangeException)]
        public void ThrowExceptionIfFuncIsTrue_Throw(ExceptionType exceptionType)
        {
            Func<bool> func = () => true;

            switch (exceptionType)
            {
                case ExceptionType.Exception:
                    Assert.ThrowsException<Exception>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsTrue(exceptionType, 
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.ArgumentException:
                    Assert.ThrowsException<ArgumentException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsTrue(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.ArgumentNullException:
                    Assert.ThrowsException<ArgumentNullException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsTrue(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.ArgumentOutOfRangeException:
                    Assert.ThrowsException<ArgumentOutOfRangeException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsTrue(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.DivideByZeroException:
                    Assert.ThrowsException<DivideByZeroException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsTrue(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.FileNotFoundException:
                    Assert.ThrowsException<FileNotFoundException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsTrue(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.FormatException:
                    Assert.ThrowsException<FormatException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsTrue(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.IndexOutOfRangeException:
                    Assert.ThrowsException<IndexOutOfRangeException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsTrue(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.InvalidCastException:
                    Assert.ThrowsException<InvalidCastException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsTrue(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.InvalidOperationException:
                    Assert.ThrowsException<InvalidOperationException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsTrue(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.MissingMemberException:
                    Assert.ThrowsException<MissingMemberException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsTrue(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.KeyNotFoundException:
                    Assert.ThrowsException<KeyNotFoundException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsTrue(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.NotSupportedException:
                    Assert.ThrowsException<NotSupportedException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsTrue(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.NullReferenceException:
                    Assert.ThrowsException<NullReferenceException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsTrue(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.OverflowException:
                    Assert.ThrowsException<OverflowException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsTrue(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.OutOfMemoryException:
                    Assert.ThrowsException<OutOfMemoryException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsTrue(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.StackOverflowException:
                    Assert.ThrowsException<StackOverflowException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsTrue(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
                case ExceptionType.TimeoutException:
                    Assert.ThrowsException<TimeoutException>(
                        () => DomainEnsure.ThrowExceptionIfFuncIsTrue(exceptionType,
                            func, nameof(func), "Exception message"));
                    break;
            }
        }
    }
}