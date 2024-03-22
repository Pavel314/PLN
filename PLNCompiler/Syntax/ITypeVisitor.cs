using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax
{
    public interface ITypeVisitor
    {
        void VisitTypeNameNode(SyntaxTree.TypeNameNode node);
        void VisitArrayTypeNode(SyntaxTree.ArrayTypeNode node);
        void VisitImportsNode(SyntaxTree.ImportsNode node);
        void VisitImportNode(SyntaxTree.ImportNode node);
        void VisitUsingsNode(SyntaxTree.UsingsNode node);
        void VisitUsingNode(SyntaxTree.UsingNode node);
    }
}
