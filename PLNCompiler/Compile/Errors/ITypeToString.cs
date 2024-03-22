using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.Errors
{
  public interface ITypeToString
    {
        string ToString(Type type);
    }
}
