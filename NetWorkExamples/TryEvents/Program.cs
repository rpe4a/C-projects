using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TryEvents.AppCode;

namespace TryEvents
{
    class Program
    {
        static void Main(string[] args)
        {
            var mail = new MailMessager();
            var fax = new Fax(mail);
            var printer = new Printer();
            mail.Event += printer.OnMessageSender;

            mail.SimulateMesasge("Hellow!!!");
        }
    }
}
