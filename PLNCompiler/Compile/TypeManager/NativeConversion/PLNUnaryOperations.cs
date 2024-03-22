using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.TypeManager.NativeConversion
{
  public static class PLNUnaryOperations
    {
        public static IReadOnlyDictionary<UnaryOperation, IReadOnlyDictionary<Type, ConversionInfo>> Operations;

        static PLNUnaryOperations()
        {
            Operations = new Dictionary<UnaryOperation, IReadOnlyDictionary<Type, ConversionInfo>>
            {
                {UnaryOperation.Plus, new Dictionary<Type,ConversionInfo>{
                    { typeof(Int32), new ConversionInfo(typeof(Int32), null) },
                    { typeof(UInt32), new ConversionInfo(typeof(UInt32), null) },
                    { typeof(Int64), new ConversionInfo(typeof(Int64), null) },
                    { typeof(UInt64), new ConversionInfo(typeof(UInt64), null) },
                    { typeof(Single), new ConversionInfo(typeof(Single), null) },
                    { typeof(Double), new ConversionInfo(typeof(Double), null) },
                } },
                {UnaryOperation.Minus, new Dictionary<Type,ConversionInfo>{
                    { typeof(Int32), new ConversionInfo(typeof(Int32), NativeFunction.Neq) },
                    { typeof(Int64), new ConversionInfo(typeof(Int64), NativeFunction.Neq) },
                    { typeof(Single), new ConversionInfo(typeof(Single), NativeFunction.Neq) },
                    { typeof(Double), new ConversionInfo(typeof(Double), NativeFunction.Neq) },
                } },
                {UnaryOperation.Inverse, new Dictionary<Type,ConversionInfo>{
                    { typeof(Int32), new ConversionInfo(typeof(Int32), NativeFunction.Not) },
                    { typeof(UInt32), new ConversionInfo(typeof(UInt32), NativeFunction.Not) },
                    { typeof(Int64), new ConversionInfo(typeof(Int64), NativeFunction.Not) },
                    { typeof(UInt64), new ConversionInfo(typeof(UInt32), NativeFunction.Not) },
                    { typeof(Boolean),new ConversionInfo(typeof(Boolean),NativeFunction.Bool_Not) }
                } }
            };
        }
    }
}
