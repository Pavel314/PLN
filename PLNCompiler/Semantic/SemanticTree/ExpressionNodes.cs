using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Compile.TypeManager;
using System.Reflection;
using System.Reflection.Emit;

namespace PLNCompiler.Semantic.SemanticTree
{
    public class ConstantNode : ExpressionNode
    {
        public TypedConstant Constant { get; private set; }

        public ConstantNode(TypedConstant constant)
        {
            Constant = constant;
        }

        public ConstantNode(TypedConstant constant, NativeFunction convFunction)
        {
            Constant = constant;
            if (!ReferenceEquals(convFunction, null))
                NativeFunctions.Add(convFunction);
        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitConstantNode(this);
        }
    }



    public class VarAccessNode : ExpressionNode
    {
        public LocalWrapper Variable { get; private set; }
        public readonly bool IsRef;

        public VarAccessNode(LocalWrapper variable, bool isRef)
        {
            Variable = variable;
            IsRef = isRef;
        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitVarAccessNode(this);
        }
    }



    public class MemberAccessNode : ExpressionNode
    {
        public RootMemberNode Root { get; private set; }

        public MemberAccessNode(RootMemberNode root)
        {
            Root = root;
        }


        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitMemberAccessNode(this);
        }
    }



    //public abstract class InstanceLocalNode:ExpressionNode
    //{
    //    public LocalWrapper LocalWrapper { get; private set; }
    //}

    public class UnaryNode : ExpressionNode
    {
        public UnaryOperation Operation { get; private set; }
        public ExpressionNode Expression { get; private set; }


        public UnaryNode(UnaryOperation operation, ExpressionNode expression)
        {
            Operation = operation;
            Expression = expression;
        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitUnaryNode(this);
        }
    }


    public class NewUnaryNode : ExpressionNode
    {
        public CreateObjectNode Child { get; private set; }
        public NewUnaryNode(CreateObjectNode child)
        {
            Child = child;
        }
        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitNewUnaryNode(this);
        }
    }



    public class BinaryNode : ExpressionNode
    {
        public BinaryOperation Operation { get; private set; }
        public ExpressionNode Left { get; private set; }
        public ExpressionNode Right { get; private set; }
        public bool LeftRightIsBool { get; private set; }
        public NativeFunction OperatorFuction { get; private set; }

        public BinaryNode(BinaryOperation operation, bool leftRightIsBool, NativeFunction operatorFunction, ExpressionNode left, ExpressionNode rigt)
        {
            Operation = operation;
            LeftRightIsBool = leftRightIsBool;
            OperatorFuction = operatorFunction;
            Left = left;
            Right = rigt;
        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitBinaryNode(this);
        }
    }


    public class TypeOfNode : ExpressionNode
    {
        public Type ArgumentType { get; private set; }

        public TypeOfNode(Type argumentType)
        {
            ArgumentType = argumentType;
        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitTypeOfNode(this);
            //throw new NotImplementedException();
        }
    }


    public abstract class CastNode : ExpressionNode
    {
        public Type Type { get; private set; }
        public ExpressionNode Expression { get; private set; }
        public CastNode(Type type, ExpressionNode expression, NativeFunction first)
        {
            Type = type;
            Expression = expression;
            if (!ReferenceEquals(first, null))
                NativeFunctions.Add(first);
        }
    }



    public class CastTypeNode : CastNode
    {
        public CastTypeNode(Type type, ExpressionNode expression, NativeFunction first):base(type,expression,first)
        {

        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitCastTypeNode(this);
        }
    }



    public class IsCastTypeNode : CastNode
    {
        public IsCastTypeNode(Type type, ExpressionNode expression, NativeFunction first):base(type,expression,first)
        {

        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitIsCastTypeNode(this);
        }
    }



    public class AsCastTypeNode : CastNode
    {
        public AsCastTypeNode(Type type, ExpressionNode expression, NativeFunction first) : base(type, expression, first)
        {

        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitAsCastTypeNode(this);
        }
    }
}
