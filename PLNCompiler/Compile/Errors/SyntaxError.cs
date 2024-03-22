using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.Errors
{
    public abstract class SyntaxError : SourceCodeException
    {
        public SyntaxError(int errorCode, Syntax.Location location, string message, Exception inner) : base(new ErrorCode(ErrorType.Syntax, errorCode),location, message, inner)
        {
        }
        public SyntaxError(int errorCode, Syntax.Location location, string message) : this(errorCode, location, message, null)
        {
        }
    }
}
