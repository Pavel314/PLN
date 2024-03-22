using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax.Optimization
{
    public class ConstantInterpreterResultUnary : ConstantInterpreterResult<UnaryInitiator>
    {
        public ConstantInterpreterResultUnary(UnaryInitiator inititator, ConstantInterpreterResults result, ParsableConstant constant) : base(inititator, result, constant)
        {
        }

        public ConstantInterpreterResultUnary(UnaryInitiator inititator, ParsableConstant constant) : this(inititator, ConstantInterpreterResults.OK, constant)
        {
            if (ReferenceEquals(constant, null))
                throw new ArgumentNullException("constant");
        }
    }
}
