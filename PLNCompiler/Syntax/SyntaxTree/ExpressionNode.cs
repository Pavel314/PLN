using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax.SyntaxTree
{
    public abstract class ExpressionNode : SyntaxNode
    {
        public abstract void Visit(IExpressionVisitor visitor);
        public ExpressionNode(Location location) : base(location) { }

    }
}
