using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.TypeManager
{
    public abstract class CastTypeResult : FunctionResult<TypePair>
    {
        public CastTypeResult(TypePair typePair, bool isSuccessful, NativeFunction nativeFunction) : base(typePair,isSuccessful,nativeFunction)
        {
        }
    }
}
