using BanckAccounts;
using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using FluentAssertions.Common;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BanckAccounts.Tests
{
    [TestFixture]
    [Platform("Net-4.5, 64-Bit")]
    public class CalculatorTests
    {
        private Calculator calc;
        private string test;

        [OneTimeSetUp]
        public void Init()
        {
            test = "default";
            calc = new Calculator();
        }

        [Test]
        [Category("Type")]
        public void Add_Should_ReturnAddInt()
        {
            calc.Add(1, 3, 4).Should().BeOfType(typeof(int));
        }

        [Test]
        [Category("Type")]
        public void Average_Should_RetrunDoubleType()
        {
            calc.Average(1, 2, 3).Should().BeOfType(typeof(double));
        }

        [Test]
        [Category("Type")]
        public void Average_Should_RetrunDoubleType([Range(1, 5, 1)] int x, [Random(1, 4, 1)] int y)
        {
            calc.Average(x, y).Should().BeOfType(typeof(double));
        }

        [Test]
        [Category("Main")]
        public void Add_Should_Return6()
        {
            calc.Add(1, 2, 3).Should().Be(6);
        }

        [TestCase(1, 2, 3, ExpectedResult = 6, TestName = "Add_Should_ReturnSum6")]
        [TestCase(1, 3, 4, ExpectedResult = 8, TestName = "Add_Should_ReturnSum8")]
        [TestCase(1, 2, 22, ExpectedResult = 25, TestName = "Add_Should_ReturnSum25")]
        [Category("Main")]
        public int Add_Should_ReturnSum(int x, int y, int z)
        {
            return calc.Add(x, y, z);
        }

        [Test]
        [Repeat(5)]
        [Category("Main")]
        public void Add_Should_Return0()
        {
            calc.Add().Should().Be(0);
        }


        [Test]
        [Category("Main")]
        public void Average_Should_RetrunAverageDouble()
        {
            calc.Average(1, 2, 3, 4).Should().Be(2.5);
        }

        [Test]
        [Ignore("Not implementation yet")]
        [Category("Main")]
        public void Average_Should_ThrowException()
        {
            calc.Average(1, 2, 3, 4).Should().Be(2.5);
        }

        [Test]
        [Category("Main")]
        public void CaclDelay_Should_ThrowExceptionByDelay()
        {
            Action act = () => calc.CalcDelay();

            act.ExecutionTime().ShouldNotExceed(1200.Milliseconds());
        }

        [Test]
        [MaxTime(2000)]
        [Category("Main")]
        public void CaclDelay_Should_Good()
        {
            calc.CalcDelay();
        }

        [Test()]
        public void Add_Should_ReturnSum3()
        {
            var calculator = Substitute.For<ICalculator>();

            calculator.Add(1, 2).Returns(3);

            calculator.Add(1, 2).Should().Be(3);
        }

        [Test()]
        public void Add_Should_Call()
        {
            var calculator = Substitute.For<ICalculator>();
            calculator.Add(1, 2).Returns(3);

            calculator.Add(1, 2);

            calculator.Received().Add(1, 2);
        }

        [Test()]
        public void Mode_Should_ReturnValue()
        {
            var calculator = Substitute.For<ICalculator>();
            calculator.Mode.Returns(test);

            calculator.Mode.Should().Be(test);
        }

        [Test()]
        public void Add_Should_ReturnAnySum()
        {
            var calculator = Substitute.For<ICalculator>();
            calculator.Add(Arg.Any<int>(), Arg.Any<int>())
                      .Returns(x => (int)x[0] + (int)x[1]);

            calculator.Add(1, 3).Should().Be(4);
        }

        [Test()]
        public void Mode_Should_ReturnAnyStrings()
        {
            var calculator = Substitute.For<ICalculator>();

            calculator.Mode.Returns("HEX", "BEX", "FEX");

            calculator.Mode.Should().Be("HEX");
            calculator.Mode.Should().Be("BEX");
            calculator.Mode.Should().Be("FEX");
        }

        [Test()]
        public void Substitute_Should_RealizeDelegate()
        {
            var func = Substitute.For<Func<string>>();

            func().Returns(test);

            func().Should().Be(test);
        }

        [Test()]
        public void Add_Should_ReturnForAnyArgs()
        {
            var calculator = Substitute.For<ICalculator>();

            calculator.Add(1, 2).ReturnsForAnyArgs(100);

            calculator.Add(3, 4).Should().Be(100);
        }

        [Test()]
        public void Add_Should_CallBack3Times()
        {
            int counter = 0;
            var calculator = Substitute.For<ICalculator>();
            calculator.Add(1, 2).ReturnsForAnyArgs((x) =>
            {
                counter++;
                return 0;
            });

            calculator.Add(3, 4);
            calculator.Add(3, 4);
            calculator.Add(3, 4);

            counter.Should().Be(3);
        }

        [Test()]
        public void Add_Should_Throw()
        {
            int counter = 0;
            var calculator = Substitute.For<ICalculator>();
            calculator.Add(1, 2).Returns(x => throw new Exception());

            Action act = () => calculator.Add(1, 2);

            act.ShouldThrow<Exception>();
        }

    }
}
