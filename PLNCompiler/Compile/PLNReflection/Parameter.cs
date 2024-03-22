using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PLNCompiler.Compile.PLNReflection
{
    public struct Parameter
    {
        public readonly Type Type;
        public readonly bool IsOut;
        public readonly bool IsOptional;
        public Parameter(Type type, bool isOut, bool isOptional)
        {
            Type = type;
            IsOut = isOut;
            IsOptional = isOptional;
            if (IsOut && !Type.IsByRef)
                throw new ArgumentException("parameter should be passed by reference(if out)");
        }

        public  ParameterAttributes GetAttributes()
        {
           ParameterAttributes result = ParameterAttributes.None;
            if (IsOut)
                result |= ParameterAttributes.Out;
            if (IsOptional)
                result |= ParameterAttributes.Optional;
            return result;
        }

        public Parameter(ParameterInfo builder) : this(builder.ParameterType, builder.IsOut, builder.IsOptional)
        {
        }

        public Parameter(Type type, bool isOut) : this(type, false, false)
        {
        }

        public Parameter(Type type) : this(type, false)
        {
        }


        public override string ToString()
        {
            string result = string.Empty;
            if (IsOut)
                result += "[out] ";
            if (IsOptional)
                result += "[opt] ";
            return result += Type.ToString();
        }

        public static Parameter MakeRef(Type type, bool isOut)
        {
            return new Parameter(type.MakeByRefType(), isOut);
        }
    }
}
