using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Compile.TypeManager;

namespace PLNCompiler.Semantic.SemanticTree
{
  public abstract class ExpressionNode:SemanticNode
    {
        private AddOnlyList<NativeFunction> nativeFunctions;

        public AddOnlyList<NativeFunction> NativeFunctions
        {
            get
            {
                if (nativeFunctions == null)
                    nativeFunctions = new AddOnlyList<NativeFunction>();
                return nativeFunctions;
            }
        }

        public ExpressionNode()
        {
        }

        public void EmitNativeFunctions(System.Reflection.Emit.ILGenerator il)
        {
            if (nativeFunctions.IsNullOrEmpty()) return;
            foreach (var function in NativeFunctions)
                function.Emit(il);
        }

    }
}
