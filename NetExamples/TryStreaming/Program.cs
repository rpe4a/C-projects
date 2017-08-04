using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryStreaming
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = new FileStream(@"syncDemo.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 1000);
            file.WriteByte(Convert.ToByte('A'));
            byte[] massbitBytes = Encoding.ASCII.GetBytes("it's string of demo chars");
            file.Write(massbitBytes, 0, massbitBytes.Length);

            Console.WriteLine("{0} - position", file.Position);

            file.Seek(0, SeekOrigin.Begin);

            Console.WriteLine("{0} - position", file.Position);

            Console.WriteLine("{0} - first char", Convert.ToChar(file.ReadByte()));

            byte[] readBytes = new byte[file.Length - 1];

            file.Read(readBytes, 0, Convert.ToInt16(file.Length - 1));

            Console.WriteLine(Encoding.ASCII.GetString(readBytes));

            file.Close();


            MemoryStream ms = new MemoryStream();
            ms.Write(massbitBytes, 0, massbitBytes.Length);

            ms.Position = 0;
            readBytes = new byte[ms.Length];
            ms.Read(readBytes, 0, Convert.ToInt32(ms.Length));
            Console.WriteLine(Encoding.ASCII.GetString(readBytes));

            file = new FileStream(@"syncDemo.txt", FileMode.OpenOrCreate, FileAccess.Write);
            ms.WriteTo(file);

            file.Close();
        }
    }
}
