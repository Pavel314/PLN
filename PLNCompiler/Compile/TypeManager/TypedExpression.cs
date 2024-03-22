using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Semantic.SemanticTree;

namespace PLNCompiler.Compile.TypeManager
{
    public struct TypedExpression
    {
        public readonly ExpressionNode Expression;
        public readonly Type Type;

        public TypedExpression(ExpressionNode expression, Type type)
        {
            Expression = expression;
            Type = type;
        }
    }
}
