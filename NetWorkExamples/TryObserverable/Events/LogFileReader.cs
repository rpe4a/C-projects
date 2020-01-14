using System;
using System.Collections.Generic;
using System.Threading;

namespace TryObserverable.Events
{
    internal class LogFileReader : IDisposable
    {
        private static readonly TimeSpan interval = TimeSpan.FromSeconds(5);
        private readonly List<int> _logs = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        private readonly Timer _timer;

        public LogFileReader()
        {
            _timer = new Timer(state => CheckFile(), null, interval, interval);
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public event EventHandler<LogEntryEventsArgs> OnNewLogEntry;

        protected virtual void RaiseNewLogEntry(string log)
        {
            OnNewLogEntry?.Invoke(this, new LogEntryEventsArgs {LogEntry = log});
        }

        private void CheckFile()
        {
            foreach (var log in _logs)
            {
                RaiseNewLogEntry(log.ToString()); Thread.Sleep(500);
            }
        }
    }
}