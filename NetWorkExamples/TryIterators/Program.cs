using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryIterators
{
    class Program
    {
        static void Main(string[] args)
        {

            var dateRange = new DateRange(DateTime.Now, DateTime.Now.AddDays(10));

            foreach (var day in dateRange)
            {
                Console.WriteLine(day.ToShortDateString());
            }

            foreach (var data in ReadLinesFile.DoIt(() => File.OpenText(@"test.txt")))
            { 
                Console.WriteLine(data);
            }
        }
    }
}
