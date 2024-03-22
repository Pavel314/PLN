using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace PLNCompiler.Syntax.TreeConverter
{
   public class DirectiveInfo
    {
        public bool UseSystemLibrary { get; private set; }
        public ApplicationKind ApplicationKind { get; private set; }
        public string CompilationPath { get; private set; }

        public DirectiveInfo(ApplicationKind applicationKind, bool useSystemLibrary,string compilationPath)
        {
            ApplicationKind = applicationKind;
            UseSystemLibrary = useSystemLibrary;
            CompilationPath = compilationPath;
        }

        public DirectiveInfo(DirectiveInfo info, string compilationPath)
        {
            ApplicationKind = info.ApplicationKind;
            UseSystemLibrary = info.UseSystemLibrary;
            CompilationPath = compilationPath;
        }

        public static PEFileKinds ToPEFileKinds(ApplicationKind kind)
        {
            switch (kind)
            {
                case ApplicationKind.Console:return PEFileKinds.ConsoleApplication;
                case ApplicationKind.Window:return PEFileKinds.WindowApplication;
                default:throw new PresentVariantNotImplementedException(typeof(ApplicationKind));
            }
        }

    }

    public enum ApplicationKind {Console,Window }

}
