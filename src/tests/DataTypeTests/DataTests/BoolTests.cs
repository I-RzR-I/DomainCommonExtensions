// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2022-08-14 19:18
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-15 19:39
// ***********************************************************************
//  <copyright file="BoolTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Diagnostics;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Text;
#endregion

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class BoolTests
    {
        [TestMethod]
        public void NegateTest()
        {
            const bool tr = true;
            var neg = tr.Negate();

            Assert.IsTrue(neg.Equals(false));
        }

        [TestMethod]
        public void NegateNullTest()
        {
            var neg = ((bool?)null).Negate();

            Assert.IsTrue(neg.Equals(true));
        }
    }
}