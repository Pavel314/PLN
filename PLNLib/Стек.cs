using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNLib
{
   public class Стек<T>: Stack<T>
    {
        public Стек()
        {
        }

        public Стек(int count):base(count)
        {
        }

        public void Добавить(T item) => Push(item);

        public T Удалить() => Pop();

        public int Длина() => Count;

        public T Вершина() => Peek();

        public void УсесьДлину() => TrimExcess();

        public Массив<T> КМассиву() => new Массив<T>(this);

    }
}
