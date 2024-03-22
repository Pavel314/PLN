using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PLNCompiler.Compile.PLNReflection
{

    public struct MethodHeader:IParameterMemberHeader
    {
        public MethodHeader(string name, MemberFlags flags, IReadOnlyList<Parameter> parameters)
        {
            Name = name;
            UseDefaultName = String.IsNullOrEmpty(name);
            Flags = flags;
            MemberType = MemberType.Method;
            Parameters = parameters;
        }

        public BindingFlags GetBindingsFlags()
        {
            return MemberFlags.GetBindings(Flags);
        }

        public  bool CompareBindings(MethodInfo info)
        {
            return Flags.CompareFromBool(info.IsPublic, info.IsStatic);
        }

        public override string ToString()
        {
            return String.Format("MethodHeader:[Name={0}, Flags={1}]", Name, Flags);
        }

        public IReadOnlyList<Parameter> Parameters { get; private set; }
        public string Name { get; private set; }
        public bool UseDefaultName { get; private set; }
        public MemberFlags Flags { get; private set; }
        public MemberType MemberType { get; private set; }
    }
}
