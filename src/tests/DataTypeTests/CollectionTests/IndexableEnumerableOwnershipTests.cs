using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Collections.Types;

namespace DataTypeTests.CollectionTests
{
    [TestClass]
    public class IndexableEnumerableOwnershipTests
    {
        [TestMethod]
        public void Dispose_CallerListKeepsItems_Test()
        {
            var data = new List<int> { 1, 2, 3 };
            var indexList = new IndexableEnumerable<int>(data);

            indexList.Dispose();

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, data);
            Assert.AreEqual(3, indexList.Count);
        }

        [TestMethod]
        public void Dispose_InitializedFromList_Test()
        {
            var data = new List<int> { 1, 2, 3 };
            var indexList = IndexableEnumerable<int>.Initialize(data);

            indexList.Dispose();

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, data);
            Assert.AreEqual(3, indexList.Count);
        }

        [TestMethod]
        public void Dispose_FromArray_Test()
        {
            var data = new[] { 1, 2, 3 };
            var indexList = new IndexableEnumerable<int>(data);

            indexList.Dispose();

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, data);
            Assert.AreEqual(3, indexList.Count);
        }

        [TestMethod]
        public void Constructor_NonListSequence_Test()
        {
            IEnumerable<int> data = Enumerable.Range(1, 3).Select(x => x * 2);

            var indexList = new IndexableEnumerable<int>(data);

            Assert.AreEqual(3, indexList.Count);
            Assert.AreEqual(2, indexList[0]);
            Assert.AreEqual(4, indexList[1]);
            Assert.AreEqual(6, indexList[2]);
            CollectionAssert.AreEqual(new[] { 2, 4, 6 }, indexList.ToArray());
        }

        [TestMethod]
        public void Constructor_ListAsSequence_Test()
        {
            var source = new List<int> { 1 };
            IEnumerable<int> data = source;

            var indexList = new IndexableEnumerable<int>(data);
            source.Add(2);

            Assert.AreEqual(2, indexList.Count);
            Assert.AreEqual(2, indexList[1]);
        }

        [TestMethod]
        public void Initialize_NonListSequence_Test()
        {
            IEnumerable<int> data = Enumerable.Range(1, 3).Select(x => x * 2);

            var indexList = IndexableEnumerable<int>.Initialize(data);

            Assert.AreEqual(3, indexList.Count);
            Assert.AreEqual(2, indexList[0]);
            Assert.AreEqual(4, indexList[1]);
            Assert.AreEqual(6, indexList[2]);
            CollectionAssert.AreEqual(new[] { 2, 4, 6 }, indexList.ToArray());
        }

        [TestMethod]
        public void Constructor_NullSequence_Test()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => new IndexableEnumerable<int>((IEnumerable<int>)null));

            Assert.AreEqual("list", exception.ParamName);
        }

        [TestMethod]
        public void Initialize_NullSequence_Test()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => IndexableEnumerable<int>.Initialize((IEnumerable<int>)null));

            Assert.AreEqual("list", exception.ParamName);
        }
    }
}
