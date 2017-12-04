using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanckAccounts
{
    public interface ICommand
    {
        void Execute();
        event EventHandler Executed;
    }

    public class SomethingThatNeedsACommand
    {
        ICommand command;
        public SomethingThatNeedsACommand(ICommand command)
        {
            this.command = command;
        }
        public void DoSomething() { command.Execute(); }
        public void DontDoAnything() { }

        public void DoSomethingManyTimes(int times)
        {
            for (int i = 0; i < times; i++)
            {
                command.Execute();
            }
        }

    }
}
