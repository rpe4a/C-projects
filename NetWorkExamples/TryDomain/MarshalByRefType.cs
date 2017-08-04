using System;
using System.Threading;

namespace TryDomain
{
    public sealed class MarshalByRefType: MarshalByRefObject
    {
        public MarshalByRefType()
        {
            Console.WriteLine("{0} ctor running in {1}", this.GetType().ToString(), Thread.GetDomain().FriendlyName);
        }

        public void SomeMethod()
        {
            Console.WriteLine("Executing in " + Thread.GetDomain().FriendlyName);
        }

        public MarshalByValType MethodWithReturn()
        {
            Console.WriteLine("Executing in " + Thread.GetDomain().FriendlyName);
            return new MarshalByValType();
        }

        public NonMarshalableType MethodArgAndReturn(string callingDomainName)
        {
            Console.WriteLine("Calling from '{0} to '{1}'", callingDomainName, Thread.GetDomain().FriendlyName);
            return new NonMarshalableType();
        }
    }
}