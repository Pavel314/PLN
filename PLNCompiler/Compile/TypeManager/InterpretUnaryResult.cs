using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.TypeManager
{
  public  class InterpretUnaryResult: InterpretResult<UnaryInitiator>
    {
        protected InterpretUnaryResult(UnaryInitiator initiator,bool isSuccesfull, Type interpType, NativeFunction nativeFunction) : 
            base(initiator,isSuccesfull,interpType,nativeFunction)
        {
        }
        public static InterpretUnaryResult CreateSuccesful(UnaryInitiator initiator, Type interpType, NativeFunction nativeFunction) 
        {
            return new InterpretUnaryResult(initiator, true, interpType, nativeFunction);
        }
        public static InterpretUnaryResult CanNotBeApplied(UnaryInitiator initiator)
        {
            return new InterpretUnaryResult(initiator,false,null,null);
        }
    }
}
