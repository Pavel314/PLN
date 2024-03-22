using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace PLNCompiler.Compile.PLNReflection
{
    public class GenericVerify
    {

        public virtual GenericVerifyResult Verify(Type genericType, IReadOnlyList<Type> arguments)
        {
#if DEBUG
            foreach (var arg in arguments)
                if (arg.IsGenericTypeDefinition) throw new ArgumentException("argument types should not be not generic", "arguments");
#endif
          
            var args = genericType.GetGenericArguments();
            if (args.Length == 0) throw new ArgumentException("this type not has a genericArguments", "genericType");
            if (arguments.Count != args.Length) throw new ArgumentException("TypeArguments.Count!=Arguments.Length");

            var result = new List<GenericArgumentError>();

            for (int i = 0; i < args.Length; i++)
            {
                var res = VerifyArgument(args[i], i, arguments[i]);
                if (!res.IsNull()) result.Add(res);
            }
            if (result.Count == 0) return new GenericVerifyResult(genericType, null);
            return new GenericVerifyResult(genericType, result);
        }

        protected virtual GenericArgumentError VerifyArgument(Type targetType, int position, Type initiatorType)
        {
            GenericParameterAttributes NotVerifyFlags = GenericParameterAttributes.None;
            var flags = targetType.GenericParameterAttributes;
            if (flags != GenericParameterAttributes.None)
            {
                if (flags.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint) && !initiatorType.IsValueType)
                {
                    NotVerifyFlags |= GenericParameterAttributes.NotNullableValueTypeConstraint;
                }

                if (flags.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint) && initiatorType.IsValueType)
                {
                    NotVerifyFlags |= GenericParameterAttributes.ReferenceTypeConstraint;
                }

                if (flags.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint) && !HasDefaultConstructor(initiatorType))
                {
                    NotVerifyFlags |= GenericParameterAttributes.DefaultConstructorConstraint;
                }
            }
            var NotVerifyTypes = new List<Type>();
            foreach (var type in targetType.GetGenericParameterConstraints())
            {
                if (!type.IsAssignableFrom(initiatorType))
                    NotVerifyTypes.Add(type);
            }
            if (NotVerifyTypes.Count == 0 && NotVerifyFlags == GenericParameterAttributes.None) return null;
            return new GenericArgumentError(initiatorType,targetType, NotVerifyTypes, position, NotVerifyFlags);
            //if (NotVerifyFlags != GenericParameterAttributes.None) return new GenericArgumentError(initiatorType, NotVerifyTypes,position, NotVerifyFlags);
            //if (NotVerifyTypes.IsNull()) return null;
            //return new GenericArgumentError(initiatorType, NotVerifyTypes, position, NotVerifyFlags);
        }

        protected virtual bool HasDefaultConstructor(Type type)
        {
            return type.IsValueType || !type.GetConstructor(Type.EmptyTypes).IsNull();
        }
    }
}
