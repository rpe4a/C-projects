using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TryEvents.Events;

namespace TryEvents.AppCode
{
    public sealed class MailMessager
    {
        public event EventHandler<MyNewEvent> Event;
        public MailMessager()
        {
        }

        public void SimulateMesasge(string message)
        {
            OnNewMessage(new MyNewEvent(message, Guid.NewGuid()));
        }

        private void OnNewMessage(MyNewEvent e)
        {
            var temp = Volatile.Read(ref Event);

            if (temp != null)
                temp(this, e);
        }
    }
}
