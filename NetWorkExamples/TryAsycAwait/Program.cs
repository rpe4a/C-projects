using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryAsycAwait
{
    class Program
    {
        static void Main(string[] args)
        {
            //try
            //{ 
            //    var result = GetLengthSyncEx("");
            //    Console.WriteLine(result.Result);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            var t = GetSomeValue().Result;

            Console.WriteLine(t);

            Console.ReadKey();
        }

        public static async Task<int> GetLengthAsyncEx(string text)
        {
            return await GetLengthAsyncExImp(text);
        }

        public static async Task<int> GetLengthAsyncExImp(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException();
            }

            await Task.Delay(3000);
            return text.Length;
        }

        public static  Task<int> GetLengthSyncEx(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException();
            }

            return GetLengthSyncExImp(text);
        }

        private static async Task<int> GetLengthSyncExImp(string text)
        {
            await Task.Delay(3000);
            return text.Length;
        }

        private static Task<int> GetSomeValue()
        {
            var tcs = new TaskCompletionSource<int>();

            Task.Run(async () =>
            {
                await Task.Delay(3000);
                tcs.SetResult(SomeValue());
            });

            return tcs.Task;
        }

        private static int SomeValue()
        {
            return 5;
        }
    }
}
