using System.Collections.Generic;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Collections;
using RzR.Extensions.Domain.Data;
using RzR.Extensions.Domain.Data.DataReader;
using RzR.Extensions.Domain.Text;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DataExtensionBindingTests
    {
        [TestMethod]
        public void ToBytes_StringReceiverKeepsAsciiResult_Test()
        {
            var result = "ab".ToBytes();

            CollectionAssert.AreEqual(new byte[] { 0x61, 0x62 }, result);
        }

        [TestMethod]
        public void ToEnum_StringReceiverKeepsCaseInsensitiveParse_Test()
        {
            var result = "second".ToEnum<OrdinalTestEnum>();

            Assert.AreEqual(OrdinalTestEnum.Second, result);
        }

        [TestMethod]
        public void ToEnum_StringReceiverUnknownNameKeepsDefault_Test()
        {
            var result = "Unknown".ToEnum<OrdinalTestEnum>();

            Assert.AreEqual(default(OrdinalTestEnum), result);
        }

        [TestMethod]
        public void GetValueOrDefault_DictionaryReceiverReturnsDictionaryValue_Test()
        {
            IDictionary<string, int> source = new Dictionary<string, int> { { "a", 5 } };

            var result = source.GetValueOrDefault("a");

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void GetValueOrDefault_DictionaryReceiverMissingKeyReturnsDefault_Test()
        {
            IDictionary<string, int> source = new Dictionary<string, int> { { "a", 5 } };

            var result = source.GetValueOrDefault("b");

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetValueOrDefault_RowDictionaryAsIDictionaryReturnsColumnValue_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(int), 42)))
            {
                IDictionary<string, object> row = reader.ToRowDictionary();

                Assert.AreEqual(42, row.GetValueOrDefault("ID"));
            }
        }
    }
}
