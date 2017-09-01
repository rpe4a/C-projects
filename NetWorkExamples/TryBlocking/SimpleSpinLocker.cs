using System.Threading;

namespace TryBlocking
{
    //Синхронизация в пользовательском режиме (быстрая), не поддерживает рекурсивные вызовы
    public sealed class SimpleSpinLocker
    {
        private int m_resource = 0;

        public void Enter()
        {
            while (true)
            {
                //Если зашел 1 поток, возвращает управление, т.к. метод Exchange вернет 0 и присвоит в m_resource = 1, следовательно 2 поток застрянет в бесконечном цикле
                if (Interlocked.Exchange(ref m_resource, 1) == 0)
                    return;

            }
        }

        public void Leave()
        {
            //m_resource = 0, тогда поток который находился в бесконченом цикле выйдет из него и порлучит доступ к заблокированному ресурсу
            Interlocked.Decrement(ref m_resource);
        }
    }
}