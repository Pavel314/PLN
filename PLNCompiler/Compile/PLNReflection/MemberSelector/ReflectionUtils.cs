using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PLNCompiler.Compile.PLNReflection.MemberSelector
{
  public  class ReflectionUtils
    {
        public static string GetDefaultName(Type type)
        {
            if (!type.IsClass && !type.IsValueType && !type.IsInterface) return null;
            var attributes = type.GetCustomAttributes(typeof(DefaultMemberAttribute), false);
            if (attributes.IsNullOrEmpty()) return null;
            // if (atrs.Length >1) return null;//!TODO
            return ((DefaultMemberAttribute)(attributes[0])).MemberName;
        }
    }
}
