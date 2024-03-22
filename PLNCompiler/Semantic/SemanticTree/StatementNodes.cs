using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using PLNCompiler.Compile.TypeManager;

namespace PLNCompiler.Semantic.SemanticTree
{
    public class BlockNode : StatementNode
    {
        public StatementCollection Statements { get; private set; }

        public BlockNode()
        {
            Statements = new StatementCollection();
        }

        public BlockNode(StatementNode first) : this()
        {
            Statements.Add(first);
        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitBlockNode(this);
        }
    }



    public class VarDefineNode : StatementNode
    {
        //   public Type Type { get; private set; }
        public VariableCollection Variables { get; private set; }

        public VarDefineNode()
        {
            Variables = new VariableCollection();
            //   Type = type;
        }

        public VarDefineNode(LocalWrapper first):this()
        {
            if (!ReferenceEquals(first, null))
                Variables.Add(first);
        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitVarDefineNode(this);
        }
    }



    public class VarAssignNode : StatementNode
    {
        public LocalWrapper Variable { get; private set; }
        public ExpressionNode Expression { get; private set; }

        public VarAssignNode(LocalWrapper variable, ExpressionNode expression)
        {
            Variable = variable;
            Expression = expression;
        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitVarAssignNode(this);
        }
    }



    public enum RootKind { StaticMethod, StaticField, Instance,CreateNewObject }

    public abstract class RootMemberNode : SemanticNode
    {
        public abstract RootKind Kind { get; }
        public InstanceMemberNode Child { get; set; }
        public bool HasChild() => !ReferenceEquals(Child, null);
        public RootMemberNode(InstanceMemberNode child)
        {
            Child = child;
        }
        public RootMemberNode() : this(null)
        {
        }
    }


    public abstract class StaticRootMemberNode : RootMemberNode
    {

    }



    public class InstanceRootMemberNode : RootMemberNode
    {
        public LocalWrapper Local { get; private set; }
        public override RootKind Kind => RootKind.Instance;

        public InstanceRootMemberNode(LocalWrapper local)
        {
            Local = local;
        }

        public InstanceRootMemberNode(LocalWrapper local,InstanceMemberNode child):this(local)
        {
            Child = child;
        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitInstanceRootMemberNode(this);
        }
    }



    public class CreateObjectNode : RootMemberNode
    {
        public Type Type { get; private set; }
        public ConstructorInfo Constructor { get; private set; }
        public ExpressionCollection Arguments { get; private set; }
        public override RootKind Kind => RootKind.CreateNewObject;

        public CreateObjectNode(Type type, ConstructorInfo constructor,IEnumerable<TypedExpression> arguments)
        {
            Type = type;
            Constructor = constructor;
            Arguments = new ExpressionCollection(arguments);
        }


        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitCreateObjectNode(this);
        }
    }



    public class Create1DArray : RootMemberNode
    {
        public Type ElementType { get; private set; }
        public TypedExpression Length { get; private set; }

        public override RootKind Kind => throw new NotImplementedException();

        public Create1DArray(Type elementType, TypedExpression length)
        {
            ElementType = elementType;
            Length = length;
        }


        public override void Visit(ISemanticVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }


    public abstract class InstanceMemberNode : SemanticNode
    {
        public InstanceMemberNode Child { get; set; }

        public bool HasChild() => !ReferenceEquals(Child, null);
    }



    public class InstanceMethodNode : InstanceMemberNode
    {
        public MethodInfo Method { get; private set; }
        public ExpressionCollection Arguments { get; private set; }

        public InstanceMethodNode(MethodInfo method, IEnumerable<TypedExpression> arguments)
        {
            Method = method;
            if (arguments != null)
                Arguments = new ExpressionCollection(arguments);
            else
                Arguments = new ExpressionCollection();
        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitInstanceMethodNode(this);
        }
    }



    public class StaticMethodNode : StaticRootMemberNode
    {
        public MethodInfo Method { get; private set; }
        public ExpressionCollection Arguments { get; private set; }

        public override RootKind Kind => RootKind.StaticMethod;

        public StaticMethodNode(MethodInfo method)
        {
            Method = method;
            Arguments = new ExpressionCollection();
        }

        public StaticMethodNode(MethodInfo method, IEnumerable<TypedExpression> arguments)
        {
            Method = method;
            if (arguments != null)
                Arguments = new ExpressionCollection(arguments);
            else
                Arguments = new ExpressionCollection();
        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitStaticMethodNode(this);
        }
    }



    public class StaticFieldNode : StaticRootMemberNode
    {
        public override RootKind Kind => RootKind.StaticField;
        public FieldInfo Field { get; private set; }
        public bool IsRef { get; private set; }

        public StaticFieldNode(FieldInfo field,bool isRef)
        {
            Field = field;
            IsRef = isRef;
        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitStaticFieldNode(this);
        }
    }



    public class CallStaticMethodNode : StatementNode
    {
        public RootMemberNode Child { get; private set; }
        public CallStaticMethodNode(RootMemberNode child)
        {
            Child = child;
        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitCallStaticMethodNode(this);
        }
    }


    //public class CallConstructorNode : StatementNode
    //{

    //    public override void Visit(ISemanticVisitor visitor)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}



    public class IfElseNode : StatementNode
    {
        public ExpressionNode Expression { get; private set; }
        public StatementNode TrueBranch { get; private set; }
        public StatementNode FalseBranch { get; private set; }

        public IfElseNode(ExpressionNode expression, StatementNode trueBranch, StatementNode falseBranch)
        {
            Expression = expression;
            TrueBranch = trueBranch;
            FalseBranch = falseBranch;
        }

        public bool HasFalseBranch()
        {
            return !ReferenceEquals(FalseBranch, null);
        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitIfElseNode(this);
        }
    }



    public abstract class LoopNodeBase : StatementNode
    {
        public ExpressionNode Expression { get; private set; }
        public StatementNode Statement { get; private set; }
        public LoopNodeBase(ExpressionNode expression, StatementNode statement) 
        {
            Expression = expression;
            Statement = statement;
        }
    }



    public class WhileLoopNode : LoopNodeBase
    {
        public WhileLoopNode(ExpressionNode expression, StatementNode statement):base(expression,statement)
        {
        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitWhileLoopNode(this);
        }
    }



    public class DoWhileLoopNode : LoopNodeBase
    {
        public DoWhileLoopNode(StatementNode statement, ExpressionNode expression) : base(expression, statement)
        {
        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitDoWhileLoopNode(this);
        }
    }



    public abstract class LoopSpecialStatementNode : StatementNode
    {
        public Int32 ParentLevel { get; private set; }
        public LoopSpecialStatementNode(Int32 parentLevel)
        {
            ParentLevel = parentLevel;
        }
    }



    public class BreakNode : LoopSpecialStatementNode
    {
        public BreakNode(Int32 parentLevel) : base(parentLevel)
        {
        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitBreakNode(this);
        }
    }



    public class ContinueNode : LoopSpecialStatementNode
    {
        public ContinueNode(Int32 parentLevel) : base(parentLevel)
        {
        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitContinueNode(this);
        }
    }



    public class LabelDefineNode : StatementNode
    {
        public LabelWrapper Label { get; private set; }

        public LabelDefineNode(LabelWrapper label)
        {
            Label = label;
        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitLabelDefineNode(this);
        }
    }



    public class GOTOLabelNode : StatementNode
    {
        public LabelWrapper Label { get; private set; }

        public GOTOLabelNode(LabelWrapper label)
        {
            Label = label;
        }

        public override void Visit(ISemanticVisitor visitor)
        {
            visitor.VisitGOTOLabelNode(this);
        }
    }



    //public abstract class FactArgumentNode : StatementNode
    //{
    //    public FactArgumentNode()
    //    {

    //    }
    //}



    //public class ExpressionArgumentNode : FactArgumentNode
    //{
    //    public ExpressionNode Expression { get; private set; }

    //    public ExpressionArgumentNode(ExpressionNode expression) 
    //    {
    //        Expression = expression;
    //    }

    //    public override void Visit(ISemanticVisitor visitor)
    //    {
    //        throw new NotImplementedException();
    //    //    visitor.VisitExpressionArgumentNode(this);
    //    }
    //}



    //public abstract class RefArgumentNode : FactArgumentNode
    //{

    //    public RefArgumentNode()
    //    {
    //    }

    //    public override void Visit(ISemanticVisitor visitor)
    //    {
    //        throw new NotImplementedException();
    //        //visitor.VisitRefArgumentNode(this);
    //    }
    //}



    //public class RefFieldArgumentNode : FactArgumentNode
    //{
    //    public FieldInfo Field { get; private set; }

    //    public RefFieldArgumentNode(FieldInfo field)
    //    {
    //        Field = field;
    //    }

    //    public override void Visit(ISemanticVisitor visitor)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}



    //public class RefLocalArgumentNode : FactArgumentNode
    //{
    //    public LocalWrapper Local { get; private set; }

    //    public RefLocalArgumentNode(LocalWrapper local)
    //    {
    //        Local = local;
    //    }

    //    public override void Visit(ISemanticVisitor visitor)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    //public class StaticMethodNode:StatementNode
    //{
    //    public MethodInfo Method { get; private set; }
    //    public ExpressionCollection Arguments { get; private set; }

    //    public StaticMethodNode(MethodInfo method, ExpressionCollection arguments)
    //    {
    //        Method = method;
    //        if (arguments.IsNullOrEmpty())
    //            arguments = null;
    //        Arguments = arguments;
    //    }

    //    public override void Visit(ISemanticVisitor visitor)
    //    {
    //        visitor.VisitStaticMethodNode(this);
    //    }
    //}

    //public class IdentiferNode: SemanticNode
    //{
    //    public string Name { get; private set; }
    //    public IdentiferNode(string name)
    //    {
    //        Name = name;
    //    }

    //    public override void Visit(ISemanticVisitor visitor)
    //    {
    //        visitor.VisitIdentiferNode(this);
    //    }
    //}
}
