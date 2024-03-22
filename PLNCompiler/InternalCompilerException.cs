using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler
{
    [Serializable]
    public class InternalCompilerException : PLNException
    {
        public InternalCompilerException() : base() { }
        public InternalCompilerException(string desc) : base(desc) { }
        protected InternalCompilerException(string desc, Exception inner) : base(desc, inner) { }
    }
}
