using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.IO.Ini;
namespace DataTypeTests.HelperTests
{
    [TestClass]
    public class IniFileHelperTwoArgGetEntryTests
    {
        [TestMethod]
        public void GetEntry_TwoArgumentCallReturnsValue_Test()
        {
            var helper = IniFileHelper.Instance.Load("[s]\r\nk=x");

            string result = helper.GetEntry("s", "k");

            Assert.AreEqual("x", result);
        }
    }
}
