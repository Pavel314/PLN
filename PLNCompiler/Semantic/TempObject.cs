using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Semantic
{
    public class TempObject
    {
        public object Value { get; private set; }
        public Type Type { get; private set; }
        public TypeCode Code { get; private set; }
        public bool IsNull { get; private set; }

        public TempObject(object value, Type type, TypeCode code)
        {
            Value = value;
             if (ReferenceEquals(value, null) && ReferenceEquals(type,null) && code==TypeCode.Empty) IsNull = true;
          //  if (ReferenceEquals(value, null) && code == TypeCode.Empty) IsNull = true;
            Type = type;
            Code = code;
#if DEBUG
            if (IsNull) return;

            if (Code != Type.ToTypeCode())
                throw new ArgumentException("Type and code not equivalent");
#endif
        }
        public TempObject(object value, Type type) : this(value, type, type.ToTypeCode())
        {

        }

        public override string ToString()
        {
            return string.Format("TempObject:[Value={0}, Type={1}, TypeCode={2}]", Value, Type, Code);
        }

    }
}
