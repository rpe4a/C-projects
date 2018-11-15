using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TryWaitAndPulse
{
    class Program
    {
        static readonly object _locker = new object();
        static bool _go;

        static void Main()
        {
            // The new thread will block
            new Thread(Work).Start(); // because _go==false.

            Console.ReadLine(); // Wait for user to hit Enter

            lock (_locker) // Let's now wake up the thread by
            {
                // setting _go=true and pulsing.
                _go = true;
                Monitor.Pulse(_locker);
            }
        }

        static void Work()
        {
            lock (_locker)
                while (!_go)
                {
                    Console.WriteLine("Work!");
                    Monitor.Wait(_locker);    // Lock is released while we’re waiting
                }

            Console.WriteLine("Woken!!!");
        }
    }
}