using System;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Diagnostics;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class SocketKeepAliveRangeTests
    {
        [TestMethod]
        public void SetSocketKeepAliveValues_NegativeKeepAliveTime_Test()
        {
            using var tcp = new TcpClient();

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => tcp.SetSocketKeepAliveValues(-1, 50));

            Assert.AreEqual("keepAliveTime", exception.ParamName);
        }

        [TestMethod]
        public void SetSocketKeepAliveValues_NegativeKeepAliveInterval_Test()
        {
            using var tcp = new TcpClient();

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => tcp.SetSocketKeepAliveValues(200, -1));

            Assert.AreEqual("keepAliveInterval", exception.ParamName);
        }

        [TestMethod]
        public void SetSocketKeepAliveValues_MinValueKeepAliveTime_Test()
        {
            using var tcp = new TcpClient();

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => tcp.SetSocketKeepAliveValues(int.MinValue, 50));

            Assert.AreEqual("keepAliveTime", exception.ParamName);
        }

        [TestMethod]
        public void SetSocketKeepAliveValues_BothNegative_Test()
        {
            using var tcp = new TcpClient();

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => tcp.SetSocketKeepAliveValues(-1, -1));
        }

        [TestMethod]
        public void SetSocketKeepAliveValues_NullClient_Test()
        {
            TcpClient tcp = null;

            Assert.ThrowsException<ArgumentNullException>(() => tcp.SetSocketKeepAliveValues(-1, -1));
        }
    }
}
