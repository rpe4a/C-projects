using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamReaderEx
{
    class Program
    {
        static void Main(string[] args)
        {
            var fs = new FileStream(@"syncDemo.txt", FileMode.Open, FileAccess.Read);

            var sr = new StreamReader(fs, Encoding.UTF8);

            string data;
            int line = 0;

            while ((data = sr.ReadLine()) != null)
            {
                Console.WriteLine("data: {0}", data);
            }

            sr.BaseStream.Position = 0;

            Console.WriteLine(sr.ReadToEnd());

        }
    }
}
