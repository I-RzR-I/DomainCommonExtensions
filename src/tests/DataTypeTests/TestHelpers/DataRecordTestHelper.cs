using System;
using System.Data;

namespace DataTypeTests.TestHelpers
{
    internal static class DataRecordTestHelper
    {
        internal const string IdColumn = "Id";

        internal static DataTableReader SingleRow(params (string Name, Type Type, object Value)[] columns)
        {
            var table = new DataTable();
            foreach (var column in columns)
                table.Columns.Add(column.Name, column.Type);

            var row = table.NewRow();
            for (var i = 0; i < columns.Length; i++)
                row[i] = columns[i].Value ?? DBNull.Value;
            table.Rows.Add(row);

            var reader = table.CreateDataReader();
            reader.Read();

            return reader;
        }

        internal static DataTableReader IdRows(params int[] ids)
        {
            return new DataTableReader(CreateIdTable(ids));
        }

        internal static DataTableReader IdResultSets(int[] first, int[] second)
        {
            return new DataTableReader(new[] { CreateIdTable(first), CreateIdTable(second) });
        }

        private static DataTable CreateIdTable(int[] ids)
        {
            var table = new DataTable();
            table.Columns.Add(IdColumn, typeof(int));
            foreach (var id in ids)
                table.Rows.Add(id);

            return table;
        }
    }
}
