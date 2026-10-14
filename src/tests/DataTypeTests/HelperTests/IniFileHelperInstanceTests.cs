using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.IO.Ini;
namespace DataTypeTests.HelperTests
{
    [TestClass]
    public class IniFileHelperInstanceTests
    {
        [TestMethod]
        public void Load_LaterSingletonLoadKeepsEarlierResult_Test()
        {
            var first = IniFileHelper.Instance.Load("[s]\r\nkey=x");

            IniFileHelper.Instance.Load("[s]\r\nkey=y");

            Assert.AreEqual("x", first.GetEntry("s", "key", "none"));
        }

        [TestMethod]
        public void Load_ReloadOnReturnedInstanceReplacesData_Test()
        {
            var helper = IniFileHelper.Instance.Load("[s]\r\nkey=x");

            helper.Load("[s]\r\nkey=y");

            Assert.AreEqual("y", helper.GetEntry("s", "key", "none"));
        }

        [TestMethod]
        public void Load_SingletonQueriedAfterLoadReturnsLoadedData_Test()
        {
            IniFileHelper.Instance.Load("[s]\r\nkey=singleton");

            var result = IniFileHelper.Instance.GetEntry("s", "key", "none");

            Assert.AreEqual("singleton", result);
        }

        [TestMethod]
        public void Load_ReturnedInstanceIsNotSingleton_Test()
        {
            var helper = IniFileHelper.Instance.Load("[s]\r\nkey=x");

            Assert.AreNotSame(IniFileHelper.Instance, helper);
        }

        [TestMethod]
        public void Load_FromMemoryStreamReadsEntry_Test()
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("[s]\r\nkey=1"));

            var helper = IniFileHelper.Instance.Load(stream);

            Assert.AreEqual("1", helper.GetEntry("s", "key", "none"));
        }
    }
}
