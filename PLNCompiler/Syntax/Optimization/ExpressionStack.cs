using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax.Optimization
{
 public class ExpressionStack:Stack<ExpressionStackValue>
    {
        public ExpressionStack() { }
    }
}
