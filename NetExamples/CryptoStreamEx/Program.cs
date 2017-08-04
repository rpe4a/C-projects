using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptoStreamEx
{
    class Program
    {
        static void Main(string[] args)
        {
            var fs = new FileStream(@"secretData.dat", FileMode.Create, FileAccess.ReadWrite);

            var buffer = Encoding.ASCII.GetBytes("The T:System.Net.Sockets.TcpClient class provides simple methods for connecting, sending, and receiving stream data over a network in synchronous blocking mode.");

            SymmetricAlgorithm des = new TripleDESCryptoServiceProvider();

            var cryptostream = new CryptoStream(fs, des.CreateEncryptor(), CryptoStreamMode.Write);

            cryptostream.Write(buffer, 0, buffer.Length);

            cryptostream.Close();

            fs.Close();

            fs = new FileStream(@"secretData.dat", FileMode.Open, FileAccess.ReadWrite);

            var buffer2 = new byte[fs.Length];

            fs.Read(buffer2, 0, buffer2.Length);

            Console.WriteLine(Encoding.ASCII.GetString(buffer2));

            fs.Position = 0;

            ICryptoTransform descryptor = des.CreateDecryptor();

            cryptostream = new CryptoStream(fs, descryptor, CryptoStreamMode.Read);

            var decBuffer = new byte[fs.Length];

            cryptostream.Read(decBuffer, 0, decBuffer.Length);

            Console.WriteLine(Encoding.ASCII.GetString(decBuffer));

            cryptostream.Close();

            fs.Close();
        }
    }
}
