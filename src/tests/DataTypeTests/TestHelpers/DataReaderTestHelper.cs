using System;
using System.Data;

namespace DataTypeTests.TestHelpers
{
    internal static class DataReaderTestHelper
    {
        internal const string ColumnName = "Value";

        internal static IDataReader SingleValue(Type columnType, object value)
        {
            var table = new DataTable();
            table.Columns.Add(ColumnName, columnType);

            return ReadFirstRow(table, value);
        }

        internal static IDataReader SingleUtcDateTime(DateTime value)
        {
            var table = new DataTable();
            var column = table.Columns.Add(ColumnName, typeof(DateTime));
            column.DateTimeMode = DataSetDateTime.Utc;

            return ReadFirstRow(table, value);
        }

        private static IDataReader ReadFirstRow(DataTable table, object value)
        {
            var row = table.NewRow();
            row[ColumnName] = value ?? DBNull.Value;
            table.Rows.Add(row);

            var reader = table.CreateDataReader();
            reader.Read();

            return reader;
        }
    }
}
