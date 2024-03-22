using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PLNCompiler.Syntax;
using PLNCompiler.Syntax.SyntaxTree;
using PLNCompiler.Syntax.TreeConverter;
using PLNCompiler.Syntax.Analysis;
using QUT.Gppg;

namespace PLNCompiler.Compile
{
    public interface ICompiler
    {
        //   AbstractScanner<ValueType,Location> Scanner { get; }
        // ShiftReduceParser<ValueType, Location> Parser { get; }
        CompilationResult CompileFromFile(ComilerSettings settings, string soureceCodePath);
        CompilationResult CompileFromStream(ComilerSettings settings, Stream stream, int fallbackCodePage);
        CompilationResult CompileFromSyntaxTree(ComilerSettings settings, ProgramNode programNode);
        //ComilerSettings Settings { get; }
    }
}
