using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TrySerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            var m = new MyType(10);

            Console.WriteLine("Variables before serialization: 1 - {0}, 2 - {1}", m.Param1, m.Param2);

            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, m);

                ms.Position = 0; 

                var mDeserial = (MyType)new BinaryFormatter().Deserialize(ms);

                Console.WriteLine("Variables after serializati  on: 1 - {0}, 2 - {1}", mDeserial.Param1, mDeserial.Param2);

            }

            //var list = new List<string>() {"Karl", "Masha", "Tom"};

            //var memoryStream = new MemoryStream();
            //new BinaryFormatter().Serialize(memoryStream, list);



            //var file = new FileStream("test.txt", FileMode.CreateNew, FileAccess.Write);

            //var data = memoryStream.GetBuffer();



            //file.Write(data, 0, data.Length);

            //memoryStream.Dispose();
            //file.Close();
            //data = new byte[1024];
            //file = null; 

            //file = new FileStream("test.txt", FileMode.Open, FileAccess.Read);
            //file.Read(data, 0, (int)file.Length);

            //memoryStream = new MemoryStream(data);

            //var t = (List<string>)new BinaryFormatter().Deserialize(memoryStream);

            //foreach (var name in t)
            //{
            //    Console.WriteLine(name);
            //}

            //file.Close();
            //File.Delete("test.txt");
        }
    }
}
