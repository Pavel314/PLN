using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax.Optimization
{
  public interface IExpressionOptimizer
    {
        event ConstantNeedingCallback ConstantNeeding;
        SyntaxTree.ExpressionNode Optimize(SyntaxTree.ExpressionNode expressionNode);
    }
}
