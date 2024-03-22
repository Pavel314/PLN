using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.PLNReflection.TypeFinder.TypeTree
{
    public interface ITypeTreeVisitor
    {
        void VisitNamespaceNode(NamespaceNode node);
        void VisitTypeNode(TypeNode node);
    }
}
