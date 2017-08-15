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
            var array = new[] { 1, 9, 4, 5, 6, 42, 28, 3, 13, 15 };

            ToStringArray(array);

            Console.WriteLine($"Сортируем массив {nameof(array)}");

            //ToStringArray(BubbleSort(array));
            ToStringArray(InsertionSort(array));

        }

        static int[] BubbleSort(int[] array)
        {
            var count = 0;

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

                    Console.Write($"Проход {count++}: ");
                    ToStringArray(array);
                }
            }

            return array;
        }

        static int[] InsertionSort(int[] array)
        {
            var count = 0;

            int[] result = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                int j = i;
                while (j > 0 && result[j - 1] > array[i])
                {
                    result[j] = result[j - 1];
                    j--;

                    Console.Write($"Проход {count++}: ");
                    ToStringArray(result);

                }
                result[j] = array[i];
            }
            return result;
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
