using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryObserverable.Events
{
    class LogEntryEventsArgs : EventArgs
    {
        public string LogEntry { get; internal set; }

        public LogEntryEventsArgs()
        {
        }
    }
}
