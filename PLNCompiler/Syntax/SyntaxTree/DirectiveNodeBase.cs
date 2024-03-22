using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax.SyntaxTree
{
    public abstract class DirectiveNodeBase:SyntaxNode
    {
        public abstract void Visit(IDirectiveVisitor visitor);

        public DirectiveNodeBase(Location location):base(location)
        {
            Location = location;
        }

    }
}
