using System;
using System.Threading;

namespace TryBlocking
{
    public sealed class SimpleMutex:IDisposable
    {
        private AutoResetEvent _lock = new AutoResetEvent(true);
        private int _owningThreadID = 0;
        private int _recursionCount = 0;

        public void Enter()
        {
            int currentThreadID = Thread.CurrentThread.ManagedThreadId;

            if (_owningThreadID == currentThreadID)
            {
                _recursionCount++;
                return;
            }

            _lock.WaitOne();

            _owningThreadID = currentThreadID;
            _recursionCount++;
        }

        public void Leave()
        {
            if(_owningThreadID != Thread.CurrentThread.ManagedThreadId)
                throw new InvalidOperationException();

            if (--_recursionCount == 0)
            {
                _owningThreadID = 0;
                _lock.Set();
            }
        }

        public void Dispose()
        {
            _lock.Dispose();
        }
    }
}