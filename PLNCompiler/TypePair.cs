using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler
{
    public struct TypePair
    {
        public TypePair(Type type1, Type type2)
        {
            Type1 = type1;
            Type2 = type2;
        }

        public readonly Type Type1;
        public readonly Type Type2;
    }
}
