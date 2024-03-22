using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNLib
{
    public class Очередь<T> : Queue<T>
    {

        public Очередь(int count):base(count)
        {
        }

        public Очередь()
        {
        }

        public void Добавить(T item) => Enqueue(item);

        public T Удалить() => Dequeue();

        public int Длина() => Count;

        public T Вершина() => Peek();

        public void УсесьДлину() => TrimExcess();

        public Массив<T> КМассиву() => new Массив<T>(this);
    }
}
