using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace PLNCompiler.Compile.TypeManager.NativeConversion
{
    public static class ConversionCodes
    {
        private static IReadOnlyDictionary<Type, NativeTypeInfo> Codes;
        static ConversionCodes()
        {
            Codes = new Dictionary<Type, NativeTypeInfo>
            {
                { typeof(SByte), new NativeTypeInfo(OpCodes.Conv_I1, OpCodes.Conv_Ovf_I1, OpCodes.Conv_Ovf_I1_Un,1,0,NativeTypeKind.Sign) },
                { typeof(Byte), new NativeTypeInfo(OpCodes.Conv_U1, OpCodes.Conv_Ovf_U1, OpCodes.Conv_Ovf_U1_Un,1,0,NativeTypeKind.UnSign) },
                { typeof(Int16), new NativeTypeInfo(OpCodes.Conv_I2, OpCodes.Conv_Ovf_I2, OpCodes.Conv_Ovf_I2_Un,2,1,NativeTypeKind.Sign) },
                { typeof(UInt16), new NativeTypeInfo(OpCodes.Conv_U2, OpCodes.Conv_Ovf_U2, OpCodes.Conv_Ovf_U2_Un,2,1,NativeTypeKind.UnSign) },

                { typeof(Char), new NativeTypeInfo(OpCodes.Conv_U2, OpCodes.Conv_Ovf_U2, OpCodes.Conv_Ovf_U2_Un,2,1,NativeTypeKind.UnSign) },

                { typeof(Int32), new NativeTypeInfo(OpCodes.Conv_I4, OpCodes.Conv_Ovf_I4, OpCodes.Conv_Ovf_I4_Un,4,2,NativeTypeKind.Sign) },
                { typeof(UInt32), new NativeTypeInfo(OpCodes.Conv_U4, OpCodes.Conv_Ovf_U4, OpCodes.Conv_Ovf_U4_Un,4,2,NativeTypeKind.UnSign) },
                { typeof(Int64), new NativeTypeInfo(OpCodes.Conv_I8, OpCodes.Conv_Ovf_I8, OpCodes.Conv_Ovf_I8_Un,8,3,NativeTypeKind.Sign) },
                { typeof(UInt64), new NativeTypeInfo(OpCodes.Conv_U8, OpCodes.Conv_Ovf_U8, OpCodes.Conv_Ovf_U8_Un,8,3,NativeTypeKind.UnSign) },

                { typeof(Single), new NativeTypeInfo(OpCodes.Conv_R4, OpCodes.Conv_R4, OpCodes.Conv_R_Un,4,2,NativeTypeKind.Float) },
                { typeof(Double), new NativeTypeInfo(OpCodes.Conv_R8, OpCodes.Conv_R8, OpCodes.Conv_R_Un,8,3,NativeTypeKind.Float) }
            };
        }
        public static bool Get(Type type, out NativeTypeInfo value)
        {
            return Codes.TryGetValue(type, out value);
        }
    }
}
