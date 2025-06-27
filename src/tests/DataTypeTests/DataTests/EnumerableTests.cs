// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2025-06-27 08:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-06-27 08:10
// ***********************************************************************
//  <copyright file="EnumerableTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System.Linq;
using DomainCommonExtensions.ArraysExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class EnumerableTests
    {
        [TestMethod]
        public void ChunkArray_Chunked_5_Test()
        {
            var arrInput = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            var chunks = arrInput.Chunked(5).ToArray();

            Assert.IsNotNull(chunks);
            Assert.AreEqual(3, chunks.Count());
            Assert.AreEqual(5, chunks[0].Count());
            Assert.AreEqual(5, chunks[1].Count());
            Assert.AreEqual(2, chunks[2].Count());
        }

        [TestMethod]
        public void ChunkArray_Chunked_2_Test()
        {
            var arrInput = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            var chunks = arrInput.Chunked(2).ToArray();

            Assert.IsNotNull(chunks);
            Assert.AreEqual(6, chunks.Count());
            Assert.AreEqual(2, chunks[0].Count());
            Assert.AreEqual(2, chunks[1].Count());
            Assert.AreEqual(2, chunks[2].Count());
            Assert.AreEqual(2, chunks[3].Count());
            Assert.AreEqual(2, chunks[4].Count());
            Assert.AreEqual(2, chunks[5].Count());
        }

        [TestMethod]
        public void ChunkArray_Chunked_20_Test()
        {
            var arrInput = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            var chunks = arrInput.Chunked(20).ToArray();

            Assert.IsNotNull(chunks);
            Assert.AreEqual(1, chunks.Count());
            Assert.AreEqual(12, chunks[0].Count());
        }
    }
}