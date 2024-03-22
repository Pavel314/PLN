using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.TypeManager.NativeConversion
{
    public struct CastInfo
    {
        public readonly NativeFunction NativeFunction;
        public readonly WeightInfo Weight;

        public CastInfo(NativeFunction nativeFunction, WeightInfo weight)
        {
            NativeFunction = nativeFunction;
            Weight = weight;
        }
    }
}
