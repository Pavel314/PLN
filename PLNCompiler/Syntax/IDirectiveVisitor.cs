using PLNCompiler.Syntax.SyntaxTree;

namespace PLNCompiler.Syntax
{
    public interface IDirectiveVisitor
    {
        void VisitDirectivesNode(DirectivesNode node);
        void VisitConstDirectiveNode(ConstDirectiveNode node);
    }
}
