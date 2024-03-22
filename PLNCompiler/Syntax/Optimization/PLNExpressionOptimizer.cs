using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Syntax.SyntaxTree;
using PLNCompiler.Compile;

namespace PLNCompiler.Syntax.Optimization
{
    public class PLNExpressionOptimizer : IExpressionOptimizer, IExpressionVisitor
    {
        public event ConstantNeedingCallback ConstantNeeding;

        public void VisitParsableConstantNode(ParsableConstNode node)
        {
            optimizedExpression = node;
            stack.Push(new ExpressionStackValue(node.Constant));
        }


        public void VisitUnaryNode(UnaryNode node)
        {
            node.Right.Visit(this);
            if (stack.Peek().Kind == ExpressionStackValueKind.Constant)// && node.Operation!=UnaryOperation.New
            {
                var result = MakeUnary((ParsableConstant)stack.Pop().Value, node.Operation,node.Location);
                stack.Push(new ExpressionStackValue(result));
                optimizedExpression = new ParsableConstNode(result, node.Location);
            }
            else
            {
                stack.Push(new ExpressionStackValue(node.Operation));
                optimizedExpression = new UnaryNode(optimizedExpression, node.Operation, node.Location);
            }
        }


        public void VisitBinaryNode(BinaryNode node)
        {
            node.Left.Visit(this);
            var left = optimizedExpression;
            node.Right.Visit(this);

            if (stack.Peek().Kind == ExpressionStackValueKind.Constant)
            {
                var value2 = stack.Pop();
                if (stack.Peek().Kind == ExpressionStackValueKind.Constant)
                {
                    var value1 = stack.Pop();
                    var result = MakeBinary((ParsableConstant)value1.Value, (ParsableConstant)value2.Value, node.Opration,node.Location);
                    stack.Push(new ExpressionStackValue(result));
                    optimizedExpression = new ParsableConstNode(result, node.Location);
                    return;
                }
                else
                {
                    stack.Push(value2);
                    stack.Push(new ExpressionStackValue(node.Opration));
                }
            }
            optimizedExpression = new BinaryNode(left, optimizedExpression, node.Opration, node.Location);
        }


        public void VisitConstantNode(ConstNode node)
        {
            VisitParsableConstantNode(PLNCompiler.Syntax.TreeConverter.PLNTreeConverter.ParseConstant(node));
        }


        public void VisitMemberAccessNode(MemberAccessNode node)
        {
            if (!ReferenceEquals(node.ParentExpression, null))
            {
                node.ParentExpression.Visit(this);
                node = new MemberAccessNode(optimizedExpression, node.ChildMember, node.Location);
            }

            //if (ConstantNeeding != null)
            //{
            //    var neededConstant = ConstantNeeding(this, new ConstantNeedingEventArgs(node));
            //    if (neededConstant != null)
            //    {
                   
            //        return;
            //    }
            //}
            var optimized = OnConstantNeeding(node);
            if (optimized != null)
            {
                VisitParsableConstantNode(new ParsableConstNode(optimized, node.Location));
                return;
            }

            stack.Push(new ExpressionStackValue(node));
            optimizedExpression = node;
        }

        public void VisitExplicitCastNode(ExplicitCastNode node)
        {
            stack.Push(new ExpressionStackValue(node));
            optimizedExpression = node;
        }

        public void VisitIsCastNode(IsCastNode node)
        {
            stack.Push(new ExpressionStackValue(node));
            optimizedExpression = node;
           // throw new NotImplementedException();
        }

        public void VisitAsCastNode(AsCastNode node)
        {
            stack.Push(new ExpressionStackValue(node));
            optimizedExpression = node;
        }

        public void VisitNewUnaryNode(NewUnaryNode node)
        {
            stack.Push(new ExpressionStackValue(node));
            optimizedExpression = node;
        }

        protected virtual ParsableConstant OnConstantNeeding(MemberAccessNode node)
        {
            if (ConstantNeeding != null)
            {
                var neededConstant = ConstantNeeding(this, new ConstantNeedingEventArgs(node));
                return neededConstant;
            }
            return null;
        }

        public ExpressionNode Optimize(ExpressionNode expressionNode)
        {
            Reset();
            expressionNode.Visit(this);
            return optimizedExpression;
        }


        public PLNExpressionOptimizer(IConstantInterpreter constantInterpreter)
        {
            stack = new ExpressionStack();
            ConstantInterpreter = constantInterpreter;
            Reset();
        }

        public PLNExpressionOptimizer():this(new PLNConstantInterpreter())
        {
        }

        private void Reset()
        {
            optimizedExpression = null;
            stack.Clear();
        }


        private ParsableConstant MakeUnary(ParsableConstant value, UnaryOperation operation, Location location)
        {
          var interpResult =  ConstantInterpreter.Compute(value, operation);
            if (!interpResult.IsSuccessful) throw Compile.Errors.SemanticErrors.Get(location, interpResult);
            return interpResult.Constant;
        }


        private ParsableConstant MakeBinary(ParsableConstant value1, ParsableConstant value2, BinaryOperation operation, Location location)
        {
            var interpResult = ConstantInterpreter.Compute(value1,value2, operation);
            if (!interpResult.IsSuccessful) throw Compile.Errors.SemanticErrors.Get(location, interpResult);
            return interpResult.Constant;
        }

       

        public IConstantInterpreter ConstantInterpreter { get; private set; }
        private ExpressionNode optimizedExpression;
        private ExpressionStack stack;
    }
}
