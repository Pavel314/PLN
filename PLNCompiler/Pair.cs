using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler
{
    public struct Pair<TValue1, TValue2>
    {
        public Pair(TValue1 value1, TValue2 value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public readonly TValue1 Value1;
        public readonly TValue2 Value2;
    }
}
