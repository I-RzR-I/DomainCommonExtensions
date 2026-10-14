using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class NumbersPrimeSuffixTests
    {
        [DataRow(-1)]
        [DataRow(-3)]
        [DataRow(-7)]
        [TestMethod]
        public void IsPrime_NegativeOdd_Test(int number)
        {
            var result = number.IsPrime();

            Assert.IsFalse(result, $"Number: {number}");
        }

        [DataRow(int.MinValue)]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(4)]
        [DataRow(9)]
        [TestMethod]
        public void IsPrime_NonPrime_Test(int number)
        {
            var result = number.IsPrime();

            Assert.IsFalse(result, $"Number: {number}");
        }

        [DataRow(2)]
        [DataRow(3)]
        [DataRow(5)]
        [DataRow(7)]
        [DataRow(97)]
        [TestMethod]
        public void IsPrime_Prime_Test(int number)
        {
            var result = number.IsPrime();

            Assert.IsTrue(result, $"Number: {number}");
        }

        [DataRow(-1, "st")]
        [DataRow(-2, "nd")]
        [DataRow(-3, "rd")]
        [DataRow(-21, "st")]
        [TestMethod]
        public void GetNumberSuffix_NegativeOrdinal_Test(int number, string expected)
        {
            var result = number.GetNumberSuffix();

            Assert.AreEqual(expected, result, $"Number: {number}");
        }

        [DataRow(-4, "th")]
        [DataRow(-11, "th")]
        [DataRow(-12, "th")]
        [DataRow(-13, "th")]
        [DataRow(-111, "th")]
        [TestMethod]
        public void GetNumberSuffix_NegativeTh_Test(int number, string expected)
        {
            var result = number.GetNumberSuffix();

            Assert.AreEqual(expected, result, $"Number: {number}");
        }

        [TestMethod]
        public void GetNumberSuffix_MinValue_Test()
        {
            var result = int.MinValue.GetNumberSuffix();

            Assert.AreEqual("th", result);
        }

        [DataRow(0, "th")]
        [DataRow(1, "st")]
        [DataRow(2, "nd")]
        [DataRow(3, "rd")]
        [DataRow(4, "th")]
        [DataRow(11, "th")]
        [DataRow(12, "th")]
        [DataRow(13, "th")]
        [DataRow(21, "st")]
        [DataRow(22, "nd")]
        [DataRow(23, "rd")]
        [DataRow(101, "st")]
        [DataRow(111, "th")]
        [DataRow(112, "th")]
        [TestMethod]
        public void GetNumberSuffix_NonNegative_Test(int number, string expected)
        {
            var result = number.GetNumberSuffix();

            Assert.AreEqual(expected, result, $"Number: {number}");
        }
    }
}
