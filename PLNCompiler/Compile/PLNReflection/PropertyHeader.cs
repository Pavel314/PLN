using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PLNCompiler.Compile.PLNReflection
{
    public enum PropertyAcces { Public, NonPublic, Exists, NotExists, Ignore }
    public struct PropertyHeader : IParameterMemberHeader
    {

        public PropertyHeader(string name, PropertyAcces getter, PropertyAcces setter, MemberFlags flags, IReadOnlyList<Parameter> parameters)
        {
            Name = name;
            UseDefaultName = String.IsNullOrEmpty(name);
            Flags = flags;
            MemberType = MemberType.Property;
            Parameters = parameters;

            Getter = getter;
            Setter = setter;
            IsIndexer = !parameters.IsNullOrEmpty();
        }

        public PropertyHeader(string name, PropertyAcces getter, PropertyAcces setter, MemberFlags flags) : this(name, getter, setter, flags, null)
        {
        }

        public BindingFlags GetBindingsFlags()
        {
            return MemberFlags.GetBindings(Flags);
        }

        public bool CompareBindings(MethodInfo info)
        {
            return Flags.CompareFromBool(info.IsPublic, info.IsStatic);
        }


        public IReadOnlyList<Parameter> Parameters { get; private set; }
        public string Name { get; private set; }
        public bool UseDefaultName { get; private set; }
        public MemberFlags Flags { get; private set; }
        public MemberType MemberType { get; private set; }

        public bool IsIndexer { get; private set; }
        public PropertyAcces Getter { get; private set; }
        public PropertyAcces Setter { get; private set; }


        public static bool CompareAcces(System.Reflection.MethodInfo method, PropertyAcces acces)
        {
            if (acces != PropertyAcces.Ignore)
            {
                switch (acces)
                {
                    case PropertyAcces.Exists: if (method.IsNull()) return false; break;
                    case PropertyAcces.NotExists: if (!method.IsNull()) return false; break;
                    case PropertyAcces.Public: if (method.IsNull() || !method.IsPublic) return false; break;
                    case PropertyAcces.NonPublic: if (method.IsNull() || method.IsPublic) return false; break;
                    default: throw new PresentVariantNotImplementedException(typeof(PropertyAcces));
                }
            }
            return true;
        }
    }
}
