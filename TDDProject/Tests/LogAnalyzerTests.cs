using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using LogAn;
using NSubstitute;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class LogAnalyzerTests
    {
        private LogAnalyzer analyzer;

        [SetUp]
        public void Setup()
        {
            //analyzer = MakeAnalyzer();
        }

        //private LogAnalyzer MakeAnalyzer()
        //{
        //    return new LogAnalyzer();
        //}

        //[Test]
        //public void IsValidLogFileName_BadExtension_returnsFalse()
        //{
        //    var result = analyzer.IsValidLogFileName("file.foo");

        //    Assert.False(result);
        //}

        //[TestCase("file.slf")]
        //[TestCase("file.SLF")]
        //public void IsValidLogFileName_GoodExtension_returnsTrue(string file)
        //{
        //    var result = analyzer.IsValidLogFileName(file);

        //    Assert.True(result);
        //}

        //[Test]
        //public void IsValidLogFileName_EmptyFileName_Throws()
        //{ 
        //    var ex = Assert.Catch<ArgumentException>(() => analyzer.IsValidLogFileName(""));

        //    StringAssert.Contains("Имя файла должно быть задано", ex.Message);
        //}

        //[TestCase("file.foo", false)]
        //[TestCase("file.SLF", true)]
        //public void IsValidFileName_WhenCalled_ChangesWasLastFileNameValid(string file, bool expected)
        //{
        //    analyzer.IsValidLogFileName(file);

        //    Assert.AreEqual(expected, analyzer.WasLastFileNameValid);
        //}

        //[Test]
        //public void IsValidFileName_NameSupportedExtension_ReturnsTrue()
        //{
        //    var fakeManager = new FakeExtensionManager();
        //    fakeManager.WillBeValid = true;

        //    var _analyzer = new LogAnalyzer(fakeManager);

        //    bool result = _analyzer.IsValidLogFileName("file.ext");
        //    Assert.True(result);
        //}

        //[Test]
        //public void IsValidFileName_ExtManagerThrowsException_ReturnsFalse()
        //{
        //    var fakeManager = new FakeExtensionManager();
        //    fakeManager.WillThrow = new Exception("Это подделка");

        //    var _analyzer = new LogAnalyzer(fakeManager);

        //    bool result = _analyzer.IsValidLogFileName("file.ext");
        //    Assert.False(result);
        //}

        //[Test]
        //public void Analyze_TooShortFileName_CallsWebService()
        //{
        //    IWebService mockService = Substitute.For<IWebService>();


        //    var log = new LogAnalyzer(mockService);

        //    log.Analyze("file.ext");

        //    mockService.Received().Write("Слишком короткое имя файла: file.ext");

        //}

        [Test]
        public void Returns_ByDefault_WorksForHardCodedArgument()
        {
            IExtensionManager fakeManager = Substitute.For<IExtensionManager>();

            fakeManager.IsValid(Arg.Any<string>()).Returns(true);

            Assert.IsTrue(fakeManager.IsValid("File.txt"));
        }

        [Test]
        public void Returns_ArgAny_Throws()
        {
            IExtensionManager fakeManager = Substitute.For<IExtensionManager>();

            fakeManager.When(x => x.IsValid(Arg.Any<string>())).Do(x => { throw new Exception("fake exception"); });

            //fakeManager.When(x => x.IsValid(Arg.Any<string>())).Throw(new Exception("fake exception"));

            Assert.Throws<Exception>(() => fakeManager.IsValid("file.txt"));
        }

        [Test]
        public void Analyze_LoggerThrows_CallsWebService()
        {
            var mockWebService = Substitute.For<IWebService>();
            var stubLogger = Substitute.For<Ilogger>();

            stubLogger.When(x => x.LogError(Arg.Any<string>())).Do(info => { throw new Exception("fake exception");});

            var analyzer = new LogAnalyzer(stubLogger, mockWebService);

            analyzer.MinNameLength = 10;
            analyzer.Analyze("file.txt");

            mockWebService.Received().Write(Arg.Is<string>(s => s.Contains("fake exception")));

        }

        [Test]
        public void Get_data()
        {
            var stubService = Substitute.For<IWebService>();
            var stubLogger = Substitute.For<Ilogger>();

            stubService.Get().Returns("help");

            var analyzer = new LogAnalyzer(stubLogger, stubService);
            var result = analyzer.GetData();

            StringAssert.Contains("help", result);
        }
    }

    
}
