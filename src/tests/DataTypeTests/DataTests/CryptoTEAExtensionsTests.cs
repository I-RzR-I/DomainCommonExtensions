// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2025-10-14 14:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-10-14 14:36
// ***********************************************************************
//  <copyright file="CryptoTEAExtensionsTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.CommonExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

// ReSharper disable InconsistentNaming

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class CryptoTEAExtensionsTests
    {
        [TestMethod]
        public void EncryptDecrypt_WithPassCode_Test()
        {
            var passCode = "Temp-#4f1-/45&%!ff9b5r3cvd]wer)er;;r3";
            var message = "Test-Message^-0";

            var encrypted = message.TEAEncrypt(passCode, out var newPassCode);
            Assert.IsNotNull(encrypted);
            Assert.AreEqual(passCode, newPassCode);

            var decrypt = encrypted.TEADecrypt(passCode);
            Assert.IsNotNull(decrypt);
            Assert.AreEqual(message, decrypt);
        }
    }
}