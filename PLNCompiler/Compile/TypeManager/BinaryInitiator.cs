using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.TypeManager
{
   public struct BinaryInitiator
    {
        public readonly BinaryOperation Operation;
        public readonly TypedExpression Left;
        public readonly TypedExpression Right;

        public BinaryInitiator(BinaryOperation operation, TypedExpression left, TypedExpression right)
        {
            Operation = operation;
            Left = left;
            Right = right;
        }
    }
}
