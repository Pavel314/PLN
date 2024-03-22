using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Compile.Errors;

namespace PLNCompiler.Compile
{
   public class CompilationErrors
    {
        public IReadOnlyList<LexicalError> LexicalErrors { get; private set; }
        public IReadOnlyList<SyntaxError> SyntaxErrors { get; private set; }
        public IReadOnlyList<SemanticError> SemanticErrors { get; private set; }

        public CompilationErrors(IReadOnlyList<LexicalError> lexicalErrors, IReadOnlyList<SyntaxError> syntaxErrors, IReadOnlyList<SemanticError> semanticErrors)
        {
            LexicalErrors = lexicalErrors;
            SyntaxErrors = syntaxErrors;
            SemanticErrors = semanticErrors;
        }
    }
}
