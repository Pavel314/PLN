using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler
{
    public class NameManagerReadOnly:ICloneable,IReadOnlyList<string>,IEquatableWithComparer<NameManagerReadOnly,string>
    {
        protected static readonly IEqualityComparer<string> DefaultComparer = StringHelper.PLNComparer; 

        public NameManagerReadOnly(IEnumerable<string> names)
        {
            Names = new List<string>(names);
        }
        public NameManagerReadOnly(string name,string delim):this(Split(name, delim))
        {
        }

        protected NameManagerReadOnly()
        {
            Names = new List<string>();
        }

        protected virtual string GetAddjectString() { return null; }

        public string ToString(string Delim)
        {
            if (Delim == _oldDelim && !_ShouldBeUpdate) return _TmpString;

            _TmpString = null;

            var length = Names.Count;
            if (length <= 0) return _TmpString;

            for (int i = 0; i < length - 1; i++)
                _TmpString += Names[i] + Delim;

            _TmpString += Names[length - 1]+ GetAddjectString();
            _ShouldBeUpdate = false;
            _oldDelim = Delim;
            return _TmpString;
        }

        public string ToStringNet()
        {
            return ToString(StringHelper.NetTypeDelimiter);
        }

        public string ToStringNested()
        {
            return ToString(StringHelper.NestedDelimiter);
        }

        public override string ToString()
        {
            return ToString(".");
        }
        public string this[int index]
        {
            get { return Names[index]; }
        }

        public string Last()
        {
            return this[Count - 1];
        }

        public string First()
        {
            return this[0];
        }

        public int Count { get { return Names.Count; } }

        public IEnumerator<string> GetEnumerator()
        {
            return Names.GetEnumerator();
        }

        public virtual object Clone()
        {
            return CloneAs();
        }
        public virtual NameManagerReadOnly CloneAs()
        {
            return new NameManagerReadOnly(this.ToArray());
        }



        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool StartsWith(NameManagerReadOnly value, IEqualityComparer<string> comparer)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException("value");
            if (value.Count > Count) return false;
            if (value.Count == 0) return Count == 0;

            for (int i = 0; i < value.Count; i++)
                if (!comparer.Equals(value[i], this[i])) return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as NameManagerReadOnly);
        }

        public bool Equals(NameManagerReadOnly obj, IEqualityComparer<string> comparer)
        {
            if (ReferenceEquals(obj, null)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (Count != obj.Count) return false;
            for (int i = 0; i < Count; i++)
                if (!comparer.Equals(this[i], obj[i])) { return false; }
            return true;
        }

        public virtual bool Equals(NameManagerReadOnly obj)
        {
            return Equals(obj,DefaultComparer);
        }

        public override int GetHashCode()
        {
            return Count;
        }

        public static bool operator ==(NameManagerReadOnly name1, NameManagerReadOnly name2)
        {
            if (ReferenceEquals(name1, null)) return ReferenceEquals(name2, null);
            return name1.Equals(name2);
        }
        public static bool operator !=(NameManagerReadOnly name1, NameManagerReadOnly name2)
        {
            return !(name1 == name2);
        }

        protected List<string> Names = new List<string>();

        protected void _UpdateOnToString()
        {
            _ShouldBeUpdate = true;
        }

        private bool _ShouldBeUpdate = true;
        private string _TmpString = null;
        private string _oldDelim = null;

        public static string[] Split(string nameSpace, string delim)
        {
            delimArr[0] = delim;
            return nameSpace.Split(delimArr, StringSplitOptions.RemoveEmptyEntries);
        }
        public static string[] SplitNetDelim(string nameSpace)
        {
            delimArr[0] = StringHelper.NetTypeDelimiter;
            return nameSpace.Split(delimArr, StringSplitOptions.RemoveEmptyEntries);
        }
        private static string[] delimArr = new string[1];
      
    }




}
