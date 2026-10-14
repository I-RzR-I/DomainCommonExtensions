using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Text;

using static DataTypeTests.TestHelpers.StringTestHelper;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class StringTruncateEdgeTests
    {
        [DataTestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void Truncate_TextShorterThanMaxLength_Test(bool useDots)
        {
            var result = "abc".Truncate(5, useDots);

            Assert.AreEqual("abc", result);
        }

        [DataTestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void TruncateFromStart_TextShorterThanMaxLength_Test(bool useDots)
        {
            var result = "abc".TruncateFromStart(5, useDots);

            Assert.AreEqual("abc", result);
        }

        [DataTestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void Truncate_TextEqualToMaxLength_Test(bool useDots)
        {
            var result = "abcdef".Truncate(6, useDots);

            Assert.AreEqual("abcdef", result);
        }

        [DataTestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void TruncateFromStart_TextEqualToMaxLength_Test(bool useDots)
        {
            var result = "abcdef".TruncateFromStart(6, useDots);

            Assert.AreEqual("abcdef", result);
        }

        [TestMethod]
        public void Truncate_NullText_Test()
        {
            string text = null;

            var result = text.Truncate(3);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void TruncateFromStart_NullText_Test()
        {
            string text = null;

            var result = text.TruncateFromStart(3);

            Assert.AreEqual(string.Empty, result);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void Truncate_MaxLengthNotPositive_Test(int maxLength)
        {
            var result = "abcdef".Truncate(maxLength);

            Assert.AreEqual("abcdef", result);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void TruncateFromStart_MaxLengthNotPositive_Test(int maxLength)
        {
            var result = "abcdef".TruncateFromStart(maxLength);

            Assert.AreEqual("abcdef", result);
        }

        [TestMethod]
        public void Truncate_WithoutDotsTrimsCutSide_Test()
        {
            var result = "Hello World".Truncate(6);

            Assert.AreEqual("Hello", result);
        }

        [TestMethod]
        public void Truncate_WithDots_Test()
        {
            var result = "Hello World".Truncate(8, true);

            Assert.AreEqual("Hello...", result);
        }

        [TestMethod]
        public void TruncateFromStart_WithDots_Test()
        {
            var result = "Hello World".TruncateFromStart(8, true);

            Assert.AreEqual("...World", result);
        }

        [TestMethod]
        public void Truncate_WithDotsMaxLengthFour_Test()
        {
            var result = "abcdef".Truncate(4, true);

            Assert.AreEqual("a...", result);
        }

        [TestMethod]
        public void TruncateFromStart_WithDotsMaxLengthFour_Test()
        {
            var result = "abcdef".TruncateFromStart(4, true);

            Assert.AreEqual("...f", result);
        }

        [DataTestMethod]
        [DataRow(1, "a")]
        [DataRow(2, "ab")]
        [DataRow(3, "abc")]
        public void Truncate_WithDotsMaxLengthUpToThree_Test(int maxLength, string expected)
        {
            var result = "abcdef".Truncate(maxLength, true);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result.Length <= maxLength);
        }

        [DataTestMethod]
        [DataRow(1, "f")]
        [DataRow(2, "ef")]
        [DataRow(3, "def")]
        public void TruncateFromStart_WithDotsMaxLengthUpToThree_Test(int maxLength, string expected)
        {
            var result = "abcdef".TruncateFromStart(maxLength, true);

            Assert.AreEqual(expected, result);
            Assert.IsTrue(result.Length <= maxLength);
        }

        [TestMethod]
        public void Truncate_CutInsideSurrogatePair_Test()
        {
            var result = ("ab" + Emoji + "cd").Truncate(3);

            Assert.AreEqual("ab", result);
            Assert.IsFalse(char.IsHighSurrogate(result[result.Length - 1]));
        }

        [TestMethod]
        public void TruncateFromStart_CutInsideSurrogatePair_Test()
        {
            var result = ("ab" + Emoji + "cd").TruncateFromStart(3);

            Assert.AreEqual("cd", result);
            Assert.IsFalse(char.IsLowSurrogate(result[0]));
        }

        [TestMethod]
        public void Truncate_CutAfterSurrogatePair_Test()
        {
            var result = ("ab" + Emoji + "cd").Truncate(4);

            Assert.AreEqual("ab" + Emoji, result);
        }

        [TestMethod]
        public void Truncate_WithDotsCutInsideSurrogatePair_Test()
        {
            var result = ("ab" + Emoji + "cdef").Truncate(6, true);

            Assert.AreEqual("ab...", result);
        }

        [TestMethod]
        public void Truncate_WithDotsSurrogatePairAtStart_Test()
        {
            var result = (Emoji + "abcd").Truncate(4, true);

            Assert.AreEqual("...", result);
        }

        [TestMethod]
        public void TruncateFromStart_WithDotsCutInsideSurrogatePair_Test()
        {
            var result = ("abcde" + Emoji + "x").TruncateFromStart(5, true);

            Assert.AreEqual("...x", result);
        }

        [TestMethod]
        public void TruncateFromStart_CutBeforeSurrogatePair_Test()
        {
            var result = ("ab" + Emoji + "cd").TruncateFromStart(4);

            Assert.AreEqual(Emoji + "cd", result);
        }
    }
}
