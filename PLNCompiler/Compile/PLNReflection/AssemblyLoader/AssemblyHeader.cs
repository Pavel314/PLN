using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.PLNReflection.AssemblyLoader
{
    [Flags]
    public enum AssemblyLoadFrom {All=7, File = 1, GAC = 2, StrongName=4 }
    public struct AssemblyLoadHeader
    {
        public readonly string Name;
        public readonly string CompilationPath;
        public readonly AssemblyLoadFrom LoadFrom;

        public AssemblyLoadHeader(string name,string compilationPath, AssemblyLoadFrom loadFrom)
        {
            Name = name;
            LoadFrom = loadFrom;
            CompilationPath = compilationPath;
        }

        public AssemblyLoadHeader(string name, string compilationPath) : this(name,compilationPath, AssemblyLoadFrom.All)
        {
        }

    }
}
