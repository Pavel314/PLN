using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax.Optimization
{
    public class ConstantInterpreterResultBinary : ConstantInterpreterResult<BinaryInitiator>
    {
        public ConstantInterpreterResultBinary(BinaryInitiator inititator, ConstantInterpreterResults result, ParsableConstant constant) : base(inititator, result, constant)
        {
        }

        public ConstantInterpreterResultBinary(BinaryInitiator inititator, ParsableConstant constant) : this(inititator, ConstantInterpreterResults.OK, constant)
        {
            if (ReferenceEquals(constant, null))
                throw new ArgumentNullException("constant");
        }

    }
}
