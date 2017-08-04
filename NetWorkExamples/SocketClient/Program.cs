using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {

            byte[] buffer = new byte[1024];

            IPHostEntry iHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = iHost.AddressList[1];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 11000);

            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            SocketPermission permission = new SocketPermission(NetworkAccess.Connect, TransportType.Tcp,"127.0.0.1", SocketPermission.AllPorts);


            sender.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, (object)new LingerOption(false, 0));

            try
            {
                sender.Connect(ipEndPoint);
                 
                Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint);

                while (true)
                {
                    if (!sender.Connected)
                        break;

                    string msg = Console.ReadLine();

                    byte[] msgbyte = Encoding.ASCII.GetBytes(msg);

                    //Отправляем данные через сокет
                    sender.Send(msgbyte);

                    //Получаем данные от серверsadfsafа
                    int bufferRes = sender.Receive(buffer);

                    
                    Console.WriteLine("the server says: {0}", Encoding.ASCII.GetString(buffer, 0, bufferRes));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Socket connection is abort");
            }
            finally
            {
                if (sender.Connected)
                {
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
            }

        }
    }
}
