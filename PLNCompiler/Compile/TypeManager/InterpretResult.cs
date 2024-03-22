using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.TypeManager
{
  public  class InterpretResult<T>: FunctionResult<T>
    {
        public InterpretResult(T initiator, bool isSuccesful,Type interpType, NativeFunction nativeFunction) : base(initiator, isSuccesful, nativeFunction)
        {
            this.interpType = interpType;
        }

        public Type InterpType
        {
            get
            {
                if (!IsSuccessful)
                    throw new InvalidOperationException("IsSuccessful!=true");
                return interpType;
            }
        }

        private Type interpType;

    }
}
