using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace generics
{
    public class Pair<T1,T2>
    {
        T1 First;
        T2 Second;
        public Pair(T1 first, T2 second)
        {
            this.First = first;
            this.Second = second;
        }
    }
}
