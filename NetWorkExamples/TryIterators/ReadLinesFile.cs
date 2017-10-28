using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryIterators
{
    public static class ReadLinesFile
    {
        public static IEnumerable<string> DoIt(Func<StreamReader> reader)
        {
            string line = string.Empty;

            using (var _reader = reader())
            {
                while ((line =_reader.ReadLine()) != null)
                { 
                    yield return line;
                }
            }
        }
    }

}
