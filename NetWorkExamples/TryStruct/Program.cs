using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryStruct
{
    struct Point2D:IEquatable<Point2D>
    {
        private List<int> a;
        public int X;
        public int Y;

        public override bool Equals(object obj)
        {
            if (!(obj is Point2D))
            {
                return false;
            }

            var d = (Point2D)obj;
            return X == d.X && Y == d.Y;
        }

        public bool Equals(Point2D point2D)
        {
            return X == point2D.X && Y == point2D.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = -24562241;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<int>>.Default.GetHashCode(a);
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Point2D d1, Point2D d2)
        {
            return d1.Equals(d2);
        }

        public static bool operator !=(Point2D d1, Point2D d2)
        {
            return !(d1 == d2);
        }


    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
