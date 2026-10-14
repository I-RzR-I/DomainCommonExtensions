using System;
using System.Collections;
using System.Collections.Generic;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Text;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class StringInjectExtensionTests
    {
        [TestMethod]
        public void Inject_SwappedPlaceholders_Test()
        {
            var values = new Hashtable { { "a", "{b}" }, { "b", "{a}" } };

            var result = "{a}{b}".Inject(values);

            Assert.AreEqual("{b}{a}", result);
        }

        [TestMethod]
        public void Inject_ValueContainingOtherPlaceholder_Test()
        {
            var values = new Hashtable { { "a", "{b}" }, { "b", "x" } };

            var result = "{a}".Inject(values);

            Assert.AreEqual("{b}", result);
        }

        [TestMethod]
        public void Inject_ValueContainingOtherPlaceholderFromObject_Test()
        {
            var result = "{a}".Inject(new { a = "{b}", b = "x" });

            Assert.AreEqual("{b}", result);
        }

        [TestMethod]
        public void Inject_ValueContainingOtherPlaceholderFromDictionary_Test()
        {
            var values = new Dictionary<string, object> { { "b", "x" }, { "a", "{b}" } };

            var result = "{a}".Inject((IDictionary)values);

            Assert.AreEqual("{b}", result);
        }

        [TestMethod]
        public void Inject_IntKeyHashtable_Test()
        {
            var values = new Hashtable { { 1, "one" } };

            var result = "{1}".Inject(values);

            Assert.AreEqual("one", result);
        }

        [TestMethod]
        public void Inject_IntKeyDictionary_Test()
        {
            var values = new Dictionary<int, string> { { 1, "one" } };

            var result = "[{1}]".Inject((IDictionary)values);

            Assert.AreEqual("[one]", result);
        }

        [TestMethod]
        public void Inject_MixedKeyTypes_Test()
        {
            var values = new Hashtable { { 1, "one" }, { "b", "two" } };

            var result = "{1}-{b}".Inject(values);

            Assert.AreEqual("one-two", result);
        }

        [TestMethod]
        public void Inject_WhitespaceValue_Test()
        {
            var values = new Hashtable { { "a", " " } };

            var result = "[{a}]".Inject(values);

            Assert.AreEqual("[ ]", result);
        }

        [TestMethod]
        public void InjectSingleValue_WhitespaceValue_Test()
        {
            var result = "[{a}]".InjectSingleValue("a", " ");

            Assert.AreEqual("[ ]", result);
        }

        [TestMethod]
        public void Inject_KeyWithDotMatchesOnlyLiteral_Test()
        {
            var values = new Hashtable { { "a.b", "V" } };

            var result = "{axb} {a.b}".Inject(values);

            Assert.AreEqual("{axb} V", result);
        }

        [TestMethod]
        public void Inject_KeyWithUnbalancedParenthesis_Test()
        {
            var values = new Hashtable { { "x(", "V" } };

            var result = "[{x(}]".Inject(values);

            Assert.AreEqual("[V]", result);
        }

        [TestMethod]
        public void InjectSingleValue_KeyWithDotMatchesOnlyLiteral_Test()
        {
            var result = "{axb} {a.b}".InjectSingleValue("a.b", "V");

            Assert.AreEqual("{axb} V", result);
        }

        [TestMethod]
        public void Inject_LiteralKeyWithFormatSpecifier_Test()
        {
            var values = new Hashtable { { "a.b", 5 } };

            var result = "{a.b:D2}".Inject(values);

            Assert.AreEqual("05", result);
        }

        [TestMethod]
        public void Inject_AnonymousObject_Test()
        {
            var result = "Hello {name}, you are {age}".Inject(new { name = "Ann", age = 30 });

            Assert.AreEqual("Hello Ann, you are 30", result);
        }

        [TestMethod]
        public void Inject_RepeatedPlaceholder_Test()
        {
            var values = new Hashtable { { "a", "1" } };

            var result = "{a}-{a}".Inject(values);

            Assert.AreEqual("1-1", result);
        }

        [TestMethod]
        public void Inject_MissingKeyLeftUntouched_Test()
        {
            var values = new Hashtable { { "a", "A" } };

            var result = "{a} {missing}".Inject(values);

            Assert.AreEqual("A {missing}", result);
        }

        [TestMethod]
        public void Inject_CaseDifferentKeyLeftUntouched_Test()
        {
            var values = new Hashtable { { "a", "A" } };

            var result = "{A}".Inject(values);

            Assert.AreEqual("{A}", result);
        }

        [TestMethod]
        public void Inject_NullValue_Test()
        {
            var values = new Hashtable { { "a", null } };

            var result = "[{a}]".Inject(values);

            Assert.AreEqual("[]", result);
        }

        [TestMethod]
        public void Inject_ValueWithDollarSign_Test()
        {
            var values = new Hashtable { { "a", "$1$0" } };

            var result = "[{a}]".Inject(values);

            Assert.AreEqual("[$1$0]", result);
        }

        [TestMethod]
        public void Inject_FormatSpecifier_Test()
        {
            var values = new Hashtable { { "n", 7 } };

            var result = "{n:D3}".Inject(values);

            Assert.AreEqual("007", result);
        }

        [TestMethod]
        public void Inject_HexFormatSpecifier_Test()
        {
            var values = new Hashtable { { "n", 255 } };

            var result = "0x{n:X}".Inject(values);

            Assert.AreEqual("0xFF", result);
        }

        [TestMethod]
        public void Inject_PlainAndFormattedSamePlaceholder_Test()
        {
            var values = new Hashtable { { "n", 7 } };

            var result = "{n} and {n:D3}".Inject(values);

            Assert.AreEqual("7 and 007", result);
        }

        [TestMethod]
        public void Inject_NullTemplate_Test()
        {
            var result = ((string)null).Inject(new Hashtable { { "a", "A" } });

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Inject_NullHashtable_Test()
        {
            var result = "{a}".Inject((Hashtable)null);

            Assert.AreEqual("{a}", result);
        }

        [TestMethod]
        public void Inject_NullDictionary_Test()
        {
            var result = "{a}".Inject((IDictionary)null);

            Assert.AreEqual("{a}", result);
        }

        [TestMethod]
        public void Inject_NullObject_Test()
        {
            var result = "{a}".Inject((object)null);

            Assert.AreEqual("{a}", result);
        }

        [TestMethod]
        public void Inject_WhitespaceTemplate_Test()
        {
            var result = "  ".Inject(new Hashtable { { "a", "A" } });

            Assert.AreEqual("  ", result);
        }

        [TestMethod]
        public void Inject_EmptyTemplate_Test()
        {
            var result = string.Empty.Inject(new Hashtable { { "a", "A" } });

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void Inject_KeyContainingColon_Test()
        {
            var values = new Hashtable { { "a:b", "V" } };

            var result = "[{a:b}]".Inject(values);

            Assert.AreEqual("[V]", result);
        }

        [TestMethod]
        public void Inject_MultiColonDateFormat_Test()
        {
            using var culture = new CultureScopeTestHelper("");
            var values = new Hashtable { { "d", new DateTime(2024, 1, 2, 13, 45, 0) } };

            var result = "{d:HH:mm}".Inject(values);

            Assert.AreEqual("13:45", result);
        }

        [TestMethod]
        public void Inject_DoubleBracedPlaceholder_Test()
        {
            var values = new Hashtable { { "a", "v" } };

            var result = "{{a}}".Inject(values);

            Assert.AreEqual("{v}", result);
        }

        [TestMethod]
        public void Inject_LeadingColonKeyWithFormat_Test()
        {
            using var culture = new CultureScopeTestHelper("");
            var values = new Hashtable { { ":x", 5 } };

            var result = "{:x:N2}".Inject(values);

            Assert.AreEqual("5.00", result);
        }

        [TestMethod]
        public void Inject_DuplicateKeyText_Test()
        {
            var values = new Dictionary<object, object> { { 1, "a" }, { "1", "b" } };

            var result = "{1}".Inject((IDictionary)values);

            Assert.AreEqual("a", result);
        }
    }
}
