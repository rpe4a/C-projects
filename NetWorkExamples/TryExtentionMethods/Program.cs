using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryExtentionMethods
{
    class A
    {
        
    }

    class Program
    {
        static void Main(string[] args)
        {
            A x = null;

            Console.WriteLine(x.IsNull());

            x = new A();

            Console.WriteLine(x.IsNull());

            var t = "dsdasda";

            Console.WriteLine(t.IsNullOrEmpty());
        }
    }
}
