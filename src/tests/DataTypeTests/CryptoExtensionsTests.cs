// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2025-01-09 19:34
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-01-09 19:34
// ***********************************************************************
//  <copyright file="CryptoExtensionsTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.Cryptography;
using DomainCommonExtensions.CommonExtensions;

namespace DataTypeTests
{
    [TestClass]
    public class CryptoExtensionsTests
    {
        private static string RandomString(int size)
        {
            const string alphabet = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            var chars = new char[size];
            for (var i = 0; i < size; i++)
            {
                chars[i] = alphabet[random.Next(alphabet.Length)];
            }

            return new string(chars);
        }

        private static string GenerateXBitKey(int keySize)
        {
            using var aesAlg = Aes.Create();
            aesAlg.KeySize = keySize;
            aesAlg.GenerateKey();

            return Convert.ToBase64String(aesAlg.Key);
        }

        [TestMethod]
        public void AesCrypt_Decrypt_iv_16_Test()
        {
            // aes-128-cfb / 16 / wE/c3UTejZsKQozI4UN7DQ==
            var message = "Here is your current message";
            var key = "wE/c3UTejZsKQozI4UN7DQ==";
            var iv = new byte[16];
            new Random().NextBytes(iv);

            var encryptedText = message.AesEncryptString(key, iv);

            Assert.IsNotNull(encryptedText);

            var decryptedText = encryptedText.AesDecryptString(key, iv);

            Assert.IsNotNull(decryptedText);
            Assert.AreEqual(message, decryptedText);
        }

        [TestMethod]
        public void AesCrypt_Decrypt_iv_16_2_Test()
        {
            var message = "Here is your current message";
            var key = "4ZdI7TzQzOsXRG/tRNf1L2DcLOEztZuH";
            var iv = new byte[16];
            new Random().NextBytes(iv);

            var encryptedText = message.AesEncryptString(key, iv);

            Assert.IsNotNull(encryptedText);

            var decryptedText = encryptedText.AesDecryptString(key, iv);

            Assert.IsNotNull(decryptedText);
            Assert.AreEqual(message, decryptedText);
        }

        [TestMethod]
        public void AesCrypt_Decrypt_iv_16_2_Test_2()
        {
            var message = "Here is your current message";
            var key = RandomString(24);
            var iv = new byte[16];
            new Random().NextBytes(iv);

            var encryptedText = message.AesEncryptString(key, iv);

            Assert.IsNotNull(encryptedText);

            var decryptedText = encryptedText.AesDecryptString(key, iv);

            Assert.IsNotNull(decryptedText);
            Assert.AreEqual(message, decryptedText);
        }

        [TestMethod]
        public void AesCrypt_Decrypt_iv_16_LongKey_Test()
        {
            var message = "Here is your current message";
            var key = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC7pbDRvuqRL0EsKjYGqPZWu4VF";
            var iv = new byte[16];
            new Random().NextBytes(iv);

            var encryptedText = message.AesEncryptString(key, iv, true);

            Assert.IsNotNull(encryptedText);

            var decryptedText = encryptedText.AesDecryptString(key, iv, true);

            Assert.IsNotNull(decryptedText);
            Assert.AreEqual(message, decryptedText);
        }

        [DataRow(128, 128, 16, false)]
        [DataRow(128, 128, 16, false)]
        [DataRow(192, 128, 16, true)]
        [DataRow(192, 128, 16, false)]
        [DataRow(256, 128, 16, true)]
        [TestMethod]
        public void AesCrypt_Decrypt_iv_16_LongKey_Test(int blockKeySize, int blockSize, int blockByteSize, bool useLongKey)
        {
            var message = "Here is your current message";
            var key = GenerateXBitKey(blockKeySize);
            var iv = new byte[blockByteSize];
            new Random().NextBytes(iv);

            var encryptedText = message.AesEncryptString(key, iv, useLongKey, blockKeySize, blockSize);

            Assert.IsNotNull(encryptedText);

            var decryptedText = encryptedText.AesDecryptString(key, iv, useLongKey, blockKeySize, blockSize);

            Assert.IsNotNull(decryptedText);
            Assert.AreEqual(message, decryptedText);
        }
    }
}