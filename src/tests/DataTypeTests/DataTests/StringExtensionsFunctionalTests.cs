using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Text;

using static DataTypeTests.TestHelpers.StringTestHelper;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class StringExtensionsFunctionalTests
    {
        [DataTestMethod]
        [DataRow(null, null, true)]
        [DataRow(null, "a", false)]
        [DataRow("a", null, false)]
        [DataRow("", null, false)]
        [DataRow("", "", true)]
        [DataRow("a", "a", true)]
        [DataRow("a", "A", false)]
        [DataRow("a ", "a", false)]
        public void IsEquals_AcceptanceRows_ReturnsOrdinalCaseSensitiveEquality(string source, string compareValue, bool expected)
        {
            var result = source.IsEquals(compareValue);

            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(null, null, true)]
        [DataRow(null, "a", false)]
        [DataRow("a", null, false)]
        [DataRow("abc", "ABC", true)]
        [DataRow("i", "I", true)]
        [DataRow("a", "b", false)]
        [DataRow("", "", true)]
        [DataRow("", " ", false)]
        public void IsEqualsIgnoreCase_AcceptanceRows_ReturnsOrdinalCaseInsensitiveEquality(string source, string compareValue, bool expected)
        {
            var result = source.IsEqualsIgnoreCase(compareValue);

            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(null, "d", "d")]
        [DataRow("", "d", "d")]
        [DataRow("   ", "d", "d")]
        [DataRow("\t\n", "d", "d")]
        [DataRow(" x ", "d", " x ")]
        [DataRow("x", null, "x")]
        [DataRow(null, null, null)]
        public void IfIsMissing_AcceptanceRows_ReturnsFallbackOnlyWhenMissing(string source, string ifMissingValue, string expected)
        {
            var result = source.IfIsMissing(ifMissingValue);

            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(null, null, true)]
        [DataRow(null, "a", false)]
        [DataRow("a", null, false)]
        [DataRow("a", "a", true)]
        [DataRow("a", "A", false)]
        [DataRow("", "", true)]
        public void IsEqualsInvariantCulture_AcceptanceRows_ReturnsInvariantCaseSensitiveEquality(string source, string compareValue, bool expected)
        {
            var result = source.IsEqualsInvariantCulture(compareValue);

            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(null, (string)null)]
        [DataRow("", (string)null)]
        [DataRow("   ", (string)null)]
        [DataRow("\t\r\n", (string)null)]
        [DataRow("\U000000A0x\U000000A0", "x")]
        [DataRow(" ab c ", "ab c")]
        [DataRow("x", "x")]
        public void TrimToNull_AcceptanceRows_ReturnsTrimmedValueOrNull(string source, string expected)
        {
            var result = source.TrimToNull();

            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(null, "")]
        [DataRow("", "")]
        [DataRow("   ", "")]
        [DataRow(",,,", "")]
        [DataRow(" , , ", "")]
        [DataRow("a, b ,,c", "a|b|c")]
        [DataRow("a b", "a b")]
        [DataRow("a,a", "a|a")]
        public void SplitAndTrim_NoSeparators_SplitsOnCommaTrimsAndDropsEmpty(string source, string expectedJoined)
        {
            var expected = ParseExpected(expectedJoined);

            var result = source.SplitAndTrim();

            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("a b,c", "a b|c")]
        public void SplitAndTrim_NullSeparators_DefaultsToComma(string source, string expectedJoined)
        {
            var expected = ParseExpected(expectedJoined);

            var result = source.SplitAndTrim((char[])null);

            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("a b,c", "", "a b|c")]
        [DataRow("x.com; y.com ;", ";", "x.com|y.com")]
        [DataRow("a;b,c", ";,", "a|b|c")]
        public void SplitAndTrim_ExplicitSeparators_SplitsOnGivenCharsOrDefaultsToCommaWhenEmpty(string source, string separators, string expectedJoined)
        {
            var separatorChars = separators.ToCharArray();
            var expected = ParseExpected(expectedJoined);

            var result = source.SplitAndTrim(separatorChars);

            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(null, "/", null)]
        [DataRow("", "/", "")]
        [DataRow("  ", "/", "  ")]
        [DataRow("users", "/", "/users")]
        [DataRow("/users", "/", "/users")]
        [DataRow("users", null, "users")]
        [DataRow("users", "", "users")]
        [DataRow("line", " ", " line")]
        [DataRow("JSON", ".", ".JSON")]
        [DataRow("HTTPS://x", "https://", "HTTPS://x")]
        public void EnsureStartsWith_DefaultComparison_AcceptanceRows(string source, string prefix, string expected)
        {
            var result = source.EnsureStartsWith(prefix);

            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("HTTPS://x", "https://", StringComparison.Ordinal, "https://HTTPS://x")]
        public void EnsureStartsWith_ExplicitComparison_AcceptanceRows(string source, string prefix, StringComparison comparison, string expected)
        {
            var result = source.EnsureStartsWith(prefix, comparison);

            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(null, "/", null)]
        [DataRow("", "/", "")]
        [DataRow("https://api.x.com", "/", "https://api.x.com/")]
        [DataRow("https://api.x.com/", "/", "https://api.x.com/")]
        [DataRow("a", null, "a")]
        [DataRow("a", "", "a")]
        [DataRow("line", "\n", "line\n")]
        [DataRow("line\n", "\n", "line\n")]
        [DataRow("file.JSON", ".json", "file.JSON")]
        public void EnsureEndsWith_DefaultComparison_AcceptanceRows(string source, string suffix, string expected)
        {
            var result = source.EnsureEndsWith(suffix);

            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("file.JSON", ".json", StringComparison.Ordinal, "file.JSON.json")]
        public void EnsureEndsWith_ExplicitComparison_AcceptanceRows(string source, string suffix, StringComparison comparison, string expected)
        {
            var result = source.EnsureEndsWith(suffix, comparison);

            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(null, "@", null)]
        [DataRow("", "@", "")]
        [DataRow("   ", "x", "   ")]
        [DataRow("  x  ", " ", "")]
        [DataRow("user@mail.com", "@", "user")]
        [DataRow("nouser", "@", "nouser")]
        [DataRow("a@b@c", "@", "a")]
        [DataRow("@b", "@", "")]
        [DataRow("text/html; charset=utf-8", ";", "text/html")]
        [DataRow("abc", null, "abc")]
        [DataRow("abc", "", "abc")]
        [DataRow("KeyName=1", "name=", "KeyName=1")]
        public void SubstringBefore_DefaultCaseSensitive_AcceptanceRows(string source, string separator, string expected)
        {
            var result = source.SubstringBefore(separator);

            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("KeyName=1", "name=", true, "Key")]
        public void SubstringBefore_ExplicitIgnoreCase_AcceptanceRows(string source, string separator, bool ignoreCase, string expected)
        {
            var result = source.SubstringBefore(separator, ignoreCase);

            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(null, "=", null)]
        [DataRow("", "=", "")]
        [DataRow("   ", "=", "")]
        [DataRow("key=a=b", "=", "a=b")]
        [DataRow("key", "=", "")]
        [DataRow("key=", "=", "")]
        [DataRow("abc", null, "abc")]
        [DataRow("abc", "", "abc")]
        [DataRow("text/html; charset=utf-8", "charset=", "utf-8")]
        [DataRow("Basic abc", "Bearer ", "")]
        [DataRow("xx Bearer abc", "Bearer ", "abc")]
        [DataRow("X-Token: v", "token: ", "")]
        public void SubstringAfter_DefaultCaseSensitive_AcceptanceRows(string source, string separator, string expected)
        {
            var result = source.SubstringAfter(separator);

            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("X-Token: v", "token: ", true, "v")]
        public void SubstringAfter_ExplicitIgnoreCase_AcceptanceRows(string source, string separator, bool ignoreCase, string expected)
        {
            var result = source.SubstringAfter(separator, ignoreCase);

            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(null, "a", "b", null)]
        [DataRow("", "a", "b", "")]
        [DataRow("   ", " ", "_", "___")]
        [DataRow("Hello WORLD world", "world", "there", "Hello there there")]
        [DataRow("aaa", "AA", "b", "ba")]
        [DataRow("aaa", "a", "aa", "aaaaaa")]
        [DataRow("pwd=X&PWD=Y", "pwd=", "p=", "p=X&p=Y")]
        [DataRow("aXbxc", "x", null, "abc")]
        [DataRow("ABC", "abc", "", "")]
        [DataRow("....//", "../", "", "../")]
        public void ReplaceIgnoreCase_AcceptanceRows_ReplacesAllOrdinalIgnoreCaseMatches(string source, string oldValue, string newValue, string expected)
        {
            var result = source.ReplaceIgnoreCase(oldValue, newValue);

            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("abc", "", "z")]
        [DataRow("abc", null, "z")]
        [DataRow("abc", "x", "y")]
        public void ReplaceIgnoreCase_NoReplacementPossible_ReturnsSameReference(string source, string oldValue, string newValue)
        {
            var result = source.ReplaceIgnoreCase(oldValue, newValue);

            Assert.AreEqual("abc", result);
            Assert.AreSame(source, result);
        }
    }
}
