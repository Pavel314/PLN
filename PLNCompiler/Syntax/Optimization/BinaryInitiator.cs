using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax.Optimization
{
    public struct BinaryInitiator
    {
        public readonly ParsableConstant Left;
        public readonly ParsableConstant Right;
        public readonly BinaryOperation Operation;

        public BinaryInitiator(ParsableConstant left, ParsableConstant right, BinaryOperation operation)
        {
            Left = left;
            Right = right;
            Operation = operation;
        }
    }
}
