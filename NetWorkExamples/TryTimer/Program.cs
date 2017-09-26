using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TryTimer
{
    class Program
    {
        private static Timer s_timer;

        static void Main(string[] args)
        {
            ////Создаем таймер, но не запускаем его
            //s_timer = new Timer(DoWork, null, Timeout.Infinite, Timeout.Infinite);

            ////Запускаем таймер, 1 раз, параметр Timeout.Infinite - говорит, что мы не знаем через сколько закончиться метод обратного вызова
            //s_timer.Change(0, Timeout.Infinite);

            //C помощью Task
            DoWorkAsync();


            Console.ReadLine();
        }

        private static async void DoWorkAsync()
        {
            while (true)
            {
                Console.WriteLine("Do some work {0}", DateTime.Now.ToShortTimeString());

                await Task.Delay(2000);
            }
        }

        private static void DoWork(object state)
        {
            Console.WriteLine("Do some work {0}", DateTime.Now.ToShortTimeString());
            Thread.Sleep(1000);

            //Снова запустим таймер через 2 сек.
            s_timer.Change(2000, Timeout.Infinite);

        }
    }
}
