using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TryBlocking
{
    class Program
    {
        private static int count = 0;

        static void Main(string[] args)
        {
            var tasks = new Task[1500000];
            var ssl = new SimpleHybridLock();
            var mutex = new Mutex(); //поддерживает рекурсивные вызовы (из-за чего очень медленный)

            Action acrtion = () =>
            {
                mutex.WaitOne();
                count++;
                mutex.ReleaseMutex();
            };

            for (int i = 0; i < tasks.Length; i++)
            {

                tasks[i] = Task.Run(() =>
                {
                    ssl.Enter();
                    //mutex.WaitOne();
                    count++;
                    //acrtion();
                    //mutex.ReleaseMutex();
                    ssl.Leave();
                });
            }

            Task.WaitAll(tasks); 

            Console.WriteLine("Count = {0}", count);

            //AcknowledgedWaitHandle.GO();

        }
    }
}
