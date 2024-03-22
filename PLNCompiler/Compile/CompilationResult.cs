using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Specialized;
using PLNCompiler.Compile.CodeGeneration;

namespace PLNCompiler.Compile
{
  public  class CompilationResult
    {
        public const int SUCCESSFUL_COMPILER_CODE = 0;

        public int CompilerCode { get; private set; }
        public CompilationErrors Errors { get; private set; }
        public IReadOnlyList<string> CompilerMessages { get; private set; }
        public Assembly GeneratedAssembly { get; private set; }

        public CompilationResult(int compilerCode,CompilationErrors errors, IReadOnlyList<string> compilerMessages, Assembly generatedAssembly)
        {
            CompilerCode = compilerCode;
            Errors = errors;
            CompilerMessages = compilerMessages;
            GeneratedAssembly = generatedAssembly;
        }

        public bool IsSuccessful { get { return CompilerCode == SUCCESSFUL_COMPILER_CODE; } }

        public static CompilationResult CreateSuccesful(Assembly generatedAssembly)
        {
            return new CompilationResult(SUCCESSFUL_COMPILER_CODE, null, null, generatedAssembly);
        }

    }
}
