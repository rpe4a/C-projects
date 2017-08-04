using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn
{
    public interface IWebService
    {
        void Write(string message);
        string Get();
    }
}
