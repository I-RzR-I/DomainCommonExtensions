using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace DataTypeTests.TestHelpers
{
    internal sealed class GatedFactoryTestHelper<T>
    {
        private readonly ConcurrentDictionary<int, TaskCompletionSource<T>> _gates =
            new ConcurrentDictionary<int, TaskCompletionSource<T>>();

        private readonly ConcurrentDictionary<int, TaskCompletionSource<CancellationToken>> _started =
            new ConcurrentDictionary<int, TaskCompletionSource<CancellationToken>>();

        private int _calls;

        internal int Calls => Volatile.Read(ref _calls);

        internal Task<T> InvokeAsync(CancellationToken cancellationToken)
        {
            var call = Interlocked.Increment(ref _calls);
            Started(call).TrySetResult(cancellationToken);

            return Gate(call).Task;
        }

        internal Task<CancellationToken> WhenStartedAsync(int call)
        {
            return Started(call).Task;
        }

        internal void Complete(int call, T value)
        {
            Gate(call).TrySetResult(value);
        }

        internal void Fail(int call, Exception exception)
        {
            Gate(call).TrySetException(exception);
        }

        private TaskCompletionSource<T> Gate(int call)
        {
            return _gates.GetOrAdd(call, _ => new TaskCompletionSource<T>());
        }

        private TaskCompletionSource<CancellationToken> Started(int call)
        {
            return _started.GetOrAdd(call,
                _ => new TaskCompletionSource<CancellationToken>(TaskCreationOptions.RunContinuationsAsynchronously));
        }
    }
}
