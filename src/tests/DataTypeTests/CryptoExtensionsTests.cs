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
using DomainCommonExtensions.CommonExtensions;

namespace DataTypeTests
{
    [TestClass]
    public class CryptoExtensionsTests
    {
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
        public void RsaCrypt_Decrypt_Test()
        {
            var message = "Here is your current message";
            var key = "W+CLseRlv3I9dZmyaeDoeoKPFiFRWHkJkZOBOCPtx9c=";

            var encryptedText = message.EncryptWithRSA(key);

            Assert.IsNotNull(encryptedText);

            var decryptedText = encryptedText.DecryptWithRSA(key);

            Assert.IsNotNull(decryptedText);
            Assert.AreEqual(message, decryptedText);
        }
    }
}