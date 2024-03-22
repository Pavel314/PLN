using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace PLNCompiler.Compile.CodeGeneration
{
    public class RequireIFBodyEventArgs : RequireBodyEventArgs
    {
        public RequireIFBodyEventArgs(ILGenerator il) : base(il)
        {
        }
    }
    public delegate void RequireIFBody(object sender, RequireIFBodyEventArgs args);
}
