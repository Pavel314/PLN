using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace PLNCompiler.Compile.CodeGeneration
{

    public class ConstantPusher
    {
        public ILGenerator IL { get; set; }
        public ConstantPusher(ILGenerator il = null) { IL = il; }

        protected virtual void Push(object value,TypeCode code)
        {
            switch (code)
            {
                case TypeCode.SByte: Push((SByte)value); break;
                case TypeCode.Byte: Push((Byte)value); break;
                case TypeCode.Int16: Push((Int16)value); break;
                case TypeCode.UInt16: Push((UInt16)value); break;
                case TypeCode.Int32: Push((Int32)value); break;
                case TypeCode.UInt32: Push((UInt32)value); break;
                case TypeCode.Int64: Push((Int64)value); break;
                case TypeCode.UInt64: Push((UInt64)value); break;
                case TypeCode.Single: Push((Single)value); break;
                case TypeCode.Double: Push((Double)value); break;
                case TypeCode.Boolean: Push((Boolean)value); break;
                case TypeCode.Char: Push((Char)value); break;
                case TypeCode.String: Push((String)value); break;
                case TypeCode.Empty: PushNull(); break;
                default: throw new PresentVariantNotImplementedException(typeof(TypeCode));
            }
        }
        public void Push(object value)
        {
            Push(value,Type.GetTypeCode(value.GetType()));
        }
        public void Push(object value,Type type)
        {
#if DEBUG
            if (value.GetType() != type)
                throw new ArgumentException("value type != type");
#endif
            Push(value,Type.GetTypeCode(type));
        }

        public void Push(Semantic.TypedConstant value)
        {
            Push(value.Value, value.Code);
            //switch (value.Code)
            //{
            //    case TypeCode.SByte: Push((SByte)value.Value); break;
            //    case TypeCode.Byte: Push((Byte)value.Value); break;
            //    case TypeCode.Int16: Push((Int16)value.Value); break;
            //    case TypeCode.UInt16: Push((UInt16)value.Value); break;
            //    case TypeCode.Int32: Push((Int32)value.Value); break;
            //    case TypeCode.UInt32: Push((UInt32)value.Value); break;
            //    case TypeCode.Int64: Push((Int64)value.Value); break;
            //    case TypeCode.UInt64: Push((UInt64)value.Value); break;
            //    case TypeCode.Single: Push((Single)value.Value); break;
            //    case TypeCode.Double: Push((Double)value.Value); break;
            //    case TypeCode.Boolean: Push((Boolean)value.Value); break;
            //    case TypeCode.Char: Push((Char)value.Value); break;
            //    case TypeCode.String: Push((String)value.Value); break;
            //    case TypeCode.Empty:IL.Emit(OpCodes.Ldnull);break;
            //    default: throw new PresentVariantNotImplementedException(typeof(TypeCode));
            //}
        }

        public virtual void Push(String value)
        {
            IL.Emit(OpCodes.Ldstr, value);
        }

        public virtual void Push(Char value)
        {
            Push((UInt16)value);
        }


        public virtual void Push(Int64 value)
        {
            //if (value >= Int32.MinValue && value <= Int32.MaxValue)
            //{
            //    Push(unchecked((Int32)value));
            //    IL.Emit(OpCodes.Conv_I8);
            //}
            //else
                IL.Emit(OpCodes.Ldc_I8, value);
        }
        public virtual void Push(UInt64 value)
        {
            Push(unchecked((Int64)value));
        }
        public virtual void Push(Int32 value)
        {
            switch (value)
            {
                case 0: IL.Emit(OpCodes.Ldc_I4_0); break;
                case 1: IL.Emit(OpCodes.Ldc_I4_1); break;
                case 2: IL.Emit(OpCodes.Ldc_I4_2); break;
                case 3: IL.Emit(OpCodes.Ldc_I4_3); break;
                case 4: IL.Emit(OpCodes.Ldc_I4_4); break;
                case 5: IL.Emit(OpCodes.Ldc_I4_5); break;
                case 6: IL.Emit(OpCodes.Ldc_I4_6); break;
                case 7: IL.Emit(OpCodes.Ldc_I4_7); break;
                case 8: IL.Emit(OpCodes.Ldc_I4_8); break;
                case -1: IL.Emit(OpCodes.Ldc_I4_M1); break;
                default:
                    if (value >= SByte.MinValue && value <= SByte.MaxValue)
                        IL.Emit(OpCodes.Ldc_I4_S, (SByte)value);
                    else
                        IL.Emit(OpCodes.Ldc_I4, value); break;
            }
        }
        public virtual void Push(UInt32 value)
        {
            Push(unchecked((Int32)value));
        }

        public virtual void Push(Boolean value)
        {
            if (value) IL.Emit(OpCodes.Ldc_I4_1); else IL.Emit(OpCodes.Ldc_I4_0);
        }

        public virtual void Push(Int16 value)
        {
            Push(unchecked((Int32)value));
        }
        public virtual void Push(UInt16 value)
        {
            Push(unchecked((Int32)value));
        }

        public virtual void Push(SByte value)
        {
            Push(unchecked((Int32)value));
        }
        public virtual void Push(Byte value)
        {
            Push(unchecked((Int32)value));
        }

        public virtual void Push(Single value)
        {
            IL.Emit(OpCodes.Ldc_R4, value);
        }

        public virtual void Push(Double value)
        {
            IL.Emit(OpCodes.Ldc_R8, value);
        }

        public virtual void PushNull()
        {
            IL.Emit(OpCodes.Ldnull);
        }
    }
}
