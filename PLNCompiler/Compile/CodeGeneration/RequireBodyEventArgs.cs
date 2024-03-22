using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace PLNCompiler.Compile.CodeGeneration
{
    public class RequireBodyEventArgs : EventArgs
    {
        public ILGenerator IL { get; private set; }
        public RequireBodyEventArgs(ILGenerator il)
        {
            IL = il;
        }
    }
}
