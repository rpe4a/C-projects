using System;
using System.Threading;

namespace TryDomain
{
    //[Serializable]
    public sealed class NonMarshalableType: Object
    {
        public NonMarshalableType()
        {
            Console.WriteLine("Executing in " + Thread.GetDomain().FriendlyName);
        }
    }
}