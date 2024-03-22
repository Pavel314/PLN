using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler
{
    public class UniqueCollection<T> : AddOnlyUniqueCollection<T>, ICollection<T>
    {
        public UniqueCollection() : base() { }

        public UniqueCollection(IEnumerable<T> collection) : base(collection) { }

        public UniqueCollection(IEnumerable<T> collection, IEqualityComparer<T> comparer):base(collection,comparer)
        {
        }

        public UniqueCollection(IEqualityComparer<T> comparer):base(comparer)
        {
        }

        public virtual void Clear()
        {
            Set.Clear();
        }

        public virtual bool Remove(T item)
        {
            return Set.Remove(item);
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }
    }
}
