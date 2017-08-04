using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TryEvents.Events;

namespace TryEvents.AppCode
{
    sealed class Printer
    {
        public Printer()
        {
        }

        public void OnMessageSender(object sender, MyNewEvent e)
        {
            Console.WriteLine(string.Format("Object Call:{2}, {0}-{1}", e.Id, e.Message, this.GetType()));
        }
    }
}
