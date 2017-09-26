using System.Threading;

namespace TryBlocking
{
    //������������� � ���������������� ������ (�������), �� ������������ ����������� ������
    public sealed class SimpleSpinLocker
    {
        private int m_resource = 0;

        public void Enter()
        {
            while (true)
            {
                //���� ����� 1 �����, ���������� ����������, �.�. ����� Exchange ������ 0 � �������� � m_resource = 1, ������������� 2 ����� ��������� � ����������� �����
                if (Interlocked.Exchange(ref m_resource, 1) == 0)
                    return;

            }
        }

        public void Leave()
        {
            //m_resource = 0, ����� ����� ������� ��������� � ����������� ����� ������ �� ���� � �������� ������ � ���������������� �������
            Interlocked.Decrement(ref m_resource);
        }
    }
}