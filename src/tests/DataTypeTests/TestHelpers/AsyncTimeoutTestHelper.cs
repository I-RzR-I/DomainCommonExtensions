using System;
using System.Threading.Tasks;

namespace DataTypeTests.TestHelpers
{
    internal static class AsyncTimeoutTestHelper
    {
        internal static async Task<T> WithinAsync<T>(Task<T> task, int milliseconds = 5000)
        {
            var completed = await Task.WhenAny(task, Task.Delay(milliseconds));

            if (completed != task)
                throw new TimeoutException("Task did not complete within " + milliseconds + " ms.");

            return await task;
        }

        internal static async Task<bool> CompletesWithinAsync(Task task, int milliseconds = 5000)
        {
            var completed = await Task.WhenAny(task, Task.Delay(milliseconds));

            return completed == task;
        }
    }
}
