using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.Errors
{
    public abstract class LexicalError : SourceCodeException
    {
        public LexicalError(int errorCode, Syntax.Location location, string message, Exception inner) : base(new ErrorCode(ErrorType.Lexical, errorCode),location, message, inner)
        {
        }
        public LexicalError(int errorCode, Syntax.Location location, string message) : this(errorCode, location, message, null)
        {
        }
    }
}
