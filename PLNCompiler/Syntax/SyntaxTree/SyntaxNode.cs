
namespace PLNCompiler.Syntax.SyntaxTree
{
    public abstract class SyntaxNode
    {
       public Location Location { get; set; }

       public SyntaxNode(Location location)
        {
            Location = location;
        } 

    }
}
