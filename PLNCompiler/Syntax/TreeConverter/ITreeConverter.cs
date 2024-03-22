using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Syntax.Optimization;
using PLNCompiler.Semantic.SemanticTree;
using PLNCompiler.Syntax.SyntaxTree;
using PLNCompiler.Compile.TypeManager;

namespace PLNCompiler.Syntax.TreeConverter
{
    public interface ITreeConverter
    {
        ITypeGenerator TypeGenerator { get; }
        ITypeConversion TypeConversion { get; }
        IExpressionOptimizer ExpressionOptimizer { get; }
        //SemanticNode ConvertToSemanticTree(SyntaxTree.StatementNode node);
        SemanticMain GenerateSemanticTree(SyntaxTree.StatementNode node);
    }
}
