using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;


namespace PLNCompiler.Compile.PLNReflection.TypeFinder
{
    //public class NestedTypeFinder
    //{
    //    public NestedTypeFinder()
    //    {

    //    }

    //    public FindNestedTypeResult FindNested(Type type,TypeName nestedType)
    //    {

    //        return null;
    //    }
    //}


    public static class NestedTypeFinder
    {

        public static FindNestedTypeResult FindNestedType(TypeAssociation parentType, TypeName nestedType)
        {
            NestedType = nestedType;
            MatchesMax = -1;
            Matches = new List<TypeAssociation>();

            var type = parentType.Type;
            var generics = 0;
            if (type.IsGenericTypeDefinition || type.IsConstructedGenericType)
                generics = type.GetGenericArguments().Length;

            _FindNestedType(type, 0,new NameManager(parentType.Name), generics);

            var member = new NameManagerReadOnly(nestedType.Where((f, f1) => f1 > MatchesMax));

             return new FindNestedTypeResult(parentType,nestedType, member, Matches);
        }

        private static void _FindNestedType(Type type, int index, NameManager path, int lastGenericCount)
        {
            var nesteds = type.GetNestedTypes();

            foreach (var nested in nesteds)
            {

                var name = nested.GetGenericNameIgnore();
                var genericCount = 0;
                if (nested.IsGenericTypeDefinition)
                {
                    genericCount = nested.GetGenericArguments().Length - lastGenericCount;
                }

                if (StringHelper.PLNComparer.EqualsStr(NestedType[index], name))
                {
                    path.Add(name);
                    if (index >= MatchesMax)
                    {
                        if (index > MatchesMax)
                        {
                            Matches.Clear();
                            MatchesMax = index;
                        }

                        if (index == NestedType.Count - 1)
                        {
                            if (genericCount == NestedType.GenericParametersCount)
                                Matches.Add(new TypeAssociation(nested, new TypeName(path, NestedType.GenericParametersCount)));
                        }
                        else
                        {
                            Matches.Add(new TypeAssociation(nested, new TypeName(path, NestedType.GenericParametersCount)));
                            _FindNestedType(nested, index + 1, path, lastGenericCount + genericCount);
                        }
                    }
                    path.RemoveLast();
                }
            }
        }

        private static int MatchesMax;
        private static List<TypeAssociation> Matches;
        private static TypeName NestedType;
    }
}
