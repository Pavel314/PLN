using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax
{
    public enum ParsableConstantKind { UInt64,Int64, Double,  String, Char, Boolean,Null }

    public class ParsableConstant
    {
        public ParsableConstantKind Kind { get; private set; }
        public object Value { get; private set; }

        private ParsableConstant(ParsableConstantKind kind,object value)
        {
            Kind = kind;
            Value = value;
        }

        public ParsableConstant(UInt64 value):this(ParsableConstantKind.UInt64, value)
        {
            if (value <= Int64.MaxValue)
            {
             //  if (!MinimizeInt64ToInt32(int64))
             //   {
                    Value = (Int64)value;
                    Kind = ParsableConstantKind.Int64;
                //}
            }
        }
        public ParsableConstant(Int64 value) : this(ParsableConstantKind.Int64, value)
        {
          //  MinimizeInt64ToInt32(value);
        }
        //public ParsableConstant(Int32 value) : this(ParsableConstantKind.Int32, value)
        //{
        //}
        public ParsableConstant(Double  value) : this( ParsableConstantKind.Double, value)
        {
        }
        public ParsableConstant(String value) : this( ParsableConstantKind.String, value)
        {
        }
        public ParsableConstant(Char value) : this( ParsableConstantKind.Char, value)
        {
        }
        public ParsableConstant(Boolean value) : this(ParsableConstantKind.Boolean, value)
        {
        }

        public virtual Semantic.TypedConstant ToTypedConstant()
        {
            switch (Kind)
            {
                case ParsableConstantKind.Boolean:return new Semantic.TypedConstant((Boolean)Value);
                case ParsableConstantKind.Char: return new Semantic.TypedConstant((Char)Value);
                case ParsableConstantKind.Double: return new Semantic.TypedConstant((Double)Value);
                case ParsableConstantKind.String: return new Semantic.TypedConstant((String)Value);
                case ParsableConstantKind.UInt64: return new Semantic.TypedConstant((UInt64)Value);
                case ParsableConstantKind.Null:return Semantic.TypedConstant.CreateForNull();
                case ParsableConstantKind.Int64:
                    {
                        var i64 = (Int64)Value;
                        if ( i64 >= Int32.MinValue && i64 <= Int32.MaxValue)
                            return new Semantic.TypedConstant((Int32)i64);
                        return new Semantic.TypedConstant(i64);
                    }
                default:throw new PresentVariantNotImplementedException(typeof(ParsableConstantKind));
            }

        }

        public override string ToString()
        {
            return string.Format("ParsableConstant:[Kind={0} Value={1}]", Kind, Value);
        }

        //private bool MinimizeInt64ToInt32(Int64 number)
        //{
        //    if (number <= Int32.MaxValue && number >= Int32.MinValue)
        //    {
        //        Value = (Int32)number;
        //        Kind = ParsableConstantKind.Int32;
        //        return true;
        //    }
        //    return false;
        //}

        public static ParsableConstant CreateForNull()
        {
            return new ParsableConstant(ParsableConstantKind.Null, null);
        }

        public static ParsableConstant CreatePLNString1(string value)
        {
            var trim = value.TrimFLChar();
            if (trim.Length==1) return new ParsableConstant(trim[0]);
            return new ParsableConstant(trim);
        }

        public static ParsableConstant CreatePLNString2(string value)
        {
            return new ParsableConstant(value.TrimFLChar());
        }

    }
}
