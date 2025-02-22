﻿// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2022-08-16 17:35
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-16 18:35
// ***********************************************************************
//  <copyright file="ObjectTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using DomainCommonExtensions.DataTypeExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class ObjectTests
    {
        private readonly object _dataNull = null;
        private readonly object _dataString = "str1";
        private readonly object _date = new DateTime();

        [TestMethod]
        public void TryGetDateTimeFromDbObjTest()
        {
            Assert.IsNotNull(_date.TryGetDateTimeFromDbObj());
            Assert.IsNull(_dataNull.TryGetDateTimeFromDbObj());
            Assert.IsNull(_dataString.TryGetDateTimeFromDbObj());
        }

        [TestMethod]
        public void TryCastTest()
        {
            var castDate = _date.TryCast(out DateTime _);
            var castString = _dataString.TryCast<string>(out _);
            var castNull = _dataString.TryCast(out int _);

            Assert.IsTrue(castDate);
            Assert.IsTrue(castString);
            Assert.IsFalse(castNull);
        }

        [TestMethod]
        public void ThrowIfNullTest()
        {
            object obj = null;

            Assert.ThrowsException<Exception>(() => obj.ThrowIfNull("Obj is null"), "Obj is null");
        }

        [TestMethod]
        public void ThrowIfArgNullTest()
        {
            object obj = null;

            Assert.ThrowsException<ArgumentNullException>(() => obj.ThrowIfArgNull(nameof(obj)), "Value cannot be null. (Parameter 'obj')");
        }
    }
}