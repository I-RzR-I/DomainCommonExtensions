// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2025-11-06 15:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-11-06 15:02
// ***********************************************************************
//  <copyright file="IniFileHelperTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using DomainCommonExtensions.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataTypeTests.HelperTests
{
    [TestClass]
    public class IniFileHelperTests
    {
        private string _iniFileRaw = string.Empty;

        [TestInitialize]
        public void Init()
        {
            _iniFileRaw = @"
title = Title Example
title app = Title Example

[main]
animated=false
type=png
type=jpeg
origins=pseudeo:map:3
item_main_category=basemaps
title=Standard
background=#70B5D5
widthWeight=0.5

[mci]
woafont=dosapp.FON
EGA80WOA.FON=EGA80WOA.FON

[database]
server = ""192.168.1.1""
ports = [ 8000, 8001, 8002 ]
public.ports = [ 9000, 9001, 9002 ]
server.name = local
connection_max = 5000
enabled = true
db name = TEMP

[boot loader]
timeout=40

[servers]

  [servers.alpha]
  ip = ""10.0.0.1""
  dc = ""eqdc10""

  [servers.beta]
  ip = ""10.0.0.2""
  dc = ""eqdc10""
";
        }

        [TestMethod]
        public void LoadFromClearText_GetEntries_BySection_WithOutDuplicate_Test()
        {
            var instance = IniFileHelper.Instance.Load(_iniFileRaw);

            var secMain = instance.GetEntries("main");
            Assert.IsNotNull(secMain);
            Assert.IsTrue(secMain.IsNotNullOrEmptyEnumerable());
            Assert.AreEqual(7, secMain.Count);
            Assert.AreEqual("Standard", secMain["title"][0]);
            Assert.AreEqual("jpeg", secMain["type"][0]);

            var secMci = instance.GetEntries("mci");
            Assert.IsNotNull(secMci);
            Assert.IsTrue(secMci.IsNotNullOrEmptyEnumerable());
            Assert.AreEqual("dosapp.FON", secMci["woafont"][0]);

            var secDatabase = instance.GetEntries("database");
            Assert.IsNotNull(secDatabase);
            Assert.IsTrue(secDatabase.IsNotNullOrEmptyEnumerable());
            Assert.AreEqual("5000", secDatabase["connection_max"][0]);
            Assert.AreEqual("[ 8000, 8001, 8002 ]", secDatabase["ports"][0]);
            Assert.AreEqual("[ 9000, 9001, 9002 ]", secDatabase["public.ports"][0]);
            Assert.AreEqual("local", secDatabase["server.name"][0]);
            
            var secServerAlpha = instance.GetEntries("servers.alpha");
            Assert.IsNotNull(secServerAlpha);
            Assert.IsTrue(secServerAlpha.IsNotNullOrEmptyEnumerable());
            Assert.AreEqual("\"10.0.0.1\"", secServerAlpha["ip"][0]);
            Assert.AreEqual("\"eqdc10\"", secServerAlpha["dc"][0]);
        }

        [TestMethod]
        public void LoadFromClearText_GetEntries_BySection_WithDuplicate_Test()
        {
            var file = @"
[main]
animated=false
type=png
type=jpeg
origins=pseudeo:map:3
item_main_category=basemaps
title=Standard
background=#70B5D5
widthWeight=0.5
";

            var instance = IniFileHelper.Instance;

            instance.Load(file);
            var info = instance.GetEntries("main", true);

            Assert.IsNotNull(info);
            Assert.IsTrue(info.IsNotNullOrEmptyEnumerable());
            Assert.AreEqual(7, info.Count);
            Assert.AreEqual("Standard", info["title"][0]);
            Assert.AreEqual("png", info["type"][0]);
            Assert.AreEqual("jpeg", info["type"][1]);
        }

        [TestMethod]
        public void LoadFromClearText_GetEntries_BySectionAndKey_WithOutDuplicate_Test()
        {
            var file = @"
[main]
animated=false
type=png
type=jpeg
origins=pseudeo:map:3
item_main_category=basemaps
title=Standard
background=#70B5D5
widthWeight=0.5
";

            var instance = IniFileHelper.Instance;

            instance.Load(file);
            var info = instance.GetEntry("main", "type", false);

            Assert.IsNotNull(info);
            Assert.AreEqual(1, info.Item2.Length);
            Assert.AreEqual("type", info.Item1);
        }

        [TestMethod]
        public void LoadFromClearText_GetEntries_BySectionAndKey_WithDuplicate_Test()
        {
            var file = @"
[main]
animated=false
type=png
type=jpeg
origins=pseudeo:map:3
item_main_category=basemaps
title=Standard
background=#70B5D5
widthWeight=0.5
";

            var instance = IniFileHelper.Instance;

            instance.Load(file);
            var info = instance.GetEntry("main", "type", true);

            Assert.IsNotNull(info);
            Assert.AreEqual("type", info.Item1);
            Assert.AreEqual(2, info.Item2.Length);
        }

        [TestMethod]
        public void LoadFromClearText_GetDistinctEntryValue_Test()
        {
            var file = @"
[main]
animated=false
type=png
type=jpeg
origins=pseudeo:map:3
item_main_category=basemaps
title=Standard
background=#70B5D5
widthWeight=0.5
";

            var instance = IniFileHelper.Instance.Load(file);

            var info = instance.GetDistinctEntryValue("main");

            Assert.IsNotNull(info);
            Assert.IsTrue(info.IsNotNullOrEmptyEnumerable());
            Assert.AreEqual(7, info.Count);
            Assert.AreEqual("jpeg", info["type"]);
        }

        [DataRow("title", "Standard", "")]
        [DataRow("title1", "Standard", "NoValue")]
        [DataRow("type", "jpeg", "")]
        [TestMethod]
        public void LoadFromClearText_GetEntry_Test(string key, string exceptedValue, string defaultValue)
        {
            var file = @"
[main]
animated=false
type=png
type=jpeg
origins=pseudeo:map:3
item_main_category=basemaps
title=Standard
background=#70B5D5
widthWeight=0.5
";

            var instance = IniFileHelper.Instance.Load(file);

            var info = instance.GetEntry("main", key, defaultValue);

            Assert.IsNotNull(info);
            Assert.IsTrue(info.IsPresent());

            if (defaultValue.IsPresent())
                Assert.AreEqual(defaultValue, info);
            else Assert.AreEqual(exceptedValue, info);
        }

        [TestMethod]
        public void Load_SectionWithSpace_Test()
        {
            var instance = IniFileHelper.Instance.Load(_iniFileRaw);

            var info = instance.GetEntries("boot loader");
            Assert.IsNotNull(info);
            Assert.IsTrue(info.IsNotNullOrEmptyEnumerable());
            Assert.AreEqual("40", info["timeout"][0]);
        }

        [TestMethod]
        public void Load_EntryWithNoSection_Test()
        {
            var instance = IniFileHelper.Instance.Load(_iniFileRaw);

            var secEmpty = instance.GetEntries("");
            Assert.IsNotNull(secEmpty);
            Assert.IsTrue(secEmpty.IsNotNullOrEmptyEnumerable());
            Assert.IsTrue(secEmpty.Count == 2);

            Assert.IsTrue(secEmpty.ContainsKey("title"));
            Assert.AreEqual("Title Example", secEmpty["title"][0]);

            Assert.IsTrue(secEmpty.ContainsKey("title app"));
            Assert.AreEqual("Title Example", secEmpty["title app"][0]);
        }

        [TestMethod]
        public void Load_EntryWithSectionAndKeyWithSpace_Test()
        {
            var instance = IniFileHelper.Instance.Load(_iniFileRaw);

            var info = instance.GetEntries("database");
            Assert.IsNotNull(info);
            Assert.IsTrue(info.IsNotNullOrEmptyEnumerable());
            Assert.IsTrue(info.ContainsKey("db name"));
            Assert.AreEqual("TEMP", info["db name"][0]);
        }

        [TestMethod]
        public void LoadFromClearText_GetEntry_BySectionAndKey_WithoutDuplicate_Test()
        {
            var file = @"
[main]
animated=false
type=png
type=jpeg
origins=pseudeo:map:3
item_main_category=basemaps
title=Standard
background=#70B5D5
widthWeight=0.5
";

            var instance = IniFileHelper.Instance;

            instance.Load(file);
            var info = instance.GetEntry("main", "type", false);

            Assert.IsNotNull(info);
            Assert.AreEqual(1, info.Item2.Length);
            Assert.AreEqual("jpeg", info.Item2[0]);
        }
    }
}