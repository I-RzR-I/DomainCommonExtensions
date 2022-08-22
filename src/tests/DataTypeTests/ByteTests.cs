// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2022-08-14 19:25
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-15 19:39
// ***********************************************************************
//  <copyright file="ByteTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Linq;
using DomainCommonExtensions.DataTypeExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace DataTypeTests
{
    [TestClass]
    public class ByteTests
    {
        private const string ClearString = "Hi there.";
        private readonly byte[] _bytesFromClear;

        public ByteTests()
        {
            _bytesFromClear = ClearString.ToBytes();
        }

        [TestMethod]
        public void ToBase64StringTest()
        {
            var resultB64 = _bytesFromClear.ToBase64String();
            Assert.IsNotNull(resultB64);

            var resBytes = resultB64.DeCodeBytesFromBase64();
            Assert.IsNotNull(resBytes);

            Assert.IsTrue(_bytesFromClear.SequenceEqual(resBytes));
        }

        [TestMethod]
        public void GetHashSha256StringTest()
        {
            var sha256 = _bytesFromClear.GetHashSha256String();

            Assert.IsNotNull(sha256);
        }

        [TestMethod]
        public void GetHashSha1StringTest()
        {
            var sha1 = _bytesFromClear.GetHashSha1String();

            Assert.IsNotNull(sha1);
        }

        [TestMethod]
        public void GetStringUtf8Test()
        {
            var str = _bytesFromClear.GetStringUTF8();

            Assert.IsNotNull(str);
            Assert.IsTrue(ClearString.Equals(str));
        }

        [TestMethod]
        public void GetStringTest()
        {
            var str = _bytesFromClear.GetString();

            Assert.IsNotNull(str);
            Assert.IsTrue(ClearString.Equals(str));
        }
    }
}