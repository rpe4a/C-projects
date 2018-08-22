using System;
using System.Collections.Generic;
using System.Linq;
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

        public static async Task<IEnumerable<TOut>> WithThrottle<TIn, TOut>(
            this IEnumerable<TIn> collection,
            Func<TIn, Task<TOut>> actionAsync,
            int throttle)
        {
            var semaphore = new SemaphoreSlim(throttle, throttle);

            return await Task.WhenAll(collection.Select((el) => Task.Run(async () =>
                {
                    await semaphore.WaitAsync().ConfigureAwait(false);

                    try
                    {
                        return await actionAsync(el).ConfigureAwait(false);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }))).ConfigureAwait(false);
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

        public static async Task<TResult[]> WenAllOrError<TResult>(params Task<TResult>[] tasks)
        {
            var tcs = new TaskCompletionSource<TResult[]>();
            foreach (var task in tasks)
            {
                task.ContinueWith(arg =>
                {
                    if (arg.IsCanceled)
                        tcs.TrySetCanceled();
                    if (arg.IsFaulted)
                        tcs.TrySetException(arg.Exception.InnerException);
                }).ConfigureAwait(false);
            }

            return await (await Task.WhenAny(tcs.Task, Task.WhenAll(tasks)).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}