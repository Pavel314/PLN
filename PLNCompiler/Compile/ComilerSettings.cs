using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace PLNCompiler.Compile
{
    public class ComilerSettings
    {
        public string FileName { get; private set; }
        public string DirectoryPath { get; private set; }
        public string ShortName { get; private set; }
        public string ShortNameWithoutExtension { get; private set; }

        public bool DebugMode { get; private set; }

        public AssemblyBuilderAccess AssemblyAccess { get; private set; }

        public string MainClassName { get; private set; }
        public string MainMethodName { get; private set; }


        public ComilerSettings(bool debugMode,string fileName, string mainClassName="Program",string mainMethodName="Main", AssemblyBuilderAccess assemblyAccess= AssemblyBuilderAccess.Save)
        {
            DebugMode = debugMode;
            FileName = fileName;
            MainClassName = mainClassName;
            MainMethodName = mainMethodName;
            AssemblyAccess = assemblyAccess;
            DirectoryPath = System.IO.Path.GetDirectoryName(fileName);
            if (string.IsNullOrEmpty(DirectoryPath))
                DirectoryPath = null;
            ShortName = System.IO.Path.GetFileName(fileName);
            ShortNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(ShortName);
        }

        //public ComilerSettings(bool debugMode) : this(debugMode, "PLNProgram")
        //{
        //}



        //public virtual void ResetProgramName()
        //{
        //    ProgramName = "PLNProgram";
        //}

        //public virtual string GetFileName()
        //{
        //    return ProgramName + ".exe";
        //}

    }
}
