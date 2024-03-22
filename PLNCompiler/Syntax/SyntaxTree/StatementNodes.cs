using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax.SyntaxTree
{

    public class BlockNode : StatementNode
    {
        public StatementCollection Statements { get; private set; }
        public BlockNode(StatementNode first, Location location) : this(location)
        {
            Statements.Add(first);
        }
        public BlockNode(Location location) : base(location)
        {
            Statements = new StatementCollection();
        }
        public override void Visit(IStatementVisitor visitor)
        {
            visitor.VisitBlockNode(this);
        }
    }

    public enum MemberKind { Field, Method,TypeOf }

    public class MethodNode : MemberNode
    {
        public MethodArgumentsCollection Arguments { get; private set; }
        public override MemberKind Kind { get { return MemberKind.Method; } }

        public MethodNode(IdentiferNode name, TypeCollection generics, MethodArgumentsCollection arguments, IndexerArgumentsCollection indexs, Location location) : base(name, generics, indexs, location)
        {
            if (arguments.IsNullOrEmpty()) arguments = null;
            Arguments = arguments;
        }

        public MethodNode(IdentiferNode name, TypeCollection generics,MethodInfo info, Location location) : this(name,generics, info.Arguments,info.Indexes, location)
        {

        }

        public override void Visit(IStatementVisitor visitor)
        {
            visitor.VisitMethodNode(this);
        }

        public bool HasArguments() => !Arguments.IsNullOrEmpty();
    }

    public class TypeOfNode : MemberNode
    {
        public TypeNode Argument { get; private set; }

        public override MemberKind Kind => MemberKind.TypeOf;

        public TypeOfNode(TypeNode argument,Location location):base(null,null,null,location)
        {
            Argument = argument;
        }

        public override void Visit(IStatementVisitor visitor)
        {
            visitor.VisitTypeOfNode(this);
        }
    }


    public class FieldNode : MemberNode
    {
        public override MemberKind Kind { get { return MemberKind.Field; } }

        public FieldNode(IdentiferNode name, TypeCollection generics, IndexerArgumentsCollection indexs, Location location) : base(name, generics, indexs, location)
        {

        }
        public override void Visit(IStatementVisitor visitor)
        {
            visitor.VisitFieldNode(this);
        }

        public IdentiferNode ToVariable()
        {
            if (!IsVariable())
                throw new InvalidOperationException();
            return Name;
        }

        public bool IsVariable() => (GetGenericsCount() == 0 && !HasChild());
    }



    public abstract class MemberNode : StatementNode
    {
        public IdentiferNode Name { get; private set; }
        public TypeCollection Generics { get; private set; }
        public IndexerArgumentsCollection Indexs { get; private set; }
        public MemberNode Child { get; set; }
        public abstract MemberKind Kind { get; }

        public MemberNode(IdentiferNode name, TypeCollection generics, IndexerArgumentsCollection indexs, Location location) : base(location)
        {
            Name = name;
            if (generics.IsNullOrEmpty()) generics = null;
            if (indexs.IsNullOrEmpty()) indexs = null;
            Generics = generics;
            Indexs = indexs;
        }

        public bool HasChild() => !ReferenceEquals(Child, null);

        public int GetGenericsCount()
        {
            if (Generics.IsNullOrEmpty())
                return 0;
            return Generics.Count;
        }

        public int GetIndexsCount()
        {
            if (Indexs.IsNullOrEmpty())
                return 0;
            return Indexs.Count;
        }

    }



    public class AssignNode : StatementNode
    {
        public MemberNode Assignable { get; private set; }
        public ExpressionNode Expression { get; private set; }
        public AssignKind AssignType { get; private set; }

        public AssignNode(MemberNode assignable, ExpressionNode expression, AssignKind assignKind, Location location) : base(location)
        {
            Assignable = assignable;
            Expression = expression;
        }

        public override void Visit(IStatementVisitor visitor)
        {
            visitor.VisitAssignNode(this);
        }
    }



    public abstract class VarNodeBase : StatementNode
    {
        public VarNodeBase(Location location) : base(location)
        {
        }
    }

    public class VarDefineNode : StatementNode
    {
        public TypeNode TypeNode { get; private set; }
        public IdentiferCollection Variables { get; private set; }
        public VarDefineNode(TypeNode typeNode, IdentiferCollection variables, Location location) : base(location)
        {
            TypeNode = typeNode;
            Variables = variables;
        }

        public override void Visit(IStatementVisitor visitor)
        {
            visitor.VisitVarDefineNode(this);
        }
    }


    public abstract class VarDefineAssignNodeBase:StatementNode
    {
        public IdentiferNode Variable { get; private set; }
        public ExpressionNode Expression { get; private set; }
        public VarDefineAssignNodeBase( IdentiferNode variable, ExpressionNode expression, Location location) : base(location)
        {
            Variable = variable;
            Expression = expression;
        }
    }



    public class VarDefineAssignNode : VarDefineAssignNodeBase
    {
        public TypeNode TypeNode { get; private set; }

        public VarDefineAssignNode(TypeNode typeNode, IdentiferNode variable, ExpressionNode expression, Location location) : base(variable,expression, location)
        {
            TypeNode = typeNode;
        }

        public override void Visit(IStatementVisitor visitor)
        {
            visitor.VisitVarDefineAssignNode(this);
        }
    }



    public class VarDefineAssignAutoNode: VarDefineAssignNodeBase
    {
        public VarDefineAssignAutoNode(IdentiferNode variable, ExpressionNode expression, Location location) : base(variable,expression, location)
        {
        }
        public override void Visit(IStatementVisitor visitor)
        {
            visitor.VisitVarDefineAssignAutoNode(this);
        }
    }



    public class IfElseNode : StatementNode
    {
        public ExpressionNode Expression { get; private set; }
        public StatementNode TrueBranch { get; private set; }
        public StatementNode FalseBranch { get; private set; }

        public IfElseNode(ExpressionNode expression, StatementNode trueBranch, StatementNode falseBranch, Location location) : base(location)
        {
            Expression = expression;
            TrueBranch = trueBranch;
            FalseBranch = falseBranch;
        }

        public IfElseNode(ExpressionNode expression, StatementNode trueBranch, Location location) : this(expression, trueBranch, null, location)
        {
        }

        public bool HasFalseBranch()
        {
            return !ReferenceEquals(FalseBranch, null);
        }

        public override void Visit(IStatementVisitor visitor)
        {
            visitor.VisitIfElseNode(this);
        }
    }



    public abstract class LoopNode : StatementNode
    {
        public ExpressionNode Expression { get; private set; }
        public StatementNode Statement { get; private set; }
        public LoopNode(ExpressionNode expression, StatementNode statement, Location location) : base(location)
        {
            Expression = expression;
            Statement = statement;
        }
    }



    public class WhileLoopNode : LoopNode
    {

        public WhileLoopNode(ExpressionNode expression, StatementNode statement, Location location) : base(expression, statement, location)
        {
        }

        public override void Visit(IStatementVisitor visitor)
        {
            visitor.VisitWhileLoopNode(this);
        }
    }




    public class DoWhileLoopNode : LoopNode
    {

        public DoWhileLoopNode(StatementNode statement, ExpressionNode expression, Location location) : base(expression, statement, location)
        {
        }

        public override void Visit(IStatementVisitor visitor)
        {
            visitor.VisitDoWhileLoopNode(this);
        }
    }



    public class BuiltInLoopNode:LoopNode
    {
        public StatementNode DefineVariable { get; private set; }
        public StatementNode Increament { get; private set; }

        public BuiltInLoopNode(StatementNode defineVariable,ExpressionNode expression,StatementNode increment,StatementNode body,Location location):base(expression,body,location)
        {
            DefineVariable = defineVariable;
            Increament = increment;
        }

        public override void Visit(IStatementVisitor visitor)
        {
            visitor.VisitBuiltInLoopNode(this);
        }


    }

    public abstract class LoopSpecialStatementNode : StatementNode
    {
        public enum ConstKind { HasConstant, ApplyNear, ApplyMax }

        public ConstNode ConstNode { get; private set; }
        public ConstKind Kind { get; private set; }

        private LoopSpecialStatementNode(ConstKind kind,ConstNode constNode, Location location):base(location)
        {
            Kind = kind;
            ConstNode = constNode;
        }

        public LoopSpecialStatementNode(ConstNode constNode, Location location) : this(ConstKind.HasConstant,constNode, location)
        {
            if (ReferenceEquals(constNode, null)) throw new ArgumentNullException("constNode");
        }

        public LoopSpecialStatementNode(bool applyNear, Location location) : this(BoolToKind(applyNear), null, location)
        {

        }

        private static ConstKind BoolToKind(bool applyNear)
        {
            if (applyNear)
                return ConstKind.ApplyNear;
            return ConstKind.ApplyMax;
        }
    }



    public class BreakNode : LoopSpecialStatementNode
    {

        public BreakNode(ConstNode constNode, Location location) : base(constNode, location)
        {
        }

        public BreakNode(bool applyNear,Location location) : base(applyNear, location)
        {
        }

        public override void Visit(IStatementVisitor visitor)
        {
            visitor.VistitBreakNode(this);
        }
    }


    //public class BreakAllNode : LoopSpecialStatementNode
    //{
    //    public BreakAllNode(Location location) : base( location)
    //    {
    //    }
    //    public override void Visit(IStatementVisitor visitor)
    //    {
    //        visitor.VistitBreakAllNode(this);
    //    }
    //}



    public class ContinueNode : LoopSpecialStatementNode
    {
        public ContinueNode(ConstNode constNode, Location location) : base(constNode, location)
        {
        }

        public ContinueNode(bool applyNear,Location location) : base(applyNear, location)
        {
        }

        public override void Visit(IStatementVisitor visitor)
        {
            visitor.VistContinueNode(this);
        }
    }


    //public class ContinueAllNode : LoopSpecialStatementNode
    //{
    //    public ContinueAllNode(Location location) : base(location)
    //    {
    //    }
    //    public override void Visit(IStatementVisitor visitor)
    //    {
    //        visitor.VistContinueAllNode(this);
    //    }
    //}



        public class LabelDefineNode : StatementNode
    {
        public IdentiferNode Name { get; private set; }
        public StatementNode Statement { get; private set; }

        public LabelDefineNode(IdentiferNode name, StatementNode statement, Location location) : base(location)
        {
            Name = name;
            Statement = statement;
        }

        public override void Visit(IStatementVisitor visitor)
        {
            visitor.VisitLabelDefineNode(this);
        }
    }



    public class GOTOLabelNode : StatementNode
    {
        public IdentiferNode Label { get; private set; }

        public GOTOLabelNode(IdentiferNode label, Location location) : base(location)
        {
            Label = label;
        }

        public override void Visit(IStatementVisitor visitor)
        {
            visitor.VisitGOTOLabelNode(this);
        }
    }



    public abstract class CommentNode : StatementNode
    {
        public string Comment { get; private set; }
        public CommentNode(string comment, Location location) : base(location)
        {
            Comment = comment;
        }
    }



    public class OneLineCommentNode : CommentNode
    {
        public OneLineCommentNode(string comment, Location location) : base(comment, location)
        {
        }

        public override void Visit(IStatementVisitor visitor)
        {
            visitor.VisitOneLineCommentNode(this);
        }
    }



    public abstract class FactArgumentNode : StatementNode
    {
        public FactArgumentNode(Location location) : base(location)
        {

        }
    }



    public class ExpressionArgumentNode : FactArgumentNode
    {
        public ExpressionNode Expression { get; private set; }

        public ExpressionArgumentNode(ExpressionNode expression, Location location) : base(location)
        {
            Expression = expression;
        }

        public override void Visit(IStatementVisitor visitor)
        {
            visitor.VisitExpressionArgumentNode(this);
        }
    }



    public class RefArgumentNode: FactArgumentNode
    {
        public FieldNode Field { get; private set; }

        public RefArgumentNode(FieldNode field, Location location) : base(location)
        {
            Field = field;
        }

        public override void Visit(IStatementVisitor visitor)
        {
            visitor.VisitRefArgumentNode(this);
        }
    }
}
