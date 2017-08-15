using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrySort
{
    class Program
    {
        static void Main(string[] args)
        {
            var array = new[] { 1, 9, 4, 5, 6, 42, 28, 3, 13,15, 26, 8, 2, 34, 7, 321 };

            ToStringArray(array);

            Console.WriteLine($"Сортируем массив {nameof(array)}");

            ToStringArray(BubbleSort(array));

        }

        static int[] BubbleSort(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array.Length - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        array[j] = array[j] + array[j + 1];
                        array[j + 1] = array[j] - array[j + 1];
                        array[j] = array[j] - array[j + 1];
                    }

                    ToStringArray(array);
                }
            }

            return array;
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
