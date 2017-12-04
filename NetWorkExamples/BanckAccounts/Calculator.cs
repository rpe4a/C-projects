using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BanckAccounts
{
    public interface ICalculator
    {
        int Add(int a, int b);
        string Mode { get; set; }

        event EventHandler PoweringUp;
    }

    public class Calculator:ICalculator
    {
        public double Average(params int[] array)
        {
            return array.Sum() / (double)array.Length;
        }

        public int Add(params int[] array)
        {
            return array.Sum();
        }

        public void CalcDelay()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        public int Add(int a, int b)
        {
            throw new NotImplementedException();
        }

        public string Mode { get; set; }
        public event EventHandler PoweringUp;
    }
}
