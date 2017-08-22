using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TryParallel
{
    class Program
    {
        static void Main(string[] args)
        {
            var array = Enumerable.Range(0, 10000000);
            var cts = new CancellationTokenSource(1000);
            var ct = cts.Token;


            GetMethodElapsedTime(() =>
            {
                try
                {
                    var t = Parallel.ForEach(array,
                    //Если мы используем токен с опциями, то по прошествию 1 сек. будет брошено исключение об отмене
                    new ParallelOptions()
                    {
                        CancellationToken = ct,
                    },
                   (i, loopState) =>
                   {
                       //Так мы сами контролируем токен, и мягко выходим из цикла
                       //if (ct.IsCancellationRequested)
                       //{
                       //    loopState.Break();
                       //}

                       Task.Delay(15);

                   });

                    Console.WriteLine(t.LowestBreakIteration);

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            });

            Console.WriteLine("Ended!");
        }

        static void GetMethodElapsedTime(Action action)
        {
            var sw = new Stopwatch();
            sw.Start();

            action();

            sw.Stop();

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }


}
