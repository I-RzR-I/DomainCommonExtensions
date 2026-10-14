using System;
using System.Data;

namespace DataTypeTests.TestHelpers
{
    internal sealed class FakeDataRecordTestHelper : IDataRecord
    {
        private readonly string[] _names;
        private readonly object[] _values;

        internal FakeDataRecordTestHelper(string[] names, object[] values)
        {
            _names = names;
            _values = values;
        }

        public int FieldCount => _names.Length;

        public object this[int i] => _values[i];

        public object this[string name] => throw new NotSupportedException();

        public string GetName(int i) => _names[i];

        public object GetValue(int i) => _values[i];

        public bool IsDBNull(int i) => _values[i] is DBNull;

        public int GetOrdinal(string name) => throw new NotSupportedException();

        public bool GetBoolean(int i) => throw new NotSupportedException();

        public byte GetByte(int i) => throw new NotSupportedException();

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
            => throw new NotSupportedException();

        public char GetChar(int i) => throw new NotSupportedException();

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
            => throw new NotSupportedException();

        public IDataReader GetData(int i) => throw new NotSupportedException();

        public string GetDataTypeName(int i) => throw new NotSupportedException();

        public DateTime GetDateTime(int i) => throw new NotSupportedException();

        public decimal GetDecimal(int i) => throw new NotSupportedException();

        public double GetDouble(int i) => throw new NotSupportedException();

        public Type GetFieldType(int i) => throw new NotSupportedException();

        public float GetFloat(int i) => throw new NotSupportedException();

        public Guid GetGuid(int i) => throw new NotSupportedException();

        public short GetInt16(int i) => throw new NotSupportedException();

        public int GetInt32(int i) => throw new NotSupportedException();

        public long GetInt64(int i) => throw new NotSupportedException();

        public string GetString(int i) => throw new NotSupportedException();

        public int GetValues(object[] values) => throw new NotSupportedException();
    }
}
