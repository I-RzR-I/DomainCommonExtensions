using System;
using System.Collections;
using System.Collections.Generic;

namespace DataTypeTests.TestHelpers
{
    internal sealed class IndexOnlyListTestHelper<T> : IList<T>
    {
        private readonly List<T> _items;

        internal IndexOnlyListTestHelper(IEnumerable<T> items)
        {
            _items = new List<T>(items);
        }

        public T this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public void Add(T item) => _items.Add(item);

        public void Clear() => _items.Clear();

        public bool Contains(T item) => _items.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

        public int IndexOf(T item) => _items.IndexOf(item);

        public void Insert(int index, T item) => _items.Insert(index, item);

        public bool Remove(T item) => _items.Remove(item);

        public void RemoveAt(int index) => _items.RemoveAt(index);

        public IEnumerator<T> GetEnumerator() => throw new InvalidOperationException("The list was enumerated.");

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
