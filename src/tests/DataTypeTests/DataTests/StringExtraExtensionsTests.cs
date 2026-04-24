// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-04-24 14:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-24 14:35
// ***********************************************************************
//  <copyright file="StringExtraExtensionsTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Globalization;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.DataTypeExtensions;

#endregion

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class StringExtraExtensionsTests
    {
        [TestMethod]
        public void ToBase64Url_KnownVector_FooBar()
        {
            Assert.AreEqual("Zm9vYmFy", "foobar".ToBase64Url());
        }

        [TestMethod]
        public void ToBase64Url_StripsPadding()
        {
            Assert.AreEqual("Zg", "f".ToBase64Url());
        }

        [TestMethod]
        public void ToBase64Url_ReplacesUrlUnsafeChars()
        {
            // Known input that produces '+' and '/' in standard Base64.
            // bytes {0xfb, 0xff, 0xbf} => standard "+/+/" => Base64Url "-_-_"
            var bytes = new byte[] { 0xfb, 0xff, 0xbf };
            var standard = Convert.ToBase64String(bytes);
            Assert.IsTrue(standard.Contains("+") || standard.Contains("/"),
                "Sanity: expected standard Base64 to contain +/.");

            var s = Encoding.UTF8.GetString(bytes);
            var url = s.ToBase64Url();

            Assert.IsFalse(url.Contains("+"));
            Assert.IsFalse(url.Contains("/"));
            Assert.IsFalse(url.Contains("="));
        }

        [TestMethod]
        public void ToBase64Url_NullInput_ReturnsEmpty()
        {
            string s = null;
            Assert.AreEqual(string.Empty, s.ToBase64Url());
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("a")]
        [DataRow("hello world")]
        [DataRow("☺ unicode 漢字")]
        public void ToBase64Url_RoundTrip(string input)
        {
            Assert.AreEqual(input, input.ToBase64Url().FromBase64Url());
        }

        [TestMethod]
        public void FromBase64Url_NullThrows()
        {
            string s = null;
            Assert.ThrowsException<ArgumentNullException>(() => s.FromBase64Url());
        }

        [TestMethod]
        public void ToSlug_BasicWords()
        {
            Assert.AreEqual("hello-world", "Hello World".ToSlug());
        }

        [TestMethod]
        public void ToSlug_CollapsesAndTrimsSeparators()
        {
            Assert.AreEqual("a-b-c", "  --  a!!!b___c--  ".ToSlug());
        }

        [TestMethod]
        public void ToSlug_KeepsDigits()
        {
            Assert.AreEqual("net-8-rocks", "  .NET 8 rocks!".ToSlug());
        }

        [TestMethod]
        public void ToSlug_DropsNonAsciiCharacters()
        {
            Assert.AreEqual("hello", "hello 漢字".ToSlug());
        }

        [TestMethod]
        public void ToSlug_NullOrEmpty_ReturnsEmpty()
        {
            Assert.AreEqual(string.Empty, ((string)null).ToSlug());
            Assert.AreEqual(string.Empty, string.Empty.ToSlug());
        }

        [TestMethod]
        public void ToSlug_RespectsMaxLength_AtWordBoundary()
        {
            var slug = "the quick brown fox jumps".ToSlug(maxLength: 15);

            Assert.IsTrue(slug.Length <= 15);
            Assert.IsFalse(slug.EndsWith("-"));
            // First whole-word truncation under 15 chars: "the-quick-brown"
            Assert.AreEqual("the-quick-brown", slug);
        }

        [TestMethod]
        public void ToSlug_TurkishCulture_DoesNotProduceDottedI()
        {
            var slug = "TITLE".ToSlug(new CultureInfo("tr-TR"));

            foreach (var ch in slug)
                Assert.IsTrue((ch >= 'a' && ch <= 'z') || (ch >= '0' && ch <= '9') || ch == '-',
                    $"Unexpected char '{ch}' in slug '{slug}'.");
        }

        [TestMethod]
        public void Mask_CreditCardLastFour()
        {
            // 16-digit card => "****-****-****-####" keeps the last 4.
            var masked = "1234567890123456".Mask("****-****-****-####");
            Assert.AreEqual("****-****-****-3456", masked);
        }

        [TestMethod]
        public void Mask_PhonePattern()
        {
            var masked = "5551234567".Mask("(###) ###-####");
            Assert.AreEqual("(555) 123-4567", masked);
        }

        [TestMethod]
        public void Mask_MaskFirstFourKeepRest()
        {
            var masked = "1234567890".Mask("****-######");
            Assert.AreEqual("****-567890", masked);
        }

        [TestMethod]
        public void Mask_CustomMaskChar()
        {
            var masked = "1234".Mask("##**", 'X');
            Assert.AreEqual("12XX", masked);
        }

        [TestMethod]
        public void Mask_InputShorterThanPattern_DropsMissingPlaceholders()
        {
            // Pattern requests 6 input chars, only 3 supplied -> remainder placeholders dropped.
            var masked = "ABC".Mask("##-####");
            Assert.AreEqual("AB-C", masked);
        }

        [TestMethod]
        public void Mask_NullInput_ReturnsLiteralPattern()
        {
            string s = null;
            // No '#' or '*' means literals are emitted regardless of input.
            Assert.AreEqual("---", s.Mask("---"));
        }

        [TestMethod]
        public void Mask_NullPattern_Throws()
        {
            Assert.ThrowsException<ArgumentNullException>(() => "abc".Mask(null));
        }

        [TestMethod]
        public void Mask_EmptyPattern_Throws()
        {
            Assert.ThrowsException<ArgumentException>(() => "abc".Mask(string.Empty));
        }
    }
}