using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNLib
{
    public class Список<T> : List<T>
    {
        public Список()
        {
        }

        public Список(int count) : base(count)
        {
        }

        public T Элемент(int index) => this[index];

        public void Элемент(int index, T item) => this[index] = item;

        public int Длина() => Count;

        public void Очистить() => Clear();

        public bool Содержит(T item) => Contains(item);

        public void Cкопировать(T[] array, int arrayIndex) => CopyTo(array, arrayIndex);

        public bool Эквивалентно(object other, IEqualityComparer comparer) => Equals(other, comparer);

        public int ИндексОт(T item) => IndexOf(item);

        public void Вставить(int index, T item) => Insert(index, item);

        public bool Удалить(T item) => Remove(item);

        public void УдалитьС(int index) => RemoveAt(index);

        public void Добавить(T item) => Add(item);

        public void УсесьДлину() => TrimExcess();

        public Массив<T> КМассиву() => new Массив<T>(this);
    }
}
