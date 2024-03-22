using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.PLNReflection.TypeFinder.TypeTree
{
    public class TypeVisitor : ITypeTreeVisitor
    {
        public const bool TYPE_SUSPENSE_IS_ENABLED = true;

        public TypeVisitor()
        {
            Matches = new List<TypeAssociation>();
            TypePath = new NameManager();
            ResetFields(null);
        }

        public IReadOnlyList<TypeAssociation> FindType(NamespaceNode tree, TypeName typeName, out int matches)
        {
            ResetFields(typeName);
            tree.Visit(this);

            if (Matches.Count == 0)
            {
                matches = 0;
                return null;
            }
            matches = MatchesMax;
            int targetGenericCount = 0;
            if (typeName.Count == MatchesMax)
                targetGenericCount = typeName.GenericParametersCount;

            var filteredByGenericCount = new List<TypeAssociation>(Matches.Count);
            foreach (var match in Matches)
            {
                if (match.Name.GenericParametersCount == targetGenericCount)
                    filteredByGenericCount.Add(match);
            }
            return filteredByGenericCount;

        }

        public void VisitTypeNode(TypeNode node)
        {
            if (index >= MatchesMax)
            {
                if (index > MatchesMax)
                {
                    Matches.Clear();
                    MatchesMax = index;
                }

                Matches.Add(new TypeAssociation(node.Type, new TypeName(TypePath, node.GetGenericParametersCount())));
            }
        }

        public void VisitNamespaceNode(NamespaceNode node)
        {
            if (index >= Type.Count) return;
            foreach (var child in node.Childs)
            {
                if (StringHelper.PLNComparer.Equals(child.Name, Type[index]))
                {
                    TypePath.Add(child.Name);
                    index++;
                    child.Visit(this);
                    index--;
                    TypePath.RemoveAt(TypePath.Count - 1);
                    if (Matches.Count>0 && !TYPE_SUSPENSE_IS_ENABLED) break;
                }
            }
        }

        private void ResetFields(TypeName findType)
        {
            index = 0;
            MatchesMax = -1;
            Type = findType;
            TypePath.Clear();
            Matches.Clear();
        }

        private int index;
        private TypeName Type;
        private NameManager TypePath;
        private List<TypeAssociation> Matches;
        private int MatchesMax;


    }
}
