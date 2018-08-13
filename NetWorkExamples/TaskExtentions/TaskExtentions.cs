using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskExtentions
{
    public static class TaskExtentions
    {
        public static async Task<TResult> WithTimeout<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            var winner = await Task.WhenAny(task, Task.Delay(timeout)).ConfigureAwait(false);

            if (winner != task)
                throw new TimeoutException();

            return await task.ConfigureAwait(false);
        }

        public static Task<TResult> WithCancellation<TResult>(this Task<TResult> task, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<TResult>();
            var reg = token.Register(() => tcs.TrySetCanceled());

            task.ContinueWith(result =>
            {
                reg.Dispose();

                if (result.IsCanceled)
                    tcs.TrySetCanceled();
                else if (result.IsFaulted)
                    tcs.TrySetException(result.Exception.InnerException);
                else
                    tcs.TrySetResult(result.Result);
            });

            return tcs.Task;
        }
    }
}