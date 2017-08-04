using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryEvents.Events
{
    public sealed class MyNewEvent : EventArgs
    {
        public string Message { get; set; }

        public Guid Id { get; set; }

        public MyNewEvent(string message, Guid id)
        {
            Message = message;
            Id = id;
        }
    }
}
