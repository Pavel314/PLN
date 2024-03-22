using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Syntax;

namespace PLNCompiler.Syntax.Optimization
{
    public enum ExpressionStackValueKind { Other, Constant, UnaryOperation, BinaryOperation }

    public struct ExpressionStackValue
    {
        public readonly ExpressionStackValueKind Kind;
        public readonly object Value;
        private ExpressionStackValue(ExpressionStackValueKind kind, object value)
        {
            Kind = kind;
            Value = value;
        }

        public ExpressionStackValue(BinaryOperation operation) : this(ExpressionStackValueKind.BinaryOperation, operation)
        {
        }

        public ExpressionStackValue(UnaryOperation operation) : this(ExpressionStackValueKind.UnaryOperation, operation)
        {
        }

        public ExpressionStackValue(ParsableConstant constant) : this(ExpressionStackValueKind.Constant, constant)
        {
        }

        public ExpressionStackValue(object value) : this(ExpressionStackValueKind.Other, value)
        {
        }

        public override string ToString()
        {
            return string.Format("ExpressionStackValue:[Kind={0} Value{1}]", Kind, Value);
        }

    }
}
