using NUnit.Framework;
using BanckAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;

namespace BanckAccounts.Tests
{
    [TestFixture()]
    public class SomethingThatNeedsACommandTests
    {
        [Test()]
        public void DoSomething_Should_ExecuteComand()
        {
            var comand = Substitute.For<ICommand>();

            var someClass = new SomethingThatNeedsACommand(comand);

            someClass.DoSomething();

            comand.Received().Execute();
        }

        [Test()]
        public void DontDoAnything_Should_NotExecuteComand()
        {
            var comand = Substitute.For<ICommand>();

            var someClass = new SomethingThatNeedsACommand(comand);

            someClass.DontDoAnything();

            comand.DidNotReceive().Execute();
        }

        [Test()]
        public void DoSomethingManyTimes_Should_ExecuteComandManyTimes()
        {
            var comand = Substitute.For<ICommand>();

            var someClass = new SomethingThatNeedsACommand(comand);

            someClass.DoSomethingManyTimes(5);

            comand.Received(5).Execute();
        }
    }
}