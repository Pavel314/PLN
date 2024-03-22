using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Compile;

namespace PLNCompiler.Semantic
{
    public class TypedConstant:TempObject
    {
        public TypedConstant(SByte value) : base(value, typeof(SByte), TypeCode.SByte) { }
        public TypedConstant(Byte value) : base(value, typeof(Byte), TypeCode.Byte) { }
        public TypedConstant(Int16 value) : base(value, typeof(Int16), TypeCode.Int16) { }
        public TypedConstant(UInt16 value) : base(value, typeof(UInt16), TypeCode.UInt16) { }
        public TypedConstant(Int32 value) : base(value, typeof(Int32), TypeCode.Int32) { }
        public TypedConstant(UInt32 value) : base(value, typeof(UInt32), TypeCode.UInt32) { }
        public TypedConstant(Int64 value) : base(value, typeof(Int64), TypeCode.Int64) { }
        public TypedConstant(UInt64 value) : base(value, typeof(UInt64), TypeCode.UInt64) { }
        public TypedConstant(Double value) : base(value, typeof(Double), TypeCode.Double) { }
        public TypedConstant(Single value) : base(value, typeof(Single), TypeCode.Single) { }
        public TypedConstant(Boolean value) : base(value, typeof(Boolean), TypeCode.Boolean) { }
        public TypedConstant(Char value) : base(value, typeof(Char), TypeCode.Char) { }
        public TypedConstant(String value) : base(value, typeof(String), TypeCode.String) { }
     //   public TypedConstant(Object value) : base(value, typeof(Object), TypeCode.Object) { }
        private TypedConstant() : base(null, null, TypeCode.Empty) { }
        //PLN.Compile.TypeManager.PLNTypeConversion.OBJECT

        public static TypedConstant CreateForNull()
        {
            return new TypedConstant();
        }

    }
}
