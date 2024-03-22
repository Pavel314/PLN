using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.TypeManager
{
    public struct UnaryInitiator
    {
        public readonly UnaryOperation Operation;
        public readonly TypedExpression Right;

        public UnaryInitiator(UnaryOperation operation, TypedExpression right)
        {
            Operation = operation;
            Right = right;
        }
    }
}
