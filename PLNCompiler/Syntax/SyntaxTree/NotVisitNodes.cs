using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax.SyntaxTree
{
    public class IdentiferNode : NotVisitNode
    {
        public string Name { get; set; }
        public IdentiferNode(string name, Location location) : base(location) { Name = name; }
    }

    public struct MethodInfo
    {
        public MethodArgumentsCollection Arguments { get; private set; }
        public IndexerArgumentsCollection Indexes { get; private set; }

        public MethodInfo(MethodArgumentsCollection arguments, IndexerArgumentsCollection indexes)
        {
            Arguments = arguments;
            Indexes = indexes;
        }
    }


    //public abstract class VarNodeInfoBase
    //{
    //}

    //public class VarDefineNodeInfo : VarNodeInfoBase
    //{
    //    public TypeNode TypeNode { get; private set; }
    //    public IdentiferCollection Variables { get; private set; }
    //    public VarDefineNodeInfo(TypeNode typeNode, IdentiferCollection variables) 
    //    {
    //        TypeNode = typeNode;
    //        Variables = variables;
    //    }
    //}

    //public class VarDefineAssignNodeInfo:VarNodeInfoBase
    //{
    //    public TypeNode TypeNode { get; private set; }
    //    public VarDefineAssignNodeInfo(TypeNode typeNode, IdentiferNode variable, ExpressionNode expression) : base(variable,expression)
    //    {
    //        TypeNode = typeNode;
    //    }
    //}
    public enum VariableDefineInfoKind { Variables, Variable, AutoType }
    public struct VariableDefineInfo
    {
        public readonly TypeNode Type;
        public readonly IdentiferCollection Identifers;
        public readonly IdentiferNode Identifer;
        public readonly ExpressionNode Expression;
        public readonly VariableDefineInfoKind Kind;

        private VariableDefineInfo(TypeNode type, IdentiferCollection identifers, IdentiferNode identifer, ExpressionNode expression, VariableDefineInfoKind kind)
        {
            Type = type;
            Identifers = identifers;
            Identifer = identifer;
            Expression = expression;
            Kind = kind;
        }

        public VariableDefineInfo(TypeNode type, IdentiferNode identifer, ExpressionNode expression) : this(type, null, identifer, expression, VariableDefineInfoKind.Variable)
        {
        }
        public VariableDefineInfo(TypeNode type, IdentiferCollection identifers) : this(type, identifers, null, null, VariableDefineInfoKind.Variables)
        {
        }
        public VariableDefineInfo(IdentiferNode identifer, ExpressionNode expression) : this(null, null, identifer, expression, VariableDefineInfoKind.AutoType)
        {
        }


        //}

        //    public class Argument : NotVisitNode
        //    {
        //        public ExpressionNode Expression { get; private set; }
        //        public FieldNode RefField { get; private set; } 
        //        public bool IsRef { get; private set; }

        //        public Argument (ExpressionNode expression,bool isRef,Location location):base(location)
        //        {
        //            Expression = expression;
        //            IsRef = isRef;
        //#if DEBUG
        //            if (IsRef && !(expression is MemberAccessNode))
        //            {
        //                throw new ArgumentException("this is ref and not MemberAccessNode");
        //            }
        //#endif
        //        }
        //    }
    }
}
