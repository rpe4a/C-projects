using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TryDomains
{
    class Program
    {
        private const string FileCreatorExe = "DomainHelper.exe";
        private const string DomainHelperDirectory = @"DomainHelper\bin\Debug\";
        private static readonly string MainDomainDirectory = Environment.CurrentDirectory;

        private static readonly string RootDirectory =
            new DirectoryInfo(MainDomainDirectory).Parent.Parent.Parent.FullName;


        static void Main(string[] args)
        {
            Console.WriteLine("Setup domain.");

            Setup();

            var appDomainSetup = new AppDomainSetup
                {ApplicationBase = Path.Combine(RootDirectory, DomainHelperDirectory)};

            Console.WriteLine("Run domain.");
            var domain = AppDomain.CreateDomain("FileCreator", null, appDomainSetup);
            domain.ExecuteAssembly("DomainHelper.exe");

            Console.WriteLine("Unload domain.");
            AppDomain.Unload(domain);
        }

        private static void Setup()
        {
            AppDomain.MonitoringIsEnabled = true;

            CopyFileCreatorExe();
        }

        private static void CopyFileCreatorExe()
        {
            var FileCreatorExeMainDomainPath = Path.Combine(MainDomainDirectory, FileCreatorExe);

            if (File.Exists(FileCreatorExeMainDomainPath))
                File.Delete(FileCreatorExeMainDomainPath);
            else
            {

            }



            File.Copy(Path.Combine(RootDirectory, DomainHelperDirectory, FileCreatorExe),
                Path.Combine(MainDomainDirectory, FileCreatorExe));
        }
    }
}