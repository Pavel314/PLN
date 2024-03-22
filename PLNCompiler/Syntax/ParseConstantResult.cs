using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax
{
    public enum ParseConstantResults { OK, FloatError, IntError }

    public   class ParseConstantResult:Result<Constant>
    {
        public ParseConstantResults Result { get; private set; }
        private ParsableConstant _constant;

        public ParseConstantResult(Constant initiator, ParseConstantResults result, ParsableConstant constant):base(initiator,result==ParseConstantResults.OK)
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
