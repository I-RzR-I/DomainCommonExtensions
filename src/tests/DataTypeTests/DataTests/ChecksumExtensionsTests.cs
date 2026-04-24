// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2026-04-24 13:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-24 13:27
// ***********************************************************************
//  <copyright file="ChecksumExtensionsTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Diagnostics;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Text;
#endregion

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class ChecksumExtensionsTests
    {
        private static readonly Regex LowerHex64 = new Regex("^[0-9a-f]{64}$", RegexOptions.Compiled);

        [TestMethod]
        public void ToHexString_LowerCase_IsDefault()
        {
            var bytes = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF };

            Assert.AreEqual("deadbeef", bytes.ToHexString());
        }

        [TestMethod]
        public void ToHexString_UpperCase_WhenRequested()
        {
            var bytes = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF };

            Assert.AreEqual("DEADBEEF", bytes.ToHexString(true));
        }

        [TestMethod]
        public void ToHexString_EmptyArray_ReturnsEmptyString()
        {
            Assert.AreEqual(string.Empty, new byte[0].ToHexString());
        }

        [TestMethod]
        public void ToHexString_NullArray_Throws()
        {
            byte[] bytes = null;

            Assert.ThrowsException<ArgumentNullException>(() => bytes.ToHexString());
        }

        [TestMethod]
        public void Sha256Hex_Bytes_KnownVector_Empty()
        {
            var hash = new byte[0].Sha256Hex();

            Assert.AreEqual("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855", hash);
        }

        [TestMethod]
        public void Sha256Hex_Bytes_KnownVector_Abc()
        {
            var hash = new[] { (byte)'a', (byte)'b', (byte)'c' }.Sha256Hex();

            Assert.AreEqual("ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad", hash);
        }

        [TestMethod]
        public void Sha256Hex_Bytes_FormatIsLowerHex64()
        {
            var hash = new byte[] { 1, 2, 3 }.Sha256Hex();

            Assert.IsTrue(LowerHex64.IsMatch(hash), "Hash must be 64 lower-case hex characters.");
        }

        [TestMethod]
        public void Sha256Hex_Bytes_NullThrows()
        {
            byte[] bytes = null;

            Assert.ThrowsException<ArgumentNullException>(() => bytes.Sha256Hex());
        }

        [TestMethod]
        public void Sha256Hex_String_KnownVector_Empty()
        {
            Assert.AreEqual(
                "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855",
                string.Empty.Sha256Hex());
        }

        [TestMethod]
        public void Sha256Hex_String_KnownVector_Abc()
        {
            Assert.AreEqual(
                "ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad",
                "abc".Sha256Hex());
        }

        [TestMethod]
        public void Sha256Hex_String_IsDeterministic()
        {
            Assert.AreEqual("hello".Sha256Hex(), "hello".Sha256Hex());
        }

        [TestMethod]
        public void Sha256Hex_String_DifferentInputs_DifferentHashes()
        {
            Assert.AreNotEqual("abc".Sha256Hex(), "abd".Sha256Hex());
        }

        [TestMethod]
        public void Sha256Hex_String_NullThrows()
        {
            string s = null;

            Assert.ThrowsException<ArgumentNullException>(() => s.Sha256Hex());
        }
    }
}