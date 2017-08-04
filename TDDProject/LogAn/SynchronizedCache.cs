using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogAn
{
    public class SynchronizedCache
    {
        private ReaderWriterLockSlim cacheLock;
        //private MemoryCache memoryCache;
        //private CacheItemPolicy policy = new CacheItemPolicy();
        private Dictionary<string, string> cach = new Dictionary<string, string>(); 

        public SynchronizedCache(int seconds)
        {
            //memoryCache = MemoryCache.Default;
            //policy.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(seconds));
            cacheLock = new ReaderWriterLockSlim();

        }

        public void Add<T>(string key, T value) where T : class
        {
            cacheLock.EnterWriteLock();
            try
            {
                cach.Add(key, value.ToString());
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }

        public string Read<T>(string key) where T : class
        {
            cacheLock.EnterReadLock();
            try
            {
                var data = cach[key];

                if (data == null)
                    throw new Exception();
                return data.ToString();

            }
            finally
            {
                cacheLock.ExitReadLock();
            }
        }

        public void Update<T>(string key, T value)
        {
            cacheLock.EnterReadLock();
            try
            {
                cach[key] = value.ToString();
            }
            finally
            {
                cacheLock.ExitReadLock();
            }

        }


    }
}
