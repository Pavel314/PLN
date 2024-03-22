using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Syntax.SyntaxTree;

namespace PLNCompiler.Syntax
{
  public  interface IStatementVisitor
    {
        void VisitVarDefineNode(VarDefineNode node);
        void VisitAssignNode(AssignNode node);
        void VisitVarDefineAssignNode(VarDefineAssignNode varDefineAssignNode);
        void VisitBlockNode(BlockNode node);
        void VisitMethodNode(MethodNode node);
        void VisitFieldNode(FieldNode fieldNode);
        void VisitIfElseNode(IfElseNode ifElseNode);
        void VisitLabelDefineNode(LabelDefineNode labelDefineNode);
        void VisitGOTOLabelNode(GOTOLabelNode gotoLabelNode);
        void VisitOneLineCommentNode(OneLineCommentNode oneLineCommentNode);
        void VisitWhileLoopNode(WhileLoopNode whileLoopNode);
        void VistitBreakNode(BreakNode node);
        void VistContinueNode(ContinueNode node);
        void VisitDoWhileLoopNode(DoWhileLoopNode node);
        void VisitExpressionArgumentNode(ExpressionArgumentNode expressionArgumentNode);
        void VisitRefArgumentNode(RefArgumentNode refArgumentNode);
        void VisitTypeOfNode(TypeOfNode typeOfNode);
        void VisitVarDefineAssignAutoNode(VarDefineAssignAutoNode varDefineAssignAutoNode);
        void VisitBuiltInLoopNode(BuiltInLoopNode builtInLoopNode);
        //  void VistContinueAllNode(ContinueAllNode continueAllNode);
        // void VistitBreakAllNode(BreakAllNode breakAllNode);
    }
}
