using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler
{
  public  class VirtualList<T>: IList<T>,IReadOnlyList<T>
    {
 
       public  int Count => List.Count;

        public bool IsReadOnly => ((IList<T>)List).IsReadOnly;

        public T this[int index] { get => List[index]; set => List[index] = value; }

        public virtual int IndexOf(T item)
        {
            return List.IndexOf(item);
        }

        public virtual void Insert(int index, T item)
        {
            List.Insert(index, item);
        }

        public virtual void RemoveAt(int index)
        {
           List.RemoveAt(index);
        }

        public virtual void Add(T item)
        {
            List.Add(item);
        }

        public virtual void Clear()
        {
            List.Clear();
        }

        public virtual bool Contains(T item)
        {
            return List.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            List.CopyTo(array, arrayIndex);
        }

        public virtual bool Remove(T item)
        {
            return List.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return List.GetEnumerator();
        }

        public VirtualList()
        {
            List = new List<T>();
        }

        public VirtualList(int capacity)
        {
            List = new List<T>(capacity);
        }


        private List<T> List;

    }
}
