// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2025-10-14 08:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-10-14 08:29
// ***********************************************************************
//  <copyright file="TeaTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.DataTypeExtensions;
using DomainCommonExtensions.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class TeaTests
    {
        [DataRow("test.mmm-info", "bHBtMlgU1W7P6NezdRlI5g==", 572342334, 984612573, 958361538, 763518593)]
        [DataRow("test.mmm-info", "1L7l4t74sZDaNjHrreSyTg==", 10, 20, 30, 40)]
        [DataRow("test.mmm-info", "ol1zoNz2KoSyfS91ssjP6w==", 99, 98, 97, 44456)]
        [TestMethod]
        public void Encrypt_Test(string rawText, string encryptedText, int k1, int k2, int k3, int k4)
        {
            var key = new uint[4];
            key[0] = (uint)k1;
            key[1] = (uint)k2;
            key[2] = (uint)k3;
            key[3] = (uint)k4;

            uint[] data = null;
            var encryptedRawData = string.Empty;

            TinyEncryptionAlgorithmHelper.ConvertTextToData(ref rawText, ref data);
            TinyEncryptionAlgorithmHelper.XxTeaEncrypt(ref data, key);
            TinyEncryptionAlgorithmHelper.WriteData(ref data, ref encryptedRawData);

            Assert.IsNotNull(data);
            Assert.IsNotNull(encryptedRawData);
            Assert.IsTrue(encryptedRawData.IsPresent());
            Assert.AreEqual(encryptedText, encryptedRawData);
        }

        [DataRow("test.mmm-info", "bHBtMlgU1W7P6NezdRlI5g==", 572342334, 984612573, 958361538, 763518593)]
        [DataRow("test.mmm-info", "1L7l4t74sZDaNjHrreSyTg==", 10, 20, 30, 40)]
        [DataRow("test.mmm-info", "ol1zoNz2KoSyfS91ssjP6w==", 99, 98, 97, 44456)]
        [TestMethod]
        public void Decrypt_Test(string rawText, string encryptedText, int k1, int k2, int k3, int k4)
        {
            var key = new uint[4];
            key[0] = (uint)k1;
            key[1] = (uint)k2;
            key[2] = (uint)k3;
            key[3] = (uint)k4;

            uint[] readData = null;
            var decryptedRawData = string.Empty;

            TinyEncryptionAlgorithmHelper.ReadData(ref encryptedText, ref readData);
            TinyEncryptionAlgorithmHelper.XxTeaDecrypt(ref readData, key);
            TinyEncryptionAlgorithmHelper.ConvertDataToText(ref decryptedRawData, readData);

            Assert.IsNotNull(readData);
            Assert.IsNotNull(decryptedRawData);
            Assert.IsTrue(decryptedRawData.IsPresent());
            Assert.AreEqual(rawText, decryptedRawData);
        }

        [TestMethod]
        public void EncryptDecrypt_WithPassCode_Test()
        {
            var passCode = "Temp-#4f1-/45&%!ff9b5r3cvd]wer)er;;r3";
            TinyEncryptionAlgorithmHelper.GeneratePassKeyFromPassCode(passCode, out var passKey);
            var rawText = "Test-Message^-0";
            uint[] data = null;
            var encryptedRawData = string.Empty;

            TinyEncryptionAlgorithmHelper.ConvertTextToData(ref rawText, ref data);
            TinyEncryptionAlgorithmHelper.XxTeaEncrypt(ref data, passKey);
            TinyEncryptionAlgorithmHelper.WriteData(ref data, ref encryptedRawData);

            Assert.IsNotNull(data);
            Assert.IsNotNull(encryptedRawData);
            Assert.IsTrue(encryptedRawData.IsPresent());

            var decryptedRawData = string.Empty;
            uint[] readData = null;

            TinyEncryptionAlgorithmHelper.ReadData(ref encryptedRawData, ref readData);
            TinyEncryptionAlgorithmHelper.XxTeaDecrypt(ref readData, passKey);
            TinyEncryptionAlgorithmHelper.ConvertDataToText(ref decryptedRawData, readData);

            Assert.IsNotNull(readData);
            Assert.IsNotNull(decryptedRawData);
            Assert.IsTrue(decryptedRawData.IsPresent());
            Assert.AreEqual(rawText, decryptedRawData);
        }

        [DataRow("p@$4")]
        [DataRow("p@$4s")]
        [DataRow("p@$4s9")]
        [DataRow("p@$4s9$")]
        [DataRow("p@$4s9$&")]
        [DataRow("p@$4s9$&!")]
        [DataRow("p@$4s9$&![")]
        [DataRow("p@$4s9$&![)")]
        [DataRow("p@$4s9$&![))")]
        [DataRow("p@$4s9$&![))(")]
        [DataRow("p@$4s9$&![))()")]
        [DataRow("p@$4s9$&![))()8")]
        [DataRow("p@$4s9$&![))()87")]
        [DataRow("p@$4s9$&![))()87-")]
        [DataRow("p@$4s9$&![))()87-*22f4")]
        [TestMethod]
        public void EncryptDecrypt_WithPassCode_2_Test(string passCode)
        {
            TinyEncryptionAlgorithmHelper.GeneratePassKeyFromPassCode(passCode, out var passKey);
            var rawText = "Test-Message^-0";
            uint[] data = null;
            var encryptedRawData = string.Empty;

            TinyEncryptionAlgorithmHelper.ConvertTextToData(ref rawText, ref data);
            TinyEncryptionAlgorithmHelper.XxTeaEncrypt(ref data, passKey);
            TinyEncryptionAlgorithmHelper.WriteData(ref data, ref encryptedRawData);

            Assert.IsNotNull(data);
            Assert.IsNotNull(encryptedRawData);
            Assert.IsTrue(encryptedRawData.IsPresent());

            var decryptedRawData = string.Empty;
            uint[] readData = null;

            TinyEncryptionAlgorithmHelper.ReadData(ref encryptedRawData, ref readData);
            TinyEncryptionAlgorithmHelper.XxTeaDecrypt(ref readData, passKey);
            TinyEncryptionAlgorithmHelper.ConvertDataToText(ref decryptedRawData, readData);

            Assert.IsNotNull(readData);
            Assert.IsNotNull(decryptedRawData);
            Assert.IsTrue(decryptedRawData.IsPresent());
            Assert.AreEqual(rawText, decryptedRawData);
        }
    }
}