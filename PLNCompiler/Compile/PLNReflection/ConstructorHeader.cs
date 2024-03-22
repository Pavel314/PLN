using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PLNCompiler.Compile.PLNReflection
{
    public struct ConstructorHeader : IParameterMemberHeader
    {
        public readonly static string CTOR_NAME = ConstructorInfo.ConstructorName;
        public readonly static string TYPE_CTOR_NAME = ConstructorInfo.TypeConstructorName;

        public ConstructorHeader(MemberFlags flags, IReadOnlyList<Parameter> parameters)
        {
            if (flags.UseLevel != UseLevel.Static)
                Name = CTOR_NAME;
            else
                Name = TYPE_CTOR_NAME;

            UseNameComparer = flags.UseLevel != UseLevel.Ignore;

            UseDefaultName = false;
            Flags = flags;
            MemberType = MemberType.Constructor;
            Parameters = parameters;
        }

        public BindingFlags GetBindingsFlags()
        {
            return MemberFlags.GetBindings(Flags);
        }

        public bool CompareBindings(MethodInfo info)
        {
            return Flags.CompareFromBool(info.IsPublic, info.IsStatic);
        }

        public override string ToString()
        {
            return String.Format("ConstructorHeader:[Flags={0}]", Flags);
        }

        public bool UseNameComparer { get; private set; }
        public IReadOnlyList<Parameter> Parameters { get; private set; }
        public string Name { get; private set; }
        public bool UseDefaultName { get; private set; }
        public MemberFlags Flags { get; private set; }
        public MemberType MemberType { get; private set; }
    }
}
