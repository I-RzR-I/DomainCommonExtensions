using System;
using System.Globalization;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class EnumConversionEdgeTests
    {
        [TestMethod]
        public void ToStringGeneric_MemberName_Test()
        {
            var value = OrdinalTestEnum.Second;

            var result = value.ToString<OrdinalTestEnum>();

            Assert.AreEqual("Second", result);
        }

        [TestMethod]
        public void ToStringGeneric_ByteEnum_Test()
        {
            var value = ByteBasedTestEnum.Seven;

            var result = value.ToString<ByteBasedTestEnum>();

            Assert.AreEqual("Seven", result);
        }

        [TestMethod]
        public void ToStringGeneric_FlagsCombination_Test()
        {
            var value = FlagsTestEnum.A | FlagsTestEnum.B;

            Assert.AreEqual("A, B", value.ToString<FlagsTestEnum>());
        }

        [TestMethod]
        public void ToStringGeneric_UndefinedValue_Test()
        {
            var value = (OrdinalTestEnum)5;

            Assert.AreEqual("5", value.ToString<OrdinalTestEnum>());
        }

        [TestMethod]
        public void ToInt_NegativeMember_Test()
        {
            Assert.AreEqual(-5, NegativeTestEnum.Neg.ToInt());
        }

        [TestMethod]
        public void GetEnumValueInt_NegativeMember_Test()
        {
            Assert.AreEqual(NegativeTestEnum.Neg, (-5).GetEnumValue<NegativeTestEnum>());
            Assert.AreEqual(NegativeTestEnum.Pos, 5.GetEnumValue<NegativeTestEnum>());
        }

        [TestMethod]
        public void ToInt_IntEnum_Test()
        {
            var result = OrdinalTestEnum.Third.ToInt();

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void ToInt_ByteEnum_Test()
        {
            var result = ByteBasedTestEnum.Seven.ToInt();

            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void ToInt_LongEnumInRange_Test()
        {
            var result = LongBasedTestEnum.Small.ToInt();

            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public void ToInt_LongEnumOverflow_Test()
        {
            var value = LongBasedTestEnum.Large;

            Assert.ThrowsException<OverflowException>(() => value.ToInt());
        }

        [DataRow("second")]
        [DataRow("SECOND")]
        [DataRow("Second")]
        [TestMethod]
        public void GetEnumValueString_CaseInsensitive_Test(string input)
        {
            var result = input.GetEnumValue<OrdinalTestEnum>();

            Assert.AreEqual(OrdinalTestEnum.Second, result);
        }

        [TestMethod]
        public void GetEnumValueString_TurkishCulture_Test()
        {
            using (new CultureScopeTestHelper("tr-TR"))
            {
                Assert.AreEqual("İ", "i".ToUpper(CultureInfo.CurrentCulture));

                var result = "info".GetEnumValue<LevelTestEnum>();

                Assert.AreEqual(LevelTestEnum.INFO, result);
            }
        }

        [TestMethod]
        public void GetEnumValueString_TurkishCultureUpper_Test()
        {
            using (new CultureScopeTestHelper("tr-TR"))
            {
                Assert.AreEqual("İ", "i".ToUpper(CultureInfo.CurrentCulture));

                var result = "INFO".GetEnumValue<LevelTestEnum>();

                Assert.AreEqual(LevelTestEnum.INFO, result);
            }
        }

        [DataRow("unknown")]
        [DataRow("Fourth")]
        [TestMethod]
        public void GetEnumValueString_UnknownFallback_Test(string input)
        {
            var result = input.GetEnumValue<OrdinalTestEnum>();

            Assert.AreEqual(OrdinalTestEnum.First, result);
        }

        [TestMethod]
        public void GetEnumValueString_NullFallback_Test()
        {
            string input = null;

            var result = input.GetEnumValue<OrdinalTestEnum>();

            Assert.AreEqual(OrdinalTestEnum.First, result);
        }

        [TestMethod]
        public void GetEnumValueString_EmptyFallback_Test()
        {
            var result = string.Empty.GetEnumValue<OrdinalTestEnum>();

            Assert.AreEqual(OrdinalTestEnum.First, result);
        }

        [TestMethod]
        public void GetEnumValueInt_UIntEnumMatch_Test()
        {
            var result = 1.GetEnumValue<UIntBasedTestEnum>();

            Assert.AreEqual(UIntBasedTestEnum.One, result);
        }

        [TestMethod]
        public void GetEnumValueInt_UIntEnumFallback_Test()
        {
            var result = 7.GetEnumValue<UIntBasedTestEnum>();

            Assert.AreEqual(UIntBasedTestEnum.Zero, result);
        }

        [TestMethod]
        public void GetEnumValueInt_IntEnumMatch_Test()
        {
            var result = 2.GetEnumValue<OrdinalTestEnum>();

            Assert.AreEqual(OrdinalTestEnum.Third, result);
        }

        [TestMethod]
        public void GetEnumValueInt_IntEnumFallback_Test()
        {
            var result = 99.GetEnumValue<OrdinalTestEnum>();

            Assert.AreEqual(OrdinalTestEnum.First, result);
        }
    }
}
