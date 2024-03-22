using PLNCompiler.Syntax.SyntaxTree;

namespace PLNCompiler.Syntax
{
    public interface IExpressionVisitor
    {
        void VisitParsableConstantNode(ParsableConstNode node);
        void VisitConstantNode(ConstNode node);
        void VisitUnaryNode(UnaryNode node);
        void VisitBinaryNode(BinaryNode node);
        void VisitMemberAccessNode(MemberAccessNode node);
        void VisitNewUnaryNode(NewUnaryNode newUnaryNode);
        void VisitExplicitCastNode(ExplicitCastNode castNode);
        void VisitIsCastNode(IsCastNode isCastNode);
        void VisitAsCastNode(AsCastNode asCastNode);
    }
}

