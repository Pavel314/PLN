using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.TypeManager.NativeConversion
{
   public static class PLNExplicitCast
    {
        public static IReadOnlyDictionary<Type, IReadOnlyDictionary<Type, CastInfo>> Rules;

        static PLNExplicitCast()
        {
            Rules = new Dictionary<Type, IReadOnlyDictionary<Type, CastInfo>>
            {
                {typeof(SByte),new Dictionary<Type, CastInfo>{
                    { typeof(Byte), new CastInfo(NativeFunction.Conv_Ovf_U1 ,WeightInfo.Native(1)) },
                    { typeof(UInt16), new CastInfo(NativeFunction.Conv_Ovf_U2,WeightInfo.Native(2)) },
                    { typeof(UInt32), new CastInfo(NativeFunction.Conv_Ovf_U4,WeightInfo.Native(3)) },
                    { typeof(UInt64), new CastInfo(NativeFunction.Conv_Ovf_U8,WeightInfo.Native(4)) },
                } },


                {typeof(Byte),new Dictionary<Type, CastInfo>{
                    { typeof(SByte), new CastInfo(NativeFunction.Conv_Ovf_I1_Un,WeightInfo.Native(1)) },
                } },


                {typeof(Int16),new Dictionary<Type, CastInfo>{
                    { typeof(SByte), new CastInfo(NativeFunction.Conv_Ovf_I1,WeightInfo.Native(1)) },
                    { typeof(Byte), new CastInfo(NativeFunction.Conv_Ovf_U1,WeightInfo.Native(2)) },
                    { typeof(UInt16), new CastInfo(NativeFunction.Conv_Ovf_U2,WeightInfo.Native(3)) },
                    { typeof(UInt32), new CastInfo(NativeFunction.Conv_Ovf_U4,WeightInfo.Native(4)) },
                    { typeof(UInt64), new CastInfo(NativeFunction.Conv_Ovf_U8,WeightInfo.Native(5)) }
                } },


                {typeof(UInt16),new Dictionary<Type, CastInfo>{
                    { typeof(SByte), new CastInfo(NativeFunction.Conv_Ovf_I1_Un,WeightInfo.Native(1)) },
                    { typeof(Byte), new CastInfo(NativeFunction.Conv_Ovf_U1_Un,WeightInfo.Native(2)) },
                    { typeof(Int16), new CastInfo(NativeFunction.Conv_Ovf_I2_Un,WeightInfo.Native(3)) }
                } },


               {typeof(Int32),new Dictionary<Type, CastInfo>{
                    { typeof(SByte), new CastInfo(NativeFunction.Conv_Ovf_I1,WeightInfo.Native(1)) },
                    { typeof(Byte), new CastInfo(NativeFunction.Conv_Ovf_U1,WeightInfo.Native(2)) },
                    { typeof(Int16), new CastInfo(NativeFunction.Conv_Ovf_I2,WeightInfo.Native(3)) },
                    { typeof(UInt16), new CastInfo(NativeFunction.Conv_Ovf_U1,WeightInfo.Native(4)) },
                    { typeof(UInt32), new CastInfo(NativeFunction.Conv_Ovf_U4,WeightInfo.Native(5)) },
                    { typeof(UInt64), new CastInfo(NativeFunction.Conv_Ovf_U8,WeightInfo.Native(6)) }
                } },


               {typeof(UInt32),new Dictionary<Type, CastInfo>{
                    { typeof(SByte), new CastInfo(NativeFunction.Conv_Ovf_I1_Un,WeightInfo.Native(1)) },
                    { typeof(Byte), new CastInfo(NativeFunction.Conv_Ovf_U1_Un,WeightInfo.Native(2)) },
                    { typeof(Int16), new CastInfo(NativeFunction.Conv_Ovf_I2_Un,WeightInfo.Native(3)) },
                    { typeof(UInt16), new CastInfo(NativeFunction.Conv_Ovf_U2_Un,WeightInfo.Native(4)) },
                    { typeof(Int32), new CastInfo(NativeFunction.Conv_Ovf_I4_Un,WeightInfo.Native(5)) }
                } },


               {typeof(Int64),new Dictionary<Type, CastInfo>{
                    { typeof(SByte), new CastInfo(NativeFunction.Conv_Ovf_I1,WeightInfo.Native(1)) },
                    { typeof(Byte), new CastInfo(NativeFunction.Conv_Ovf_U1,WeightInfo.Native(2)) },
                    { typeof(Int16), new CastInfo(NativeFunction.Conv_Ovf_I2,WeightInfo.Native(3)) },
                    { typeof(UInt16), new CastInfo(NativeFunction.Conv_Ovf_U2,WeightInfo.Native(4)) },
                    { typeof(Int32), new CastInfo(NativeFunction.Conv_Ovf_I4,WeightInfo.Native(5)) },
                    { typeof(UInt32), new CastInfo(NativeFunction.Conv_Ovf_U4,WeightInfo.Native(6)) },
                    { typeof(UInt64), new CastInfo(NativeFunction.Conv_Ovf_U8,WeightInfo.Native(7)) }
                } },


               {typeof(UInt64),new Dictionary<Type, CastInfo>{
                    { typeof(SByte), new CastInfo(NativeFunction.Conv_Ovf_I1_Un,WeightInfo.Native(1)) },
                    { typeof(Byte), new CastInfo(NativeFunction.Conv_Ovf_U1_Un,WeightInfo.Native(2)) },
                    { typeof(Int16), new CastInfo(NativeFunction.Conv_Ovf_I2_Un,WeightInfo.Native(3)) },
                    { typeof(UInt16), new CastInfo(NativeFunction.Conv_Ovf_U2_Un,WeightInfo.Native(4)) },
                    { typeof(Int32), new CastInfo(NativeFunction.Conv_Ovf_I4_Un,WeightInfo.Native(5)) },
                    { typeof(UInt32), new CastInfo(NativeFunction.Conv_Ovf_U4_Un,WeightInfo.Native(6)) },
                    { typeof(Int64), new CastInfo(NativeFunction.Conv_Ovf_I8_Un,WeightInfo.Native(7)) }
                } },


               {typeof(Double),new Dictionary<Type, CastInfo>{
                    { typeof(Single), new CastInfo(NativeFunction.Conv_R4,WeightInfo.Native(1)) }
                } }
            };
        }

    }
}
