using System;

namespace TryTypeObject.project
{
    class TestClass : IFactory<TestClass>
    {
        private int _p1;
        private string _p2;

        public TestClass()
        {
            _p1 = 5;
            _p2 = "qwerty";
        }

        //Вызывается только один раз! для всех объектов данного типа
        static TestClass() { }

        public TestClass(string p2) : this()
        {
            this._p2 = p2;
        }
        public TestClass(int p1) : this()
        {
            this._p1 = p1;
        }

        public TestClass CreateInstance()
        {
            return new TestClass();
        }

        public static void StaticSend(string message)
        {
            Console.WriteLine(message);
        }

        public void ErrorSend(string message)
        {
            throw new Exception("Error");
        }

        public void Send(string message)
        {
            Console.WriteLine(this._p2 + " " + message);
        }
    }
}