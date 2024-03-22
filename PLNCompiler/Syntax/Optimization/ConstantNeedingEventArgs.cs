using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax.Optimization
{
    public class ConstantNeedingEventArgs : EventArgs
    {
        public SyntaxTree.MemberAccessNode MemberAccessNode { get; private set;}

        public ConstantNeedingEventArgs(SyntaxTree.MemberAccessNode memberAccessNode)
        {
            MemberAccessNode = memberAccessNode;
        }
    }
    public delegate ParsableConstant ConstantNeedingCallback(object sender, ConstantNeedingEventArgs e);
}
