using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrySerialization
{
    [Serializable]
    internal sealed class MyType
    {
        private double m_param1;

        //Не сериализуется, т.к. является вычисляемым полем
        [NonSerialized]
        private double m_param2;

        public double Param1
        {
            get { return m_param1; }
        }

        public double Param2
        {
            get { return m_param2; }
        }

        public MyType(double param)
        {
            m_param1 = param;
            m_param2 = 10 * param * param;
        }

        //Вызывается при десериализации
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            m_param2 = 10 * m_param1 * m_param1;
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
        }
        [OnSerialized]
        private void OnSerialized(StreamingContext context)
        { }
        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
        }

    }
}
