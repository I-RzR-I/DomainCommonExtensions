using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.IO;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class StreamReadAllBytesPositionTests
    {
        [TestMethod]
        public void ReadAllBytes_MemoryStreamMidPosition_Test()
        {
            using var stream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5 });
            stream.Position = 2;

            var result = stream.ReadAllBytes();

            CollectionAssert.AreEqual(new byte[] { 1, 2, 3, 4, 5 }, result);
            Assert.AreEqual(2, stream.Position);
        }

        [TestMethod]
        public void ReadAllBytes_BufferedStreamMidPosition_Test()
        {
            using var inner = new MemoryStream(new byte[] { 1, 2, 3, 4, 5 });
            using var stream = new BufferedStream(inner);
            stream.Position = 2;

            var result = stream.ReadAllBytes();

            CollectionAssert.AreEqual(new byte[] { 3, 4, 5 }, result);
            Assert.AreEqual(5, stream.Position);
        }
    }
}
