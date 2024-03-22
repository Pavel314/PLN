using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler
{

    public class AddOnlyUniqueCollection<T> : IAddOnlyCollection<T>//, ISet<T>
    {
        public AddOnlyUniqueCollection() { Set = new HashSet<T>(); }

        public AddOnlyUniqueCollection(IEnumerable<T> collection)
        {
            Set = new HashSet<T>(collection);
        }

        public AddOnlyUniqueCollection(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            Set = new HashSet<T>(collection,comparer);
        }

        public AddOnlyUniqueCollection( IEqualityComparer<T> comparer)
        {
            Set = new HashSet<T>(comparer);
        }

        public virtual bool Add(T item)
        {
            return Set.Add(item);
        }

        public bool Contains(T item)
        {
            return Set.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Set.CopyTo(array, arrayIndex);
        }


        public int Count { get { return Set.Count; } }

        public IEnumerator<T> GetEnumerator()
        {
            return Set.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEqualityComparer<T> Comarer { get { return Set.Comparer; } }

        protected HashSet<T> Set;
    }
}
