using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
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

        //Монада, в С# 6.0 для этого появился оператор ?.
        public static TOut Maybe<T, TOut>(T obj, Func<T, TOut> func) where T : class
            where TOut: class 
        {
            return obj == null ? null : func(obj);
        }

    }
}
