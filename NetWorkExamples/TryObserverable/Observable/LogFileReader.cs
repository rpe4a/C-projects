using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading;

namespace TryObserverable.Observable
{
    internal class LogFileReader:IDisposable
    {
        private static readonly TimeSpan interval = TimeSpan.FromSeconds(5);
        private readonly List<int> _logs = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        private readonly Timer _timer;
        private readonly Subject<string> _logObservable = new Subject<string>();


        public LogFileReader()
        {
            _timer = new Timer(state => CheckFile(), null, interval, interval);
        }

        public IObservable<string> NewMessage => _logObservable;

        private void CheckFile()
        {
            foreach (var log in _logs)
            {
                _logObservable.OnNext(log.ToString());
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _logObservable.OnCompleted();

        }
    }
}