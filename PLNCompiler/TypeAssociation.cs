using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler
{
    public struct TypeAssociation
    {
        public TypeAssociation(Type type, TypeName name)
        {
            Type = type;
            Name = name;
        }

        public readonly Type Type; 
        public readonly TypeName Name;
    }
}
