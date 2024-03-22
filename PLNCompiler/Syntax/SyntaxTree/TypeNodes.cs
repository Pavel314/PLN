using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QUT.Gppg;

namespace PLNCompiler.Syntax.SyntaxTree
{
  public abstract class TypeNode:SyntaxNode
    {
        public abstract void Visit(ITypeVisitor visitor);
        public TypeNode(Location location) : base(location) { }
    }

    public class ArrayTypeNode : TypeNode
    {
        public int DimensionCount { get;private set; }
        public TypeNode ArrayChild { get; private set; }

        public ArrayTypeNode(TypeNode arrayChild,int dimensionCount,Location location):base(location)
        {
            ArrayChild = arrayChild;
            DimensionCount = dimensionCount;
        }

        public ArrayTypeNode(TypeNode arrayChild, Location location) :this(arrayChild,1,location)
        {
        }

        public override void Visit(ITypeVisitor visitor)
        {
            visitor.VisitArrayTypeNode(this);
        }
    }

    public class TypeNameNode:TypeNode
    {
        public TypeCollection Generics { get; private set; }
        public TypeNameNode Child { get; set; }
        public IdentiferNode Identifer { get; set; }

        public TypeNameNode(IdentiferNode identifer,TypeCollection generics, Location location):base(location)
        {
            Generics = generics;
            Identifer = identifer;
            Child = null;
        }

        public TypeNameNode(IdentiferNode identifer, Location location) :this(identifer, new TypeCollection(),location)
        {
        }

        public int GetGenerisCount()
        {
            return Generics.IsNullOrEmpty() ? 0 : Generics.Count;
        }

        public override void Visit(ITypeVisitor visitor)
        {
            visitor.VisitTypeNameNode(this);
        }

        public override string ToString()
        {
            return Identifer.Name;
        }
    }




    public class ImportsNode : TypeNode
    {
        public ImportCollection Imports { get; private set; }

        public ImportsNode(ImportNode importeNode) :base(importeNode.Location)
        {
            Imports = new ImportCollection();
            Imports.Add(importeNode);
        }
        public override void Visit(ITypeVisitor visitor)
        {
            visitor.VisitImportsNode(this);
        }
    }

    public class ImportNode : TypeNode
    {
        public string Name { get; private set; }
        public ImportNode(string name, Location location) : base(location)
        {
            Name = name;
        }
        public override void Visit(ITypeVisitor visitor)
        {
            visitor.VisitImportNode(this);
        }
    }





    public class UsingsNode : TypeNode
    {
        public UsingCollection Usings { get; private set; }

        public UsingsNode(UsingNode usingNode) : base(usingNode.Location)
        {
            Usings = new UsingCollection();
            Usings.Add(usingNode);
        }

        public override void Visit(ITypeVisitor visitor)
        {
            visitor.VisitUsingsNode(this);
        }
    }


    public class UsingNode : TypeNode
    {
        public IdentiferCollection Namespace { get; private set; }

        public UsingNode(IdentiferCollection namespace_,Location location) : base(location)
        {
            Namespace = namespace_;
        }

        public override void Visit(ITypeVisitor visitor)
        {
            visitor.VisitUsingNode(this);
        }
    }



}
