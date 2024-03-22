using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PLNCompiler.Compile.PLNReflection.MemberSelector
{
    public interface INetExclusiver<TReturn>
    {
       TReturn ExclusiveWithNetComparer();
    }


    public class SelectMethodResult : SelectMemberResult<MethodHeader, MethodInfo>
    {
        public SelectMethodResult(HeaderInitiator<MethodHeader> initiator, IReadOnlyCollection<MethodInfo> candidates) : base(initiator, candidates)
        {
        }

    }

    public class SelectFieldResult : SelectMemberResult<FieldHeader, FieldInfo>
    {
        public SelectFieldResult(HeaderInitiator<FieldHeader> initiator, IReadOnlyCollection<FieldInfo> candidates) : base(initiator, candidates)
        {
        }

   
    }

    public class SelectPropertyResult : SelectMemberResult<PropertyHeader, PropertyInfo>
    {
        public SelectPropertyResult(HeaderInitiator<PropertyHeader> initiator, IReadOnlyCollection<PropertyInfo> candidates) : base(initiator, candidates)
        {
        }
    }

    public class SelectConstructorResult : SelectMemberResult<ConstructorHeader, ConstructorInfo>
    {
        public SelectConstructorResult(HeaderInitiator<ConstructorHeader> initiator, IReadOnlyCollection<ConstructorInfo> candidates) : base(initiator, candidates)
        {
        }
    }

    public class SelectStringMemberResult : SelectMemberResult<IMemberHeader, MemberInfo>,INetExclusiver<SelectStringMemberResult>
    {
        public SelectStringMemberResult(HeaderInitiator<IMemberHeader> initiator, IReadOnlyCollection<MemberInfo> candidates) : base(initiator, candidates)
        {
        }

        public SelectStringMemberResult ExclusiveWithNetComparer()
        {
            if (Result != SearchingResults.Suspense) return this;
            var result = new List<MemberInfo>();
            ExclusiveWithNetComparer(Candidates, Initiator, f => result.Add(f));
            return new SelectStringMemberResult(Initiator,result);
        }
    }
}
