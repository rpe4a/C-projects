using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EasyTcpClients
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect("127.0.0.1", 5001);
                NetworkStream nw = tcpClient.GetStream();

                while (true)
                {
                    var line = Console.ReadLine();

                    var writeBuff = Encoding.ASCII.GetBytes(line);

                    nw.Write(writeBuff, 0, writeBuff.Length);

                    var data = new byte[100];

                    var bytes = nw.Read(data, 0, data.Length);

                    Console.WriteLine("Received: {0}", Encoding.ASCII.GetString(data, 0, bytes));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }
    }
}
