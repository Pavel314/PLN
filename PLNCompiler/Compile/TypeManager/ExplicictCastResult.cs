using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.TypeManager
{
   public class ExplicictCastResult:CastTypeResult
    {
        public ExplicictCastResult(TypePair typePair, bool isSuccessful, NativeFunction convertFunction) : base(typePair,isSuccessful,convertFunction)
        {
        }

        public static ExplicictCastResult CreateSuccessful(TypePair typePair, NativeFunction convertFunction)
        {
            return new ExplicictCastResult(typePair, true, convertFunction);
        }
    }
}
