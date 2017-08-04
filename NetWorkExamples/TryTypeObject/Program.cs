using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TryTypeObject.project;

namespace TryTypeObject
{



    namespace project
    {
    }



    class Program
    {

        static void Main(string[] args)
        {
            //object name = "plyusnin";
            //object arg = "sn";

            //Type[] argTypes = new Type[] {arg.GetType()};
            //var method = name.GetType().GetMethod("Contains", argTypes);

            //object[] argiments = new object[] {arg};
            //var result = method.Invoke(name, argiments);

            //Console.WriteLine(result);


            //dynamic target = "plyusnin";  
            //dynamic arg2 = "sn";
            //result = target.Contains("sn");

            //Console.WriteLine(result);

            //Статический конструктор вызывается
            var t = new TestClass(15);
            
            //Статический конструктор не вызывается, т.к. уже вызывался при инициализации выше
            //var t2 = new TestClass("qe1");

            //var t3 = new MyStruct();

            
        }
    }
}
