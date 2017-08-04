using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrySocketServer
{
    class Program
    {
        private static List<Socket> arraySockets = new List<Socket>();

        private static ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();
        static void Main(string[] args)
        {
            IPHostEntry iHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = iHost.AddressList[1];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 11000);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, (object)new LingerOption(false, 0));

            try
            {

                socket.Bind(ipEndPoint);

                socket.Listen(1);

                while (true)
                {
                    Console.WriteLine("Waiting on port {0}", ipEndPoint);

                    Socket handler = socket.Accept();

                    if (arraySockets.Count < 2)
                    {
                        arraySockets.Add(handler);
                        string data = string.Empty;

                        Task.Run(() =>
                        {
                            var bytes = new byte[1024];
                            while (true)
                            {
                                if (!handler.Connected) { break; }

                                try
                                {
                                    int bytesRec = handler.Receive(bytes);

                                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                                    Console.WriteLine("Time for disconect {0}", ((LingerOption)handler.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger)).LingerTime);
                                    Console.WriteLine("Text Received: {0}", data);
                                    Console.WriteLine("Main thread has {0} socket's connections", arraySockets.Count);

                                    if (data.IndexOf("exit") > -1)
                                    {
                                        break;
                                    }

                                    handler.Send(
                                        Encoding.ASCII.GetBytes(string.Format("Catch data from {0}",
                                            Thread.CurrentThread.ManagedThreadId)));
                                }
                                catch (Exception)
                                {
                                    break;
                                }
                            }

                            handler.Shutdown(SocketShutdown.Both);
                            handler.Close();

                            RemoveSocket(handler);
                        });
                    }
                    else
                    {
                        handler.Send(Encoding.ASCII.GetBytes(string.Format("Server is full")));
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }

                    readerWriterLockSlim.EnterUpgradeableReadLock();
                    try
                    {
                        for (int i = 0; i < arraySockets.Count; i++)
                        {
                            if (!arraySockets[i].Connected)
                            {
                                RemoveSocket(arraySockets[i]);
                            }
                        }
                    }
                    finally
                    {
                        readerWriterLockSlim.ExitUpgradeableReadLock();

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void RemoveSocket(Socket handler)
        {
            readerWriterLockSlim.EnterWriteLock();
            try
            {
                arraySockets.Remove(handler);
            }
            finally
            {
                readerWriterLockSlim.ExitWriteLock();
            }
        }
    }
}
