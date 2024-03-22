using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.CodeGeneration
{
    public struct ModuleSetting
    {
        public readonly string ModuleName;
        public readonly string FileName;
        public readonly bool EmitSymInfo;
        public readonly string MainClassName;
        public readonly string MainMethodName;

        public ModuleSetting(string moduleName, string fileName, bool emitSymInfo,string mainClassname,string mainMethodName="Main")
        {
            ModuleName = moduleName;
            FileName = fileName;
            EmitSymInfo = emitSymInfo;
            MainClassName = mainClassname;
            MainMethodName = mainMethodName;
        }
    }
}
