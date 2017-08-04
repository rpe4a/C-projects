using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogAn
{
    class Program
    {
        private static List<string> list = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
        private static object _lock = new object();
        static void Main(string[] args)
        {
            var tasks = new List<Task>();
            var sc = new SynchronizedCache(60);
            
            try
            {
                tasks.Add(Task.Run(() =>
                {
                    for (int i = 1; i <= 20; i++)
                    {
                        sc.Add(i.ToString(), i.ToString());
                        Console.WriteLine("Элемент {0} добавлен в коллекцию на потоке {1}.", i.ToString(), Task.CurrentId);
                    }

                }));

                tasks.Add(Task.Run(() =>
                {
                    for (int i = 1; i <= 40; i++)
                    {
                        //Thread.Sleep(100);
                        sc.Update(i.ToString(), (i + "0").ToString());
                        Console.WriteLine("Элемент {0} обновлен в потоке {1}.", i.ToString(), Task.CurrentId);
                    }

                }));

                tasks.Add(Task.Run(() =>
                {
                    for (int i = 21; i <= 40; i++)
                    {
                        //Thread.Sleep(200);
                        sc.Add(i.ToString(), i.ToString());
                        Console.WriteLine("Элемент {0} добавлен в коллекцию на потоке {1}.", i.ToString(), Task.CurrentId);
                    }
                }));

                tasks.Add(Task.Run(() =>
                {
                    for (int i = 1; i <= 40; i++)
                    {
                        Task.Delay(1000);
                        //Thread.Sleep(400);
                        Console.WriteLine(sc.Read<string>(i.ToString()));
                    }
                }));




                Task.WaitAll(tasks.ToArray());
            }
            catch (Exception ex) { }
            
            Console.WriteLine("Конец");

            Console.ReadKey();
        }

        private static List<string> CachingData()
        {
            try
            {
                var memoryCache = MemoryCache.Default;
                var data = new List<string>();
                //lock (data)
                //{
                    data = memoryCache.Get("list") as List<string>;
                    if (data == null)
                    {
                        var policy = new CacheItemPolicy()
                        {
                            AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(5))
                        };

                        data = LongOperationTakeData();



                        //data.Add("dsdasasd");
                        memoryCache.Set("list", data, policy);
                    }
                //}
                return data;

            }
            catch (Exception)
            {

                throw;
            }

        }

        private static void SeTCachingData()
        {
            try
            {
                var memoryCache = MemoryCache.Default;

                lock (_lock)
                {
                    var data = memoryCache.Get("list") as List<string>;
                    data.Add("eeee");

                    var policy = new CacheItemPolicy()
                    {
                        AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(5))
                    };

                    memoryCache.Set("list", data, policy);
                }

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private static List<string> LongOperationTakeData()
        {
            Thread.Sleep(500);
            return list;
        }
    }
}
