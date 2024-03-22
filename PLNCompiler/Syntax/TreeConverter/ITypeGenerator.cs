using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Compile.PLNReflection.TypeFinder;

namespace PLNCompiler.Syntax.TreeConverter
{
    public interface ITypeGenerator
    {
        Type GenerateType(SyntaxTree.TypeNode typeNode);
        ITypeFinder TypeFinder { get; }
    }
}
