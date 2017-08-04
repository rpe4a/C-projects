using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryGC
{
    class Program
    {
        static void Main(string[] args)
        {
            Debug.WriteLine("Данные с кучи:");
            Debug.Indent();
            Debug.WriteLine(string.Format("Сколько раз была уборка: {0}", GC.CollectionCount(0)));
            Debug.WriteLine(string.Format("Объем памяти: {0}", GC.GetTotalMemory(true)));
            Debug.Unindent();

            var arra = new long[10000][];

            for (int i = 0; i < 10000; i++)
            {
                arra[i] = new long[1000];

            }
            //GC.Collect();
            Debug.WriteLine("Данные с кучи:");
            Debug.Indent();
            Debug.WriteLine(string.Format("Сколько раз была уборка: {0}", GC.CollectionCount(0)));
            Debug.WriteLine(string.Format("Объем памяти: {0}", GC.GetTotalMemory(true)));
            Debug.Unindent();
            Console.ReadKey();
        }
    }
}
