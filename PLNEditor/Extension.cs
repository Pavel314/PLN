using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNEditor
{
   public static class Extension
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> items)
        {
            return ReferenceEquals(items, null) || !items.Any();
        }

        public static bool IsNull(this object obj)
        {
            return ReferenceEquals(obj, null);
        }
    }
}
