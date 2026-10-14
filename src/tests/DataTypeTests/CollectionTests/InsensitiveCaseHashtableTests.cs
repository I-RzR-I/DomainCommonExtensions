using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Collections.Types;

namespace DataTypeTests.CollectionTests
{
    [TestClass]
    public class InsensitiveCaseHashtableTests
    {
        [TestMethod]
        public void Indexer_TurkishCultureUpperI_Test()
        {
            using (new CultureScopeTestHelper("tr-TR"))
            {
                Assert.AreEqual("tr-TR", CultureInfo.CurrentCulture.Name);
                Assert.AreEqual("ı", "I".ToLower(CultureInfo.CurrentCulture));
                var table = new InsensitiveCaseHashtable();

                table.Add("ID", 1);

                Assert.AreEqual(1, table["id"]);
                Assert.AreEqual(1, table["Id"]);
            }
        }

        [TestMethod]
        public void ContainsKey_TurkishCultureUpperI_Test()
        {
            using (new CultureScopeTestHelper("tr-TR"))
            {
                Assert.AreEqual("tr-TR", CultureInfo.CurrentCulture.Name);
                Assert.AreEqual("ı", "I".ToLower(CultureInfo.CurrentCulture));
                var table = new InsensitiveCaseHashtable();

                table.Add("ID", 1);

                Assert.IsTrue(table.ContainsKey("Id"));
                Assert.IsTrue(table.ContainsKey("id"));
            }
        }

        [TestMethod]
        public void Add_KeyStoredLowerCase_Test()
        {
            var table = new InsensitiveCaseHashtable();

            table.Add("Name", 1);

            Assert.AreEqual(1, table.Count);
            CollectionAssert.Contains(table.Keys.Cast<object>().ToArray(), "name");
        }

        [TestMethod]
        public void Remove_DifferentCase_Test()
        {
            var table = new InsensitiveCaseHashtable();
            table.Add("Name", 1);

            table.Remove("NAME");

            Assert.AreEqual(0, table.Count);
            Assert.IsFalse(table.ContainsKey("name"));
        }

        [TestMethod]
        public void HashtableReference_IndexerDifferentCase_Test()
        {
            var table = new InsensitiveCaseHashtable();
            Hashtable hashtable = table;

            table.Add("name", 1);

            Assert.AreEqual(1, hashtable["NAME"]);
        }

        [TestMethod]
        public void HashtableReference_ContainsKeyDifferentCase_Test()
        {
            var table = new InsensitiveCaseHashtable();
            Hashtable hashtable = table;

            table.Add("name", 1);

            Assert.IsTrue(hashtable.ContainsKey("NAME"));
        }

        [TestMethod]
        public void Indexer_NullKey_Test()
        {
            var table = new InsensitiveCaseHashtable();

            Assert.ThrowsException<ArgumentNullException>(() => table[(string)null]);
        }

        [TestMethod]
        public void ContainsKey_NullKey_Test()
        {
            var table = new InsensitiveCaseHashtable();

            Assert.ThrowsException<ArgumentNullException>(() => table.ContainsKey((string)null));
        }

        [TestMethod]
        public void Add_NullKey_Test()
        {
            var table = new InsensitiveCaseHashtable();

            Assert.ThrowsException<ArgumentNullException>(() => table.Add((string)null, 1));
            Assert.AreEqual(0, table.Count);
        }

        [TestMethod]
        public void Remove_NullKey_Test()
        {
            var table = new InsensitiveCaseHashtable();

            Assert.ThrowsException<ArgumentNullException>(() => table.Remove((string)null));
        }
    }
}
