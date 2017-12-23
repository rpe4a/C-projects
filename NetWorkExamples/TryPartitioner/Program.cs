using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryPartitioner
{
    class Program
    {
        static void Main(string[] args)
        {
            // Source must be array or IList.
            var source = Enumerable.Range(0, 1000000).ToArray();

            // Partition the entire source array.
            var rangePartitioner = Partitioner.Create(0, source.Length);

            double[] results = new double[source.Length];


            var sw = new Stopwatch();

            sw.Start();

            // Loop over the partitions in parallel.
            //Parallel.ForEach(rangePartitioner, () => new List<double>(), (range, loopState, localState) =>
            //{
            //    // Loop over each range element without a delegate invocation.
            //    for (int i = range.Item1; i < range.Item2; i++)
            //    {
            //        localState.Add(source[i] * Math.PI);
            //    }

            //    return localState;
            //}, list =>
            //{
            //    lock (results)
            //    {
            //        results.AddRange(list);
            //    }
            //});

            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                // Loop over each range element without a delegate invocation.  
                for (int i = range.Item1; i < range.Item2; i++)
                {

                    results[i] = source[i] * Math.PI;

                }

            });

            Console.WriteLine($"Operation complete. StopWatch = {sw.ElapsedTicks}");
            Console.ReadKey();
        }
    }
}
