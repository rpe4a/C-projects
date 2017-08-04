using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TryParallelLinq
{
    class Program
    {
        static void Main(string[] args)
        {
            var timer = new Stopwatch();

            timer.Start();

            var source = Enumerable.Range(0, 3000);

            var evenNumsQuery = from num in source.AsParallel()
                                where num % 2 == 0
                                select Math.Sqrt(num);

            timer.Stop();
            
            Console.WriteLine("{0} even numbers out of {1} total. Elapsed time = {2}",
                  evenNumsQuery.Count(), source.Count(), timer.ElapsedTicks);

            //var parallelQuery = from num in source.AsParallel().AsOrdered()
            //                    where num % 3 == 0
            //                    select num;

            //// Use foreach to preserve order at execution time.
            //foreach (var v in parallelQuery)
            //    Console.Write("{0} ", v);

            //// Some operators expect an ordered source sequence.
            //var lowValues = parallelQuery.Take(10);

            //foreach (var v in lowValues)
            //    Console.Write("{0} ", v);
        }
    }
}
