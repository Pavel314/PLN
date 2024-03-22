using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PLNCompiler.Compile.PLNReflection
{
    public enum MemberType { Unkown, Field, Property, Method, Event, Constructor, Nested }
    public interface IMemberHeader
    {
        string Name { get; }
        bool UseDefaultName { get; }
        MemberFlags Flags { get; }
        MemberType MemberType { get; }
    }
}
