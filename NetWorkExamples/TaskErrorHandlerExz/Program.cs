using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskErrorHandlerExz
{
    class Program
    {
        static void Main(string[] args)
        {
            //HandleExceptions();
            try
            {
                RethrowAllExceptions();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);   
            }
            Console.Write("Press any key.");
            Console.ReadKey();
        }

        static string[] GetValidExtensions(string path)
        {
            if (path == @"C:\")
                throw new ArgumentException("The system root is not a valid path.");

            return new string[10];
        }
        static void RethrowAllExceptions()
        {
            // Assume this is a user-entered string.
            string path = @"C:\";


            Task<string[]>[] tasks = new Task<string[]>[3];
            tasks[0] = Task<string[]>.Factory.StartNew(() => GetAllFiles(path));
            tasks[1] = Task<string[]>.Factory.StartNew(() => GetValidExtensions(path));
            tasks[2] = Task<string[]>.Factory.StartNew(() => new string[10]);


            //int index = Task.WaitAny(tasks2);
            //double d = tasks2[index].Result;
            try
            {
                Task.WaitAll(tasks);
            }
            catch (AggregateException ae)
            {
                throw ae.Flatten();
            }

            Console.WriteLine("task1 has completed.");
        }


        static string[] GetAllFiles(string str)
        {
            // Should throw an AccessDenied exception on Vista.
            return System.IO.Directory.GetFiles(str, "*.txt",
                                                System.IO.SearchOption.AllDirectories);
        }

        static void HandleExceptions()
        {
            // Assume this is a user-entered string.
            string path = @"C:\";

            // Use this line to throw UnauthorizedAccessException, which we handle.
            Task<string[]> task1 = Task<string[]>.Factory.StartNew(() => GetAllFiles(path));

            // Use this line to throw an exception that is not handled.
            //  Task task1 = Task.Factory.StartNew(() => { throw new IndexOutOfRangeException(); } );
            try
            {
                task1.Wait();
            }
            catch (AggregateException ae)
            {
                ae.Handle((x) =>
                {
                    if (x is UnauthorizedAccessException) // This we know how to handle.
                    {
                        Console.WriteLine("You do not have permission to access all folders in this path.");
                        Console.WriteLine("See your network administrator or try another path.");
                        return true;
                    }
                    return false; // Let anything else stop the application.
                });
            }

            Console.WriteLine("task1 has completed.");
        }
    }
}
