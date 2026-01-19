// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-01-18 22:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-18 22:17
// ***********************************************************************
//  <copyright file="CharTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DomainCommonExtensions.DataTypeExtensions;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class CharTests
    {
        [DataTestMethod]
        [DataRow(' ', true)]
        [DataRow('\t', true)]
        [DataRow('\n', true)]
        [DataRow('a', false)]
        public void IsMissing_Works(char input, bool expected)
            => Assert.AreEqual(expected, input.IsMissing());

        [DataTestMethod]
        [DataRow(' ', false)]
        [DataRow('\n', false)]
        [DataRow('x', true)]
        public void IsPresent_Works(char input, bool expected)
            => Assert.AreEqual(expected, input.IsPresent());

        [DataTestMethod]
        [DataRow('0', true)]
        [DataRow('9', true)]
        [DataRow('a', false)]
        public void IsDigit_Works(char input, bool expected)
            => Assert.AreEqual(expected, input.IsDigit());

        [DataTestMethod]
        [DataRow('0', true)]
        [DataRow('1', true)]
        [DataRow('2', false)]
        public void IsBinaryDigit_Works(char input, bool expected)
            => Assert.AreEqual(expected, input.IsBinaryDigit());

        [DataTestMethod]
        [DataRow('7', true)]
        [DataRow('8', false)]
        public void IsOctalDigit_Works(char input, bool expected)
            => Assert.AreEqual(expected, input.IsOctalDigit());

        [DataTestMethod]
        [DataRow('0', true)]
        [DataRow('9', true)]
        [DataRow('A', true)]
        [DataRow('f', true)]
        [DataRow('g', false)]
        public void IsHexDigit_Works(char input, bool expected)
            => Assert.AreEqual(expected, input.IsHexDigit());

        [DataTestMethod]
        [DataRow('A', true)]
        [DataRow('z', true)]
        [DataRow('é', false)]
        public void IsAscii_Works(char input, bool expected)
            => Assert.AreEqual(expected, input.IsAscii());

        [DataTestMethod]
        [DataRow('A', true)]
        [DataRow('z', true)]
        [DataRow('1', false)]
        public void IsAsciiLetter_Works(char input, bool expected)
            => Assert.AreEqual(expected, input.IsAsciiLetter());

        [DataTestMethod]
        [DataRow('A', true)]
        [DataRow('Z', true)]
        [DataRow('a', false)]
        public void IsAsciiUpper_Works(char input, bool expected)
            => Assert.AreEqual(expected, input.IsAsciiUpper());

        [DataTestMethod]
        [DataRow('a', true)]
        [DataRow('z', true)]
        [DataRow('A', false)]
        public void IsAsciiLower_Works(char input, bool expected)
            => Assert.AreEqual(expected, input.IsAsciiLower());

        [DataTestMethod]
        [DataRow('\n', true)]
        [DataRow('\r', true)]
        [DataRow('a', false)]
        public void IsNewLine_Works(char input, bool expected)
            => Assert.AreEqual(expected, input.IsNewLine());

        [DataTestMethod]
        [DataRow('"', true)]
        [DataRow('\'', true)]
        [DataRow('a', false)]
        public void IsQuote_Works(char input, bool expected)
            => Assert.AreEqual(expected, input.IsQuote());

        [DataTestMethod]
        [DataRow('(', true)]
        [DataRow(']', true)]
        [DataRow('a', false)]
        public void IsBracket_Works(char input, bool expected)
            => Assert.AreEqual(expected, input.IsBracket());

        [DataTestMethod]
        [DataRow('+', true)]
        [DataRow('^', true)]
        [DataRow('a', false)]
        public void IsOperator_Works(char input, bool expected)
            => Assert.AreEqual(expected, input.IsOperator());

        [DataTestMethod]
        [DataRow('+', true)]
        [DataRow('=', true)]
        [DataRow('&', false)]
        public void IsMathSign_Works(char input, bool expected)
            => Assert.AreEqual(expected, input.IsMathSign());

        [DataTestMethod]
        [DataRow(' ', true)]
        [DataRow('\t', true)]
        [DataRow('\n', true)]
        [DataRow('\u00A0', false)] // non-breaking space
        public void IsWhiteSpaceFast_Works(char input, bool expected)
            => Assert.AreEqual(expected, input.IsWhiteSpaceFast());

        [TestMethod]
        public void IsSymbolOrPunctuation_Works()
        {
            Assert.IsTrue('$'.IsSymbolOrPunctuation());
            Assert.IsTrue('.'.IsSymbolOrPunctuation());
            Assert.IsFalse('A'.IsSymbolOrPunctuation());
        }

        [DataTestMethod]
        [DataRow('0', 0)]
        [DataRow('9', 9)]
        [DataRow('A', 10)]
        [DataRow('f', 15)]
        public void ToHexValue_Works(char input, int expected)
            => Assert.AreEqual(expected, input.ToHexValue());

        [TestMethod]
        public void ToHexValue_Throws_On_Invalid()
            => Assert.ThrowsException<InvalidOperationException>(() => 'G'.ToHexValue());

        [DataTestMethod]
        [DataRow('a', 'A')]
        [DataRow('A', 'a')]
        [DataRow('1', '1')]
        public void ToggleCase_Works(char input, char expected)
            => Assert.AreEqual(expected, input.ToggleCase());

        [DataTestMethod]
        [DataRow('A', true)]
        [DataRow('\n', false)]
        public void IsPrintable_Works(char input, bool expected)
            => Assert.AreEqual(expected, input.IsPrintable());

        [TestMethod]
        public void Next_Works()
            => Assert.AreEqual('b', 'a'.Next());

        [TestMethod]
        public void Previous_Works()
            => Assert.AreEqual('a', 'b'.Previous());

        [TestMethod]
        public void IsEmoji_Works()
        {
            Assert.IsTrue('❤'.IsEmoji());
            Assert.IsFalse('A'.IsEmoji());
        }

        [TestMethod]
        public void HasValue_Works()
        {
            char? a = 'x';
            char? b = null;

            Assert.IsTrue(a.HasValue());
            Assert.IsFalse(b.HasValue());
        }

        [TestMethod]
        public void DefaultIfNull_Works()
        {
            char? a = null;
            Assert.AreEqual('x', a.DefaultIfNull('x'));
        }

        [TestMethod]
        public void IsNullOrWhiteSpace_Works()
        {
            char? a = null;
            char? b = ' ';
            char? c = 'x';

            Assert.IsTrue(a.IsNullOrWhiteSpace());
            Assert.IsTrue(b.IsNullOrWhiteSpace());
            Assert.IsFalse(c.IsNullOrWhiteSpace());
        }

        [TestMethod]
        public void SpaceIfNull_Works()
        {
            char? a = null;
            Assert.AreEqual(' ', a.SpaceIfNull());
        }
    }
}