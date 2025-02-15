// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2025-01-25 23:22
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-01-25 23:30
// ***********************************************************************
//  <copyright file="CryptoRSAExtensionsTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Security.Cryptography;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable InconsistentNaming

#endregion

namespace DataTypeTests
{
    [TestClass]
    public class CryptoRSAExtensionsTests
    {
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

        [TestMethod]
        [DataRow(1024)]
        [DataRow(2048)]
        public void RSA_XmlKey_Encrypt_Decrypt_Test(int keySize)
        {
            var rsa = new RSACryptoServiceProvider(keySize);
            var privateKey = rsa.ToXmlString(true);
            var publicKey = rsa.ToXmlString(false);
            var message = "Here is your current message";

            var encrypted = message.EncryptRSAXmlKey(keySize, publicKey);
            Assert.IsNotNull(encrypted);

            var decrypt = encrypted.DecryptRSAXmlKey(keySize, privateKey);
            Assert.IsNotNull(decrypt);

            Assert.AreEqual(message, decrypt);
        }

        [TestMethod]
        [DataRow(1024)]
        [DataRow(2048)]
        public void RSA_XmlKey_Encrypt_Decrypt_KeyInBase64_Test(int keySize)
        {
            var rsa = new RSACryptoServiceProvider(keySize);
            var privateKey = rsa.ToXmlString(true).Base64Encode();
            var publicKey = rsa.ToXmlString(false).Base64Encode();
            var message = "Here is your current message";

            var encrypted = message.EncryptRSAXmlKey(keySize, publicKey, true);
            Assert.IsNotNull(encrypted);

            var decrypt = encrypted.DecryptRSAXmlKey(keySize, privateKey, true);
            Assert.IsNotNull(decrypt);

            Assert.AreEqual(message, decrypt);
        }
    }
}