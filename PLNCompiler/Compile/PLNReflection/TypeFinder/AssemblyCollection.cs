using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PLNCompiler.Compile.PLNReflection.TypeFinder
{
    public sealed class AssemblyCollection : UniqueCollection<Assembly>
    {
        public AssemblyCollection(TypeFinder owner) : base()
        {
            if (owner == null) throw new ArgumentNullException("owner");
            Owner = owner;
        }

        public override bool Add(Assembly item)
        {
            var res = base.Add(item);
            if (res) Owner.OnChangedAssembly(item, TypeFinder.ChangedType.Add);
            return res;
        }

        public override bool Remove(Assembly item)
        {
            var res = base.Remove(item);
            if (res) Owner.OnChangedAssembly(item, TypeFinder.ChangedType.Remove);
            return res;
        }

        public override void Clear()
        {
            Owner.OnChangedAssembly(null, TypeFinder.ChangedType.Clear);
            base.Clear();
        }

        private TypeFinder Owner;
    }
}
