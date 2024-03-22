using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace PLNCompiler.Semantic
{
    public class LocalWrapper
    {
        public Type Type { get; private set; }
        public string Name { get; private set; }
        public LocalBuilder Local { get; private set; }

        public void SetLocal(LocalBuilder local)
        {
            if (!ReferenceEquals(Local, null))
                throw new InvalidOperationException("Local arleady setted");
            Local = local;
        }

        public LocalWrapper(Type type,string name)
        {
            Type = type;
            Name = name;
        }
    }
}
