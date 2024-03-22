using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Syntax.SyntaxTree;

namespace PLNCompiler.Syntax.TreeConverter
{
    public class PLNDirectiveGenerator : IDirectiveGenerator, IDirectiveVisitor
    {
        public PLNDirectiveGenerator()
        {
            hashSet = new HashSet<DirectiveNode>();
            Reset();
        }

        public DirectiveInfo GenerateInfo(DirectivesNode node,string compilationPath)
        {
            Reset();
            if (node == null) return new DirectiveInfo(StringHelper.PLNDefaultDirectirs,compilationPath);
            node.Visit(this);
            

            return new DirectiveInfo(applicationKind, useSystemLibrary,compilationPath);
        }

        public void VisitDirectivesNode(DirectivesNode node)
        {
            foreach (var item in node.Directives)
            {
                item.Visit(this);
            }
        }

        public void VisitConstDirectiveNode(ConstDirectiveNode node)
        {
            if (!hashSet.Add(node))
            {
                throw new Compile.Errors.SemanticErrors.DirectiveArleadyUsing(node);
            }
            switch (node.DirectiveKind)
            {
                case DirectiveKind.DisableSystemLibrary: useSystemLibrary = false;break;
                case DirectiveKind.ConsoleApplication: applicationKind = ApplicationKind.Console;break;
                case DirectiveKind.WindowApplication: applicationKind = ApplicationKind.Window; break;
                default: throw new PresentVariantNotImplementedException(typeof(DirectiveKind));
            };
        }

        public void Reset()
        {
            hashSet.Clear();
            applicationKind = StringHelper.PLNDefaultDirectirs.ApplicationKind;
            useSystemLibrary = StringHelper.PLNDefaultDirectirs.UseSystemLibrary;
        }

        private HashSet<DirectiveNode> hashSet;
        private ApplicationKind applicationKind;
        private bool useSystemLibrary;
    }
}
