using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PLNCompiler.Compile.PLNReflection
{
    public enum AccessLevel { Public, NonPubluc, Ignore }
    public enum UseLevel { Ignore, Instance, Static }

    public struct MemberFlags
    {
        public readonly AccessLevel AccessLevel;
        public readonly UseLevel UseLevel;
        public MemberFlags(UseLevel useLevel, AccessLevel accessLevel)
        {
            UseLevel = useLevel;
            AccessLevel = accessLevel;
        }
        public MemberFlags(UseLevel useLevel) : this(useLevel, AccessLevel.Public)
        {
        }

        public bool CompareFromBool(bool isPublic, bool isStatic)
        {
            var res = true;
            if (AccessLevel != AccessLevel.Ignore)
                res = isPublic == IsPublic();
            if (UseLevel != UseLevel.Ignore)
                res &= isStatic == IsStatic();
            return res;
        }

        public override string ToString()
        {
            return string.Format("UseLevel={0},AccessLevel={1}", UseLevel, AccessLevel);
        }

        public bool IsPublic()
        {
            switch (AccessLevel)
            {
                case AccessLevel.Public: return true;
                case AccessLevel.NonPubluc: return false;
                default: throw new InvalidOperationException("Accesslevel is ignored");
            }
        }
        public bool IsStatic()
        {
            switch (UseLevel)
            {
                case UseLevel.Static: return true;
                case UseLevel.Instance: return false;
                default: throw new InvalidOperationException("UseLevel is ignored");
            }
        }



        public static BindingFlags GetBindings(MemberFlags memberFlags)
        {
            return UseLevelFlag(memberFlags.UseLevel) | AccessLevelFlag(memberFlags.AccessLevel);
        }

        public static BindingFlags AccessLevelFlag(AccessLevel accessLevel)
        {
            switch (accessLevel)
            {
                case AccessLevel.Public: return BindingFlags.Public;
                case AccessLevel.NonPubluc: return BindingFlags.NonPublic;
                case AccessLevel.Ignore: return BindingFlags.Public | BindingFlags.NonPublic;
                default: throw new PresentVariantNotImplementedException(typeof(AccessLevel));
            }
        }
        public static BindingFlags UseLevelFlag(UseLevel useLevel)
        {
            switch (useLevel)
            {
                case UseLevel.Instance: return BindingFlags.Instance;
                case UseLevel.Static: return BindingFlags.Static;
                case UseLevel.Ignore: return BindingFlags.Instance | BindingFlags.Static;
                default: throw new PresentVariantNotImplementedException(typeof(UseLevel));
            }
        }
        public static readonly MemberFlags Static = new MemberFlags(UseLevel.Static,AccessLevel.Public);
    }
}
