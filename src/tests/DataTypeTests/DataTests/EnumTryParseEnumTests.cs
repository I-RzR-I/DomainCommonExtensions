using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class EnumTryParseEnumTests
    {
        [TestMethod]
        public void TryParseEnum_NullValue_Test()
        {
            string value = null;

            var parsed = value.TryParseEnum<OrdinalTestEnum>(out var result);

            Assert.IsFalse(parsed);
            Assert.AreEqual(default(OrdinalTestEnum), result);
        }

        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t\r\n")]
        [TestMethod]
        public void TryParseEnum_EmptyOrWhiteSpace_Test(string value)
        {
            var parsed = value.TryParseEnum<OrdinalTestEnum>(out var result);

            Assert.IsFalse(parsed);
            Assert.AreEqual(default(OrdinalTestEnum), result);
        }

        [TestMethod]
        public void TryParseEnum_ExactName_Test()
        {
            var parsed = "Third".TryParseEnum<OrdinalTestEnum>(out var result);

            Assert.IsTrue(parsed);
            Assert.AreEqual(OrdinalTestEnum.Third, result);
        }

        [TestMethod]
        public void TryParseEnum_SurroundingWhiteSpace_Test()
        {
            var parsed = "  Second  ".TryParseEnum<OrdinalTestEnum>(out var result);

            Assert.IsTrue(parsed);
            Assert.AreEqual(OrdinalTestEnum.Second, result);
        }

        [DataRow("second")]
        [DataRow("SECOND")]
        [TestMethod]
        public void TryParseEnum_WrongCaseDefault_Test(string value)
        {
            var parsed = value.TryParseEnum<OrdinalTestEnum>(out var result);

            Assert.IsFalse(parsed);
            Assert.AreEqual(default(OrdinalTestEnum), result);
        }

        [TestMethod]
        public void TryParseEnum_WrongCaseExplicitFalse_Test()
        {
            var parsed = "second".TryParseEnum<OrdinalTestEnum>(out var result, false);

            Assert.IsFalse(parsed);
            Assert.AreEqual(default(OrdinalTestEnum), result);
        }

        [DataRow("second")]
        [DataRow("SECOND")]
        [DataRow("sEcOnD")]
        [TestMethod]
        public void TryParseEnum_IgnoreCase_Test(string value)
        {
            var parsed = value.TryParseEnum<OrdinalTestEnum>(out var result, true);

            Assert.IsTrue(parsed);
            Assert.AreEqual(OrdinalTestEnum.Second, result);
        }

        [TestMethod]
        public void TryParseEnum_UnknownName_Test()
        {
            var parsed = "Fourth".TryParseEnum<OrdinalTestEnum>(out var result, true);

            Assert.IsFalse(parsed);
            Assert.AreEqual(default(OrdinalTestEnum), result);
        }

        [TestMethod]
        public void TryParseEnum_DefinedNumber_Test()
        {
            var parsed = "2".TryParseEnum<OrdinalTestEnum>(out var result);

            Assert.IsTrue(parsed);
            Assert.AreEqual(OrdinalTestEnum.Third, result);
        }

        [TestMethod]
        public void TryParseEnum_UndefinedNumber_Test()
        {
            var parsed = "42".TryParseEnum<OrdinalTestEnum>(out var result);

            Assert.IsFalse(parsed);
            Assert.AreEqual(default(OrdinalTestEnum), result);
        }

        [TestMethod]
        public void TryParseEnum_UndefinedNumberResetsResult_Test()
        {
            var parsed = "5".TryParseEnum<NoZeroTestEnum>(out var result);

            Assert.IsFalse(parsed);
            Assert.AreEqual(0, (int)result);
        }

        [TestMethod]
        public void TryParseEnum_ZeroWithoutZeroMember_Test()
        {
            var parsed = "0".TryParseEnum<NoZeroTestEnum>(out var result);

            Assert.IsFalse(parsed);
            Assert.AreEqual(0, (int)result);
        }

        [TestMethod]
        public void TryParseEnum_ZeroWithZeroMember_Test()
        {
            var parsed = "0".TryParseEnum<OrdinalTestEnum>(out var result);

            Assert.IsTrue(parsed);
            Assert.AreEqual(OrdinalTestEnum.First, result);
        }

        [TestMethod]
        public void TryParseEnum_NegativeDefinedNumber_Test()
        {
            var parsed = "-5".TryParseEnum<NegativeTestEnum>(out var result);

            Assert.IsTrue(parsed);
            Assert.AreEqual(NegativeTestEnum.Neg, result);
        }

        [TestMethod]
        public void TryParseEnum_NegativeUndefinedNumber_Test()
        {
            var parsed = "-4".TryParseEnum<NegativeTestEnum>(out var result);

            Assert.IsFalse(parsed);
            Assert.AreEqual(default(NegativeTestEnum), result);
        }

        [TestMethod]
        public void TryParseEnum_NonFlagsNameCombination_Test()
        {
            var parsed = "Second, Third".TryParseEnum<OrdinalTestEnum>(out var result);

            Assert.IsFalse(parsed);
            Assert.AreEqual(default(OrdinalTestEnum), result);
        }

        [TestMethod]
        public void TryParseEnum_NonFlagsNoZeroCombination_Test()
        {
            var parsed = "One, Two".TryParseEnum<NoZeroTestEnum>(out var result);

            Assert.IsFalse(parsed);
            Assert.AreEqual(0, (int)result);
        }

        [TestMethod]
        public void TryParseEnum_ByteEnumDefinedNumber_Test()
        {
            var parsed = "7".TryParseEnum<ByteBasedTestEnum>(out var result);

            Assert.IsTrue(parsed);
            Assert.AreEqual(ByteBasedTestEnum.Seven, result);
        }

        [TestMethod]
        public void TryParseEnum_ByteEnumOutOfRange_Test()
        {
            var parsed = "300".TryParseEnum<ByteBasedTestEnum>(out var result);

            Assert.IsFalse(parsed);
            Assert.AreEqual(default(ByteBasedTestEnum), result);
        }

        [TestMethod]
        public void TryParseEnum_LongEnumLargeNumber_Test()
        {
            var parsed = "2147483648".TryParseEnum<LongBasedTestEnum>(out var result);

            Assert.IsTrue(parsed);
            Assert.AreEqual(LongBasedTestEnum.Large, result);
        }

        [TestMethod]
        public void TryParseEnum_UIntEnumLargeNumber_Test()
        {
            var parsed = "3000000000".TryParseEnum<UIntBasedTestEnum>(out var result);

            Assert.IsTrue(parsed);
            Assert.AreEqual(UIntBasedTestEnum.Big, result);
        }

        [TestMethod]
        public void TryParseEnum_FlagsNameCombination_Test()
        {
            var parsed = "A, B".TryParseEnum<FlagsTestEnum>(out var result);

            Assert.IsTrue(parsed);
            Assert.AreEqual(FlagsTestEnum.A | FlagsTestEnum.B, result);
        }

        [TestMethod]
        public void TryParseEnum_FlagsNumberCombination_Test()
        {
            var parsed = "3".TryParseEnum<FlagsTestEnum>(out var result);

            Assert.IsTrue(parsed);
            Assert.AreEqual(FlagsTestEnum.A | FlagsTestEnum.B, result);
        }

        [TestMethod]
        public void TryParseEnum_FlagsCombinationWrongCase_Test()
        {
            var parsed = "a, b".TryParseEnum<FlagsTestEnum>(out var result);

            Assert.IsFalse(parsed);
            Assert.AreEqual(default(FlagsTestEnum), result);
        }

        [TestMethod]
        public void TryParseEnum_FlagsCombinationIgnoreCase_Test()
        {
            var parsed = "a, b".TryParseEnum<FlagsTestEnum>(out var result, true);

            Assert.IsTrue(parsed);
            Assert.AreEqual(FlagsTestEnum.A | FlagsTestEnum.B, result);
        }

        [TestMethod]
        public void TryParseEnum_FlagsUndeclaredBit_Test()
        {
            var parsed = "4".TryParseEnum<FlagsTestEnum>(out var result);

            Assert.IsFalse(parsed);
            Assert.AreEqual(default(FlagsTestEnum), result);
        }

        [TestMethod]
        public void TryParseEnum_FlagsNegativeNumber_Test()
        {
            var parsed = "-1".TryParseEnum<FlagsTestEnum>(out var result);

            Assert.IsFalse(parsed);
            Assert.AreEqual(default(FlagsTestEnum), result);
        }

        [TestMethod]
        public void TryParseEnum_FlagsZeroWithZeroMember_Test()
        {
            var parsed = "0".TryParseEnum<FlagsTestEnum>(out var result);

            Assert.IsTrue(parsed);
            Assert.AreEqual(FlagsTestEnum.None, result);
        }

        [TestMethod]
        public void TryParseEnum_FlagsZeroNameWithZeroMember_Test()
        {
            var parsed = "None".TryParseEnum<FlagsTestEnum>(out var result);

            Assert.IsTrue(parsed);
            Assert.AreEqual(FlagsTestEnum.None, result);
        }

        [TestMethod]
        public void TryParseEnum_FlagsZeroWithoutZeroMember_Test()
        {
            var parsed = "0".TryParseEnum<NoZeroFlagsTestEnum>(out var result);

            Assert.IsFalse(parsed);
            Assert.AreEqual(0, (int)result);
        }

        [TestMethod]
        public void TryParseEnum_FlagsNoZeroNameCombination_Test()
        {
            var parsed = "X, Z".TryParseEnum<NoZeroFlagsTestEnum>(out var result);

            Assert.IsTrue(parsed);
            Assert.AreEqual(NoZeroFlagsTestEnum.X | NoZeroFlagsTestEnum.Z, result);
        }

        [TestMethod]
        public void TryParseEnum_FlagsNoZeroAllBits_Test()
        {
            var parsed = "7".TryParseEnum<NoZeroFlagsTestEnum>(out var result);

            Assert.IsTrue(parsed);
            Assert.AreEqual(NoZeroFlagsTestEnum.X | NoZeroFlagsTestEnum.Y | NoZeroFlagsTestEnum.Z, result);
        }

        [TestMethod]
        public void TryParseEnum_FlagsNoZeroUndeclaredBit_Test()
        {
            var parsed = "9".TryParseEnum<NoZeroFlagsTestEnum>(out var result);

            Assert.IsFalse(parsed);
            Assert.AreEqual(0, (int)result);
        }

        [TestMethod]
        public void TryParseEnum_FlagsUnknownNameInList_Test()
        {
            var parsed = "X, W".TryParseEnum<NoZeroFlagsTestEnum>(out var result);

            Assert.IsFalse(parsed);
            Assert.AreEqual(0, (int)result);
        }

        [DataRow("First, Second")]
        [DataRow("Second, Second")]
        [DataRow("First, Third")]
        [TestMethod]
        public void TryParseEnum_NonFlagsListEqualToMember_Test(string value)
        {
            var parsed = value.TryParseEnum<OrdinalTestEnum>(out var result);

            Assert.IsFalse(parsed);
            Assert.AreEqual(default(OrdinalTestEnum), result);
        }

        [TestMethod]
        public void TryParseEnum_FlagsHighBitMember_Test()
        {
            var parsed = "-2147483648".TryParseEnum<HighBitFlagsTestEnum>(out var result);

            Assert.IsTrue(parsed);
            Assert.AreEqual(HighBitFlagsTestEnum.High, result);
        }

        [TestMethod]
        public void TryParseEnum_FlagsHighBitCombination_Test()
        {
            var parsed = "-2147483647".TryParseEnum<HighBitFlagsTestEnum>(out var result);

            Assert.IsTrue(parsed);
            Assert.AreEqual(HighBitFlagsTestEnum.High | HighBitFlagsTestEnum.Low, result);
        }

        [TestMethod]
        public void TryParseEnum_FlagsHighBitUndeclared_Test()
        {
            var parsed = "-2147483648".TryParseEnum<LowOnlyFlagsTestEnum>(out var result);

            Assert.IsFalse(parsed);
            Assert.AreEqual(0, (int)result);
        }

        [TestMethod]
        public void TryParseEnum_ULongFlagsTopBit_Test()
        {
            var parsed = "9223372036854775808".TryParseEnum<ULongFlagsTestEnum>(out var result);

            Assert.IsTrue(parsed);
            Assert.AreEqual(ULongFlagsTestEnum.Top, result);
        }

        [TestMethod]
        public void TryParseEnum_ULongFlagsTopCombination_Test()
        {
            var parsed = "9223372036854775809".TryParseEnum<ULongFlagsTestEnum>(out var result);

            Assert.IsTrue(parsed);
            Assert.AreEqual(ULongFlagsTestEnum.Top | ULongFlagsTestEnum.Low, result);
        }
    }
}
