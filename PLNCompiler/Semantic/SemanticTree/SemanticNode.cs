using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Semantic.SemanticTree
{
    public abstract class SemanticNode
    {
        public abstract void Visit(ISemanticVisitor visitor);
    }
}
