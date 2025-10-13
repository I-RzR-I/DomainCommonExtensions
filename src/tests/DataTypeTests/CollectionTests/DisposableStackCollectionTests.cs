// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2025-10-13 18:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-10-13 18:39
// ***********************************************************************
//  <copyright file="DisposableStackCollectionTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using DomainCommonExtensions.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace DataTypeTests.CollectionTests
{
    [TestClass]
    public class DisposableStackCollectionTests
    {
        [TestMethod]
        public void DisposableStackCollection_Test()
        {
            var data = new DisposableStackCollection();
            data.Add(new Temp.XClass());
            data.Add(new Temp.YClass());
            data.Add(new Temp.ZClass());

            Assert.IsNotNull(data);
            Assert.AreEqual(3, data.Length);

            var x = data.Get<Temp.XClass>(2);
            Assert.IsNotNull(x);
            Assert.IsInstanceOfType(x, typeof(Temp.XClass));
            Assert.AreEqual(3, data.Length);

            var y = data.Get<Temp.YClass>(1);
            Assert.IsNotNull(y);
            Assert.IsInstanceOfType(y, typeof(Temp.YClass));
            Assert.AreEqual(3, data.Length);
            
            var y10 = data.GetOrDefault<Temp.YClass>(10);
            Assert.IsNull(y10);
            Assert.AreEqual(3, data.Length);

            var peekZ = data.Peek<Temp.ZClass>();
            Assert.IsNotNull(peekZ);
            Assert.AreEqual(3, data.Length);

            var popZ = data.Pop<Temp.ZClass>();
            Assert.IsNotNull(popZ);
            Assert.AreEqual(2, data.Length);

            data.Clear();
            Assert.IsNotNull(data);
            Assert.AreEqual(0, data.Length);

            data.Add(new Temp.XClass());
            Assert.AreEqual(1, data.Length);

            data.Dispose();
            Assert.IsNotNull(data);
            Assert.AreEqual(0, data.Length);
        }
    }

    internal static class Temp
    {
        internal class XClass : IDisposable
        {
            /// <inheritdoc />
            public void Dispose()
            {
            }
        }

        internal class YClass : IDisposable
        {
            /// <inheritdoc />
            public void Dispose()
            {
            }
        }
        internal class ZClass : IDisposable
        {
            /// <inheritdoc />
            public void Dispose()
            {
            }
        }
    }
}