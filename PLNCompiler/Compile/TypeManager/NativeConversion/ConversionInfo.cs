using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.TypeManager.NativeConversion
{
    public struct ConversionInfo
    {
        public readonly Type ResultType;
        public readonly NativeFunction OperatorFunction;
        public readonly NativeFunction LeftRightFunc;

        public ConversionInfo(Type resultType, NativeFunction operatorFunction, NativeFunction leftRightFunc)
        {
            ResultType = resultType;
            OperatorFunction = operatorFunction;
            LeftRightFunc = leftRightFunc;
        }
        public ConversionInfo(Type resultType, NativeFunction operatorFunction) : this(resultType, operatorFunction, null)
        {
        }
    }
}
