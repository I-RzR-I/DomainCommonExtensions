// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2025-01-03 19:33
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-01-03 19:33
// ***********************************************************************
//  <copyright file="DynamicListTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using DataTypeTests.Models;
using DomainCommonExtensions.ArraysExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable ArrangeObjectCreationWhenTypeEvident

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DynamicListTests
    {
        private ICollection<TempModel> _sourceData;

        [TestInitialize]
        public void Init()
            => _sourceData = new List<TempModel>()
            {
                new TempModel()
                {
                    Id = 0,
                    Code = "Code-000",
                    Name = "Name-000",
                    IsActive = true,
                    DeletedAt = null,
                    CreatedAt = DateTime.Now.AddDays(-10),
                    Version = 1,
                    Price = null,
                    RecordType = RecordType.Undefined,
                    DblValue1 = 22.22,
                    DblValue2 = 22.22
                },
                new TempModel()
                {
                    Id = 1,
                    Code = "Code-001",
                    Name = "Name-001",
                    IsActive = false,
                    DeletedAt = DateTime.Now.Date,
                    CreatedAt = DateTime.Now.AddDays(-20),
                    Version = (decimal)1.5,
                    Price = 10,
                    RecordType = RecordType.Primary,
                    DblValue1 = 0,
                    DblValue2 = 0
                },
                new TempModel()
                {
                    Id = 2,
                    Code = "Code-002",
                    Name = "Name-002",
                    IsActive = true,
                    DeletedAt = null,
                    CreatedAt = DateTime.Now.AddDays(-30),
                    Version = (decimal)2.8,
                    Price = (decimal)10.55,
                    RecordType = RecordType.Secondary,
                    DblValue1 = 10.99,
                    DblValue2 = 10.99
                },
                new TempModel()
                {
                    Id = 3,
                    Code = "Code-003",
                    Name = "Name-003",
                    IsActive = false,
                    DeletedAt = DateTime.Now.Date,
                    CreatedAt = DateTime.Now.AddDays(-40),
                    Version = 9,
                    Price = (decimal)200.99,
                    DblValue1 = 99.99,
                    DblValue2 = null
                }
            };

        #region OLD Using System.Linq.Dynamic.Core

        /*
        //  Using System.Linq.Dynamic.Core
        [TestMethod]
        public void ParseListOfTInDynamicTest()
        {
            var temp = _sourceData.ToList();

            var selected = temp.ParseListOfTInDynamic(new[]
            {
                nameof(TempModel.Id),
                nameof(TempModel.Code)
            });

            Assert.IsNotNull(selected);
            Assert.AreEqual(_sourceData.Count, selected.Count);
            Assert.AreEqual(_sourceData.First().Id, selected.First().Id);
            Assert.AreEqual(_sourceData.First().Code, selected.First().Code);
        }
        */

        /*
        //  Using System.Linq.Dynamic.Core
        [TestMethod]
        public void ParseEnumerableOfTInDynamicTest()
        {
            var temp = (IEnumerable<TempModel>)_sourceData;

            var selected = temp.ParseEnumerableOfTInDynamic(new[]
            {
                nameof(TempModel.Id),
                nameof(TempModel.Code)
            });

            Assert.IsNotNull(selected);
            Assert.AreEqual(_sourceData.Count, selected.Count);
            Assert.AreEqual(_sourceData.First().Id, selected.First().Id);
            Assert.AreEqual(_sourceData.First().Code, selected.First().Code);
        }
        */

        #endregion

        [TestMethod]
        public void SelectPropertyTest()
        {
            var temp = _sourceData.AsQueryable();

            //             .Select(x => x.Id)
            var selected = temp.SelectProperty(nameof(TempModel.Id));

            Assert.IsNotNull(selected);

            var items = selected.Cast<int>().ToList();

            Assert.IsNotNull(items);
            Assert.AreEqual(_sourceData.Count, items.Count);
            Assert.AreEqual(_sourceData.First().Id, items.First());
        }

        [TestMethod]
        public void SelectMultiplePropertyTest()
        {
            var temp = _sourceData.AsQueryable();

            //             .Select(x => new {x.Id, x.Code})
            var selected = temp.SelectMultipleProperties(nameof(TempModel.Id), nameof(TempModel.Code));

            Assert.IsNotNull(selected);

            var items = selected.Cast<dynamic>().ToList();

            Assert.IsNotNull(items);
            Assert.AreEqual(_sourceData.Count, items.Count);
            Assert.AreEqual(_sourceData.First().Id, ((TempModel)items.First()).Id);
        }

        [TestMethod]
        public void ParseListOfTInDynamic_Local_Test()
        {
            var temp = _sourceData.ToList();

            var selected = temp.ParseListOfTInDynamic(new[]
            {
                nameof(TempModel.Id),
                nameof(TempModel.Code)
            });

            Assert.IsNotNull(selected);
            Assert.AreEqual(_sourceData.Count, selected.Count);

            var sourceFirst = _sourceData.First();
            var selectFirst = selected.First();

            Assert.IsNotNull(sourceFirst);
            Assert.IsNotNull(selectFirst);
            Assert.AreEqual(sourceFirst.Id, selectFirst.Id);
            Assert.AreEqual(sourceFirst.Code, selectFirst.Code);
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_Local_Test()
        {
            var temp = _sourceData.ToList();

            var selected = temp.ParseEnumerableOfTInDynamic(new[]
            {
                nameof(TempModel.Id),
                nameof(TempModel.Code)
            });

            Assert.IsNotNull(selected);
            Assert.AreEqual(_sourceData.Count, selected.Count);

            var sourceFirst = _sourceData.First();
            var selectFirst = selected.First();

            Assert.IsNotNull(sourceFirst);
            Assert.IsNotNull(selectFirst);
            Assert.AreEqual(sourceFirst.Id, selectFirst.Id);
            Assert.AreEqual(sourceFirst.Code, selectFirst.Code);
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_Local_SelectNullableProp_Test()
        {
            var temp = _sourceData.ToList();

            var selected = temp.ParseEnumerableOfTInDynamic(new[]
            {
                nameof(TempModel.Id),
                nameof(TempModel.Code),
                nameof(TempModel.CreatedAt),
                nameof(TempModel.DeletedAt)
            });

            Assert.IsNotNull(selected);
            Assert.AreEqual(_sourceData.Count, selected.Count);

            var sourceFirst = _sourceData.First();
            var selectFirst = selected.First();

            Assert.IsNotNull(sourceFirst);
            Assert.IsNotNull(selectFirst);
            Assert.AreEqual(sourceFirst.Id, selectFirst.Id);
            Assert.AreEqual(sourceFirst.Code, selectFirst.Code);
            Assert.AreEqual(sourceFirst.DeletedAt, selectFirst.DeletedAt);
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_Local_SelectEnumProp_Test()
        {
            var temp = _sourceData.ToList();

            var selected = temp.ParseEnumerableOfTInDynamic(new[]
            {
                nameof(TempModel.Id),
                nameof(TempModel.Code),
                nameof(TempModel.RecordType)
            });

            Assert.IsNotNull(selected);
            Assert.AreEqual(_sourceData.Count, selected.Count);

            var sourceFirst = _sourceData.First();
            var selectFirst = selected.First();

            Assert.IsNotNull(sourceFirst);
            Assert.IsNotNull(selectFirst);
            Assert.AreEqual(sourceFirst.Id, selectFirst.Id);
            Assert.AreEqual(sourceFirst.Code, selectFirst.Code);
            Assert.AreEqual(sourceFirst.RecordType, selectFirst.RecordType);
        }

        [TestMethod]
        public void ParseEnumerableOfTInDynamic_Local_SelectDoubleProp_Test()
        {
            var temp = _sourceData.ToList();

            var selected = temp.ParseEnumerableOfTInDynamic(new[]
            {
                nameof(TempModel.DblValue2)
            });

            Assert.IsNotNull(selected);
            Assert.AreEqual(_sourceData.Count, selected.Count);

            var sourceFirst = _sourceData.First();
            var selectFirst = selected.First();

            Assert.IsNotNull(sourceFirst);
            Assert.IsNotNull(selectFirst);
            Assert.AreEqual(sourceFirst.DblValue2, selectFirst.DblValue2);
        }
    }
}