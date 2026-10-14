using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Collections;

namespace DataTypeTests.CollectionTests
{
    [TestClass]
    public class ListAddToListIfNotExistTests
    {
        [TestMethod]
        public void AddToListIfNotExist_NullSource_Test()
        {
            List<string> source = null;

            var exception = Assert.ThrowsException<ArgumentNullException>(() => source.AddToListIfNotExist("Name"));

            Assert.AreEqual("source", exception.ParamName);
        }

        [TestMethod]
        public void AddToListIfNotExist_MissingItem_Test()
        {
            var source = new List<int> { 1, 2 };

            source.AddToListIfNotExist(3);

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, source);
        }

        [TestMethod]
        public void AddToListIfNotExist_ExistingItem_Test()
        {
            var source = new List<int> { 1, 2 };

            source.AddToListIfNotExist(2);

            CollectionAssert.AreEqual(new[] { 1, 2 }, source);
        }

        [TestMethod]
        public void AddToListIfNotExist_NullItemTwice_Test()
        {
            var source = new List<string> { "Id" };

            source.AddToListIfNotExist(null);
            source.AddToListIfNotExist(null);

            CollectionAssert.AreEqual(new[] { "Id", null }, source);
        }
    }
}
