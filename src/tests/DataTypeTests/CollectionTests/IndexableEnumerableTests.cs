// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-01-06 23:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-06 23:15
// ***********************************************************************
//  <copyright file="IndexableEnumerableTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainCommonExtensions.ArraysExtensions;

#endregion

namespace DataTypeTests.CollectionTests
{
    [TestClass]
    public class IndexableEnumerableTests
    {
        [TestMethod]
        public void IndexableEnumerable_From_List_Test()
        {
            var sum = 0;
            var data = new List<int>() { 0, 2, 4, 6, 8, 10, 12 };

            //var indexList = new IndexableEnumerable<int>(data);
            var indexList = IndexableEnumerable<int>.Initialize(data);

            Assert.IsNotNull(indexList);
            Assert.AreEqual(7, indexList.Count);
            Assert.AreEqual(2, indexList[1]);

            indexList.DoForEach(i => { sum = sum + i; });
            Assert.AreEqual(42, sum);
        }

        [TestMethod]
        public async Task IndexableEnumerable_From_List_ExecuteActionAsync_Test()
        {
            var sum = 0;
            var data = new List<int>() { 0, 2, 4, 6, 8, 10, 12 };

            //var indexList = new IndexableEnumerable<int>(data);
            var indexList = IndexableEnumerable<int>.Initialize(data);

            Assert.IsNotNull(indexList);
            Assert.AreEqual(7, indexList.Count);
            Assert.AreEqual(2, indexList[1]);

            await indexList.DoActionForEachAsync(asyncAction: async (i) =>
            {
                sum = sum + i;

                await Task.CompletedTask;
            });

            Assert.AreEqual(42, sum);
        }

        [TestMethod]
        public void IndexableEnumerable_From_List_Func_Test()
        {
            var sum = 0;
            IIndexableEnumerable<int> Func() 
                => new IndexableEnumerable<int>(new List<int>() { 0, 2, 4, 6, 8, 10, 12 });
            
            var indexList = Func();

            Assert.IsNotNull(indexList);
            Assert.AreEqual(7, indexList.Count);
            Assert.AreEqual(2, indexList[1]);

            indexList.DoForEach(i => { sum = sum + i; });
            Assert.AreEqual(42, sum);
        }

        [TestMethod]
        public void IndexableEnumerable_From_Array_Test()
        {
            var sum = 0;
            var data = new int[] { 0, 2, 4, 6, 8, 10, 12 };

            var indexList = new IndexableEnumerable<int>(data);

            Assert.IsNotNull(indexList);
            Assert.AreEqual(7, indexList.Count);
            Assert.AreEqual(0, indexList[0]);

            var enumerator = indexList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                sum = sum + enumerator.Current;
            }
            Assert.AreEqual(42, sum);
        }

        [TestMethod]
        public void MutableIndexableEnumerable_Test()
        {
            IMutableIndexableEnumerable<int> Func() => new MutableIndexableEnumerable<int>(5);

            var collection = Func();

            Assert.AreEqual(0, collection.Count);
            Assert.AreEqual(5, collection.Capacity);

            collection.TryAdd(1);
            collection.TryAdd(2);
            collection.TryAdd(3);
            collection.TryAdd(4);

            var addItem5Result = collection.TryAdd(5);
            Assert.IsTrue(addItem5Result);

            var addItem6Result = collection.TryAdd(6);
            Assert.IsFalse(addItem6Result);

            Assert.AreEqual(5, collection.Count);
            Assert.AreEqual(5, collection.Capacity);

            Assert.AreEqual(5, collection[4]);

            collection.Set(4, 10);
            Assert.AreEqual(10, collection[4]);

            var removeItem = collection.TryRemoveAt(4);
            Assert.IsTrue(removeItem);

            Assert.AreEqual(4, collection.Count);
            Assert.AreEqual(5, collection.Capacity);

            collection.Clear();
            Assert.AreEqual(0, collection.Count);
            Assert.AreEqual(5, collection.Capacity);

            var addItemXResult = collection.TryAdd(15);
            Assert.IsTrue(addItemXResult);
            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(5, collection.Capacity);
            Assert.AreEqual(15, collection[0]);

            Assert.ThrowsException<IndexOutOfRangeException>(() => collection[1]);

            collection.Dispose();
            Assert.ThrowsException<ObjectDisposedException>(() => collection.TryAdd(10));
        }
    }
}