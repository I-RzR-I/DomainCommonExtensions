// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DataTypeTests
//  Author           : RzR
//  Created On       : 2025-06-27 15:36
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-06-27 15:36
// ***********************************************************************
//  <copyright file="DataTableExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System;
using System.Data;
using DataTypeTests.Models;
using DomainCommonExtensions.CommonExtensions.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DataTableExtensionsTests
    {
        [TestMethod]
        public void DataTable_T_ToJson_Test()
        {
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("Id", typeof(int)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("IsActive", typeof(bool)));

            var dr1 = dt.NewRow();
            dr1[0] = 1;
            dr1[1] = "name1";
            dr1[2] = true;
            dt.Rows.Add(dr1);

            var dr2 = dt.NewRow();
            dr2[0] = 2;
            dr2[1] = "name2";
            dr2[2] = false;
            dt.Rows.Add(dr2);

            var json = dt.ToJson<IdNameActiveModel>();

            Assert.IsNotNull(json);
        }

        [TestMethod]
        public void DataTable_ToJson_Test()
        {
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("Id", typeof(int)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("IsActive", typeof(bool)));
            dt.Columns.Add(new DataColumn("CreatedOn", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("DeletedOn", typeof(DateTime)));

            var dr1 = dt.NewRow();
            dr1[0] = 1;
            dr1[1] = "name1";
            dr1[2] = true;
            dr1[3] = DateTime.MinValue;
            dr1[4] = DBNull.Value;
            dt.Rows.Add(dr1);

            var dr2 = dt.NewRow();
            dr2[0] = 2;
            dr2[1] = "name2";
            dr2[2] = false;
            dr2[3] = DateTime.MaxValue;
            dr2[4] = DateTime.Now;
            dt.Rows.Add(dr2);

            var json = dt.ToJson();

            Assert.IsNotNull(json);
        }
    }
}