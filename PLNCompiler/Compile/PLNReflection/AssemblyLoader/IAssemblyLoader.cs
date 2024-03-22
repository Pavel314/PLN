using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.PLNReflection.AssemblyLoader
{
   public interface IAssemblyLoader
    {
        AssemblyLoadResult Load(AssemblyLoadHeader header);
    }
}
