using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler
{
public interface IAddOnlyList<T>:IAddOnlyCollection<T>
    {
        new void Add(T item);
        T this[int index] { get; }
    }
}
