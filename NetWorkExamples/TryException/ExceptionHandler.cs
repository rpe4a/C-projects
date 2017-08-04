using System;

namespace TryException
{
    internal static class ExceptionHandler
    {
        public static void TryCatchTypeException<T1, TypeEx>(T1 a) where TypeEx : Exception
            where T1 : class
        {
            if (a == null)
                throw GenerateException<TypeEx>(typeof(TypeEx));
        }

        public static void TryCatchTypeException<T1, T2, TypeEx>(T1 a, T2 b) where TypeEx : Exception
            where T1 : class
            where T2 : class
        {
            if (a == null || b == null)
                throw GenerateException<TypeEx>(typeof(TypeEx));
        }

        public static void TryCatchStructException<TypeEx>(Func<bool> catchFunc) where TypeEx : Exception     
        {
            if (catchFunc())
                throw GenerateException<TypeEx>(typeof(TypeEx));
        }

        private static TypeEx GenerateException<TypeEx>(Type type) where TypeEx : Exception
        {
            return (TypeEx)Activator.CreateInstance(type);
        }
    }
}