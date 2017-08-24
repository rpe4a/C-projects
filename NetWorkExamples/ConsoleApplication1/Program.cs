using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {

        static bool Beetree(int x1, int y1, int x2, int y2)
        {
            bool result = false;

            if (x1 + y1 >= x2 + y2)
                result = false;

            if (x1 + y1 < x2 + y2)
            {
                if ((x2 == x1 + y1 && y1 == y2) || (x2 == x1 && y2 == x1 + y1))
                {
                    result = true;
                }
                else
                {
                    if(Beetree(x1 + y1, y1, x2, y2))
                        result = true;
                    else
                        result = Beetree(x1 , x1+ y1, x2, y2);
                }
            }

            return result;
        }

        static void Main(string[] args)
        {
            int x1 = 1, y1 = 4, x2 = 7, y2 = 6;
            int xResult=x1, yResult=y1;

            
            
            

            Console.WriteLine((Beetree(x1, y1, x2, y2)) ? "Yes" : "No");


            //string[] arrStr = new string[] {"<<<><><>"};
            //int[] maxRep = new int[] {2};
            //var arrResult = new int[arrStr.Length];

            //for (int i = 0; i < arrStr.Length; i++)
            //{

            //    //var str = arrStr[i];
            //    //for (int j = 0; j < str.Length - 1; j++)
            //    //{
            //    //    char l = str[j];
            //    //    char r = str[j + 1];
            //    //    if (l == r)
            //    //        str = r == '<' ? '>' : r;
            //    //}

            //    if (new[] {">>"}.Contains(arrStr[i]))
            //    {
            //        arrResult[i] = 0;
            //        continue;
            //    }

            //    var str = arrStr[i].Replace("<>", "");


            //    var lSymbol = str.Count(x => x.Equals('<'));
            //    var rSymbol = str.Count(x => x.Equals('>'));

            //    if (lSymbol > 0)
            //    {
            //        arrResult[i] = 0;
            //        continue;
            //    }

            //    if(rSymbol <= maxRep[i])
            //        arrResult[i] = 1;
            //    else
            //        arrResult[i] = 0;

            //}

            //foreach (var i in arrResult)
            //{
            //    Console.WriteLine(i);
            //}
        }
    }
}


//var result = Math.Abs(lSymbol - rSymbol);

//                if (result == 0 && maxRep[i] > 0)
//                {
//                    arrResult[i] = 1;
//                    continue;
//                }

//                arrResult[i] = maxRep[i] != 0 && result == maxRep[i] ? 1 : 0;
