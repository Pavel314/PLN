using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace PLNCompiler.Compile.CodeGeneration
{
    public class RequireLoopBodyEventArgs : RequireBodyEventArgs
    {
        public Label BeginLabel { get; protected set; }
        public Label ExitLabel { get; protected set; }
        public LocalBuilder LoopCounter { get; protected set; }

        public RequireLoopBodyEventArgs(ILGenerator il, LocalBuilder loopCounter, Label beginLabel, Label exitLabel) : base(il)
        {
            LoopCounter = loopCounter;
            BeginLabel = beginLabel;
            ExitLabel = exitLabel;
        }
    }
    public delegate void RequireLoopBody(object sender, RequireLoopBodyEventArgs args);
}
