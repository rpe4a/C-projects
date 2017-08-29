using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TryBlocking
{
    //Синхронизация в пользовательском режиме (быстрая), не поддерживает рекурсивные вызовы
    public sealed class SimpleSpinLocker
    {
        private int m_resource = 0;

        public void Enter()
        {
            while (true)
            {
                //Если зашел 1 поток, возвращает управление, т.к. метод Exchange вернет 0 и присвоит в m_resource = 1, следовательно 2 поток застрянет в бесконечном цикле
                if (Interlocked.Exchange(ref m_resource, 1) == 0)
                    return;

            }
        }

        public void Leave()
        {
            //m_resource = 0, тогда поток который находился в бесконченом цикле выйдет из него и порлучит доступ к заблокированному ресурсу
        Interlocked.Decrement(ref m_resource);
        }
    }

    //Синхронизация в режиме ядра (Медленная), не поддерживает рекурсивные вызовы
    public sealed class SimpleWaitLock:IDisposable
    {
        private Semaphore _semaphore;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxConcorrentThreads">Отвечает за то сколько потоков могут одновременно иметь доступ к ресурсу. (Важно! Если это ресурс, который нам необходим только для чтения, то можно использовать значение > 1, если нужен для записи то ставим 1)</param>
        public SimpleWaitLock(int maxConcorrentThreads)
        {
            _semaphore = new Semaphore(maxConcorrentThreads, maxConcorrentThreads);
        }

        public void Enter()
        {
            //Ожидаем в режиме ядра доступ к ресурсу и возвращаем управление
            _semaphore.WaitOne();
        }

        public void Leave()
        {
            //Освобождаем ресурс, теперь другой поток может получить доступ к ресурсу
            _semaphore.Release();
        }

        public void Dispose()
        {
            _semaphore.Dispose();
        }
    }

    class Program
    {
        private static int count = 0;

        static void Main(string[] args)
        {
            var tasks = new Task[1500000];
            var ssl = new SimpleWaitLock(1);
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
                    //ssl.Enter();
                    mutex.WaitOne();
                    count++;
                    acrtion();
                    mutex.ReleaseMutex();
                    //ssl.Leave();
                });
            }

            Task.WaitAll(tasks);

            Console.WriteLine("Count = {0}", count);

        }
    }
}
