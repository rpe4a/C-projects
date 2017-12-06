using FluentAssertions;
using NUnit.Framework;

namespace BanckAccounts.Tests
{
    //Вызывается первым перед любыми тестами в наймспайс
    [SetUpFixture]
    public class MySetUpClass
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            // ...
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            // ...
        }
    }

    [TestFixture]
    
    public class AccountTests
    {
        [Test]
        public void ToStringToString_Schould_ReturnName()
        {
            var account = new Account {Surname="Nolly", Name = "Ellise"};






            account.ToString().Should().Be("Nolly Ellise");
        }

        [Test]
        public void FullName_Should4Times_ReturnFullName([Values("A", "B")] string surname, [Values("A", "B")] string name)
        {
            var account = new Account { Surname = surname, Name = name };

            account.GetFullName().Should().BeEquivalentTo($"{surname} {name}");
        }

        [Test]
        [Sequential]
        public void FullName_Should2Times_ReturnFullName([Values("A", "B")] string surname, [Values("A", "B")] string name)
        {




            var account = new Account { Surname = surname, Name = name };

            account.GetFullName().Should().BeEquivalentTo($"{surname} {name}");
        }
    }
}
