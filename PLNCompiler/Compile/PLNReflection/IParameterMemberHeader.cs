using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PLNCompiler.Compile.PLNReflection
{
  public  interface IParameterMemberHeader:IMemberHeader
    {
        IReadOnlyList<Parameter> Parameters { get; }
    }
}
