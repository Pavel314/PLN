using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Syntax;
using PLNCompiler.Semantic;

namespace PLNCompiler.Compile.TypeManager
{
    public interface ITypeConversion
    {
        InterpretUnaryResult InterpretUnary(UnaryOperation operation, TypedExpression right);
        InterpretBinaryResult InterpretBinary(BinaryOperation operation, TypedExpression left, TypedExpression right);
        CastTypeResult ImplicitCast(Type primary, Type target);
        CastTypeResult ExplicictCast(Type primary, Type target);
        IsCastResult IsCast(Type primary, Type target);
        AsCastResult AsCast(Type primary, Type target);
        TypedConstant AssignConstant(ParsableConstant constant, Type target);
    }
}
