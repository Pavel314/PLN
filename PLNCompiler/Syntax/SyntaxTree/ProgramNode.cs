using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax.SyntaxTree
{
   public class ProgramNode:SyntaxNode
    {
        public DirectivesNode DirectiveSection { get; private set; }
        public ImportsNode ImportSection { get; private set; }
        public UsingsNode UsingSection { get; private set; }
        public BlockNode MainBlock { get; private set; }
        public string CompilationPath { get; private set; }

        public ProgramNode(DirectivesNode directiveSection,ImportsNode importSection, UsingsNode usingSection, BlockNode mainBlock, Location location):base(location)
        {
            DirectiveSection = directiveSection;
            ImportSection = importSection;
            UsingSection = usingSection;
            MainBlock = mainBlock;
        }
    }
}
