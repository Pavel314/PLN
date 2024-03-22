using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.Errors
{
    public class PLNTypeToString : ITypeToString
    {
        public string ToString(Type type)
        {
            if (ReferenceEquals(type, null))
                return "нуль";
            return type.ToString();
        }
    }
}
