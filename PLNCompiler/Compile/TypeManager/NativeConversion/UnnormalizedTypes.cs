using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.TypeManager.NativeConversion
{
  public static  class UnnormalizedTypes
    {

        public static bool Contains(Type type)
        {
            return Types.Contains(type);
        }

        private static HashSet<Type> Types;

        static UnnormalizedTypes()
        {
            Types = new HashSet<Type>
            {
                typeof(SByte),
                typeof(Byte),
                typeof(Int16),
                typeof(UInt16)
            };
        }

    }
}
