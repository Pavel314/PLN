using System.Collections.Generic;
using System.Reflection;

namespace PLNCompiler.Compile.PLNReflection.TypeFinder
{
    public interface ITypeFinder
    {
        IReadOnlyCollection<Assembly> Assemblies { get; }
        IReadOnlyCollection<NameManagerReadOnly> Namespaces { get; }
        SearchingTypeResult FindType(TypeName typeName);
    }
}
