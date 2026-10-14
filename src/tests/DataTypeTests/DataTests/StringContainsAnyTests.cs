using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Collections;
using RzR.Extensions.Domain.Text;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class StringContainsAnyTests
    {
        [TestMethod]
        public void ContainsAny_InvalidComparison_Test()
        {
            var exception = Assert.ThrowsException<ArgumentException>(
                () => "abc".ContainsAny((StringComparison)99, "a"));

            Assert.AreEqual("comparison", exception.ParamName);
        }

        [TestMethod]
        public void ContainsAny_InvalidComparisonNullSource_Test()
        {
            string source = null;

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.ContainsAny((StringComparison)(-1), (string[])null));

            Assert.AreEqual("comparison", exception.ParamName);
        }

        [TestMethod]
        public void ContainsAny_NullSource_Test()
        {
            string source = null;

            Assert.IsFalse(source.ContainsAny(StringComparison.Ordinal, "a", string.Empty));
        }

        [TestMethod]
        public void ContainsAny_NullArray_Test()
        {
            Assert.IsFalse("abc".ContainsAny(StringComparison.Ordinal, (string[])null));
        }

        [TestMethod]
        public void ContainsAny_EmptyArray_Test()
        {
            Assert.IsFalse("abc".ContainsAny(StringComparison.Ordinal));
            Assert.IsFalse("abc".ContainsAny(StringComparison.Ordinal, new string[0]));
        }

        [TestMethod]
        public void ContainsAny_OnlyNullElements_Test()
        {
            Assert.IsFalse("abc".ContainsAny(StringComparison.Ordinal, new string[] { null, null }));
        }

        [TestMethod]
        public void ContainsAny_NullElementSkipped_Test()
        {
            Assert.IsTrue("abc".ContainsAny(StringComparison.Ordinal, null, "x", "b"));
        }

        [TestMethod]
        public void ContainsAny_EmptyElement_Test()
        {
            Assert.IsTrue("abc".ContainsAny(StringComparison.Ordinal, "x", string.Empty));
            Assert.IsTrue(string.Empty.ContainsAny(StringComparison.Ordinal, string.Empty));
        }

        [TestMethod]
        public void ContainsAny_SubstringMatch_Test()
        {
            Assert.IsTrue("Hello World".ContainsAny(StringComparison.Ordinal, "xyz", "lo W"));
        }

        [TestMethod]
        public void ContainsAny_NoMatch_Test()
        {
            Assert.IsFalse("Hello World".ContainsAny(StringComparison.Ordinal, "xyz", "Worlds"));
        }

        [TestMethod]
        public void ContainsAny_OrdinalCaseSensitive_Test()
        {
            Assert.IsFalse("Hello World".ContainsAny(StringComparison.Ordinal, "WORLD"));
        }

        [TestMethod]
        public void ContainsAny_OrdinalIgnoreCase_Test()
        {
            Assert.IsTrue("Hello World".ContainsAny(StringComparison.OrdinalIgnoreCase, "WORLD"));
        }

        [TestMethod]
        public void ContainsAny_EnumerableOverloadBinding_Test()
        {
            Assert.IsTrue("abc".ContainsAny("xa"));
            Assert.IsFalse("abc".ContainsAny("xyz"));
        }

        [TestMethod]
        public void ContainsAny_EnumerableOverloadNotSubstring_Test()
        {
            Assert.IsTrue("abc".ContainsAny("ca"));
            Assert.IsFalse("abc".ContainsAny(StringComparison.Ordinal, "ca"));
        }

        [TestMethod]
        public void StartsWithAny_InvalidComparison_Test()
        {
            var exception = Assert.ThrowsException<ArgumentException>(
                () => "abc".StartsWithAny((StringComparison)99, "a"));

            Assert.AreEqual("comparison", exception.ParamName);
        }

        [TestMethod]
        public void StartsWithAny_InvalidComparisonNullSource_Test()
        {
            string source = null;

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.StartsWithAny((StringComparison)(-1), (string[])null));

            Assert.AreEqual("comparison", exception.ParamName);
        }

        [TestMethod]
        public void StartsWithAny_NullSource_Test()
        {
            string source = null;

            Assert.IsFalse(source.StartsWithAny(StringComparison.Ordinal, "a", string.Empty));
        }

        [TestMethod]
        public void StartsWithAny_NullArray_Test()
        {
            Assert.IsFalse("abc".StartsWithAny(StringComparison.Ordinal, (string[])null));
        }

        [TestMethod]
        public void StartsWithAny_EmptyArray_Test()
        {
            Assert.IsFalse("abc".StartsWithAny(StringComparison.Ordinal));
        }

        [TestMethod]
        public void StartsWithAny_OnlyNullElements_Test()
        {
            Assert.IsFalse("abc".StartsWithAny(StringComparison.Ordinal, new string[] { null }));
        }

        [TestMethod]
        public void StartsWithAny_NullElementSkipped_Test()
        {
            Assert.IsTrue("abc".StartsWithAny(StringComparison.Ordinal, null, "ab"));
        }

        [TestMethod]
        public void StartsWithAny_EmptyElement_Test()
        {
            Assert.IsTrue("abc".StartsWithAny(StringComparison.Ordinal, "x", string.Empty));
            Assert.IsTrue(string.Empty.StartsWithAny(StringComparison.Ordinal, string.Empty));
        }

        [TestMethod]
        public void StartsWithAny_PrefixOnly_Test()
        {
            Assert.IsFalse("abc".StartsWithAny(StringComparison.Ordinal, "b", "c", "bc"));
            Assert.IsTrue("abc".StartsWithAny(StringComparison.Ordinal, "b", "a"));
        }

        [TestMethod]
        public void StartsWithAny_LongerThanSource_Test()
        {
            Assert.IsFalse("ab".StartsWithAny(StringComparison.Ordinal, "abc"));
        }

        [TestMethod]
        public void StartsWithAny_OrdinalCaseSensitive_Test()
        {
            Assert.IsFalse("Hello World".StartsWithAny(StringComparison.Ordinal, "HELLO"));
        }

        [TestMethod]
        public void StartsWithAny_OrdinalIgnoreCase_Test()
        {
            Assert.IsTrue("Hello World".StartsWithAny(StringComparison.OrdinalIgnoreCase, "HELLO"));
        }

        [TestMethod]
        public void EndsWithAny_InvalidComparison_Test()
        {
            var exception = Assert.ThrowsException<ArgumentException>(
                () => "abc".EndsWithAny((StringComparison)99, "c"));

            Assert.AreEqual("comparison", exception.ParamName);
        }

        [TestMethod]
        public void EndsWithAny_InvalidComparisonNullSource_Test()
        {
            string source = null;

            var exception = Assert.ThrowsException<ArgumentException>(
                () => source.EndsWithAny((StringComparison)(-1), (string[])null));

            Assert.AreEqual("comparison", exception.ParamName);
        }

        [TestMethod]
        public void EndsWithAny_NullSource_Test()
        {
            string source = null;

            Assert.IsFalse(source.EndsWithAny(StringComparison.Ordinal, "c", string.Empty));
        }

        [TestMethod]
        public void EndsWithAny_NullArray_Test()
        {
            Assert.IsFalse("abc".EndsWithAny(StringComparison.Ordinal, (string[])null));
        }

        [TestMethod]
        public void EndsWithAny_EmptyArray_Test()
        {
            Assert.IsFalse("abc".EndsWithAny(StringComparison.Ordinal));
        }

        [TestMethod]
        public void EndsWithAny_OnlyNullElements_Test()
        {
            Assert.IsFalse("abc".EndsWithAny(StringComparison.Ordinal, new string[] { null }));
        }

        [TestMethod]
        public void EndsWithAny_NullElementSkipped_Test()
        {
            Assert.IsTrue("abc".EndsWithAny(StringComparison.Ordinal, null, "bc"));
        }

        [TestMethod]
        public void EndsWithAny_EmptyElement_Test()
        {
            Assert.IsTrue("abc".EndsWithAny(StringComparison.Ordinal, "x", string.Empty));
            Assert.IsTrue(string.Empty.EndsWithAny(StringComparison.Ordinal, string.Empty));
        }

        [TestMethod]
        public void EndsWithAny_SuffixOnly_Test()
        {
            Assert.IsFalse("abc".EndsWithAny(StringComparison.Ordinal, "a", "b", "ab"));
            Assert.IsTrue("abc".EndsWithAny(StringComparison.Ordinal, "a", "c"));
        }

        [TestMethod]
        public void EndsWithAny_LongerThanSource_Test()
        {
            Assert.IsFalse("bc".EndsWithAny(StringComparison.Ordinal, "abc"));
        }

        [TestMethod]
        public void EndsWithAny_OrdinalCaseSensitive_Test()
        {
            Assert.IsFalse("Hello World".EndsWithAny(StringComparison.Ordinal, "WORLD"));
        }

        [TestMethod]
        public void EndsWithAny_OrdinalIgnoreCase_Test()
        {
            Assert.IsTrue("Hello World".EndsWithAny(StringComparison.OrdinalIgnoreCase, "WORLD"));
        }
    }
}
