using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler
{
    public class NameManagerNetComparer : IEqualityComparer<NameManagerReadOnly>
    {
        public bool Equals(NameManagerReadOnly x, NameManagerReadOnly y)
        {
            return x.Equals(y, StringHelper.NetComparer);
        }

        public int GetHashCode(NameManagerReadOnly obj)
        {
            return obj.GetHashCode();
        }
    }
}
