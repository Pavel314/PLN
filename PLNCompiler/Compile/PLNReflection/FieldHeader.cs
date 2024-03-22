using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PLNCompiler.Compile.PLNReflection
{
   public struct FieldHeader:IMemberHeader
    {

        public FieldHeader(string name, MemberFlags flags)
        {
            Name = name;
            UseDefaultName = String.IsNullOrEmpty(name);
            Flags = flags;
            MemberType = MemberType.Field;
        }

        public BindingFlags GetBindingsFlags()
        {
            return MemberFlags.GetBindings(Flags);
        }

        public bool CompareBindings(FieldInfo info)
        {
            return Flags.CompareFromBool(info.IsPublic, info.IsStatic);
        }

        public override string ToString()
        {
            return String.Format("FieldHeader:[Name={0}, Flags={1}]", Name, Flags);
        }

        public string Name { get; private set; }
        public bool UseDefaultName { get; private set; }
        public MemberFlags Flags { get; private set; }
        public MemberType MemberType { get; private set; }
    }
}
