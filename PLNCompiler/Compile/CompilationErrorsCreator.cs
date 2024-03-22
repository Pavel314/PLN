using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Compile.Errors;

namespace PLNCompiler.Compile
{
    public class CompilationErrorsCreator
    {
        public List<LexicalError> LexicalErrors { get; private set; }
        public List<SyntaxError> SyntaxErrors { get; private set; }
        public List<SemanticError> SemanticErrors { get; private set; }
        public CompilationErrorsCreator()
        {
            LexicalErrors = new List<LexicalError>();
            SyntaxErrors = new List<SyntaxError>();
            SemanticErrors = new List<SemanticError>();
        }

        public CompilationErrors Export()
        {
            if (!HasErrors()) return null;
            var res= new CompilationErrors(LexicalErrors.ToList(), SyntaxErrors.ToList(), SemanticErrors.ToList());
            Reset();
            return res;
        }

        public bool HasErrors()
        {
            return (!LexicalErrors.IsNullOrEmpty() || !SyntaxErrors.IsNullOrEmpty() || !SemanticErrors.IsNullOrEmpty());
        }

        public void Reset()
        {
            LexicalErrors.Clear();
            SyntaxErrors.Clear();
            SemanticErrors.Clear();
        }
    }
}
