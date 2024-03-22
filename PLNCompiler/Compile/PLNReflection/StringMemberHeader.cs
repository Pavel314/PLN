using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PLNCompiler.Compile.PLNReflection
{
  public struct StringMemberHeader:IMemberHeader
    {
        public StringMemberHeader(string name, MemberFlags flags, bool useNestedTypes=true)
        {
            Name = name ?? throw new ArgumentNullException("name");
            UseDefaultName = false;
            Flags = flags;
            IncludingMembers =MemberTypes.Field | MemberTypes.Property | MemberTypes.Event;
            if (useNestedTypes)
                IncludingMembers |= MemberTypes.NestedType;

        }

        public BindingFlags GetBindingsFlags()
        {
            return MemberFlags.GetBindings(Flags);
        }

        

        public override string ToString()
        {
            return String.Format("StringMemberHeader:[Name={0}, Flags={1}]", Name, Flags);
        }

        public string Name { get; private set; }
        public bool UseDefaultName { get; private set; }
        public MemberFlags Flags { get; private set; }
        public MemberTypes IncludingMembers { get; private set; }

        MemberType IMemberHeader.MemberType { get { throw new NotImplementedException(); } }
    }
}
