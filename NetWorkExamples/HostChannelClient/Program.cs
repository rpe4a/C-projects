using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using RemoteObject;

namespace HostChannelClient
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpChannel tcpChannel = new TcpChannel();

            ChannelServices.RegisterChannel(tcpChannel, false);

            RemoteObjectClass obj =
                (RemoteObjectClass) Activator.GetObject(typeof (RemoteObjectClass), "tcp://localhost:8080/EchoMessage");

            Console.WriteLine(obj.ServerMethod("It's simple object"));
        }
    }
}
