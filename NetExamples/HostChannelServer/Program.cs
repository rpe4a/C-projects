using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;


namespace HostChannelServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpChannel tcpChannel = new TcpChannel(8080);

            ChannelServices.RegisterChannel(tcpChannel, false);

            RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemoteObject.RemoteObjectClass), "EchoMessage", WellKnownObjectMode.SingleCall);

            Console.WriteLine("waiting...");
            Console.ReadKey();

        }
    }
}
