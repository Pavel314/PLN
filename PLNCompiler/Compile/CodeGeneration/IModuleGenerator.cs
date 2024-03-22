using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using PLNCompiler.Semantic.SemanticTree;
using System.Reflection.Emit;

namespace PLNCompiler.Compile.CodeGeneration
{
    public interface IModuleGenerator
    {
        ModuleGeneratorResult GenerateModule(AssemblyBuilder assemblyBuilder, ModuleSetting moduleSetting, SemanticNode node);
    }
}
