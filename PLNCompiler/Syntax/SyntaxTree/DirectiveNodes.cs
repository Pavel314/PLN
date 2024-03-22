using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax.SyntaxTree
{
    public class DirectivesNode : DirectiveNodeBase
    {
        public DirectiveCollection Directives { get; private set; }

        public DirectivesNode(Location location) : base(location)
        {
            Location = location;
            Directives = new DirectiveCollection();
        }

        public DirectivesNode(DirectiveNode node, Location location) : this(location)
        {
            Directives.Add(node);
        }

        public override void Visit(IDirectiveVisitor visitor)
        {
            visitor.VisitDirectivesNode(this);
        }
    }


    public abstract class DirectiveNode : DirectiveNodeBase,IEquatable<DirectiveNode>
    {
        public string Spelling { get; private set; }

        public DirectiveNode(string spelling, Location location) : base(location)
        {
            Spelling = spelling;
        }

        public abstract bool Equals(DirectiveNode other);
    }

    public enum DirectiveKind { DisableSystemLibrary,ConsoleApplication, WindowApplication}

    public class ConstDirectiveNode : DirectiveNode
    {
        public DirectiveKind DirectiveKind { get; private set; }

        public ConstDirectiveNode(string spelling,DirectiveKind directiveKind, Location location) : base(spelling, location)
        {
            DirectiveKind = directiveKind;
        }

        public override int GetHashCode()
        {
            return DirectiveKind.GetHashCode();
        }

        //public override bool Equals(DirectiveNode other)
        //{
        //    var target = other as ConstDirectiveNode;
        //    if (ReferenceEquals(target, null))
        //        return false;
        //    return DirectiveKind == target.DirectiveKind;
        //}

        public override void Visit(IDirectiveVisitor visitor)
        {
            visitor.VisitConstDirectiveNode(this);
        }

        public override bool Equals(DirectiveNode other)
        {
            if (other == null)
                return false;
            var target = other as ConstDirectiveNode;
            if (target == null)
                return false;
            return DirectiveKind == target.DirectiveKind;
        }
       
    }

    public class DirectiveComparer : IEqualityComparer<DirectiveNode>
    {
        public bool Equals(DirectiveNode x, DirectiveNode y)
        {
            if (x == null || y == null)
                return x == y;
            return x.Equals(y);
        }

        public int GetHashCode(DirectiveNode obj)
        {
            return obj.GetHashCode();
        }
    }

}
