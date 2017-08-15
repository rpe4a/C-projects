using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrySearch
{
    class Program
    {
        static void Main(string[] args)
        {
            var array = Enumerable.Range(1, 1000).ToArray();

            DoubleFindArray(array, 501);
        }

        static void LineFind(int[] array, int findValue)
        {
            var count = 0;

            for (int i = 0; i < array.Length; i++)
            {
                Console.WriteLine($"Проход {count++}: ");

                if (array[i] == findValue)
                    break;

            }
        }

        static void DoubleFind(int[] array, int findValue)
        {
            var count = 0;

            while (true)
            {
                Console.WriteLine($"Проход {count++}: ");

                var t = array[array.Length/2];

                if (t == findValue)
                    break;

                if (t < findValue)
                    array = array.Skip(array.Length / 2).ToArray();
                else
                    array = array.Take(array.Length / 2).ToArray();


            }
        }
        static void DoubleFindArray(int[] array, int findValue)
        {
            var count = 0;

            var first = 0;

            var last = array.Length;

            while (first < last)
            {
                Console.WriteLine($"Проход {count++}: ");

                var mid = first + (last - first)/2;

                if (findValue <= array[mid])
                    last = mid;
                else
                    first = mid + 1;

            }

            if (array[last] == findValue)
                Console.WriteLine("Finded");
        }

        static void ToStringArray(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write("{0} ", array[i]);
            }

            Console.WriteLine();
        }
    }
}
