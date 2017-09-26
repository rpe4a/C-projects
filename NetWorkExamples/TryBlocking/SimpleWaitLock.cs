using System;
using System.Threading;

namespace TryBlocking
{
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
}