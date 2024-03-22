using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.TypeManager
{
    public enum AsResult {OK,CannotBeApplied,TargetTypeIsValueType }

 public class AsCastResult:CastTypeResult
    {
        public AsResult Result { get; private set; }

        public AsCastResult(TypePair typePair, AsResult result, NativeFunction convertFunction) : base(typePair,result==AsResult.OK,convertFunction)
        {
            Result = result;
        }
        public static AsCastResult CreateSuccessful(TypePair typePair, NativeFunction convertFunction)
        {
            return new AsCastResult(typePair,AsResult.OK, convertFunction);
        }
    }
}
