using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.TypeManager
{
 public class IsCastResult:CastTypeResult
    {
        public IsCastResult(TypePair typePair, bool isSuccessful, NativeFunction convertFunction) : base(typePair,isSuccessful,convertFunction)
        {
        }

        public static IsCastResult CreateSuccessful(TypePair typePair, NativeFunction convertFunction)
        {
            return new IsCastResult(typePair, true, convertFunction);
        }
    }
}
