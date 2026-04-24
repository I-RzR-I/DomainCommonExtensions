// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-01-28 22:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-28 22:28
// ***********************************************************************
//  <copyright file="ObservableEnumeratorTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using RzR.Extensions.Domain.ArraysExtensions;

namespace DataTypeTests.DataTests.Array
{
    [TestClass]
    public class ObservableEnumeratorTests
    {
        [TestMethod]
        public void IsLast_ReturnsFalse_WhenMoreItemsExist()
        {
            var list = new List<int> { 1, 2, 3 };
            using var e = list.WithObservable();

            e.MoveNext(); // 1
            Assert.IsFalse(e.IsLast);
            
            e.MoveNext(); // 2
            Assert.IsFalse(e.IsLast);
            
            e.MoveNext(); // 3
            Assert.IsTrue(e.IsLast);
        }

        [TestMethod]
        public void IsLast_ReturnsTrue_OnLastItem()
        {
            var list = new List<int> { 1, 2 };
            using var e = list.WithObservable();

            e.MoveNext(); // 1
            e.MoveNext(); // 2

            Assert.IsTrue(e.IsLast);
        }

        [TestMethod]
        public void TryPeek_DoesNotAdvanceEnumerator()
        {
            var list = new List<int> { 10, 20 };
            using var e = list.WithObservable();

            e.MoveNext(); // 10
            Assert.IsTrue(e.TryPeek(out var next));
            Assert.AreEqual(20, next);
            Assert.AreEqual(10, e.Current);
        }

        [TestMethod]
        public void Reset_RestartsEnumeration()
        {
            var list = new List<string> { "A", "B" };
            using var e = list.WithObservable();

            e.MoveNext();
            e.MoveNext();

            e.Reset();
            e.MoveNext();

            Assert.AreEqual("A", e.Current);
        }

        [TestMethod]
        public void Reset_Throws_WhenCreatedFromEnumerator()
        {
            var list = new List<int> { 1, 2 };
            using var inner = list.GetEnumerator();
            using var e = inner.WithObservable();

            Assert.ThrowsException<NotSupportedException>(() => e.Reset());
        }

        [TestMethod]
        public void IsLast_Throws_BeforeMoveNext()
        {
            var list = new List<int> { 1 };
            using var e = list.WithObservable();

            Assert.ThrowsException<InvalidOperationException>(() => _ = e.IsLast);
        }

    }
}