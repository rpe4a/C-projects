using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryDelegates
{
    class Program
    {
        public delegate void Send(string message);

        

        class TestClass
        {
            private int _p1;
            private string _p2;

            public TestClass()
            {
                _p1 = 5;
                _p2 = "qwerty";
            }

            //Вызывается только один раз! для всех объектов данного типа
            static TestClass() { }

            public static void StaticSend(string message)
            {
                Console.WriteLine(message);
            }

            public void ErrorSend(string message)
            {
                throw new Exception("Error");
            }

            public void Send(string message)
            {
                Console.WriteLine(this._p2 + " " + message);
            }
        }

        private static void RunChainDelegates(Send chainDelegates)
        {
            var ArrayDelegates = chainDelegates.GetInvocationList();

            foreach (Send @delegate in ArrayDelegates)
            {
                try
                {
                    @delegate("Hello");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void Main(string[] args)
        {
            var t = new TestClass();

            Send del1 = new Send(r => Console.WriteLine(r));
            var adel2 = del1.BeginInvoke("Hello", null, null);

            Console.WriteLine("1");
            Console.WriteLine("2");
            del1.EndInvoke(adel2);

            Console.WriteLine("1");
            Console.WriteLine("2");

            var staticM = new Send(TestClass.StaticSend);
            staticM("static");

            var exM = new Send(t.Send);
            exM("exM");

            
            var chainDelegates = new Send(TestClass.StaticSend);
            chainDelegates += new Send(t.ErrorSend); //если не обработать ошибку то программа упадет
            chainDelegates += new Send(t.Send); 


            //Данный метод позволяет обработать ошибку в цепочке, чтобы программа не падает 
            RunChainDelegates(chainDelegates);

            chainDelegates.RunChainDelegates();

        }
    }
}
