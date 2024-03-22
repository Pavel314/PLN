using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler
{
    public class TypeName : NameManagerReadOnly,IEquatableWithComparer<TypeName,string>
    {
        public int GenericParametersCount { get; private set; }

        public TypeName(IEnumerable<string> identifers, int genericParametersCount) : base(identifers)
        {
            GenericParametersCount = genericParametersCount;
        }

        public static TypeName UsedGenericsIfNotEmpty(IEnumerable<string> identifers, int genericParametersCount)
        {
            if (identifers.IsNullOrEmpty()) genericParametersCount = 0;
            return new TypeName(identifers, genericParametersCount);

        }

        public override object Clone()
        {
            return new TypeName(base.CloneAs(),GenericParametersCount);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TypeName);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ GenericParametersCount;  
           
        }

        public bool Equals(TypeName other)
        {
            return Equals(other, DefaultComparer);
        }

        public bool Equals(TypeName other, IEqualityComparer<string> compare)
        {
            return base.Equals(other,compare) && GenericParametersCount.Equals(other.GenericParametersCount);
        }

        public static bool operator ==(TypeName name1, TypeName name2)
        {
            if (ReferenceEquals(name1, null)) return ReferenceEquals(name2, null);
            return name1.Equals(name2);
        }
        public static bool operator !=(TypeName name1, TypeName name2)
        {
            return !(name1 == name2);
        }

        public static TypeName FromType(Type type)
        {
            //  if (type.IsNested) throw new ArgumentException("nested types not supported");
            int genericCount = 0;
            if (type.IsGenericTypeDefinition)
                genericCount = type.GetGenericArguments().Length;
            return new TypeName(SplitType(type.GetGenericFullNameIgnore()), 0);
        }

        protected override string GetAddjectString()
        {
            //if (GenericParametersCount == 0) return null;
            return string.Format("[Универсальных параметров:{0}]", GenericParametersCount);
        }

        private static string[] SplitType(string type)
        {
            return type.Split(delimType, StringSplitOptions.RemoveEmptyEntries);
        }
        private static string[] delimType = new string[2] { StringHelper.NetTypeDelimiter, StringHelper.NestedDelimiter };

    }
}
