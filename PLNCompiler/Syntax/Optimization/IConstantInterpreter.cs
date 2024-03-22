using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax.Optimization
{
  public interface IConstantInterpreter
    {
         ConstantInterpreterResultBinary Compute(ParsableConstant left, ParsableConstant right, BinaryOperation operation);
         ConstantInterpreterResultUnary Compute(ParsableConstant constant, UnaryOperation operation);
    }
}
