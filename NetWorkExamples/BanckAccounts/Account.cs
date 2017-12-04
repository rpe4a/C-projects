using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanckAccounts
{
    public class Account
    {
        public string Surname { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Surname} {Name}";
        }

        public string GetFullName()
        {
            return $"{Surname} {Name}";
        }
    }
}
