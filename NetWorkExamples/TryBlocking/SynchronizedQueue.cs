using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TryBlocking
{
    public sealed class SynchronizedQueue<T> where T: class
    {
        private static readonly object _locker = new object();
        private Queue<T> _queue = new Queue<T>();

        public void Enqueue(T item)
        {
            Monitor.Enter(_locker);

            _queue.Enqueue(item);

            Monitor.PulseAll(_locker);

            Monitor.Exit(_locker);
        }

        public T Dequeue()
        {
            Monitor.Enter(_locker);

            if (_queue.Count == 0)
                Monitor.Wait(_locker);

            T item = _queue.Dequeue();

            Monitor.Exit(_locker);

            return item;
        }
    }
}
