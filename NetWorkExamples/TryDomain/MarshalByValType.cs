using System;
using System.Threading;

namespace TryDomain
{
    [Serializable]
    public sealed class MarshalByValType : Object
    {
        private DateTime m_creationTime = DateTime.Now;
        public MarshalByValType()
        {
            Console.WriteLine("{0} ctor running in {1}, Created on {2:D}", this.GetType().ToString(), Thread.GetDomain().FriendlyName, m_creationTime);
        }

        public override string ToString()
        {
            return m_creationTime.ToLongDateString();
        }
    }
}