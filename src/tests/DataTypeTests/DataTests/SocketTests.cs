// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2022-08-16 18:41
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-16 18:50
// ***********************************************************************
//  <copyright file="SocketTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Net.Sockets;
using DomainCommonExtensions.DataTypeExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class SocketTests
    {
        [TestMethod]
        public void SetSocketKeepAliveValuesTest()
        {
            var tcp = new TcpClient("google.com", 80);

            tcp.SetSocketKeepAliveValues(200, 50);

            Assert.IsTrue(tcp.Connected);

            tcp.Close();
            Assert.IsFalse(tcp.Connected);
        }
    }
}