using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.PLNReflection.TypeFinder
{
    //public class FindNestedTypeResult : SearchingTypeResult
    //{
    //    public FindNestedTypeResult(TypeName initiator, NameManagerReadOnly member, IReadOnlyCollection<KeyValuePair<TypeName, Type>> candidates) : base(initiator, member, candidates)
    //    {
    //    }
    //}

    public class FindNestedTypeResult : SearchingMemberResult<TypeName,TypeAssociation,NameManagerReadOnly>
    {
        public TypeAssociation ParentType { get; private set; }
        public TypeName ShortInitiator { get; private set; }

        public FindNestedTypeResult(TypeAssociation parentType, TypeName shortInitiator, NameManagerReadOnly member, IReadOnlyCollection<TypeAssociation> candidates) : base(new TypeName(parentType.Name.Concat(shortInitiator),shortInitiator.Count), member, candidates)
        {
            ShortInitiator = shortInitiator;
            ParentType = parentType;
        }
        public FindNestedTypeResult ExclusiveWithNetComparer()
        {
            if (Result != SearchingResults.Suspense) return this;// || WithMember
            var result = new List<TypeAssociation>();
            ExclusiveWithNetComparer(Candidates,Initiator, f => f.Name, (candidate, index, isClear) => { if (isClear) result.Clear(); else result.Add(candidate); });
            return new FindNestedTypeResult(ParentType, Initiator, Member, result);
        }
    }
}
