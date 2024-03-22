using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.TypeManager
{
  public class ImplicitCastResult:CastTypeResult
    {
        public ImplicitCastResult(TypePair typePair, bool isSuccessful, NativeFunction convertFunction) : base(typePair,isSuccessful,convertFunction)
        {
        }
        public static ImplicitCastResult CreateSuccessful(TypePair typePair, NativeFunction convertFunction)
        {
            return new ImplicitCastResult(typePair, true, convertFunction);
        }
    }
}
