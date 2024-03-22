using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler
{
    public class NameManager : NameManagerReadOnly,IList<string>
    {
        public NameManager() : base()
        {
        }
        public NameManager(IEnumerable<string> parts) : base(parts)
        {
        }

        public void Add(string item)
        {
            Names.Add(item);
            _UpdateOnToString();
        }

        public override object Clone()
        {
            return CloneAs();
        }
        public new NameManager CloneAs()
        {
            return new NameManager(this);
        }

        public void Insert(int index, string item)
        {
            Names.Insert(index, item);
            _UpdateOnToString();
        }

        public void RemoveAt(int index)
        {
            Names.RemoveAt(index);
            _UpdateOnToString();
        }

        public int IndexOf(string item)
        {
            return Names.FindIndex(f => DefaultComparer.Equals(f, item));
        }

        public bool Remove(string item)
        {
            var index = IndexOf(item);
            if (index == -1) return false;
            RemoveAt(index);
            _UpdateOnToString();
            return true;
        }

        public bool Contains(string item)
        {
            return Names.Contains(item, DefaultComparer);
        }

        public bool IsReadOnly { get { return false; } }

        public void Clear()
        {
            Names.Clear();
            _UpdateOnToString();
        }

        public void CopyTo(string[] array, int index)
        {
            Names.CopyTo(array, index);
        }

        public new string this[int index]
        {
            get { return base[index]; }
            set { Names[index] = value; _UpdateOnToString(); }
        }

    }
}
