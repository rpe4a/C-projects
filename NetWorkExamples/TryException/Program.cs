using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryException
{

    class Program
    {
        public delegate void CatchException<T1, in T2>() where T2 : class;
        public delegate void CatchException<T1, T2, in T3>(T1 a, T2 b) where T3 : class;

        public string GetName() => "hello";
        public string Name => "hello";

        public override string ToString()
        {
            return "Hello String";
        }

        public static string sum(string a, string b)
        {
            ExceptionHandler.TryCatchTypeException<string, string, ArgumentNullException>(a, b);

            return a + b;
        }


        public static int sum(int a, int b)
        {
            ExceptionHandler.TryCatchStructException<ArgumentNullException>(() =>
            {
                return a == 0 || b == 0;
            });

            return a + b;
        }

        public static void GenerateEx(int b)
        {
            try
            {
                var t = 5 / b;

            }
            //перехватываем абсолютно все исключения
            catch
            {
                //передаем исключение дальше по стеку вызова(генерируя его снова)
                throw;
            }

        }

        public static void GenerateExDivideByZero(int b)
        {
            try
            {
                var t = 5 / b;

            }
            //перехватываем абсолютно все исключения
            catch (Exception ex)
            {
                //Можем добавить какую-нибудь полезную дополнительную информацию к исключению, и далее передать его выше по стеку
                ex.Data.Add(1, "dsadasd");

                //передаем исключение дальше по стеку вызова(генерируя его снова)
                throw;
            }

        }

        static void Main(string[] args)
        {
           

            string a = "Hello", b = "String";

            int z = 5;
            int c = 0;

            try
            {
                //Environment.FailFast("Destroy");

                //var domen = AppDomain.CurrentDomain;
                //AppDomain.Unload(domen);
                Console.WriteLine(sum(a, b));
                //GenerateEx(c);
                GenerateExDivideByZero(c);
                Console.WriteLine(sum(z, c));
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine("Деление на 0: {0}", ex.Data[1]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Finally");
            }


            var arra = new long[10000][];

            for (int i = 0; i < 10000; i++)
            {
                arra[i] = new long[1000];

            }
            GC.Collect();
            arra = null;
            Debug.WriteLine("Данные с кучи:");
            Debug.Indent();
            Debug.WriteLine("Сколько раз была уборка: {0}", GC.CollectionCount(0));
            Debug.WriteLine("Объем памяти: {0}", GC.GetTotalMemory(true));
            Debug.Unindent();
            Console.ReadKey();
        }
    }
}
