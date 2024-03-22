using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.PLNReflection.MemberSelector
{
 public interface IMemberSelector
    {
        SelectMethodResult SelectMethod(Type type, MethodHeader header);
        SelectConstructorResult SelectConstructor(Type type, ConstructorHeader header);
        SelectFieldResult SelectField(Type type, FieldHeader header);
        SelectPropertyResult SelectProperty(Type type, PropertyHeader header);
        SelectStringMemberResult SelectStringMember(Type type,StringMemberHeader header);
    }
}
