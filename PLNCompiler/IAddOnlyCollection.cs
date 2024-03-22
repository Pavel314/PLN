using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler
{
    public interface IAddOnlyCollection<T> : IReadOnlyCollection<T>
    {
        bool Add(T item);
        bool Contains(T item);
        void CopyTo(T[] array, int arrayIndex);
    }
}
