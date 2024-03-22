using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Semantic.SemanticTree;

namespace PLNCompiler.Semantic
{
   public interface ISemanticVisitor
    {
        void VisitBlockNode(BlockNode node);
        void VisitVarDefineNode(VarDefineNode node);
        void VisitVarAccessNode(VarAccessNode variableAccessNode);
        void VisitConstantNode(ConstantNode constantNode);
        void VisitUnaryNode(UnaryNode unaryNode);
        void VisitBinaryNode(BinaryNode binaryNode);
        void VisitVarAssignNode(VarAssignNode varAssignNode);
        void VisitStaticMethodNode(StaticMethodNode staticMethodNode);
        void VisitCallStaticMethodNode(CallStaticMethodNode node);
        void VisitMemberAccessNode(MemberAccessNode memberAccessNode);
        void VisitStaticFieldNode(StaticFieldNode staticFieldNode);
        void VisitInstanceMethodNode(InstanceMethodNode instanceMethodNode);
        void VisitIfElseNode(IfElseNode ifElseNode);
        void VisitLabelDefineNode(LabelDefineNode labelDefineNode);
        void VisitGOTOLabelNode(GOTOLabelNode gotoLabelNode);
        void VisitInstanceRootMemberNode(InstanceRootMemberNode instanceRootMemberNode);
        void VisitWhileLoopNode(WhileLoopNode whileLoopNode);
        void VisitBreakNode(BreakNode node);
        void VisitContinueNode(ContinueNode node);
        void VisitDoWhileLoopNode(DoWhileLoopNode doWhileLoopNode);
        void VisitNewUnaryNode(NewUnaryNode newUnaryNode);
        void VisitCreateObjectNode(CreateObjectNode createObjectNode);
        void VisitTypeOfNode(TypeOfNode typeOfNode);
        void VisitCastTypeNode(CastTypeNode castTypeNode);
        void VisitIsCastTypeNode(IsCastTypeNode isCastTypeNode);
        void VisitAsCastTypeNode(AsCastTypeNode asCastTypeNode);
    }
}
