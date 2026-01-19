// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-01-18 23:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-01-18 23:18
// ***********************************************************************
//  <copyright file="RandomHelperTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Linq;
using DomainCommonExtensions.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace DataTypeTests.HelperTests
{
    [TestClass]
    public class RandomHelperTests
    {
        private RandomHelper CreateDeterministic()
        {
            return new RandomHelper(new Random(12345));
        }

        [TestMethod]
        public void Number_ShouldBeWithinRange()
        {
            var rnd = CreateDeterministic();

            var value = rnd.Number(5, 10);

            Assert.IsTrue(value >= 5 && value <= 10);
        }

        [TestMethod]
        public void Number_DefaultRange_ShouldReturnZeroOrOne()
        {
            var rnd = CreateDeterministic();

            var value = rnd.Number();

            Assert.IsTrue(value == 0 || value == 1);
        }

        [TestMethod]
        public void Long_ShouldBeWithinRange()
        {
            var rnd = CreateDeterministic();

            var value = rnd.Long(100, 200);

            Assert.IsTrue(value >= 100 && value <= 200);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Long_MinGreaterThanMax_ShouldThrow()
        {
            var rnd = CreateDeterministic();
            rnd.Long(10, 5);
        }

        [TestMethod]
        public void Double_ShouldBeBetweenZeroAndOne()
        {
            var rnd = CreateDeterministic();

            var value = rnd.Double();

            Assert.IsTrue(value >= 0.0 && value < 1.0);
        }

        [TestMethod]
        public void Decimal_ShouldBeBetweenZeroAndOne()
        {
            var rnd = CreateDeterministic();

            var value = rnd.Decimal();

            Assert.IsTrue(value >= 0m && value < 1m);
        }

        [TestMethod]
        public void Bool_ShouldReturnTrueOrFalse()
        {
            var rnd = CreateDeterministic();

            var value = rnd.Bool();

            Assert.IsTrue(value || value == false);
        }

        [TestMethod]
        public void Letter_ShouldBeSingleAlphabetCharacter()
        {
            var rnd = CreateDeterministic();

            var letter = rnd.Letter();

            Assert.AreEqual(1, letter.Length);
            Assert.IsTrue(char.IsLetter(letter[0]));
        }

        [TestMethod]
        public void Letters_ShouldReturnCorrectLength()
        {
            var rnd = CreateDeterministic();

            var letters = rnd.Letters(10);

            Assert.AreEqual(10, letters.Length);
            Assert.IsTrue(letters.All(char.IsLetter));
        }

        [TestMethod]
        public void LowerLetter_ShouldBeLowercase()
        {
            var rnd = CreateDeterministic();

            var c = rnd.LowerLetter();

            Assert.IsTrue(char.IsLower(c));
        }

        [TestMethod]
        public void UpperLetter_ShouldBeUppercase()
        {
            var rnd = CreateDeterministic();

            var c = rnd.UpperLetter();

            Assert.IsTrue(char.IsUpper(c));
        }

        [TestMethod]
        public void AlphaNumeric_ShouldReturnCorrectLengthAndChars()
        {
            var rnd = CreateDeterministic();

            var value = rnd.AlphaNumeric(20);

            Assert.AreEqual(20, value.Length);
            Assert.IsTrue(value.All(char.IsLetterOrDigit));
        }

        [TestMethod]
        public void Digits_ShouldReturnOnlyDigits()
        {
            var rnd = CreateDeterministic();

            var value = rnd.Digits(15);

            Assert.AreEqual(15, value.Length);
            Assert.IsTrue(value.All(char.IsDigit));
        }

        [TestMethod]
        public void Guid_ShouldNotBeEmpty()
        {
            var rnd = CreateDeterministic();

            var guid = rnd.Guid();

            Assert.AreNotEqual(Guid.Empty, guid);
        }

        [TestMethod]
        public void Token_ShouldBeCorrectLength()
        {
            var rnd = CreateDeterministic();

            var token = rnd.Token(32);

            Assert.AreEqual(32, token.Length);
        }

        [TestMethod]
        public void DateTime_ShouldBeWithinRange()
        {
            var rnd = CreateDeterministic();
            var min = DateTime.UtcNow;
            var max = min.AddDays(1);

            var value = rnd.DateTime(min, max);

            Assert.IsTrue(value >= min && value <= max);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DateTime_MinGreaterThanMax_ShouldThrow()
        {
            var rnd = CreateDeterministic();
            rnd.DateTime(DateTime.Now, DateTime.Now.AddDays(-1));
        }

        [TestMethod]
        public void TimeSpan_ShouldBeWithinRange()
        {
            var rnd = CreateDeterministic();

            var min = TimeSpan.FromMinutes(1);
            var max = TimeSpan.FromMinutes(5);

            var value = rnd.TimeSpan(min, max);

            Assert.IsTrue(value >= min && value <= max);
        }

        [TestMethod]
        public void Pick_ShouldReturnOneOfValues()
        {
            var rnd = CreateDeterministic();

            var value = rnd.Pick(1, 2, 3, 4);

            Assert.IsTrue(new[] { 1, 2, 3, 4 }.Contains(value));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Pick_EmptyArray_ShouldThrow()
        {
            var rnd = CreateDeterministic();
            rnd.Pick<int>();
        }

        [TestMethod]
        public void Shuffle_ShouldNotLoseElements()
        {
            var rnd = CreateDeterministic();
            var original = new[] { 1, 2, 3, 4, 5 };
            var copy = original.ToArray();

            rnd.Shuffle(copy);

            CollectionAssert.AreEquivalent(original, copy);
        }

        [TestMethod]
        public void Enum_ShouldReturnDefinedValue()
        {
            var rnd = CreateDeterministic();

            var value = rnd.Enum<TestEnum>();

            Assert.IsTrue(Enum.IsDefined(typeof(TestEnum), value));
        }

        private enum TestEnum { A, B, C }
    }
}