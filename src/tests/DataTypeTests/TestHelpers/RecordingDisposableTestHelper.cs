using System;
using System.Collections.Generic;

namespace DataTypeTests.TestHelpers
{
    internal sealed class RecordingDisposableTestHelper : IDisposable
    {
        private readonly List<string> _disposeLog;
        private readonly Exception _exceptionToThrow;

        internal RecordingDisposableTestHelper(string name, List<string> disposeLog, Exception exceptionToThrow = null)
        {
            Name = name;
            _disposeLog = disposeLog;
            _exceptionToThrow = exceptionToThrow;
        }

        internal string Name { get; }

        internal int DisposeCount { get; private set; }

        public void Dispose()
        {
            DisposeCount++;
            _disposeLog.Add(Name);

            if (_exceptionToThrow != null)
                throw _exceptionToThrow;
        }
    }
}
