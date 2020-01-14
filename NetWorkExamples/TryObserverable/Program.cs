using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TryObserverable.Observable;

//using TryObserverable.Events;

namespace TryObserverable
{
    class Program
    {
        static void Main(string[] args)
        {
            //var logFileReader = new LogFileReader();
            //logFileReader.OnNewLogEntry += OddLogHandler;
            //logFileReader.OnNewLogEntry += EvenLogHandler;
            //logFileReader.OnNewLogEntry += RaiseLogHandler;

            var logObserver = new LogFileReader();
            logObserver.NewMessage.Subscribe(Console.WriteLine);
            logObserver.NewMessage.Subscribe((s) => Console.WriteLine($"!!!{s}!!!"));


            Console.ReadKey();
        }

        //private static void RaiseLogHandler(object sender, LogEntryEventsArgs e)
        //{
        //    Console.WriteLine($"x*2: {int.Parse(e.LogEntry) * 2}");
        //}

        //private static void EvenLogHandler(object sender, LogEntryEventsArgs e)
        //{
        //    if (int.Parse(e.LogEntry) % 2 == 0)
        //        Console.WriteLine($"Even: {e.LogEntry}");

        //}

        //private static void OddLogHandler(object sender, LogEntryEventsArgs logEntryEventsArgs)
        //{
        //    if(int.Parse(logEntryEventsArgs.LogEntry) % 2 != 0)
        //        Console.WriteLine($"Odd: {logEntryEventsArgs.LogEntry}");
        //}
    }
}
