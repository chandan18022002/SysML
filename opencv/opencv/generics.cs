using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace generics
{
    public class Pair<T1,T2>
    {
        T1 first;
        T2 second;
        public Pair(T1 first, T2 second)
        {
            this.first = first;
            this.second = second;
        }
    }
}
