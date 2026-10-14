using System;
using System.Data;

namespace DataTypeTests.TestHelpers
{
    internal static class DataTableConversionTestHelper
    {
        internal static DataTable SingleCell(string columnName, Type columnType, object value)
        {
            var table = new DataTable();
            table.Columns.Add(columnName, columnType);
            table.Rows.Add(value ?? DBNull.Value);

            return table;
        }
    }

    internal class DataTableEnumTestModel
    {
        public DayOfWeek Day { get; set; }
    }

    internal class DataTableGuidTestModel
    {
        public Guid Key { get; set; }
    }

    internal class DataTableDoubleTestModel
    {
        public double Amount { get; set; }
    }

    internal class DataTableIntTestModel
    {
        public int Count { get; set; }
    }

    internal class DataTableMissingColumnTestModel
    {
        public int Id { get; set; }

        public string Missing { get; set; }
    }

    internal class DataTableGetOnlyTestModel
    {
        public int Id { get; set; }

        public int Doubled => Id * 2;
    }

    internal class DataTableIndexerTestModel
    {
        private string _item;

        public int Id { get; set; }

        public string this[string key]
        {
            get => _item;
            set => _item = value;
        }
    }

    internal class DataTablePrivateSetterTestModel
    {
        public int Id { get; private set; }
    }

    internal class DataTableHiddenBaseTestModel
    {
        public int Code { get; set; }
    }

    internal class DataTableHiddenDerivedTestModel : DataTableHiddenBaseTestModel
    {
        public new string Code { get; set; }
    }

    internal class DataTableNullableRowTestModel
    {
        public int Count { get; set; }

        public string Name { get; set; }

        public DateTime When { get; set; }

        public int? Maybe { get; set; }

        public Guid Key { get; set; }
    }
}
