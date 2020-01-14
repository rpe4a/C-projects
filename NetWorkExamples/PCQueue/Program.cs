using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PCQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            PCQueue q = new PCQueue(5);

            Console.WriteLine("Enqueuing 10 items...");

            for (int i = 0; i < 100; i++)
            {
                int itemNumber = i;      // To avoid the captured variable trap
                q.EnqueueItem(() =>
                {
                    Thread.Sleep(1000);          // Simulate time-consuming work
                    Console.Write(" Task" + itemNumber);
                });
            }

            q.Shutdown(true);
            Console.WriteLine();
            Console.WriteLine("Workers complete!");
        }
    }
}
