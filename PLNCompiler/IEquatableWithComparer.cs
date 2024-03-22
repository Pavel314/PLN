using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler
{
  public  interface IEquatableWithComparer<TType,TComparerType>:IEquatable<TType>
    {
      bool Equals(TType other, IEqualityComparer<TComparerType> compare);
    }
}
