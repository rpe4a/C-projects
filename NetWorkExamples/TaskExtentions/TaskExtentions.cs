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

        /// <summary>
        ///     Выполнить все <see cref="Task" /> из <paramref name="collection" /> в <paramref name="throttling" /> потоков
        /// </summary>
        public static async Task RunTasksWithThrottleAsync(this IEnumerable<Task> collection, int throttling, ErrorHandler errorHandler = null)
        {
            if (collection == null)
                return;

            var tasks = new List<Task>();
            errorHandler = errorHandler ?? (_ => false);
            using (var enumerator = collection.GetEnumerator())
            using (var semaphore = new SemaphoreSlim(throttling, throttling))
            {
                await semaphore.WaitAsync().ConfigureAwait(false);
                while (enumerator.MoveNext())
                {
                    // ReSharper disable once AccessToDisposedClosure
                    tasks.Add(enumerator.Current?.ContinueWith(task => HandleTaskResult(semaphore, task, errorHandler)));
                    await semaphore.WaitAsync().ConfigureAwait(false);
                }
                semaphore.Release();
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
        }

        /// <summary>
        ///     Выполнить все <see cref="Task" /> из <paramref name="collection" /> в <paramref name="throttling" />
        ///     потоков с получением результата
        /// </summary>
        public static async Task<T[]> RunTasksWithThrottleAsync<T>(this IEnumerable<Task<T>> collection, int throttling, ErrorHandler<T> errorHandler = null)
        {
            if (collection == null)
                return null;

            var tasks = new List<Task<T>>();
            errorHandler = errorHandler ?? ((Exception exception, out T value) =>
            {
                value = default(T);
                return false;
            });
            using (var enumerator = collection.GetEnumerator())
            using (var semaphore = new SemaphoreSlim(throttling, throttling))
            {
                await semaphore.WaitAsync().ConfigureAwait(false);
                while (enumerator.MoveNext())
                {
                    // ReSharper disable once AccessToDisposedClosure
                    tasks.Add(enumerator.Current?.ContinueWith(task => HandleTaskResult(semaphore, task, errorHandler)));
                    await semaphore.WaitAsync().ConfigureAwait(false);
                }
                semaphore.Release();
                return await Task.WhenAll(tasks).ConfigureAwait(false);
            }
        }

        /// <summary>
        ///     Группирует по <paramref name="size" /> элкментов
        /// </summary>
        public static IEnumerable<TSource[]> Batch<TSource>(this IEnumerable<TSource> source, int size)
        {
            TSource[] bucket = null;
            var count = 0;

            foreach (var item in source ?? new TSource[0])
            {
                if (bucket == null)
                    bucket = new TSource[size];

                bucket[count++] = item;
                if (count < size)
                    continue;

                yield return bucket;
                bucket = null;
                count = 0;
            }

            if (bucket != null && count > 0)
                yield return bucket.Take(count).ToArray();
        }

        /// <summary>
        ///     Делегат для обработки ошибок. Если ошибка обработана возвращать <c>true</c> иначе <c>false</c>.
        /// </summary>
        public delegate bool ErrorHandler(Exception exception);

        /// <summary>
        ///     Делегат для обработки ошибок. Если ошибка обработана возвращать <c>true</c> иначе <c>false</c>.
        /// </summary>
        public delegate bool ErrorHandler<T>(Exception exception, out T errorValue);

        private static T HandleTaskResult<T>(SemaphoreSlim semaphore, Task<T> task, ErrorHandler<T> errorHandler)
        {
            try
            {
                if (!task.IsFaulted)
                    return task.Result;
                if (!errorHandler(task.Exception, out var result) && task.Exception != null)
                    throw task.Exception;
                return result;
            }
            finally
            {
                semaphore.Release();
            }
        }

        private static void HandleTaskResult(SemaphoreSlim semaphore, Task task, ErrorHandler errorHandler)
        {
            try
            {
                if (task.IsFaulted && !errorHandler(task.Exception) && task.Exception != null)
                    throw task.Exception;
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}