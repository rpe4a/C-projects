using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TryBlocking
{
    public sealed class SimpleHybridLock: IDisposable
    {
        //Используется примитивной конструкцией пользовательского режима
        private int _waiter = 0;

        //Используется примитивная конструкция режима ядра
        private AutoResetEvent _locker = new AutoResetEvent(false);

        public void Enter()
        {
            //Поток хочет получить блокировку
            if (Interlocked.Increment(ref _waiter) == 1)
                return; //Блокировка свободна, конкуренция отсутствует, возвращаем управление

            //Блокировка захвачена другим потоком(конкуренция), ожидаем
            _locker.WaitOne();
        }

        public void Leave()
        {
            //Поток освобождает блокировку
            if(Interlocked.Decrement(ref _waiter) == 0)
                return;//Другие потоки не заблокированы, возвращаем управление

            //Даем доступ к ресурсу заблокированному потоку
            _locker.Set(); 
        }

        public void Dispose()
        {
            _locker.Dispose();
        }
    }
}
