using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax.SyntaxTree
{
    public abstract class StatementNode : SyntaxNode
    {
        public abstract void Visit(IStatementVisitor visitor);
        public StatementNode(Location location) : base(location) { }

        public BlockNode ConvertToBlock()
        {
            if (this is BlockNode statement)
                return statement;
            return new BlockNode(this,Location);
        }

        public BlockNode ConvertToBlockAndAdd(StatementNode item)
        {
           BlockNode target= ConvertToBlock();
            target.Statements.Add(item);
            return target;
        }

    }
}
