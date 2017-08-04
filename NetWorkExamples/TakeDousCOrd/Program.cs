using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace TakeDousCOrd
{
    class Program
    {
        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var dbcontext = new permsadEntities();

            //var tasksInt = (from i in Enumerable.Range(1, 5)
            //               select Dosomework(i)).ToList();

            //var t = Task.WhenAll(tasksInt).Result;

            //for (int i = 0; i < 10; i++)
            //{
            //    var t = Dosomework(i);

            //    tasksInt.Add(t.Result);
            //}

            var dous = dbcontext.DOUs.ToList().Take(15);
            Stopwatch sw = new Stopwatch();

            sw.Start();


            Console.WriteLine("Main Tread: {0}", Thread.CurrentThread.ManagedThreadId);

            var tasks = dous.Select(async dou =>
            {
                Console.WriteLine("Tread: {0}", Thread.CurrentThread.ManagedThreadId);

                if (dou.Street == null && string.IsNullOrEmpty(dou.DOU_House.ToString()))
                    return await Task.FromResult(dou);

                var query = new HttpClient();

                var res = await 
                        query.GetAsync(
                            new Uri(
                                string.Format(
                                    "https://geocode-maps.yandex.ru/1.x/?geocode=Пермь,{0},{1},{2}&results=1",
                                    dou.Street.Street_name, dou.DOU_House, dou.DOU_Building)),token);

                XmlDocument xm = new XmlDocument();

                xm.LoadXml(await res.Content.ReadAsStringAsync());
                Console.WriteLine("Tread under Result: {0}", Thread.CurrentThread.ManagedThreadId);

                var node = xm.GetElementsByTagName("Point");

                dou.DOU_Cords = node[0]?.InnerText ?? "";
                return dou;
            }).ToList(); //ToList Запускает все задачи на выполнение немедленно



            Console.WriteLine("Main Tread before Tasks-Array: {0}", Thread.CurrentThread.ManagedThreadId);

            /*Отмена при загрузке одного доу*/
            //var result = Task.WhenAny(tasks).Result;

            //cts.Cancel();

            //var douResult = result.Result;

            //Console.WriteLine("{0} - координаты {1}", douResult.DOU_ShortName, douResult.DOU_Cords);

            /*последовательная обработка ДОУ*/
            while (tasks.Count > 0)
            {
                var result = Task.WhenAny(tasks).Result;

                tasks.Remove(result);

                var douResult = result.Result;

                Console.WriteLine("{0} - координаты {1}", douResult.DOU_ShortName, douResult.DOU_Cords);
            }

            /*параллельная обработка ДОУ*/
            //foreach (var task in Task.WhenAll(tasks).Result)
            //{
            //    var result = task;
            //    Console.WriteLine("{0} - координаты {1}", result.DOU_ShortName, result.DOU_Cords);

            //    //dbcontext.Entry(result).State = EntityState.Modified;
            //    //dbcontext.SaveChanges();
            //}

            Console.WriteLine("Main Tread in END: {0}", Thread.CurrentThread.ManagedThreadId);

            sw.Stop();

            Console.WriteLine("Time: {0}", sw.ElapsedMilliseconds);
            Console.ReadKey();

        }

        private static async Task<int> Dosomework(int i)
        {
            await Task.Delay(500);
            return i;
        }

    }
}
