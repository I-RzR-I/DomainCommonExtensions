// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2022-08-16 19:05
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-16 19:05
// ***********************************************************************
//  <copyright file="StringTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class StringTests
    {
        [TestMethod]
        public void SplitTest()
        {
            var str = "qaz#123";
            var result = str.Split("#");

            Assert.IsNotNull(result);
            Assert.AreEqual("qaz", result[0]);
            Assert.AreEqual("123", result[1]);
        }

        [TestMethod]
        public void FirstCharToUpperTest()
        {
            var input = "qaz";
            var output = input.FirstCharToUpper();

            Assert.IsNotNull(output);
            Assert.AreEqual("Q", output.Substring(0, 1));
        }

        [TestMethod]
        public void FirstCharToLowerTest()
        {
            var input = "QAZ";
            var output = input.FirstCharToLower();

            Assert.IsNotNull(output);
            Assert.AreEqual("q", output.Substring(0, 1));
        }

        [TestMethod]
        public void IsNullOrEmptyTest()
        {
            var input = "QAZ";
            var output = input.IsNullOrEmpty();
            var outputEmpty = "".IsNullOrEmpty();
            var outputNull = ((string)null).IsNullOrEmpty();

            Assert.IsFalse(output);
            Assert.IsTrue(outputEmpty);
            Assert.IsTrue(outputNull);
        }

        [TestMethod]
        public void IsValidEmailTest()
        {
            var em1 = "email1".IsValidEmail();
            var em2 = "email1@".IsValidEmail();
            var em3 = "email1@.de".IsValidEmail();
            var em4 = "email1@comp.de".IsValidEmail();

            Assert.IsFalse(em1);
            Assert.IsFalse(em2);
            Assert.IsFalse(em3);
            Assert.IsTrue(em4);
        }

        [TestMethod]
        public void ToSecureStringTest()
        {
            var source = "test";
            var result = source.ToSecureString();

            Assert.IsNotNull(result);
            Assert.AreEqual(source.Length, result.Length);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        public void ToSecureStringNullEmptyTest(string sourceString)
        {
            Assert.ThrowsException<ArgumentNullException>(sourceString.ToSecureString);
        }

        [TestMethod]
        public void StripHtmlTest()
        {
            var source = "<a>ss<a/>";
            var result = source.StripHtml();

            Assert.IsNotNull(result);
            Assert.AreEqual(" ss ", result);
        }

        [TestMethod]
        public void TruncateTest()
        {
            var source = "This is my PC!";
            var result = source.Truncate(10);
            var result2 = source.Truncate(10, true);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result2);
            Assert.AreEqual(10, result.Length);
            Assert.AreEqual(10, result2.Length);
            Assert.IsTrue(result2.EndsWith("..."));
        }

        [TestMethod]
        public void TruncateFromStartTest()
        {
            var source = "This is my PC!";
            var result = source.TruncateFromStart(10);
            var result2 = source.TruncateFromStart(10, true);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result2);
            Assert.AreEqual(10, result.Length);
            Assert.AreEqual(10, result2.Length);
            Assert.AreEqual(" is my PC!", result);
            Assert.AreEqual("... my PC!", result2);
            Assert.IsTrue(result2.StartsWith("..."));
        }

        [TestMethod]
        public void TruncateExactLengthTest()
        {
            var source = "This is my PC!";
            var result = source.TruncateExactLength(10);

            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Length);
        }

        [TestMethod]
        public void IsValidUrlTest()
        {
            var result = "http://ThisismyPC!".IsValidUrl();
            var result2 = "http://rzrdev.dev".IsValidUrl();

            Assert.IsFalse(result);
            Assert.IsTrue(result2);
        }

        [TestMethod]
        public void IsValidIpAddressTest()
        {
            var result = "127.1.1".IsValidIpAddress();
            var result2 = "127.0.0.0".IsValidIpAddress();

            Assert.IsFalse(result);
            Assert.IsTrue(result2);
        }

        [TestMethod]
        public void StringToHexadecimalTest()
        {
            string clearText = "HexaDecimal test";
            string resultHex = "48657861446563696D616C2074657374";
            var res = clearText.ToHexString();

            Assert.AreEqual(res, resultHex);
        }

        [TestMethod]
        public void StringToHexadecimalWithSpaceTest()
        {
            string clearText = "HexaDecimal test";
            string resultHex = "48 65 78 61 44 65 63 69 6D 61 6C 20 74 65 73 74";
            var res = clearText.ToHexString(true);

            Assert.AreEqual(res, resultHex);
        }

        [TestMethod]
        public void StringToHexadecimalWithSpaceTestNr2()
        {
            string clearText = "123456";
            string resultHex = "31 32 33 34 35 36";
            var res = clearText.ToHexString(true);

            Assert.AreEqual(res, resultHex);
        }

        [TestMethod]
        public void StringToHexadecimalWithSpaceTestNr3()
        {
            string clearText = "<4R3 3573 /\\/\\4/\\/\\4 74?";
            string resultHex = "3c 34 52 33 20 33 35 37 33 20 2f 5c 2f 5c 34 2f 5c 2f 5c 34 20 37 34 3f";
            var res = clearText.ToHexString(true);

            Assert.AreEqual(res, resultHex.ToUpper());
        }

        [TestMethod]
        public void HexadecimalToStringTest()
        {
            string clearText = "HexaDecimal test";
            string resultHex = "48657861446563696D616C2074657374";
            var res = resultHex.ToStringHex();

            Assert.AreEqual(res, clearText);
        }

        [TestMethod]
        public void HexadecimalToStringWithSpaceTest()
        {
            string clearText = "HexaDecimal test";
            string hexText = "48 65 78 61 44 65 63 69 6D 61 6C 20 74 65 73 74";
            var res = hexText.ToStringHex(true);

            Assert.AreEqual(res, clearText);
        }

        [TestMethod]
        public void HexadecimalToByte()
        {
            string strValue = "VisualStudio";
            string hexValue = strValue.ToHexString();
            byte[] btHexValue = hexValue.ToByteHex();
            string strBtHex = btHexValue.ToHexByte();
            string hexDecode = strBtHex.ToStringHex();

            Assert.AreEqual(hexValue, strBtHex);
            Assert.AreEqual(strValue, hexDecode);
        }

        [TestMethod]
        public void TrimIfNotNullTest()
        {
            string inpNull = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.IsTrue(string.IsNullOrEmpty(inpNull.TrimIfNotNull()));
        }

        [TestMethod]
        public void TrimIfNotNullEmptyValueTest()
        {
            var inpValue = " test ".TrimIfNotNull();

            Assert.IsTrue(inpValue.Length == 4);
            Assert.AreEqual("test", inpValue);
        }

        [TestMethod]
        public void TrimAndReduceSpaceTest()
        {
            var inpValue = " Input test data  ".TrimAndReduceSpace();

            Assert.IsTrue(inpValue.Length == 15);
            Assert.AreEqual("Input test data", inpValue);
        }

        [TestMethod]
        public void TrimAndReduceSpace_ReduceAllSpaces_Test()
        {
            var inpValue = " Input test data  ".TrimAndReduceSpace(true);

            Assert.IsTrue(inpValue.Length == 13);
            Assert.AreEqual("Inputtestdata", inpValue);
        }

        [TestMethod]
        public void TrimAndReduceSpaceNullTest()
        {
            string inpNull = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.IsTrue(string.IsNullOrEmpty(inpNull.TrimIfNotNull()));
        }

        [TestMethod]
        public void IsValidEmailWithMailAddressTest()
        {
            var em1 = "email1".IsValidEmailWithMailAddress();
            var em2 = "email1@".IsValidEmailWithMailAddress();
            var em3 = "email1@.de".IsValidEmailWithMailAddress();
            var em4 = "email1@comp.de".IsValidEmailWithMailAddress();

            Assert.IsFalse(em1);
            Assert.IsFalse(em2);
            Assert.IsFalse(em3);
            Assert.IsTrue(em4);
        }

        [TestMethod]
        public void GetFileNameAndExtenstionTest()
        {
            var result = "FileName.current.pf".GetFileNameAndExtenstion();
            var result2 = "FileNamecurrentpf".GetFileNameAndExtenstion();

            Assert.IsNotNull(result);
            Assert.AreEqual("FileName.current", result[0]);
            Assert.AreEqual("pf", result[1]);

            Assert.IsNotNull(result2);
            Assert.AreEqual("FileNamecurrentpf", result2[0]);
            Assert.AreEqual("FileNamecurrentpf", result2[1]);
        }

        [TestMethod]
        public void Base64EncodeDecodeTest()
        {
            var clear = "Clear text test 001?!";
            var encoded = "Q2xlYXIgdGV4dCB0ZXN0IDAwMT8h";

            var encodedTest = clear.Base64Encode();

            Assert.IsNotNull(encodedTest);
            Assert.AreEqual(encoded, encodedTest);

            var decodedTest = encodedTest.Base64Decode();

            Assert.IsNotNull(decodedTest);
            Assert.AreEqual(clear, decodedTest);
        }

        [TestMethod]
        public void FrontAppendMoveUpInDirectoryTest()
        {
            var input = "c:\\user\\ab\\music\\atb";

            var preAppend = input.FrontAppendMoveUpInDirectory(2);
            var postAppend = input.MoveUpInDirectoryBackAppend(2);

            Assert.IsNotNull(preAppend);
            Assert.IsNotNull(postAppend);

            Assert.IsTrue(preAppend.StartsWith(".."));
            Assert.IsTrue(postAppend.EndsWith("..\\"));
        }

        [TestMethod]
        public void FormatToDateTest()
        {
            var date1 = "20-01-01".FormatToDate();
            var date2 = "2020-01-01".FormatToDate();
            var date3 = "20203131".FormatToDate();
            var date4 = "20201231".FormatToDate();

            Assert.IsNull(date1);

            Assert.IsNotNull(date2);
            Assert.AreEqual(new DateTime(2020, 1, 1).Date, date2?.Date);

            Assert.IsNull(date3);

            Assert.IsNotNull(date4);
            Assert.AreEqual(new DateTime(2020, 12, 31).Date, date4?.Date);
        }

        [TestMethod]
        public void RemoveSpecialCharsTest()
        {
            var test = "".RemoveSpecialChars();
            var test2 = "select*from x where x.Id='1'".RemoveSpecialChars();

            Assert.IsNotNull(test);
            Assert.AreEqual("", test);

            Assert.IsNotNull(test2);
            Assert.AreEqual("selectfrom x where xId=1", test2);
        }

        [TestMethod]
        public void TrimAndReplaceSpecialCharactersTest()
        {
            var test2 = "select*from x where x.Id='1'    ".TrimAndReplaceSpecialCharacters();

            Assert.IsNotNull(test2);
            Assert.AreEqual("selectfrom x where xId=1", test2);
        }

        [TestMethod]
        public void TrimEndCharAndReduceLastWhiteSpaceTest()
        {
            var test2 = "select*from x where x.Id='1'    ".TrimEndCharAndReduceLastWhiteSpace('\'');

            Assert.IsNotNull(test2);
            Assert.AreEqual("select*from x where x.Id='1", test2);
        }

        [TestMethod]
        public void GetHashSha1CryptoProvTest()
        {
            var test2 = "TruestInMe".GetHashSha1CryptoProv();

            Assert.IsNotNull(test2);
        }

        [TestMethod]
        public void GetHashSha256CryptoProvTest()
        {
            var test2 = "TruestInMe".GetHashSha256CryptoProv();

            Assert.IsNotNull(test2);
        }

        [TestMethod]
        public void GetHashSha256Test()
        {
            var test2 = "TruestInMe".GetHashSha256();

            Assert.IsNotNull(test2);
        }

        [TestMethod]
        public void GetHashSha256StringTest()
        {
            var test2 = "TruestInMe".GetHashSha256String();

            Assert.IsNotNull(test2);
            Assert.AreEqual("9873FEA0657F37F052B6EE55DAF54E5B9C4F9DF994138DD4C0A3F12D6ECD1ACE", test2);
        }

        [TestMethod]
        public void IsBase64StringTest()
        {
            var clear = "Clear text test 001?!";
            var b64 = "Q2xlYXIgdGV4dCB0ZXN0IDAwMT8h";

            var clearTest = clear.IsBase64String();
            var b64Test = b64.IsBase64String();

            Assert.IsTrue(b64Test);
            Assert.IsFalse(clearTest);
        }

        [TestMethod]
        public void ReplaceSpecialCharactersTest()
        {
            var input = "Ședința de judecată";
            var output = "Sedinta de judecata";

            var result = input.ReplaceSpecialCharacters();

            Assert.IsNotNull(result);
            Assert.AreEqual(output, result);
        }

        [TestMethod]
        public void NextTest()
        {
            var input1 = "AB";
            var input2 = "AZ";
            var output1 = "AC";
            var output2 = "BA";

            var result1 = input1.Next();
            var result2 = input2.Next();

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.AreEqual(output1, result1);
            Assert.AreEqual(output2, result2);
        }

        [TestMethod]
        public void Next2Test()
        {
            var input1 = "ABCDE";

            var result1 = input1.Next(2);
            var result2 = input1.Next(3);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.AreEqual("ABCDG", result1);
            Assert.AreEqual("ABCDH", result2);
        }

        [TestMethod]
        public void FromUnicodeTest()
        {
            var input = "\u00DC";
            var output = "Ü";

            var result = input.FromUnicode();

            Assert.IsNotNull(result);
            Assert.AreEqual(output, result);
        }

        [TestMethod]
        public void TryParseGuidNullableTest()
        {
            var result = "".TryParseGuidNullable();
            var result2 = "12".TryParseGuidNullable();
            var result3 = "1d162b4d-4f50-4008-8f8a-51a5c828e0c9".TryParseGuidNullable();

            Assert.IsNull(result);
            Assert.IsNull(result2);
            Assert.IsNotNull(result3);
        }

        [TestMethod]
        public void UppercaseWordsTest()
        {
            var result = "".UppercaseWords();
            var result2 = "As da Dw".UppercaseWords();

            Assert.AreEqual(string.Empty, result);
            Assert.AreEqual("As Da Dw", result2);
        }

        [TestMethod]
        public void SimpleCompareTwoStringTest()
        {
            var result = "".SimpleCompareTwoString("");
            var result2 = "Rz".SimpleCompareTwoString("RzR");

            Assert.AreEqual(100, result);
            Assert.AreEqual("66.667", $"{result2:00.000}");
        }

        [TestMethod]
        public void ValidatePullTest()
        {
            var result = "12".ValidatePull(new[] { "10", "11", "12", "15" });

            Assert.IsTrue(result);
        }

        [DataRow("{ \"key\": \"value\" }")]
        [DataRow("[{ \"key\": \"value\" }]")]
        [DataRow("{ \"key\": 1 }")]
        [DataRow("[{ \"key\": 1 }]")]
        [TestMethod]
        public void IsValidJson_Success_Test(string source)
        {
            var result = source.IsValidJson();

            Assert.IsTrue(result);
        }

        [DataRow("{ \"key\": \"value\"")]
        [DataRow("{ \"key\": \"value\"")]
        [DataRow("[{ key: \"value\" }]")]
        [DataRow("{ \"key\": 1 }]")]
        [DataRow("\"key\": 1")]
        [TestMethod]
        public void IsValidJson_Fail_Test(string source)
        {
            var result = source.IsValidJson();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ToStringArray_Test()
        {
            var source = "abcde";
            var result = source.ToStringArray();

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Length);
            Assert.AreEqual("b", result[1]);
            Assert.AreEqual("e", result[4]);
        }

        [TestMethod]
        public void ArrayToString_Test()
        {
            var source = new string[2] { "a", "b" };
            var result = source.ArrayToString();

            Assert.IsNotNull(result);
            Assert.AreEqual("ab", result);
        }

        [DataRow("$FFFFFF", "$", "$FFFFFF")]
        [DataRow("FFFFFF", "$", "FFFFFF")]
        [TestMethod]
        public void IfContains_Test(string source, string checkValue, string exceptedResult)
        {
            var res = source.IfContains(checkValue, exceptedResult);

            Assert.IsNotNull(res);
            Assert.AreEqual(exceptedResult, res);
        }

        [DataRow("$FFFFFF", "$", "$FFFFFF")]
        [DataRow("FFFFFF", "$", "$FFFFFF")]
        [TestMethod]
        public void IfNotContains_Test(string source, string checkValue, string exceptedResult)
        {
            var res = source.IfNotContains(checkValue, exceptedResult);

            Assert.IsNotNull(res);
            Assert.AreEqual(exceptedResult, res);
        }

        [TestMethod]
        public void IsBase32StringTest()
        {
            var clear = "Clear text test 001?!";
            var b32 = "INWGKYLSEB2GK6DUEB2GK43UEAYDAMJ7EE======";

            var clearTest = clear.IsBase32String();
            var b32Test = b32.IsBase32String();

            Assert.IsTrue(b32Test);
            Assert.IsFalse(clearTest);
        }

        [DataRow("INWGKYLSEB2GK6DUEB2GK43UEAYDAMJ7EE======")]
        [TestMethod]
        public void Base32StringToByteAndBack_Test(string base32String)
        {
            var byteArray = base32String.Base32ToBytes();
            Assert.IsNotNull(byteArray);

            var decoded = byteArray.Base32BytesToString();
            Assert.IsNotNull(decoded);
            Assert.AreEqual(base32String, decoded);
        }

        [DataRow("Clear text test 001?!", "INWGKYLSEB2GK6DUEB2GK43UEAYDAMJ7EE======")]
        [DataRow("test", "ORSXG5A=")]
        [DataRow("TEST", "KRCVGVA=")]
        [DataRow("T3$t0`!-gg", "KQZSI5BQMAQS2Z3H")]
        [TestMethod]
        public void Base32Encode_Test(string clear, string encoded)
        {
            var encodeResult = clear.Base32Encode();
            Assert.IsNotNull(encodeResult);
            Assert.IsTrue(encodeResult.IsBase32String());
            Assert.AreEqual(encoded, encodeResult);
        }

        [DataRow("Clear text test 001?!", "INWGKYLSEB2GK6DUEB2GK43UEAYDAMJ7EE")]
        [DataRow("test", "ORSXG5A")]
        [DataRow("TEST", "KRCVGVA")]
        [DataRow("T3$t0`!-gg", "KQZSI5BQMAQS2Z3H")]
        [TestMethod]
        public void Base32Encode_WithOutPadding_Test(string clear, string encoded)
        {
            var encodeResult = clear.Base32Encode(false);
            Assert.IsNotNull(encodeResult);
            Assert.IsTrue(encodeResult.IsBase32String(false));
            Assert.AreEqual(encoded, encodeResult);
        }

        [DataRow("INWGKYLSEB2GK6DUEB2GK43UEAYDAMJ7EE======", "Clear text test 001?!")]
        [DataRow("ORSXG5A=", "test")]
        [DataRow("KRCVGVA=", "TEST")]
        [DataRow("KQZSI5BQMAQS2Z3H", "T3$t0`!-gg")]
        [TestMethod]
        public void Base32Decode_Test(string encoded, string clear)
        {
            var decodedResult = encoded.Base32Decode();
            Assert.IsNotNull(decodedResult);
            Assert.AreEqual(clear, decodedResult);
        }

        [DataRow("INWGKYLSEB2GK6DUEB2GK43UEAYDAMJ7EE", "Clear text test 001?!")]
        [DataRow("ORSXG5A", "test")]
        [DataRow("KRCVGVA", "TEST")]
        [DataRow("KQZSI5BQMAQS2Z3H", "T3$t0`!-gg")]
        [TestMethod]
        public void Base32Decode_WithOutPadding_Test(string encoded, string clear)
        {
            var decodedResult = encoded.Base32Decode();
            Assert.IsNotNull(decodedResult);
            Assert.AreEqual(clear, decodedResult);
        }

        [DataRow(null)]
        [DataRow("null")]
        [DataRow("")]
        [TestMethod]
        public void NotAllowedEmpty_Test(string source)
        {
            if (source.IsNull())
                Assert.ThrowsException<ArgumentNullException>(() => source.NotAllowedEmpty(nameof(source)));
            else if (source.IsEmpty())
                Assert.ThrowsException<ArgumentException>(() => source.NotAllowedEmpty(nameof(source)));
            else
            {
                var result = source.NotAllowedEmpty(nameof(source));

                Assert.IsNotNull(result);
                Assert.AreEqual(source, result);
            }
        }

        [DataRow("10", ".", "10.")]
        [DataRow("Test", ";", "Test;")]
        [DataRow("Test.", ".", "Test.")]
        [DataRow("Test.", ";", "Test.;")]
        [TestMethod]
        public void AddPeriod_Test(string sourceValue, string delimiter, string exceptedValue)
        {
            var result = sourceValue.AddPeriod(delimiter);

            Assert.IsNotNull(result);
            Assert.AreEqual(exceptedValue, result);
        }

        [DataRow("10.", ".", "10")]
        [DataRow("Test;", ";", "Test")]
        [DataRow("Test;Test2", ";", "Test;Test2")]
        [TestMethod]
        public void RemovePeriod_Test(string sourceValue, string delimiter, string exceptedValue)
        {
            var result = sourceValue.RemovePeriod(delimiter);

            Assert.IsNotNull(result);
            Assert.AreEqual(exceptedValue, result);
        }

        [DataRow("10", ".", "", "10.00")]
        [DataRow("10.", ".", "99", "10.99")]
        [DataRow("10", ".", "25", "10.25")]
        [DataRow("Test", ";", "zas", "Test;zas")]
        [DataRow("Test.", ".", " ", "Test. ")]
        [TestMethod]
        public void AddPeriodValue_Test(string sourceValue, string delimiter, string periodValue, string exceptedValue)
        {
            var result = sourceValue.AddPeriodValue(delimiter, periodValue);

            Assert.IsNotNull(result);
            Assert.AreEqual(exceptedValue, result);
        }

        [DataRow("10.00", ".", "00", "10")]
        [DataRow("10.25", ".", "25", "10")]
        [DataRow("Test;zas", ";", "zas", "Test")]
        [DataRow("Test. ", ".", " ", "Test")]
        [TestMethod]
        public void RemovePeriodValue_Test(string sourceValue, string delimiter, string periodValue, string exceptedValue)
        {
            var result = sourceValue.RemovePeriodValue(delimiter, periodValue);

            Assert.IsNotNull(result);
            Assert.AreEqual(exceptedValue, result);
        }

        [DataRow("", false)]
        [DataRow("aSw", false)]
        [DataRow("asw", false)]
        [DataRow("ASW", true)]
        [TestMethod]
        public void IsAllUpperCase_Test(string sourceValue, bool exceptedResult)
        {
            var result = sourceValue.IsAllUpperCase();

            Assert.IsNotNull(result);
            Assert.AreEqual(exceptedResult, result);
        }

        [DataRow("", false)]
        [DataRow("aSw", false)]
        [DataRow("asw", true)]
        [DataRow("ASW", false)]
        [TestMethod]
        public void IsAllLowerCase_Test(string sourceValue, bool exceptedResult)
        {
            var result = sourceValue.IsAllLowerCase();

            Assert.IsNotNull(result);
            Assert.AreEqual(exceptedResult, result);
        }

        [DataRow("", false)]
        [DataRow("aSw", true)]
        [DataRow("asw", true)]
        [DataRow("ASW", true)]
        [DataRow("a05dw", false)]
        [TestMethod]
        public void IsAllLetters_Test(string sourceValue, bool exceptedResult)
        {
            var result = sourceValue.IsAllLetters();

            Assert.IsNotNull(result);
            Assert.AreEqual(exceptedResult, result);
        }

        [DataRow("sw13dd21d3", "sw13dd21d3")]
        [DataRow("Test!23", "Test23")]
        [DataRow("", "")]
        [DataRow("T3st!23 ", "T3st23 ")]
        [TestMethod]
        public void CleanTextToLettersNumbersAndSpace_Test(string sourceValue, string exceptedValue)
        {
            var result = sourceValue.CleanTextToLettersNumbersAndSpace();

            Assert.IsNotNull(result);
            Assert.AreEqual(exceptedValue, result);
        }
    }
}