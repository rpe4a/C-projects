using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrySemaphoreSlim
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Go work!");

            WorkerMainAsync().Wait();

            Console.WriteLine("All work is finished");

        }

        static async Task WorkerMainAsync()
        {
            SemaphoreSlim ss = new SemaphoreSlim(3);
            List<Task> trackedTasks = new List<Task>();
            int i = 0;

            while (i <= 10)
            {
                await ss.WaitAsync().ConfigureAwait(false);
                trackedTasks.Add(Task.Run(() =>
                {
                    Console.WriteLine($"Theard {Task.CurrentId} doing some work!");
                    DoPollingThenWorkAsync();
                    Console.WriteLine($"Theard {Task.CurrentId} finish work!");
                    ss.Release();
                }));

                i++;
            }
            await Task.WhenAll(trackedTasks).ConfigureAwait(false);
        }

        static void DoPollingThenWorkAsync()
        {
            var msg = "Hellow";
            if (msg != null)
            {
                Thread.Sleep(2000); // process the long running CPU-bound job
            }
        }
    }
}
