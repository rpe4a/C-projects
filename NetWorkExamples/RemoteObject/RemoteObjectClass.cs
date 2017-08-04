using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;


namespace RemoteObject
{
    public class RemoteObjectClass: MarshalByRefObject
    {
        public string ServerMethod(string arg)
        {
            Console.WriteLine(arg);

            return arg;
        }
    }
}
