using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace generics
{
    public class Pair<T1,T2>
    {
        public T1 First;
        public T2 Second;
        public Pair(T1 first, T2 second)
        {
            this.First = first;
            this.Second = second;
        }
    }

    public class Vector
    {
        public double X;
        public double Y;
        public Vector(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
