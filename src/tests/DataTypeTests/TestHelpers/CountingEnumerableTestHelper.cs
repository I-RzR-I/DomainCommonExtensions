using System;
using System.Collections;
using System.Collections.Generic;

namespace DataTypeTests.TestHelpers
{
    internal sealed class CountingEnumerableTestHelper<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _items;
        private readonly bool _throwOnSecondEnumeration;

        internal CountingEnumerableTestHelper(IEnumerable<T> items, bool throwOnSecondEnumeration = false)
        {
            _items = items;
            _throwOnSecondEnumeration = throwOnSecondEnumeration;
        }

        internal int EnumerationCount { get; private set; }

        public IEnumerator<T> GetEnumerator()
        {
            EnumerationCount++;
            if (_throwOnSecondEnumeration && EnumerationCount > 1)
                throw new InvalidOperationException("The source can be enumerated only once.");

            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
