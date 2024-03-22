using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Syntax.TreeConverter;

namespace PLNCompiler.Semantic.SemanticTree
{
   public class SemanticMain
    {
        public SemanticMain(DirectiveInfo directiveInfo, SemanticNode main)
        {
            DirectiveInfo = directiveInfo;
            Main = main;
        }
        public PLNCompiler.Syntax.TreeConverter.DirectiveInfo DirectiveInfo { get; private set; }
        public SemanticNode Main { get; private set; }

    }
}
