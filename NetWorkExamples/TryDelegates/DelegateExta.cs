using System;

namespace TryDelegates
{
    public static class DelegateExta
    {
        public static void RunChainDelegates(this MulticastDelegate chain)
        {
            var ArrayDelegates = chain.GetInvocationList();

            foreach (var @delegate in ArrayDelegates)
            {
                try
                {
                    //Проблемы с производитеьностью, так что лучше использовать отдельный метод RunChainDelegates
                    @delegate.DynamicInvoke("Hello");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

    }
}