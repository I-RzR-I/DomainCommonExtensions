using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class ByteGetStringTests
    {
        [TestMethod]
        public void GetString_Latin1Mapping_Test()
        {
            var bytes = new byte[] { 72, 105, 0xE9 };

            var result = bytes.GetString();

            Assert.AreEqual("Hié", result);
        }

        [TestMethod]
        public void GetString_AllByteValues_Test()
        {
            var bytes = new byte[256];
            for (var i = 0; i < bytes.Length; i++)
                bytes[i] = (byte)i;

            var result = bytes.GetString();

            Assert.AreEqual(256, result.Length);
            for (var i = 0; i < bytes.Length; i++)
                Assert.AreEqual((char)i, result[i], $"Index: {i}");
        }

        [TestMethod]
        public void GetString_EmptyArray_Test()
        {
            var bytes = new byte[0];

            var result = bytes.GetString();

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void GetString_LargeArray_Test()
        {
            var bytes = new byte[200000];
            for (var i = 0; i < bytes.Length; i++)
                bytes[i] = 65;

            var result = bytes.GetString();

            Assert.AreEqual(new string('A', 200000), result);
        }

        [TestMethod]
        public void GetString_Null_Test()
        {
            byte[] bytes = null;

            var exception = Assert.ThrowsException<ArgumentNullException>(() => bytes.GetString());

            Assert.AreEqual("bytes", exception.ParamName);
        }
    }
}
