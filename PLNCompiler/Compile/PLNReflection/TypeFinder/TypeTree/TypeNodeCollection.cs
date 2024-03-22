using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.PLNReflection.TypeFinder.TypeTree
{
    public class NodeCollection : List<Node>
    {
        public NodeCollection(NamespaceNode owner) : base() { Owner = owner; }
        public NodeCollection(NamespaceNode owner, int capacity) : base(capacity) { Owner = owner; }

        public new void Add(Node node)
        {
            node.Parent = Owner;
            base.Add(node);
        }

        public NamespaceNode Add1(NamespaceNode node)
        {
            Add(node);
            return node;
        }

        public TypeNode Add2(TypeNode node)
        {
            Add(node);
            return node;
        }


        public NamespaceNode Owner { get; protected set; }

    }
}
