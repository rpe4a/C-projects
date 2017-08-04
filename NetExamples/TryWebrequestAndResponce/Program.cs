using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TryWebrequestAndResponce
{
    class Program
    {
        static void Main(string[] args)
        {
            var uri = new Uri("https://permsad.permedu.ru/");
            var request = WebRequest.Create(uri);
            var responce = request.GetResponse();
            var stream = responce.GetResponseStream();
            var sr = new StreamReader(stream);

            Console.WriteLine("content-type: {0}", responce.ContentType);
            for (int i = 0; i < responce.Headers.Count; i++)
            {
                Console.WriteLine("header-key: {0}", responce.Headers[i]);
            }

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }

            sr.Close();
            stream.Close();

        }
    }
}
