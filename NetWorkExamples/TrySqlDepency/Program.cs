using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using TableDependency;
using TableDependency.Enums;
using TableDependency.SqlClient;
using TableDependency.EventArgs;

namespace TrySqlDepency
{

    class Program
    {
        private static string connect = ConfigurationManager.ConnectionStrings["Permsad"].ConnectionString;
        private static int counter = 0;

        static void Main(string[] args)
        {
            var mapping = new ModelToTableMapper<TriggerMessageEvent>();
            mapping.AddMapping(x => x.Id, "Id");
            mapping.AddMapping(x => x.TableName, "TableName");
            mapping.AddMapping(x => x.TableRowKey, "TableRowKey");
            mapping.AddMapping(x => x.Action, "Action");

            using (var _dependency = new SqlTableDependency<TriggerMessageEvent>(connect, "TriggerMessageEvent", mapping))
            {
                _dependency.OnChanged += _dependency_OnChanged;
                _dependency.OnStatusChanged += _dependency_OnStatusChanged;
                _dependency.OnError += _dependency_OnError;

                _dependency.Start();
                Console.WriteLine("SQLBroker is running");
                Console.WriteLine(Environment.NewLine);

                Console.ReadKey();

                _dependency.Stop();
            }
        }

        private static void _dependency_OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine("Event: Error, Message - {0}", e.Message);
        }

        private static void _dependency_OnStatusChanged(object sender, StatusChangedEventArgs e)
        {
            Console.WriteLine("Event: StatusChanged, Status - {0}", e.Status);

        }

        private static void _dependency_OnChanged(object sender, RecordChangedEventArgs<TriggerMessageEvent> e)
        {
            if (e.ChangeType != ChangeType.None && e.Entity != null)
            {
                counter++;
                Console.WriteLine("Entities:");
                Console.WriteLine("\tId: {0}", e.Entity.Id);
                Console.WriteLine("\tTableName: {0}", e.Entity.TableName);
                Console.WriteLine("\tTableRowKey: {0}", e.Entity.TableRowKey);
                Console.WriteLine("\tAction: {0}", e.Entity.Action);
                Console.WriteLine("Counter: {0}", counter);
                Console.WriteLine(Environment.NewLine);
            }
            else
            {
                throw new ArgumentNullException(e.ToString());
            }

        }
    }
}
