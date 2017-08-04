using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EasyTcpLister
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 5001);

                tcpListener.Start();

                byte[] readBuff = new byte[100];

                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    Console.WriteLine("Connected!");

                    NetworkStream nw = tcpClient.GetStream();

                    int i;

                    while ((i = nw.Read(readBuff, 0, readBuff.Length)) != 0)
                    {
                        var data = Encoding.ASCII.GetString(readBuff, 0, i);

                        Console.WriteLine("{0} - client sending", data);

                        data = data.ToUpper();

                        var msg = Encoding.ASCII.GetBytes(data);

                        nw.Write(msg, 0, msg.Length);
                    }

                    tcpClient.Close();
                }

                

                //tcpListener.BeginAcceptTcpClient(result =>
                //{
                //    var listen = (TcpListener)result.AsyncState;
                //    var client = listen.EndAcceptTcpClient(result);

                //    byte[] readBuff = new byte[100];
                //    using (var stream = client.GetStream())
                //    {
                //        stream.Read(readBuff, 0, readBuff.Length);
                //        Console.WriteLine("{0} - client sending", Encoding.ASCII.GetString(readBuff));
                //    }

                //    client.Close();

                //}, tcpListener);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }
    }
}
