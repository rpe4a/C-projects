using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskExtentions
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var token = new CancellationTokenSource(2000).Token;

            var task = Task.Run(async () =>
            {
                Console.WriteLine("Starting");
                await Task.Delay(5000).ConfigureAwait(false);
                Console.WriteLine("Finished");
                return 5;
            }).WithCancellation(token);

            try
            {
                task.Wait();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Console.WriteLine(task.Result);
            Console.ReadKey();
        }
    }
}