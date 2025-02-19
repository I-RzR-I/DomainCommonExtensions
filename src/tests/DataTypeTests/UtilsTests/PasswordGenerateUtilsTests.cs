// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2025-02-16 20:50
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-02-16 20:50
// ***********************************************************************
//  <copyright file="PasswordGenerateUtilsTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System.Diagnostics;
using System.Linq;
using DomainCommonExtensions.DataTypeExtensions;
using DomainCommonExtensions.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataTypeTests.UtilsTests
{
    [TestClass]
    public class PasswordGenerateUtilsTests
    {
        [DataRow(8, 4, true, true, true, true)]
        [DataRow(4, 4, true, true, true, true)]
        [DataRow(1, 4, true, true, true, true)]
        [DataRow(4, 1, true, true, true, true)]
        [DataRow(8, 4, false, true, true, true)]
        [DataRow(4, 4, true, false, true, true)]
        [DataRow(1, 4, true, true, false, true)]
        [DataRow(4, 1, true, true, true, false)]
        [TestMethod]
        public void GeneratePass_1_Test(int requiredMinimumLength, int requiredUniqueChar, bool requiredDigits,
            bool requiredLowerChars, bool requiredUpperChars, bool requiredNonAlphanumeric)
        {
            var pass = PasswordGenerateUtils.Generate(requiredMinimumLength, requiredUniqueChar, requiredDigits,
                requiredLowerChars, requiredUpperChars, requiredNonAlphanumeric);

            Assert.IsNotNull(pass);
            Debug.WriteLine($"Generated pass code: {pass}");
            Assert.AreEqual(requiredMinimumLength < requiredUniqueChar ? requiredUniqueChar : requiredMinimumLength, pass.Length);

            var passArray = pass.ToStringArray();

            Assert.IsTrue(passArray.Select(x => x).Distinct().Count() >= requiredUniqueChar);

            if (requiredDigits.IsTrue())
                Assert.IsTrue(passArray.Count(x => x.Any(char.IsDigit)) >= 1);
            else 
                Assert.IsTrue(passArray.Count(x => x.Any(char.IsDigit)) == 0);

            if (requiredLowerChars.IsTrue())
                Assert.IsTrue(passArray.Count(x => x.Any(char.IsLetter) && x == x.ToLower()) >= 1);
            else
                Assert.IsTrue(passArray.Count(x => x.Any(char.IsLetter) && x == x.ToLower()) == 0);

            if (requiredUpperChars.IsTrue())
                Assert.IsTrue(passArray.Count(x => x == x.ToUpper()) >= 1);
            else
                Assert.IsTrue(passArray.Count(x => x.Any(char.IsLetter) && x == x.ToUpper()) == 0);

            if (requiredNonAlphanumeric.IsTrue())
                Assert.IsTrue(passArray.Count(x => x.Any(char.IsLetter).IsFalse() && x.Any(char.IsDigit).IsFalse()) >= 1);
            else
                Assert.IsTrue(passArray.Count(x => x.Any(char.IsLetter).IsFalse() && x.Any(char.IsDigit).IsFalse()) == 0);
        }

        [DataRow(8, 4, true, true, true, true)]
        [DataRow(4, 4, true, true, true, true)]
        [DataRow(1, 4, true, true, true, true)]
        [DataRow(4, 1, true, true, true, true)]
        [DataRow(8, 4, false, true, true, true)]
        [DataRow(4, 4, true, false, true, true)]
        [DataRow(1, 4, true, true, false, true)]
        [DataRow(4, 1, true, true, true, false)]
        [TestMethod]
        public void GeneratePass_2_Test(int requiredMinimumLength, int requiredUniqueChar, bool requiredDigits,
            bool requiredLowerChars, bool requiredUpperChars, bool requiredNonAlphanumeric)
        {
            var ownChars = new[] { "&", "[", "]" };
            var pass = PasswordGenerateUtils.Generate(requiredMinimumLength, requiredUniqueChar, requiredDigits,
                requiredLowerChars, requiredUpperChars, requiredNonAlphanumeric, ownChars);

            Assert.IsNotNull(pass);
            Debug.WriteLine($"Generated pass code: {pass}");
            Assert.AreEqual(requiredMinimumLength < requiredUniqueChar ? requiredUniqueChar : requiredMinimumLength, pass.Length);

            var passArray = pass.ToStringArray();

            Assert.IsTrue(passArray.Select(x => x).Distinct().Count() >= requiredUniqueChar);

            if (requiredDigits.IsTrue())
                Assert.IsTrue(passArray.Count(x => x.Any(char.IsDigit)) >= 1);
            else
                Assert.IsTrue(passArray.Count(x => x.Any(char.IsDigit)) == 0);

            if (requiredLowerChars.IsTrue())
                Assert.IsTrue(passArray.Count(x => x.Any(char.IsLetter) && x == x.ToLower()) >= 1);
            else
                Assert.IsTrue(passArray.Count(x => x.Any(char.IsLetter) && x == x.ToLower()) == 0);

            if (requiredUpperChars.IsTrue())
                Assert.IsTrue(passArray.Count(x => x == x.ToUpper()) >= 1);
            else
                Assert.IsTrue(passArray.Count(x => x.Any(char.IsLetter) && x == x.ToUpper()) == 0);

            if (requiredNonAlphanumeric.IsTrue())
                Assert.IsTrue(passArray.Count(x => x.Any(char.IsLetter).IsFalse() && x.Any(char.IsDigit).IsFalse()) >= 1);
            else
                Assert.IsTrue(passArray.Count(x => x.Any(char.IsLetter).IsFalse() && x.Any(char.IsDigit).IsFalse()) == 0);
        }

        [DataRow(1, 2, true, true, true, true)]
        [DataRow(2, 4, true, true, true, true)]
        [DataRow(1, 4, true, true, true, true)]
        [DataRow(3, 1, true, true, true, true)]
        [DataRow(3, 3, false, true, true, true)]
        [DataRow(2, 4, true, false, true, true)]
        [DataRow(1, 4, true, true, false, true)]
        [DataRow(2, 1, true, true, true, false)]
        [TestMethod]
        public void GeneratePass_3_Test(int requiredMinimumLength, int requiredUniqueChar, bool requiredDigits,
            bool requiredLowerChars, bool requiredUpperChars, bool requiredNonAlphanumeric)
        {
            var ownChars = new[] { "&", "[", "]" };
            var pass = PasswordGenerateUtils.Generate(requiredMinimumLength, requiredUniqueChar, requiredDigits,
                requiredLowerChars, requiredUpperChars, requiredNonAlphanumeric, ownChars);

            Assert.IsNotNull(pass);
            Debug.WriteLine($"Generated pass code: {pass}");
            Assert.AreEqual(requiredMinimumLength < requiredUniqueChar || requiredMinimumLength < 4 ? 4 : requiredMinimumLength, pass.Length);

            var passArray = pass.ToStringArray();

            Assert.IsTrue(passArray.Select(x => x).Distinct().Count() >= requiredUniqueChar);

            if (requiredDigits.IsTrue())
                Assert.IsTrue(passArray.Count(x => x.Any(char.IsDigit)) >= 1);
            else
                Assert.IsTrue(passArray.Count(x => x.Any(char.IsDigit)) == 0);

            if (requiredLowerChars.IsTrue())
                Assert.IsTrue(passArray.Count(x => x.Any(char.IsLetter) && x == x.ToLower()) >= 1);
            else
                Assert.IsTrue(passArray.Count(x => x.Any(char.IsLetter) && x == x.ToLower()) == 0);

            if (requiredUpperChars.IsTrue())
                Assert.IsTrue(passArray.Count(x => x == x.ToUpper()) >= 1);
            else
                Assert.IsTrue(passArray.Count(x => x.Any(char.IsLetter) && x == x.ToUpper()) == 0);

            if (requiredNonAlphanumeric.IsTrue())
                Assert.IsTrue(passArray.Count(x => x.Any(char.IsLetter).IsFalse() && x.Any(char.IsDigit).IsFalse()) >= 1);
            else
                Assert.IsTrue(passArray.Count(x => x.Any(char.IsLetter).IsFalse() && x.Any(char.IsDigit).IsFalse()) == 0);
        }

        [DataRow(8, 4, true, true, true, true)]
        [DataRow(18, 8, true, true, true, false)]
        [DataRow(18, 10, true, true, true, true)]
        [DataRow(11, 10, true, true, true, true)]
        [TestMethod]
        public void TestMultiGeneration(int requiredMinimumLength, int requiredUniqueChar, bool requiredDigits,
            bool requiredLowerChars, bool requiredUpperChars, bool requiredNonAlphanumeric)
        {
            for (int i = 0; i < 10; i++)
            {
                var pass = PasswordGenerateUtils.Generate(requiredMinimumLength, requiredUniqueChar, requiredDigits,
                    requiredLowerChars, requiredUpperChars, requiredNonAlphanumeric);

                Debug.WriteLine(pass);
            }

            var ownChars = new string[] { "&", "[", "]", "*" };
            for (int i = 0; i < 10; i++)
            {
                var pass = PasswordGenerateUtils.Generate(requiredMinimumLength, requiredUniqueChar, requiredDigits,
                    requiredLowerChars, requiredUpperChars, requiredNonAlphanumeric, ownChars);

                Debug.WriteLine(pass);
            }
        }
    }
}