using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler
{
  public  class AddOnlyList<T>:IAddOnlyList<T>,IReadOnlyList<T>
    {
        public int Count => items.Count;

        public T this[int index] => items[index];

        bool IAddOnlyCollection<T>.Add(T item)
        {
            items.Add(item);
           return true;
        }

        public virtual void Add(T item)
        {
            items.Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            this.items.AddRange(items);
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
             items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public AddOnlyList()
        {
            items = new List<T>();
        }

        public AddOnlyList(IEnumerable<T> items)
        {
            if (items.IsNullOrEmpty())
                this.items = new List<T>(); else
           this.items = new List<T>(items);
        }

        private List<T> items;
    }
}
