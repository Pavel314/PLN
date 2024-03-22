using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.PLNReflection.MemberSelector
{
   public interface IMassCalculator
    {
        int CalculateMass(Type primary, Type target);
    }
}
