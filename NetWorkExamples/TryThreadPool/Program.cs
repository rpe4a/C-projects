using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TryThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            //Помещаем данные в контекст логического вызова потока MAIN
            CallContext.LogicalSetData("Name", "Misha");

            Console.WriteLine("Main thread start working");

            //Вызываем асинхронное выполнение на пуле потоков, они предпочтительнее Thread
            ThreadPool.QueueUserWorkItem(DoSomeWork, 5);

            Console.WriteLine("Main thread doing other work");

            Thread.Sleep(3000);

            //Запрещаем копирование контекста исполнения потока MAIN
            ExecutionContext.SuppressFlow();

            //Вызываем асинхронное выполнение на пуле потоков, они предпочтительнее Thread
            ThreadPool.QueueUserWorkItem(DoSomeWork, 5);

            Console.WriteLine("Main thread doing other work");

            Thread.Sleep(3000);

            //Восстанавливаем копирование контекста исполнения потока MAIN
            ExecutionContext.SuppressFlow();

            Console.WriteLine("Press <Enter> to exit");
            Console.ReadKey();
        }

        private static void DoSomeWork(object state)
        {
            Console.WriteLine("DoSomeWork with parameter = {0}", state);

            //Поток из пула имеет доступ к данным контекста логического вызова потока
            Console.WriteLine("CallContext = {0}", CallContext.LogicalGetData("Name"));
            Thread.Sleep(1000);
        }
    }
}
