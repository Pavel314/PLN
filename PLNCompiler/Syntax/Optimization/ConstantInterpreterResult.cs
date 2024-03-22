using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Syntax;

namespace PLNCompiler.Syntax.Optimization
{
    public enum ConstantInterpreterResults { OK, OverflowError, InvalidOperation }

    public class ConstantInterpreterResult<TInitiator>:Result<TInitiator>
    {
        public ConstantInterpreterResults Result { get; private set; }
        private ParsableConstant _constant;
        public ConstantInterpreterResult(TInitiator inititator, ConstantInterpreterResults result, ParsableConstant constant) :base(inititator, result == ConstantInterpreterResults.OK)
        {
            Result = result;
            _constant = constant;
        }
        public ParsableConstant Constant
        {
            get
            {
                if (!IsSuccessful)
                    throw new InvalidOperationException("IsSuccessfully != true");
                return _constant;
            }
        }
    }


}
