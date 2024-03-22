using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace PLNCompiler.Compile.TypeManager.NativeConversion
{
  public static  class PLNImplicitCast
    {
        public static IReadOnlyDictionary<Type,IReadOnlyDictionary<Type,CastInfo>> Rules;

        static PLNImplicitCast()
        {
            Rules = new Dictionary<Type, IReadOnlyDictionary<Type, CastInfo>>
            {
                {typeof(SByte),new Dictionary<Type, CastInfo>{
                    { typeof(Int16), new CastInfo(NativeFunction.Conv_I2 ,WeightInfo.Native(1)) },
                    { typeof(Int32), new CastInfo(NativeFunction.Conv_I4,WeightInfo.Native(2)) },
                    { typeof(Int64), new CastInfo(NativeFunction.Conv_I8,WeightInfo.Native(3)) },
                    { typeof(Single), new CastInfo(NativeFunction.Conv_R4,WeightInfo.Native(4)) },
                    { typeof(Double), new CastInfo(NativeFunction.Conv_R8,WeightInfo.Native(5)) }
                } },
                

                {typeof(Byte),new Dictionary<Type, CastInfo>{
                    { typeof(Int16), new CastInfo(NativeFunction.Conv_I2,WeightInfo.Native(1)) },
                    { typeof(UInt16), new CastInfo(NativeFunction.Conv_U2,WeightInfo.Native(2)) },
                    { typeof(Int32), new CastInfo(NativeFunction.Conv_I4,WeightInfo.Native(3)) },
                    { typeof(UInt32), new CastInfo(NativeFunction.Conv_U4,WeightInfo.Native(4)) },
                    { typeof(Int64), new CastInfo(NativeFunction.Conv_I8,WeightInfo.Native(5)) },
                    { typeof(UInt64), new CastInfo(NativeFunction.Conv_U8,WeightInfo.Native(6)) },
                    { typeof(Single), new CastInfo(NativeFunction.Conv_R4,WeightInfo.Native(7)) },
                    { typeof(Double), new CastInfo(NativeFunction.Conv_R8,WeightInfo.Native(8)) }
                } },


                {typeof(Int16),new Dictionary<Type, CastInfo>{
                    { typeof(Int32), new CastInfo(NativeFunction.Conv_I4,WeightInfo.Native(1)) },
                    { typeof(Int64), new CastInfo(NativeFunction.Conv_I8,WeightInfo.Native(2)) },
                    { typeof(Single), new CastInfo(NativeFunction.Conv_R4,WeightInfo.Native(3)) },
                    { typeof(Double), new CastInfo(NativeFunction.Conv_R8,WeightInfo.Native(4)) }
                } },


                {typeof(UInt16),new Dictionary<Type, CastInfo>{
                    { typeof(Int32), new CastInfo(NativeFunction.Conv_I4,WeightInfo.Native(1)) },
                    { typeof(UInt32), new CastInfo(NativeFunction.Conv_U4,WeightInfo.Native(2)) },
                    { typeof(Int64), new CastInfo(NativeFunction.Conv_I8,WeightInfo.Native(3)) },
                    { typeof(UInt64), new CastInfo(NativeFunction.Conv_U8,WeightInfo.Native(4)) },
                    { typeof(Single), new CastInfo(NativeFunction.Conv_R4,WeightInfo.Native(5)) },
                    { typeof(Double), new CastInfo(NativeFunction.Conv_R8,WeightInfo.Native(6)) }
                } },


               {typeof(Int32),new Dictionary<Type, CastInfo>{
                    { typeof(Int64), new CastInfo(NativeFunction.Conv_I8,WeightInfo.Native(1)) },
                    { typeof(Single), new CastInfo(NativeFunction.Conv_R4,WeightInfo.Native(2)) },
                    { typeof(Double), new CastInfo(NativeFunction.Conv_R8,WeightInfo.Native(3)) }
                } },


               {typeof(UInt32),new Dictionary<Type, CastInfo>{
                    { typeof(Int64), new CastInfo(NativeFunction.Conv_I8,WeightInfo.Native(1)) },
                    { typeof(UInt64), new CastInfo(NativeFunction.Conv_U8,WeightInfo.Native(2)) },
                    { typeof(Single), new CastInfo(NativeFunction.Conv_R4,WeightInfo.Native(3)) },
                    { typeof(Double), new CastInfo(NativeFunction.Conv_R8,WeightInfo.Native(4)) }
                } },


               {typeof(Int64),new Dictionary<Type, CastInfo>{
                    { typeof(Single), new CastInfo(NativeFunction.Conv_R4,WeightInfo.Native(1)) },
                    { typeof(Double), new CastInfo(NativeFunction.Conv_R8,WeightInfo.Native(2)) }
                } },


               {typeof(UInt64),new Dictionary<Type, CastInfo>{
                    { typeof(Single), new CastInfo(NativeFunction.Conv_R4,WeightInfo.Native(1)) },
                    { typeof(Double), new CastInfo(NativeFunction.Conv_R8,WeightInfo.Native(2)) }
                } },


               {typeof(Single),new Dictionary<Type, CastInfo>{
                    { typeof(Double), new CastInfo(NativeFunction.Conv_R8,WeightInfo.Native(1)) }
                } },


                {typeof(Char),new Dictionary<Type, CastInfo>
                {
                    {typeof(String),new CastInfo(NativeFunction.Chr_Str,new WeightInfo(WeightKind.CopmpConv,1)) }
                } }
            };
        }
    }
}
