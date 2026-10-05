using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Text;

using static DataTypeTests.TestHelpers.StringTestHelper;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class StringExtensionsEdgeCaseTests
    {
        [TestMethod]
        public void AllMethods_AnyCombinationOfNullEmptyWhitespaceArguments_NeverThrowAndHonourNullSourceContract()
        {
            var values = new[] { null, "", " ", "\t", "\r\n", "a", "A", "ab", Emoji, Nbsp };
            var separatorSets = new[] { null, new char[0], new[] { ',' }, new[] { ' ' } };
            var comparisons = new[] { StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase };

            foreach (var source in values)
            {
                var ctx = "source=" + Show(source);

                var trimmed = source.TrimToNull();
                if (source == null)
                    Assert.IsNull(trimmed, ctx);

                foreach (var seps in separatorSets)
                {
                    var split = source.SplitAndTrim(seps);
                    Assert.IsNotNull(split, "SplitAndTrim returned null. " + ctx);
                    if (source == null)
                        Assert.AreEqual(0, split.Length, ctx);
                }

                foreach (var other in values)
                {
                    var ctx2 = ctx + " other=" + Show(other);

                    var eq = source.IsEquals(other);
                    var eqIc = source.IsEqualsIgnoreCase(other);
                    var eqInv = source.IsEqualsInvariantCulture(other);
                    if (source == null || other == null)
                    {
                        var bothNull = source == null && other == null;
                        Assert.AreEqual(bothNull, eq, "IsEquals " + ctx2);
                        Assert.AreEqual(bothNull, eqIc, "IsEqualsIgnoreCase " + ctx2);
                        Assert.AreEqual(bothNull, eqInv, "IsEqualsInvariantCulture " + ctx2);
                    }

                    var missing = source.IfIsMissing(other);
                    if (string.IsNullOrWhiteSpace(source))
                        Assert.AreSame(other, missing, "IfIsMissing " + ctx2);
                    else
                        Assert.AreSame(source, missing, "IfIsMissing " + ctx2);

                    foreach (var cmp in comparisons)
                    {
                        var starts = source.EnsureStartsWith(other, cmp);
                        var ends = source.EnsureEndsWith(other, cmp);
                        if (string.IsNullOrWhiteSpace(source) || string.IsNullOrEmpty(other))
                        {
                            Assert.AreSame(source, starts, "EnsureStartsWith " + ctx2 + " cmp=" + cmp);
                            Assert.AreSame(source, ends, "EnsureEndsWith " + ctx2 + " cmp=" + cmp);
                        }
                    }

                    foreach (var ignoreCase in new[] { false, true })
                    {
                        var before = source.SubstringBefore(other, ignoreCase);
                        var after = source.SubstringAfter(other, ignoreCase);
                        if (source == null)
                        {
                            Assert.IsNull(before, "SubstringBefore " + ctx2);
                            Assert.IsNull(after, "SubstringAfter " + ctx2);
                        }
                        else if (string.IsNullOrEmpty(other))
                        {
                            Assert.AreEqual(source, before, "SubstringBefore " + ctx2);
                            Assert.AreEqual(source, after, "SubstringAfter " + ctx2);
                        }
                    }

                    foreach (var newValue in values)
                    {
                        var replaced = source.ReplaceIgnoreCase(other, newValue);
                        if (source == null)
                            Assert.IsNull(replaced, "ReplaceIgnoreCase " + ctx2 + " new=" + Show(newValue));
                        else if (string.IsNullOrEmpty(other))
                            Assert.AreSame(source, replaced, "ReplaceIgnoreCase " + ctx2 + " new=" + Show(newValue));
                    }
                }
            }
        }

        [TestMethod]
        public void IsEqualsFamily_EmptyVersusNull_IsFalseInBothDirections()
        {
            Assert.IsFalse(string.Empty.IsEquals(null));
            Assert.IsFalse(((string)null).IsEquals(string.Empty));
            Assert.IsFalse(string.Empty.IsEqualsIgnoreCase(null));
            Assert.IsFalse(((string)null).IsEqualsIgnoreCase(string.Empty));
            Assert.IsFalse(string.Empty.IsEqualsInvariantCulture(null));
            Assert.IsFalse(((string)null).IsEqualsInvariantCulture(string.Empty));
        }

        [DataTestMethod]
        [DataRow("", (string)null, null)]
        [DataRow("\t", (string)null, null)]
        [DataRow("", "", "")]
        [DataRow(Nbsp, "d", "d")]
        [DataRow(EmSpace, "d", "d")]
        public void IfIsMissing_MissingSource_ReturnsFallbackEvenWhenFallbackIsNullOrEmpty(
            string source, string fallback, string expected)
        {
            var result = source.IfIsMissing(fallback);

            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(Zwsp, "d")]
        [DataRow(" x ", (string)null)]
        [DataRow("x", "")]
        [DataRow(SoftHyphen, "d")]
        public void IfIsMissing_PresentSource_ReturnsSameReferenceUntrimmed(string source, string fallback)
        {
            var result = source.IfIsMissing(fallback);

            Assert.AreSame(source, result);
        }

        [TestMethod]
        public void SplitAndTrim_NullSourceAndNullSeparators_ReturnsEmptyArrayNotNull()
        {
            var result = ((string)null).SplitAndTrim((char[])null);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void SplitAndTrim_CallerSeparatorArray_IsNotMutated()
        {
            var separators = new[] { ';', ',' };

            "a; b, c".SplitAndTrim(separators);

            Assert.AreEqual(2, separators.Length);
            Assert.AreEqual(';', separators[0]);
            Assert.AreEqual(',', separators[1]);
        }

        [TestMethod]
        public void EnsureAffix_NullPrefixOrSuffixWithExplicitOrdinal_ReturnsSameReference()
        {
            const string source = "x";

            Assert.AreSame(source, source.EnsureStartsWith(null, StringComparison.Ordinal));
            Assert.AreSame(source, source.EnsureEndsWith(null, StringComparison.Ordinal));
            Assert.IsNull(((string)null).EnsureStartsWith(null));
            Assert.IsNull(((string)null).EnsureEndsWith(null));
        }

        [TestMethod]
        public void SubstringBeforeAfter_NullSourceNullSeparatorIgnoreCase_ReturnsNull()
        {
            Assert.IsNull(((string)null).SubstringBefore(null, true));
            Assert.IsNull(((string)null).SubstringAfter(null, true));
            Assert.IsNull(((string)null).SubstringBefore("", true));
            Assert.IsNull(((string)null).SubstringAfter("", true));
        }

        [TestMethod]
        public void SubstringBeforeAfter_EmptySourceWithNullOrEmptySeparator_ReturnsEmpty()
        {
            Assert.AreEqual("", "".SubstringBefore(null));
            Assert.AreEqual("", "".SubstringAfter(null));
            Assert.AreEqual("", "".SubstringBefore(""));
            Assert.AreEqual("", "".SubstringAfter(""));
        }

        [TestMethod]
        public void ReplaceIgnoreCase_AllArgumentsNull_ReturnsNull()
        {
            Assert.IsNull(((string)null).ReplaceIgnoreCase(null, null));
            Assert.IsNull(((string)null).ReplaceIgnoreCase("", null));
            Assert.IsNull(((string)null).ReplaceIgnoreCase("a", null));
        }

        [TestMethod]
        public void ReplaceIgnoreCase_NullNewValue_RemovesAllCaseInsensitiveMatches()
        {
            Assert.AreEqual("ac", "abc".ReplaceIgnoreCase("B", null));
            Assert.AreEqual("", "AbAB".ReplaceIgnoreCase("ab", null));
            Assert.AreEqual("--", "-xX-".ReplaceIgnoreCase("x", null));
        }

        [TestMethod]
        public void TrimToNull_GeneratedInputs_ResultIsNullOrNonEmptyWithNoWhiteSpaceAtEitherEnd()
        {
            var tokens = new[]
            {
                " ", "\t", "\n", "\r", Nbsp, EmSpace, IdeographicSpace, LineSeparator, Zwsp, "a", "b c", Emoji, "x"
            };
            var inputs = Generate(1001, tokens, 400, 6);

            foreach (var input in inputs)
            {
                var ctx = "input=" + Show(input);

                var result = input.TrimToNull();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Assert.IsNull(result, ctx);
                    continue;
                }

                Assert.IsNotNull(result, ctx);
                Assert.IsTrue(result.Length > 0, "Never returns empty. " + ctx);
                Assert.IsFalse(char.IsWhiteSpace(result[0]), "Leading white-space. " + ctx);
                Assert.IsFalse(char.IsWhiteSpace(result[result.Length - 1]), "Trailing white-space. " + ctx);

                var index = input.IndexOf(result, StringComparison.Ordinal);
                Assert.IsTrue(index >= 0, "Result must be a substring of input. " + ctx);
                for (var i = 0; i < index; i++)
                    Assert.IsTrue(char.IsWhiteSpace(input[i]), "Removed a non white-space leading char. " + ctx);
                for (var i = index + result.Length; i < input.Length; i++)
                    Assert.IsTrue(char.IsWhiteSpace(input[i]), "Removed a non white-space trailing char. " + ctx);

                Assert.AreEqual(result, result.TrimToNull(), "Not idempotent. " + ctx);
            }
        }

        [TestMethod]
        public void SplitAndTrim_GeneratedInputs_NeverNullNoEmptyOrUntrimmedEntriesAndOrderPreserved()
        {
            var tokens = new[] { "a", "b", "c d", " ", "\t", ",", ";", Nbsp, Zwsp, ",,", "\r\n" };
            var inputs = Generate(2002, tokens, 300, 8);
            var separatorSets = new[]
            {
                null, new char[0], new[] { ',' }, new[] { ';' }, new[] { ',', ';' }, new[] { ' ' }
            };

            foreach (var input in inputs)
            {
                foreach (var seps in separatorSets)
                {
                    var effective = seps == null || seps.Length == 0 ? new[] { ',' } : seps;
                    var ctx = "input=" + Show(input) + " seps=" + (seps == null ? "<null>" : new string(seps));

                    var result = input.SplitAndTrim(seps);

                    Assert.IsNotNull(result, ctx);
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Assert.AreEqual(0, result.Length, ctx);
                        continue;
                    }

                    var position = 0;
                    foreach (var entry in result)
                    {
                        Assert.IsNotNull(entry, ctx);
                        Assert.IsTrue(entry.Length > 0, "Empty entry. " + ctx);
                        Assert.IsFalse(char.IsWhiteSpace(entry[0]), "Leading white-space. " + ctx);
                        Assert.IsFalse(char.IsWhiteSpace(entry[entry.Length - 1]), "Trailing white-space. " + ctx);
                        Assert.IsTrue(entry.IndexOfAny(effective) < 0, "Entry contains a separator. " + ctx);

                        var found = input.IndexOf(entry, position, StringComparison.Ordinal);
                        Assert.IsTrue(found >= 0, "Entry order not preserved. " + ctx);
                        position = found + entry.Length;
                    }

                    if (seps == null || seps.Length == 0)
                        AssertSequence(input.SplitAndTrim(','), result, "Default must equal ','. " + ctx);
                }
            }
        }

        [TestMethod]
        public void EnsureStartsWith_GeneratedInputs_ResultStartsWithPrefixAndIsIdempotent()
        {
            var tokens = new[] { "a", "A", "b", "/", " ", "x y", "\t", Emoji, "ab" };
            var inputs = Generate(3003, tokens, 200, 5);
            var prefixes = new[] { "/", "a", "Ab", "ab/", " ", "\t", "\r\n", Emoji, "AB", "abcdefghij" };
            var comparisons = new[] { StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase };

            foreach (var source in inputs)
            foreach (var prefix in prefixes)
            {
                Assert.AreEqual(source.EnsureStartsWith(prefix, StringComparison.OrdinalIgnoreCase),
                    source.EnsureStartsWith(prefix),
                    "Default must be OrdinalIgnoreCase. source=" + Show(source) + " prefix=" + Show(prefix));

                foreach (var cmp in comparisons)
                {
                    var ctx = "source=" + Show(source) + " prefix=" + Show(prefix) + " cmp=" + cmp;

                    var result = source.EnsureStartsWith(prefix, cmp);

                    if (string.IsNullOrWhiteSpace(source))
                    {
                        Assert.AreSame(source, result, "Missing source must pass through. " + ctx);
                        continue;
                    }

                    Assert.IsTrue(result.StartsWith(prefix, cmp), "Result lacks prefix. " + ctx);
                    Assert.IsTrue(result.EndsWith(source, StringComparison.Ordinal), "Source not preserved. " + ctx);

                    if (source.StartsWith(prefix, cmp))
                        Assert.AreSame(source, result, "Already prefixed must return same reference. " + ctx);
                    else
                        Assert.AreEqual(prefix + source, result, ctx);

                    Assert.AreSame(result, result.EnsureStartsWith(prefix, cmp), "Not idempotent. " + ctx);
                }
            }
        }

        [TestMethod]
        public void EnsureEndsWith_GeneratedInputs_ResultEndsWithSuffixAndIsIdempotent()
        {
            var tokens = new[] { "a", "A", "b", "/", " ", "x y", "\t", Emoji, "ab", "\r" };
            var inputs = Generate(4004, tokens, 200, 5);
            var suffixes = new[] { "/", "a", "Ab", "/ab", " ", "\t", "\r\n", "\n", Emoji, "AB", "abcdefghij" };
            var comparisons = new[] { StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase };

            foreach (var source in inputs)
            foreach (var suffix in suffixes)
            {
                Assert.AreEqual(source.EnsureEndsWith(suffix, StringComparison.OrdinalIgnoreCase),
                    source.EnsureEndsWith(suffix),
                    "Default must be OrdinalIgnoreCase. source=" + Show(source) + " suffix=" + Show(suffix));

                foreach (var cmp in comparisons)
                {
                    var ctx = "source=" + Show(source) + " suffix=" + Show(suffix) + " cmp=" + cmp;

                    var result = source.EnsureEndsWith(suffix, cmp);

                    if (string.IsNullOrWhiteSpace(source))
                    {
                        Assert.AreSame(source, result, "Missing source must pass through. " + ctx);
                        continue;
                    }

                    Assert.IsTrue(result.EndsWith(suffix, cmp), "Result lacks suffix. " + ctx);
                    Assert.IsTrue(result.StartsWith(source, StringComparison.Ordinal), "Source not preserved. " + ctx);

                    if (source.EndsWith(suffix, cmp))
                        Assert.AreSame(source, result, "Already suffixed must return same reference. " + ctx);
                    else
                        Assert.AreEqual(source + suffix, result, ctx);

                    Assert.AreSame(result, result.EnsureEndsWith(suffix, cmp), "Not idempotent. " + ctx);
                }
            }
        }

        [TestMethod]
        public void EnsureStartsWith_ThenTrimPrefix_RoundTripsForNonWhiteSpacePrefixes()
        {
            var tokens = new[] { "a", "A", "b", "/", " ", "x", Emoji };
            var inputs = Generate(5005, tokens, 200, 5);
            var prefixes = new[] { "/", "a", "Ab", "ab/", Emoji, "AB", "a b" };

            foreach (var source in inputs)
            foreach (var prefix in prefixes)
            {
                if (string.IsNullOrWhiteSpace(source))
                    continue;

                var ctx = "source=" + Show(source) + " prefix=" + Show(prefix);

                var ensured = source.EnsureStartsWith(prefix);
                var roundTrip = ensured.TrimPrefix(prefix);

                if (source.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    Assert.AreSame(source, ensured, ctx);
                    Assert.AreEqual(source.Substring(prefix.Length), roundTrip, ctx);
                }
                else
                {
                    Assert.AreEqual(source, roundTrip, "Inverse of TrimPrefix violated. " + ctx);
                }
            }
        }

        [TestMethod]
        public void EnsureEndsWith_ThenTrimSuffix_RoundTripsForNonWhiteSpaceSuffixes()
        {
            var tokens = new[] { "a", "A", "b", "/", " ", "x", Emoji };
            var inputs = Generate(6006, tokens, 200, 5);
            var suffixes = new[] { "/", "a", "Ab", "/ab", Emoji, "AB", "a b" };

            foreach (var source in inputs)
            foreach (var suffix in suffixes)
            {
                if (string.IsNullOrWhiteSpace(source))
                    continue;

                var ctx = "source=" + Show(source) + " suffix=" + Show(suffix);

                var ensured = source.EnsureEndsWith(suffix);
                var roundTrip = ensured.TrimSuffix(suffix);

                if (source.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                {
                    Assert.AreSame(source, ensured, ctx);
                    Assert.AreEqual(source.Substring(0, source.Length - suffix.Length), roundTrip, ctx);
                }
                else
                {
                    Assert.AreEqual(source, roundTrip, "Inverse of TrimSuffix violated. " + ctx);
                }
            }
        }

        [TestMethod]
        public void EnsureAffix_WhiteSpaceAffix_IsAddedButNotRemovedByTrimPrefixOrTrimSuffix_CurrentBehaviour()
        {
            var prefixed = "line".EnsureStartsWith(" ");
            var suffixed = "line".EnsureEndsWith("\n");

            Assert.AreEqual(" line", prefixed);
            Assert.AreEqual("line\n", suffixed);
            Assert.AreEqual(" line", prefixed.TrimPrefix(" "));
            Assert.AreEqual("line\n", suffixed.TrimSuffix("\n"));
        }

        [TestMethod]
        public void SubstringBeforeAndAfter_GeneratedInputsOrdinal_RecomposeSourceWhenSeparatorPresent()
        {
            var tokens = new[] { "a", "A", "b", "=", ";", " ", "==", Emoji, "ab", "\r\n" };
            var inputs = Generate(7007, tokens, 300, 6);
            var separators = new[] { "=", "a", "A", "ab", "==", ";", " ", Emoji, "A=", "zz", "\r\n", "\n" };

            foreach (var source in inputs)
            foreach (var separator in separators)
            {
                var ctx = "source=" + Show(source) + " separator=" + Show(separator);

                var before = source.SubstringBefore(separator);
                var after = source.SubstringAfter(separator);

                Assert.AreEqual(source.SubstringBefore(separator, false), before, "Default must be case-sensitive. " + ctx);
                Assert.AreEqual(source.SubstringAfter(separator, false), after, "Default must be case-sensitive. " + ctx);

                if (source == null)
                {
                    Assert.IsNull(before, ctx);
                    Assert.IsNull(after, ctx);
                    continue;
                }

                if (source.IndexOf(separator, StringComparison.Ordinal) >= 0)
                {
                    Assert.AreEqual(source, before + separator + after, "Recomposition failed. " + ctx);
                    Assert.IsTrue(before.IndexOf(separator, StringComparison.Ordinal) < 0,
                        "Before must precede the FIRST occurrence. " + ctx);
                }
                else
                {
                    Assert.AreEqual(source, before, "Not found: Before returns whole source. " + ctx);
                    Assert.AreEqual(string.Empty, after, "Not found: After returns empty. " + ctx);
                }
            }
        }

        [TestMethod]
        public void SubstringBeforeAndAfter_GeneratedInputsIgnoreCase_SplitAroundCaseInsensitiveFirstMatch()
        {
            var tokens = new[] { "a", "A", "b", "B", "=", "x" };
            var inputs = Generate(8008, tokens, 300, 8);
            var separators = new[] { "a", "A", "ab", "AB", "b=", "Xa", "zz" };

            foreach (var source in inputs)
            foreach (var separator in separators)
            {
                if (source == null)
                    continue;

                var ctx = "source=" + Show(source) + " separator=" + Show(separator);

                var before = source.SubstringBefore(separator, true);
                var after = source.SubstringAfter(separator, true);

                var index = source.IndexOf(separator, StringComparison.OrdinalIgnoreCase);
                if (index >= 0)
                {
                    Assert.AreEqual(source.Substring(0, index), before, ctx);
                    Assert.AreEqual(source.Substring(index + separator.Length), after, ctx);
                    Assert.AreEqual(source.Length, before.Length + separator.Length + after.Length, ctx);
                    Assert.IsTrue(string.Equals(source.Substring(before.Length, separator.Length), separator,
                        StringComparison.OrdinalIgnoreCase), ctx);
                }
                else
                {
                    Assert.AreEqual(source, before, ctx);
                    Assert.AreEqual(string.Empty, after, ctx);
                }
            }
        }

        [TestMethod]
        public void ReplaceIgnoreCase_GeneratedAsciiInputs_MatchesBclOrdinalIgnoreCaseReplace()
        {
            var tokens = new[] { "a", "A", "b", "B", "-", "ab", "AB" };
            var inputs = Generate(9009, tokens, 300, 10);
            var oldValues = new[] { "a", "A", "ab", "AB", "aa", "b-", "-", "aAa", "xyz", "bA", "abababababababababab" };
            var newValues = new[] { null, "", "a", "AA", "ab", "-", "zz", "aAaA" };

            foreach (var source in inputs)
            foreach (var oldValue in oldValues)
            foreach (var newValue in newValues)
            {
                if (source == null)
                    continue;

                var ctx = "source=" + Show(source) + " old=" + Show(oldValue) + " new=" + Show(newValue);

                var actual = source.ReplaceIgnoreCase(oldValue, newValue);

                var expected = source.Replace(oldValue, newValue, StringComparison.OrdinalIgnoreCase);
                Assert.AreEqual(expected, actual, ctx);

                if (source.IndexOf(oldValue, StringComparison.OrdinalIgnoreCase) < 0)
                    Assert.AreSame(source, actual, "No match must return the same reference. " + ctx);
            }
        }

        [TestMethod]
        public void IsEqualsFamily_GeneratedAsciiPairs_AreConsistentWithOrdinalOracles()
        {
            var tokens = new[] { "a", "A", "b", "Z", "1" };
            var inputs = Generate(1111, tokens, 40, 3);

            foreach (var left in inputs)
            foreach (var right in inputs)
            {
                var ctx = "left=" + Show(left) + " right=" + Show(right);

                var eq = left.IsEquals(right);
                var eqIc = left.IsEqualsIgnoreCase(right);
                var eqInv = left.IsEqualsInvariantCulture(right);

                Assert.AreEqual(string.CompareOrdinal(left, right) == 0, eq, "IsEquals " + ctx);
                Assert.AreEqual(eq, right.IsEquals(left), "IsEquals not symmetric. " + ctx);
                Assert.AreEqual(eqIc, right.IsEqualsIgnoreCase(left), "IsEqualsIgnoreCase not symmetric. " + ctx);
                if (eq)
                    Assert.IsTrue(eqIc, "Ordinal-equal must be ignore-case-equal. " + ctx);
                Assert.AreEqual(left?.ToUpperInvariant() == right?.ToUpperInvariant(), eqIc, "IsEqualsIgnoreCase " + ctx);
                Assert.AreEqual(eq, eqInv, "IsEqualsInvariantCulture on ASCII alphanumerics must match ordinal. " + ctx);
            }
        }

        [DataTestMethod]
        [DataRow("a", "a", "aa", "aa")]
        [DataRow("abab", "ab", "aab", "aabaab")]
        [DataRow("AB", "ab", "aab", "aab")]
        [DataRow("xax", "A", "aAa", "xaAax")]
        [DataRow("aaa", "aa", "aaa", "aaaa")]
        [DataRow("a", "A", "Aa", "Aa")]
        [DataRow("aBc", "b", "B", "aBc")]
        public void ReplaceIgnoreCase_NewValueContainsOldValue_SinglePassNonRecursive(
            string source, string oldValue, string newValue, string expected)
        {
            var result = source.ReplaceIgnoreCase(oldValue, newValue);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [Timeout(10000)]
        public void ReplaceIgnoreCase_OneMillionCharsWithHalfMillionMatchesGrowingReplacement_CompletesWithExactLength()
        {
            var sb = new StringBuilder(1000000);
            for (var i = 0; i < 500000; i++)
                sb.Append("ab");
            var source = sb.ToString();

            var result = source.ReplaceIgnoreCase("A", "aa");

            Assert.AreEqual(1500000, result.Length);
            Assert.IsTrue(result.StartsWith("aabaab", StringComparison.Ordinal));
            Assert.IsTrue(result.EndsWith("aabaab", StringComparison.Ordinal));
        }

        [TestMethod]
        [Timeout(10000)]
        public void ReplaceIgnoreCase_OneMillionMatchingChars_RemovalAndNoMatchBehaveCorrectly()
        {
            var source = new string('a', 1000000);

            var removed = source.ReplaceIgnoreCase("A", string.Empty);
            var doubled = source.ReplaceIgnoreCase("a", "aa");
            var untouched = source.ReplaceIgnoreCase("b", "c");

            Assert.AreEqual(string.Empty, removed);
            Assert.AreEqual(2000000, doubled.Length);
            Assert.AreSame(source, untouched);
        }

        [DataTestMethod]
        [DataRow("abc", "x", "y")]
        [DataRow("abc", "abcd", "z")]
        [DataRow("ab", "abc", (string)null)]
        [DataRow("", "a", "b")]
        [DataRow("   ", "x", "y")]
        [DataRow("abc", "", "z")]
        [DataRow("abc", (string)null, "z")]
        [DataRow("abc", (string)null, (string)null)]
        [DataRow("", (string)null, "z")]
        public void ReplaceIgnoreCase_NothingToReplace_ReturnsSameReference(string source, string oldValue, string newValue)
        {
            var result = source.ReplaceIgnoreCase(oldValue, newValue);

            Assert.AreSame(source, result);
        }

        [TestMethod]
        public void EnsureStartsWith_AlreadyPrefixedOrMissing_ReturnsSameReference()
        {
            var cases = new[]
            {
                new[] { "/users", "/" },
                new[] { "/", "/" },
                new[] { "HTTPS://x", "https://" },
                new[] { " x", " " },
                new[] { "\tx", "\t" },
                new[] { "\r\nx", "\r\n" },
                new[] { Emoji + "x", Emoji },
                new[] { " ", "/" },
                new[] { "", "/" },
                new[] { "\t\n", "x" }
            };

            foreach (var c in cases)
                Assert.AreSame(c[0], c[0].EnsureStartsWith(c[1]), "source=" + Show(c[0]) + " prefix=" + Show(c[1]));
        }

        [TestMethod]
        public void EnsureEndsWith_AlreadySuffixedOrMissing_ReturnsSameReference()
        {
            var cases = new[]
            {
                new[] { "https://api/", "/" },
                new[] { "/", "/" },
                new[] { "file.JSON", ".json" },
                new[] { "x ", " " },
                new[] { "x\t", "\t" },
                new[] { "x\r\n", "\r\n" },
                new[] { "x\r\n", "\n" },
                new[] { "x" + Emoji, Emoji },
                new[] { " ", "/" },
                new[] { "", "/" }
            };

            foreach (var c in cases)
                Assert.AreSame(c[0], c[0].EnsureEndsWith(c[1]), "source=" + Show(c[0]) + " suffix=" + Show(c[1]));
        }

        [DataTestMethod]
        [DataRow("a b c", " ", "a", "b c")]
        [DataRow("a\tb", "\t", "a", "b")]
        [DataRow("a\r\nb\r\nc", "\r\n", "a", "b\r\nc")]
        [DataRow("a\r\nb", "\n", "a\r", "b")]
        [DataRow("\t", "\t", "", "")]
        [DataRow(" ", " ", "", "")]
        [DataRow("", " ", "", "")]
        [DataRow("  ", "x", "  ", "")]
        [DataRow(" \t ", "\t", " ", " ")]
        public void SubstringBeforeAfter_WhiteSpaceSeparatorOrSource_ProcessedNormally(
            string source, string separator, string expectedBefore, string expectedAfter)
        {
            var before = source.SubstringBefore(separator);
            var after = source.SubstringAfter(separator);

            Assert.AreEqual(expectedBefore, before, "SubstringBefore");
            Assert.AreEqual(expectedAfter, after, "SubstringAfter");
        }

        [DataTestMethod]
        [DataRow("a\r\nb", "\r\n", "\n", "a\nb")]
        [DataRow("\t\t", "\t", "x", "xx")]
        [DataRow("a  b", " ", "", "ab")]
        [DataRow(" ", " ", (string)null, "")]
        [DataRow("\r\n\r\n", "\r\n", " ", "  ")]
        public void ReplaceIgnoreCase_WhiteSpaceSourceOrOldValue_ProcessedNormally(
            string source, string oldValue, string newValue, string expected)
        {
            var result = source.ReplaceIgnoreCase(oldValue, newValue);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void EnsureAffix_WhiteSpaceAffixesOnPresentSource_AreAppliedVerbatim()
        {
            Assert.AreEqual("\tx", "x".EnsureStartsWith("\t"));
            Assert.AreEqual("\r\nx", "x".EnsureStartsWith("\r\n"));
            Assert.AreEqual("\n\r\nx", "\r\nx".EnsureStartsWith("\n"));
            Assert.AreEqual("x\t", "x".EnsureEndsWith("\t"));
            Assert.AreEqual("x\r\r\n", "x\r".EnsureEndsWith("\r\n"));
            Assert.AreEqual("a /", "a ".EnsureEndsWith("/"));
            Assert.AreEqual("/ a", " a".EnsureStartsWith("/"));
        }

        [TestMethod]
        public void SplitAndTrim_WhiteSpaceCharAsExplicitSeparator_SplitsOnItAndDropsEmptyRuns()
        {
            AssertSequence(new[] { "a", "b", "c" }, "a b   c ".SplitAndTrim(' '), "space separator");
            AssertSequence(new[] { "a b", "c" }, "a b\t\tc".SplitAndTrim('\t'), "tab separator");
            AssertSequence(new[] { "a", "b" }, "a\r\nb\r\n".SplitAndTrim('\n'), "LF separator, CR trimmed");
        }

        [DataTestMethod]
        [DataRow(Nbsp, (string)null)]
        [DataRow(Nbsp + "x" + Nbsp, "x")]
        [DataRow(Nbsp + EmSpace + "x" + IdeographicSpace, "x")]
        [DataRow(LineSeparator + "x" + ParagraphSeparator, "x")]
        public void TrimToNull_UnicodeWhiteSpace_IsTrimmed(string source, string expected)
        {
            Assert.AreEqual(expected, source.TrimToNull());
        }

        [DataTestMethod]
        [DataRow(Zwsp)]
        [DataRow(Zwsp + "x" + Zwsp)]
        [DataRow(SoftHyphen)]
        public void TrimToNull_ZeroWidthAndFormatChars_AreNotWhiteSpaceAndAreKept(string source)
        {
            Assert.AreEqual(source, source.TrimToNull());
        }

        [TestMethod]
        public void TrimToNull_ZwspInsideWhiteSpace_KeepsZwspCore()
        {
            Assert.AreEqual(Zwsp, (" " + Zwsp + Nbsp).TrimToNull());
        }

        [TestMethod]
        public void SplitAndTrim_NbspTrimmedZwspKept()
        {
            AssertSequence(new[] { "a", "b" }, (Nbsp + "a" + Nbsp + "," + Nbsp + "b" + Nbsp).SplitAndTrim(), "NBSP");
            AssertSequence(new[] { Zwsp, "a" }, (Zwsp + ", a").SplitAndTrim(), "ZWSP");
            AssertSequence(new string[0], (Nbsp + "," + EmSpace + ", " + IdeographicSpace).SplitAndTrim(),
                "only unicode white-space");
        }

        [TestMethod]
        public void IsEqualsFamily_InvisibleOrLookAlikeCharacters_AreNotOrdinalEqual()
        {
            Assert.IsFalse(("ad" + SoftHyphen + "min").IsEquals("admin"), "soft hyphen IsEquals");
            Assert.IsFalse(("ad" + SoftHyphen + "min").IsEqualsIgnoreCase("ADMIN"), "soft hyphen IsEqualsIgnoreCase");
            Assert.IsFalse(("a" + Zwsp + "b").IsEquals("ab"), "ZWSP IsEquals");
            Assert.IsFalse(("a" + Zwsp + "b").IsEqualsIgnoreCase("AB"), "ZWSP IsEqualsIgnoreCase");
            Assert.IsFalse(("a" + Nbsp + "b").IsEquals("a b"), "NBSP vs space");
            Assert.IsFalse(EAcutePrecomposed.IsEquals("e" + CombiningAcute), "precomposed vs decomposed IsEquals");
            Assert.IsFalse(EAcutePrecomposed.IsEqualsIgnoreCase("E" + CombiningAcute),
                "precomposed vs decomposed IsEqualsIgnoreCase");
            Assert.IsFalse("a\0".IsEquals("a"), "embedded NUL");
        }

        [TestMethod]
        public void SurrogatePairs_AsSeparatorOldValueOrAffix_AreHandledAsWholeUnits()
        {
            Assert.AreEqual(2, Emoji.Length, "Emoji constant must be a surrogate pair.");
            Assert.AreEqual("x!y!", ("x" + Emoji + "y" + Emoji).ReplaceIgnoreCase(Emoji, "!"));
            Assert.AreEqual("a" + Emoji + "b", "a-b".ReplaceIgnoreCase("-", Emoji));
            Assert.AreEqual("k", ("k" + Emoji + "v").SubstringBefore(Emoji));
            Assert.AreEqual("v", ("k" + Emoji + "v").SubstringAfter(Emoji));
            Assert.AreEqual("v" + Emoji, ("k" + Emoji + "v" + Emoji).SubstringAfter(Emoji, true));
            Assert.AreEqual(Emoji + "x", "x".EnsureStartsWith(Emoji));
            Assert.AreEqual("x" + Emoji, "x".EnsureEndsWith(Emoji));
            AssertSequence(new[] { Emoji, "a" + Emoji }, (Emoji + " , a" + Emoji + " ,").SplitAndTrim(), "emoji entries");
        }

        [TestMethod]
        public void ReplaceIgnoreCase_LoneSurrogateOldValue_MatchesBclOracleAndDoesNotThrow()
        {
            var source = "a" + Emoji + "b";

            var actual = source.ReplaceIgnoreCase(LoneHighSurrogate, "");

            Assert.AreEqual(source.Replace(LoneHighSurrogate, "", StringComparison.OrdinalIgnoreCase), actual);
        }

        [TestMethod]
        public void RuntimeDependentInputs_DoNotThrow()
        {
            var samples = new[]
            {
                "i", "I", CapitalIWithDot, DotlessI, MongolianVowelSeparator,
                "a" + Zwsp + "b", "ab", "ad" + SoftHyphen + "min", "admin"
            };

            foreach (var a in samples)
            foreach (var b in samples)
            {
                a.IsEqualsInvariantCulture(b);
                a.IsEqualsIgnoreCase(b);
                Assert.IsNotNull(a.ReplaceIgnoreCase(b, "x"));
                Assert.IsNotNull(a.SubstringBefore(b, true));
                Assert.IsNotNull(a.SubstringAfter(b, true));
                Assert.IsNotNull(a.EnsureStartsWith(b));
                Assert.IsNotNull(a.EnsureEndsWith(b));
            }

            Assert.IsNotNull(MongolianVowelSeparator.SplitAndTrim());
            MongolianVowelSeparator.TrimToNull();
        }

        [DataTestMethod]
        [DataRow("@abc", "@", "", "abc")]
        [DataRow("abc@", "@", "abc", "")]
        [DataRow("abc", "abc", "", "")]
        [DataRow("ab", "abc", "ab", "")]
        [DataRow("aaa", "aa", "", "a")]
        [DataRow("a==b", "==", "a", "b")]
        [DataRow("a=b==c", "==", "a=b", "c")]
        [DataRow("====", "==", "", "==")]
        public void SubstringBeforeAfter_SeparatorAtStartEndWholeOrLonger_ReturnsDocumentedParts(
            string source, string separator, string expectedBefore, string expectedAfter)
        {
            var before = source.SubstringBefore(separator);
            var after = source.SubstringAfter(separator);

            Assert.AreEqual(expectedBefore, before, "SubstringBefore");
            Assert.AreEqual(expectedAfter, after, "SubstringAfter");
        }

        [DataTestMethod]
        [DataRow("@abc@", "@", "#", "#abc#")]
        [DataRow("abc", "ABC", "x", "x")]
        [DataRow("AAAA", "aa", "b", "bb")]
        [DataRow("aAaAa", "AA", "-", "--a")]
        [DataRow("xa", "A", "", "x")]
        [DataRow("ax", "A", "", "x")]
        public void ReplaceIgnoreCase_MatchAtStartEndWholeOrAdjacent_ReplacesLeftToRightNonOverlapping(
            string source, string oldValue, string newValue, string expected)
        {
            var result = source.ReplaceIgnoreCase(oldValue, newValue);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void EnsureAffix_AffixLongerThanOrEqualToSource_BehavesAsDocumented()
        {
            Assert.AreEqual("abcab", "ab".EnsureStartsWith("abc"));
            Assert.AreEqual("ababc", "ab".EnsureEndsWith("abc"));
            Assert.AreEqual("ABC", "ABC".EnsureStartsWith("abc"));
            Assert.AreEqual("abcABC", "ABC".EnsureStartsWith("abc", StringComparison.Ordinal));
            Assert.AreEqual("ABCabc", "ABC".EnsureEndsWith("abc", StringComparison.Ordinal));
        }

        [TestMethod]
        public void EnsureStartsWith_ProtocolRelativeUrl_IsReturnedUnchanged_NotAnOpenRedirectGuard()
        {
            const string source = "//evil.com";

            var result = source.EnsureStartsWith("/");

            Assert.AreSame(source, result);
        }

        [TestMethod]
        public void EnsureStartsWith_InvalidComparisonOnPresentSource_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => "a".EnsureStartsWith("/", (StringComparison)99));
        }

        [TestMethod]
        public void EnsureEndsWith_InvalidComparisonOnPresentSource_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => "a".EnsureEndsWith("/", (StringComparison)99));
        }

        [TestMethod]
        public void EnsureAffix_InvalidComparisonWithMissingSourceOrNullAffix_PassesThroughWithoutThrowing()
        {
            var empty = string.Empty;
            var s = "a";

            Assert.AreSame(empty, empty.EnsureStartsWith("/", (StringComparison)99));
            Assert.AreSame(s, s.EnsureStartsWith(null, (StringComparison)99));
            Assert.AreSame(empty, empty.EnsureEndsWith("/", (StringComparison)99));
            Assert.AreSame(s, s.EnsureEndsWith(null, (StringComparison)99));
        }

        [TestMethod]
        public void EnsureStartsWith_DefaultComparisonIgnoresAsciiCase_ExplicitOrdinalDoesNot()
        {
            const string source = "Api/x";

            Assert.AreSame(source, source.EnsureStartsWith("api/"));
            Assert.AreSame(source, source.EnsureStartsWith("api/", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("api/Api/x", source.EnsureStartsWith("api/", StringComparison.Ordinal));
        }

        [TestMethod]
        public void EnsureEndsWith_DefaultComparisonIgnoresAsciiCase_ExplicitOrdinalDoesNot()
        {
            const string source = "a/B";

            Assert.AreSame(source, source.EnsureEndsWith("/b"));
            Assert.AreSame(source, source.EnsureEndsWith("/b", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("a/B/b", source.EnsureEndsWith("/b", StringComparison.Ordinal));
        }

        [TestMethod]
        public void SubstringBeforeAfter_DefaultIsCaseSensitive_IgnoreCaseTrueMatchesOtherCase()
        {
            Assert.AreEqual("aXb", "aXb".SubstringBefore("x"));
            Assert.AreEqual("", "aXb".SubstringAfter("x"));
            Assert.AreEqual("a", "aXb".SubstringBefore("x", true));
            Assert.AreEqual("b", "aXb".SubstringAfter("x", true));
            Assert.AreEqual("1", "1X2x3".SubstringBefore("x", true));
            Assert.AreEqual("1X2", "1X2x3".SubstringBefore("x"));
        }
    }
}
