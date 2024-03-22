using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace PLNCompiler.Compile.PLNReflection
{
    public class GenericArgumentError
    {
        public int Position { get; private set; }
        public Type InitiatorType { get; private set; }
        public Type GenericParameter { get; private set; }
        public IReadOnlyCollection<Type> TargetTypes { get; private set; }
        public GenericParameterAttributes NotVerifyFlags { get; private set; }

        public bool VerifyFlagsIsNone()
        {
            return NotVerifyFlags == GenericParameterAttributes.None;
        }

        public GenericArgumentError(Type initiatorType,Type genericParameter, IReadOnlyCollection<Type> targetTypes, int position, GenericParameterAttributes notVerifyFlags)
        {
            InitiatorType = initiatorType;
            if (!genericParameter.IsGenericParameter) throw new ArgumentException("This type not generic parameter", "genericParameter");
            GenericParameter = genericParameter;
            Position = position;
            if (targetTypes.IsNullOrEmpty())
                targetTypes = null;
            TargetTypes = targetTypes;
            NotVerifyFlags = notVerifyFlags;
        }

        public override string ToString()
        {
            string targetTypesString = string.Empty;
            if (!TargetTypes.IsNullOrEmpty())
            {
                foreach (var type in TargetTypes)
                    targetTypesString += type.ToString() + ", ";
                targetTypesString = "TargetTypes=[" + targetTypesString.TrimLast2Char() + "]";
            }
            string notVerifyFlagsString = string.Empty;
            if (!VerifyFlagsIsNone())
            {
                notVerifyFlagsString = "NotVerifyFlags=[" + NotVerifyFlags.ToString() + "]";
            }
            return string.Format("CurrentType={0} Position={1} {2} {3}", InitiatorType, Position, targetTypesString, notVerifyFlagsString);
        }
    }



    public class GenericVerifyResult: Result<Type>
    {
        public IReadOnlyList<GenericArgumentError> ErrorArguments { get; private set; }

        public GenericVerifyResult(Type initiator,IReadOnlyList<GenericArgumentError> errorArguments):base(initiator)
        {
            ErrorArguments = errorArguments;
            IsSuccessful = errorArguments.IsNullOrEmpty();
        }

        public override string ToString()
        {
            if (!IsSuccessful)
            {
                string s = string.Empty;
                foreach (var argumet in ErrorArguments)
                {
                    s += argumet.ToString() + Environment.NewLine;
                }
                return s.TrimLast2Char();
            }
            return "Successfully";
        }
    }
}
