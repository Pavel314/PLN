using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PLNCompiler.Compile.PLNReflection.TypeFinder.TypeTree
{
    public static class TypeTreeBuilder
    {

        public static NamespaceNode BuildTree(IEnumerable<Assembly> assemblies)
        {
            var tree = NamespaceNode.CreateRoot();
            foreach (var assembly in assemblies)
            {
                AddAssembly(tree, assembly);
            }
            return tree;
        }

        public static void AddAssembly(NamespaceNode root, Assembly assembly)
        {
            var types = assembly.GetExportedTypes();
            foreach (var type in types)
            {
                if (type.IsNested) continue;
                AddType(root, type);
            }
        }

        public static void AddType(NamespaceNode root, Type type)
        {
            var path = TypeName.FromType(type);
            var current = root;
            for (int i = 0; i < path.Count - 1; i++)
            {
                var name = path[i];
                if (!current.Childs.IsNull())
                {
                    bool isfind = false;
                    foreach (var child in current.Childs)
                    {
                        if (child.NodeType==NodeType.Namespace &&  StringHelper.NetComparer.Equals(child.Name, name))
                        {
                            current = (NamespaceNode)child;
                            isfind = true;
                            break;
                        }
                    }
                    if (!isfind)
                        current = current.Childs.Add1(new NamespaceNode(name));
                }
                else
                    current = current.Childs.Add1(new NamespaceNode(name));
            }
            current.Childs.Add(new TypeNode(type));
        }

    }
}
