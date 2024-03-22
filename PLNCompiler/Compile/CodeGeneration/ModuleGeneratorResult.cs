using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace PLNCompiler.Compile.CodeGeneration
{
    public class ModuleGeneratorResult
    {
        public ModuleBuilder GeneratedModule { get; private set; }
        public Type MainClass { get; private set; }
        public MethodInfo MainMethod { get; private set; }

        public ModuleGeneratorResult(ModuleBuilder generatedModule, Type mainClass, MethodInfo mainMethod)
        {
            GeneratedModule = generatedModule;
            MainClass = mainClass;
            MainMethod = mainMethod;
        }

    }
}
