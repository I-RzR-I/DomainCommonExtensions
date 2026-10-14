using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Text;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class StringRemoveCharsTests
    {
        [TestMethod]
        public void RemoveStartChars_LengthLessThanSource_Test()
        {
            var result = "abcdef".RemoveStartChars(2);

            Assert.AreEqual("cdef", result);
        }

        [TestMethod]
        public void RemoveEndChars_LengthLessThanSource_Test()
        {
            var result = "abcdef".RemoveEndChars(2);

            Assert.AreEqual("abcd", result);
        }

        [TestMethod]
        public void RemoveStartChars_LengthEqualsSource_Test()
        {
            var result = "abcdef".RemoveStartChars(6);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void RemoveEndChars_LengthEqualsSource_Test()
        {
            var result = "abcdef".RemoveEndChars(6);

            Assert.AreEqual(string.Empty, result);
        }

        [DataTestMethod]
        [DataRow(7)]
        [DataRow(int.MaxValue)]
        public void RemoveStartChars_LengthGreaterThanSource_Test(int length)
        {
            var result = "abcdef".RemoveStartChars(length);

            Assert.AreEqual(string.Empty, result);
        }

        [DataTestMethod]
        [DataRow(7)]
        [DataRow(int.MaxValue)]
        public void RemoveEndChars_LengthGreaterThanSource_Test(int length)
        {
            var result = "abcdef".RemoveEndChars(length);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void RemoveStartChars_LengthZero_Test()
        {
            var result = "abcdef".RemoveStartChars(0);

            Assert.AreEqual("abcdef", result);
        }

        [TestMethod]
        public void RemoveEndChars_LengthZero_Test()
        {
            var result = "abcdef".RemoveEndChars(0);

            Assert.AreEqual("abcdef", result);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(int.MinValue)]
        public void RemoveStartChars_NegativeLength_Test(int length)
        {
            var result = "abcdef".RemoveStartChars(length);

            Assert.AreEqual("abcdef", result);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(int.MinValue)]
        public void RemoveEndChars_NegativeLength_Test(int length)
        {
            var result = "abcdef".RemoveEndChars(length);

            Assert.AreEqual("abcdef", result);
        }

        [TestMethod]
        public void RemoveStartChars_NullSource_Test()
        {
            string source = null;

            var result = source.RemoveStartChars(2);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void RemoveEndChars_NullSource_Test()
        {
            string source = null;

            var result = source.RemoveEndChars(2);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void RemoveStartChars_EmptySource_Test()
        {
            var result = string.Empty.RemoveStartChars(2);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void RemoveEndChars_EmptySource_Test()
        {
            var result = string.Empty.RemoveEndChars(2);

            Assert.AreEqual(string.Empty, result);
        }
    }
}
