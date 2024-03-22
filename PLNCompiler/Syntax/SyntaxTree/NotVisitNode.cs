using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax.SyntaxTree
{
   public class NotVisitNode:SyntaxNode
    {
        public NotVisitNode(Location location) : base(location)
        {

        }
    }
}
