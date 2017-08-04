using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryExtentionMethods
{
    public static class ExtentionMethods
    {
        public static bool IsNull(this object x)
        {
            return x == null;
        }

        public static bool IsNullOrEmpty(this string x)
        {
            return string.IsNullOrEmpty(x);
        }
    }
}
