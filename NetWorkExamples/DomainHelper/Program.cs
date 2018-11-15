using System;
using System.IO;

namespace DomainHelper
{
    internal class Program
    {
        private const string TestDirectory = @"C:\DomainTestingDirectory";

        private static void Main(string[] args)
        {
            if (!Directory.Exists(TestDirectory))
                Directory.CreateDirectory(TestDirectory);

            File.Create($"{TestDirectory}\\{DateTime.Now.Ticks}.txt");
        }
    }
}