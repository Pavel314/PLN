using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNLib
{
    public class Массив<T> : IStructuralComparable, IStructuralEquatable, IList<T>, IReadOnlyList<T>, IReadOnlyCollection<T>, ICollection<T>, IEnumerable<T>
    {
        private T[] items;

        public Массив(IEnumerable<T> source)
        {
            items = source.ToArray();
        }

        public Массив(int количество)
        {
            items = new T[количество];
        }

        public T Элемент(int index)
        {
            return items[index];
        }

        public void Элемент(int index, T item)
        {
            items[index] = item;
        }

        public int Длина()
        {
            return items.Length;
        }

        public void Очистить()
        {
            Clear();
        }

        public bool Содержит(T item)
        {
            return Contains(item);
        }

        public void Cкопировать(T[] array, int arrayIndex)
        {
            CopyTo(array, arrayIndex);
        }

        public bool Эквивалентно(object other, IEqualityComparer comparer)
        {
            return Equals(other, comparer);
        }

        public int ХешКод(IEqualityComparer comparer)
        {
            return GetHashCode(comparer);
        }

        public int ИндексОт(T item)
        {
            return ((IList<T>)this).IndexOf(item);
        }

        //public void Вставить(int index, T item)
        //{
        //    ((IList<T>)items).Insert(index, item);
        //}

        //public bool Удалить(T item)
        //{
        //    return ((IList<T>)this).Remove(item);
        //}

        //public void УдалитьС(int index)
        //{
        //    ((IList<T>)this).RemoveAt(index);
        //}

        #region Original
        public T this[int index] => items[index];

        T IList<T>.this[int index] { get => items[index]; set => items[index] = value; }



        public int Count => ((IReadOnlyCollection<T>)items).Count;

        public bool IsReadOnly => items.IsReadOnly;

        int ICollection<T>.Count => throw new NotImplementedException();

        bool ICollection<T>.IsReadOnly => throw new NotImplementedException();

        public void Clear()
        {
            ((ICollection<T>)items).Clear();
        }



        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public bool Equals(object other, IEqualityComparer comparer)
        {
            return ((IStructuralEquatable)items).Equals(other, comparer);
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return ((IStructuralEquatable)items).GetHashCode(comparer);
        }

        void ICollection<T>.Add(T item)
        {
            ((ICollection<T>)items).Add(item);
        }

        void ICollection<T>.Clear()
        {
            ((ICollection<T>)items).Clear();
        }

        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            return ((IStructuralComparable)items).CompareTo(other, comparer);
        }

        bool ICollection<T>.Contains(T item)
        {
            return ((ICollection<T>)items).Contains(item);
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            ((ICollection<T>)items).CopyTo(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)items).GetEnumerator();
        }

        int IList<T>.IndexOf(T item)
        {
            return ((IList<T>)items).IndexOf(item);
        }

        void IList<T>.Insert(int index, T item)
        {
            ((IList<T>)items).Insert(index, item);
        }

        bool ICollection<T>.Remove(T item)
        {
            return ((ICollection<T>)items).Remove(item);
        }

        void IList<T>.RemoveAt(int index)
        {
            ((IList<T>)items).RemoveAt(index);
        }
        #endregion
    }

}
