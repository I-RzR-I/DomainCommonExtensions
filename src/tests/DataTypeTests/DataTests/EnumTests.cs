// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2022-08-16 03:15
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-16 04:14
// ***********************************************************************
//  <copyright file="EnumTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using DomainCommonExtensions.DataTypeExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace DataTypeTests.DataTests
{
    internal enum ResultEnum
    {
        [EnumMember(Value = "IDK")]
        Unknown,

        [System.ComponentModel.Description("Description-Valid")]
        [Display(Name = "Display-Valid")]
        Valid,

        [System.ComponentModel.Description("Description-InValid")]
        [Display(Name = "Display-InValid", Description = "InvalidDisplayDescription")]
        Invalid
    }

    [TestClass]
    public class EnumTests
    {
        [TestMethod]
        public void GetEnumDefinitionTest()
        {
            var res = typeof(ResultEnum).GetEnumDefinition();

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Any());
            Assert.IsTrue(res.First().Key == 0);
            Assert.IsTrue(res.First().Value == "Unknown");
        }

        [TestMethod]
        public void GetEnumMemberValueTest()
        {
            var resUnknown = ResultEnum.Unknown.GetEnumMemberValue();

            Assert.IsNotNull(resUnknown);
            Assert.AreEqual("IDK", resUnknown);

            var resValid = ResultEnum.Valid.GetEnumMemberValue();

            Assert.IsNull(resValid);
        }

        [TestMethod]
        public void ToEnumMemberValueTest()
        {
            var resUnknown = "IDK".ToEnumMemberValue<ResultEnum>();

            Assert.IsNotNull(resUnknown);
            Assert.IsTrue(resUnknown == ResultEnum.Unknown);
        }

        [TestMethod]
        public void ToIntTest()
        {
            var res = ResultEnum.Unknown.ToInt();

            Assert.IsNotNull(res);
            Assert.AreEqual(0, res);
        }

        [TestMethod]
        public void ToStringTest()
        {
            var res = ResultEnum.Unknown.ToString();

            Assert.IsNotNull(res);
            Assert.AreEqual("Unknown", res);
        }

        [TestMethod]
        public void GetDescriptionTest()
        {
            var resValid = ResultEnum.Valid.GetDescription();
            var resUnknown = ResultEnum.Unknown.GetDescription(true);
            var resUnknownAsIs = ResultEnum.Unknown.GetDescription();

            Assert.IsNotNull(resUnknown);
            Assert.AreEqual(string.Empty, resUnknown);

            Assert.IsNotNull(resUnknownAsIs);
            Assert.AreEqual("Unknown", resUnknownAsIs);

            Assert.IsNotNull(resValid);
            Assert.AreEqual("Description-Valid", resValid);
        }

        [TestMethod]
        public void GetEnumValue_String_Test()
        {
            var res = "Invalid".GetEnumValue<ResultEnum>();

            Assert.IsNotNull(res);
            Assert.IsTrue(res == ResultEnum.Invalid);
        }

        [TestMethod]
        public void GetEnumValue_Int_Test()
        {
            var res = 2.GetEnumValue<ResultEnum>();

            Assert.IsNotNull(res);
            Assert.IsTrue(res == ResultEnum.Invalid);
        }

        [TestMethod]
        public void GetDisplayNameTest()
        {
            var resInvalid = ResultEnum.Invalid.GetDisplayName();
            var resValid = ResultEnum.Valid.GetDisplayName();
            var resUnknown = ResultEnum.Unknown.GetDisplayName();

            Assert.IsNotNull(resInvalid);
            Assert.AreEqual("Display-InValid", resInvalid);

            Assert.IsNotNull(resValid);
            Assert.AreEqual("Display-Valid", resValid);

            Assert.IsNotNull(resUnknown);
            Assert.AreEqual("Unknown", resUnknown);
        }

        [TestMethod]
        public void GetDisplayDescriptionTest()
        {
            var resInvalid = ResultEnum.Invalid.GetDisplayDescription();
            var resValid = ResultEnum.Valid.GetDisplayDescription();
            var resUnknown = ResultEnum.Unknown.GetDisplayDescription();

            Assert.IsNotNull(resInvalid);
            Assert.AreEqual("InvalidDisplayDescription", resInvalid);

            Assert.IsNotNull(resValid);
            Assert.AreEqual("Valid", resValid);

            Assert.IsNotNull(resUnknown);
            Assert.AreEqual("Unknown", resUnknown);
        }
    }
}