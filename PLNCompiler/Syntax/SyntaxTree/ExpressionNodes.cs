using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Compile;

namespace PLNCompiler.Syntax.SyntaxTree
{
    public abstract class ConstNodeBase<TConstant>: ExpressionNode
    {
        public TConstant Constant { get; private set; }
        public ConstNodeBase(TConstant constant,Location location):base(location)
        {
            Constant = constant;
            Location = location;
        }
    }



    public class ConstNode : ConstNodeBase<Constant>
    {
        public ConstNode(Constant constant, Location location):base(constant, location)
        {
        }

        public override void Visit(IExpressionVisitor visitor)
        {
            visitor.VisitConstantNode(this);
        }
    }



    public class ParsableConstNode : ConstNodeBase<ParsableConstant>
    {
        public ParsableConstNode(ParsableConstant constant, Location location) : base(constant, location)
        {
        }

        public override void Visit(IExpressionVisitor visitor)
        {
            visitor.VisitParsableConstantNode(this);
        }
    }



    public class UnaryNode : ExpressionNode
    {
        public ExpressionNode Right { get; private set; }
        public UnaryOperation Operation { get; private set; }
        public UnaryNode(ExpressionNode right, UnaryOperation operation, Location location) :base(location)
        {
            Right = right;
            Operation = operation;
        }
        public override void Visit(IExpressionVisitor visitor)
        {
            visitor.VisitUnaryNode(this);
        }
    }

    public enum NewUnaryKind { Array, Object }
    public class NewUnaryNode:ExpressionNode
    {
        public TypeNode Type { get; private set; }
        public MethodArgumentsCollection Arguments { get; private set; }
        public IndexerArgumentsCollection Indexs { get; private set; }
        public NewUnaryKind Kind { get; private set; }
        public NewUnaryNode(TypeNode type, MethodArgumentsCollection arguments, IndexerArgumentsCollection indexs, NewUnaryKind kind, Location location):base(location)
        { 
            Type = type;
            Arguments = arguments;
            Indexs = indexs;
            Kind = kind;
        }

        public bool HasArguments()
        {
            return !Arguments.IsNullOrEmpty();
        }

        public bool HasIndexes()
        {
            return !Indexs.IsNullOrEmpty();
        }

        public NewUnaryNode(TypeNode type, MethodInfo info, NewUnaryKind kind, Location location) : this(type, info.Arguments, info.Indexes, kind, location)
        {

        }

        public override void Visit(IExpressionVisitor visitor)
        {
            visitor.VisitNewUnaryNode(this);
        }
    }





    public class BinaryNode : ExpressionNode
    {
        public ExpressionNode Left { get; private set; }
        public ExpressionNode Right { get; private set; }
        public BinaryOperation Opration { get; private set; }
        public BinaryNode(ExpressionNode left, ExpressionNode right, BinaryOperation operation,Location location):base(location)
        {
            Left = left;
            Right = right;
            Opration = operation;
        }
        public override void Visit(IExpressionVisitor visitor)
        {
            visitor.VisitBinaryNode(this);
        }
    }



    public class MemberAccessNode : ExpressionNode
    {
        public ExpressionNode ParentExpression { get; private set; }
        public MemberNode ChildMember { get; private set; }
        public MemberAccessNode(ExpressionNode parentExpression, MemberNode childMember, Location location) : base(location)
        {
            ChildMember = childMember;
            ParentExpression = parentExpression;

        }

        public override void Visit(IExpressionVisitor visitor)
        {
            visitor.VisitMemberAccessNode(this);
        }
    }


    public abstract class CastNode : ExpressionNode
    {
        public TypeNode Type { get; private set; }
        public ExpressionNode Expression { get; private set; }
        public CastNode(TypeNode type, ExpressionNode expression, Location location) : base(location)
        {
            Type = type;
            Expression = expression;
        }
    }



    public class ExplicitCastNode:CastNode
    {
        public ExplicitCastNode(TypeNode type,ExpressionNode expression,Location location):base(type,expression,location)
        {
        }

        public override void Visit(IExpressionVisitor visitor)
        {
            visitor.VisitExplicitCastNode(this);
        }
    }



    public class AsCastNode:CastNode
    {
        public AsCastNode(ExpressionNode expression, TypeNode type, Location location):base(type,expression,location)
        {
        }

        public override void Visit(IExpressionVisitor visitor)
        {
            visitor.VisitAsCastNode(this);
        }
    }



    public class IsCastNode : CastNode
    {
        public IsCastNode(ExpressionNode expression, TypeNode type, Location location) : base(type, expression, location)
        {
        }

        public override void Visit(IExpressionVisitor visitor)
        {
            visitor.VisitIsCastNode(this);
        }
    }
}
