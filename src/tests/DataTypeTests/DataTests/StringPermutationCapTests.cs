using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Text;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class StringPermutationCapTests
    {
        [TestMethod]
        [Timeout(5000)]
        public void GenerateStringPermutation_NullInput_Test()
        {
            string input = null;

            Assert.ThrowsException<ArgumentNullException>(() => input.GenerateStringPermutation());
        }

        [TestMethod]
        [Timeout(5000)]
        public void GenerateStringPermutation_TwoWords_Test()
        {
            var result = "a b".GenerateStringPermutation();

            CollectionAssert.AreEquivalent(new[] { "a b", "b a" }, result);
        }

        [TestMethod]
        [Timeout(5000)]
        public void GenerateStringPermutation_CommaSeparated_Test()
        {
            var result = "a,b".GenerateStringPermutation();

            CollectionAssert.AreEquivalent(new[] { "a b", "b a" }, result);
        }

        [DataRow("a  b")]
        [DataRow("a, b")]
        [DataRow("a,,b")]
        [DataRow(" a b ")]
        [DataRow(",a,b,")]
        [TestMethod]
        [Timeout(5000)]
        public void GenerateStringPermutation_EmptyTokensTwoWords_Test(string input)
        {
            var result = input.GenerateStringPermutation();

            CollectionAssert.AreEquivalent(new[] { "a b", "b a" }, result);
        }

        [DataRow("a,,b,c")]
        [DataRow("a  b  c")]
        [DataRow("a, b, c")]
        [TestMethod]
        [Timeout(5000)]
        public void GenerateStringPermutation_EmptyTokensThreeWords_Test(string input)
        {
            var result = input.GenerateStringPermutation();

            CollectionAssert.AreEquivalent(new[] { "a b c", "a c b", "b a c", "b c a", "c a b", "c b a" }, result);
        }

        [TestMethod]
        [Timeout(5000)]
        public void GenerateStringPermutation_FourWordsCount_Test()
        {
            var input = string.Join(" ", Enumerable.Range(0, 4).Select(i => "w" + i));

            var result = input.GenerateStringPermutation();

            Assert.AreEqual(24, result.Count);
            Assert.AreEqual(24, new HashSet<string>(result).Count);
            Assert.IsTrue(result.All(x => x.Split(' ').Length == 4));
        }

        [TestMethod]
        [Timeout(5000)]
        public void GenerateStringPermutation_EightWordsAtCap_Test()
        {
            var input = string.Join(" ", Enumerable.Range(0, 8).Select(i => "w" + i));

            var result = input.GenerateStringPermutation();

            Assert.AreEqual(40320, result.Count);
            Assert.AreEqual(40320, new HashSet<string>(result).Count);
        }

        [TestMethod]
        [Timeout(5000)]
        public void GenerateStringPermutation_LengthAtCap_Test()
        {
            var input = new string('a', 254) + " b";

            var result = input.GenerateStringPermutation();

            Assert.AreEqual(256, input.Length);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        [TestCategory("RedByConstruction")]
        [Timeout(5000)]
        public void GenerateStringPermutation_NineWordsOverCap_Test()
        {
            var input = string.Join(" ", Enumerable.Range(0, 9).Select(i => "w" + i));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => input.GenerateStringPermutation());
        }

        [TestMethod]
        [TestCategory("RedByConstruction")]
        [Timeout(5000)]
        public void GenerateStringPermutation_NineCommaWordsOverCap_Test()
        {
            var input = string.Join(",", Enumerable.Range(0, 9).Select(i => "w" + i));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => input.GenerateStringPermutation());
        }

        [TestMethod]
        [TestCategory("RedByConstruction")]
        [Timeout(5000)]
        public void GenerateStringPermutation_ThirteenWordsOverCap_Test()
        {
            var input = string.Join(" ", Enumerable.Range(0, 13).Select(i => "w" + i));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => input.GenerateStringPermutation());
        }

        [TestMethod]
        [TestCategory("RedByConstruction")]
        [Timeout(5000)]
        public void GenerateStringPermutation_LengthOverCap_Test()
        {
            var input = new string('a', 255) + " b";

            Assert.AreEqual(257, input.Length);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => input.GenerateStringPermutation());
        }

        [TestMethod]
        [TestCategory("RedByConstruction")]
        [Timeout(5000)]
        public void GenerateStringPermutation_EightWordsWithEmptyToken_Test()
        {
            var input = string.Join(" ", Enumerable.Range(0, 8).Select(i => "w" + i)).Replace("w3 ", "w3  ");

            var result = input.GenerateStringPermutation();

            Assert.AreEqual(40320, result.Count);
            Assert.AreEqual(40320, new HashSet<string>(result).Count);
        }

        [TestMethod]
        [TestCategory("RedByConstruction")]
        [Timeout(5000)]
        public void GenerateStringPermutation_WhitespaceOnly_Test()
        {
            var input = new string(' ', 3);

            var result = input.GenerateStringPermutation();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count <= 1);
            Assert.IsTrue(result.All(x => x.Length == 0));
        }

        [TestMethod]
        [Timeout(5000)]
        public void GenerateStringPermutation_ThreeWordsOrder_Test()
        {
            var result = "a b c".GenerateStringPermutation().ToArray();

            CollectionAssert.AreEqual(new[] { "c a b", "c b a", "b c a", "a c b", "b a c", "a b c" }, result);
        }
    }
}
