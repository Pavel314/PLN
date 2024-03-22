using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.PLNReflection.TypeFinder.TypeTree
{
    public enum NodeType {Namespace,Type }

    public abstract class Node
    {
        public Node(string name,NodeType nodeType)
        {
            Name = name;
            NodeType = nodeType;
        }

        public string Name { get; private set; }
        public NamespaceNode Parent { get; internal set; }
        public abstract void Visit(ITypeTreeVisitor visitor);
        public NodeType NodeType{get;private set;}
    }

    public class NamespaceNode : Node
    {
        public const string ROOT_NAME = "root";
        public NamespaceNode(string name):base(name,NodeType.Namespace)
        {
        }

        public override void Visit(ITypeTreeVisitor visitor)
        {
            visitor.VisitNamespaceNode(this);
        }

        public NodeCollection Childs { get { if (_Childs.IsNull()) _Childs = new NodeCollection(this); return _Childs; } }
        private NodeCollection _Childs;

        public static NamespaceNode CreateRoot()
        {
            return new NamespaceNode(ROOT_NAME);
        }

    }


    public class TypeNode : Node
    {
        public TypeNode(Type type):base(type.GetNameWithoutGeneric(), NodeType.Type)
        {
            Type = type;
            if (type.IsGenericTypeDefinition)
            Generics = type.GetGenericArguments();
        }

        public override void Visit(ITypeTreeVisitor visitor)
        {
            visitor.VisitTypeNode(this);
        }

        public int GetGenericParametersCount()
        {
            if (Generics.IsNullOrEmpty()) return 0;
            return Generics.Count;
        }

        public IReadOnlyList<Type> Generics { get; private set; }
        public Type Type { get; private set; }
    }

}
