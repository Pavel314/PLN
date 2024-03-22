using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.PLNReflection.TypeFinder
{
    public sealed class NamespaceCollection : UniqueCollection<NameManagerReadOnly>
    {
        public NamespaceCollection(TypeFinder owner) : base(new NameManagerNetComparer())
        {
            if (owner == null) throw new ArgumentNullException("owner");
            Owner = owner;
        }

       public override bool Add(NameManagerReadOnly item)
        {
            var res = base.Add(item);
            if (res)
            {
                if (!Owner.OnChangeNamespace(item, TypeFinder.ChangedType.Add, Location))
                {
                    base.Remove(item);
                    return false;
                }
                return true;
            }
            Owner.OnNamespaceArleadyUsing(new NamespaceArleadyUsingEventArgs(item,Location));
            return false;
        }

        public bool Add(NameManagerReadOnly item,Syntax.Location location)
        {
            Location = location;
            var res= Add(item);
            Location = null;
            return res;
        }

        public override bool Remove(NameManagerReadOnly item)
        {
            var res = base.Remove(item);
            if (res) Owner.OnChangeNamespace(item, TypeFinder.ChangedType.Remove,null);
            return res;
        }

        public override void Clear()
        {//!
            Owner.OnChangeNamespace(null, TypeFinder.ChangedType.Clear,null);
            base.Clear();
        }
        internal void _Remove(NameManagerReadOnly item)
        {
            var res = base.Remove(item);
        }

        private TypeFinder Owner;
        private Syntax.Location Location;
    }
}
