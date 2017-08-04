using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamWriterEx
{
    class Program
    {
        static void Main(string[] args)
        {
            var fs = new FileStream(@"TextOutput.txt", FileMode.CreateNew, FileAccess.Write);

            var sw = new StreamWriter(fs);

            Console.WriteLine("Encoding type = {0}", sw.Encoding.ToString());

            Console.WriteLine("Formating provider: {0}",sw.FormatProvider.ToString());

            for (int i = 0; i < 10; i++)
            {
                sw.WriteLine("{0} - line of code;", i);
            }

            sw.Close();

            fs.Close();



        }
    }
}
