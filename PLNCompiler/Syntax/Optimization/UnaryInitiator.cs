using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax.Optimization
{
    public struct UnaryInitiator
    {
        public readonly ParsableConstant Right;
        public readonly UnaryOperation Operation;

        public UnaryInitiator(ParsableConstant right, UnaryOperation operation)
        {
            Right = right;
            Operation = operation;
        }
    }

}
