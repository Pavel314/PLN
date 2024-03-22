using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.TypeManager
{
  public class FunctionResult<TInitiator>:Result<TInitiator>
    {
        private NativeFunction nativeFunction;

        public FunctionResult(TInitiator initiator, bool isSuccessful, NativeFunction nativeFunction):base(initiator, isSuccessful)
        {
            this.nativeFunction = nativeFunction;
        }

        public NativeFunction NativeFunction
        {
            get
            {
                if (!IsSuccessful)
                    throw new InvalidOperationException("IsSuccessfully != true");
                return nativeFunction;
            }
        }
    }
}
