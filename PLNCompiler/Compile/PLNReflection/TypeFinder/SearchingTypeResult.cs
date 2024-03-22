using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.PLNReflection.TypeFinder
{
    public class SearchingTypeResult :SearchingMemberResult<TypeName,TypeAssociation,TypeName>
    {
        public SearchingTypeResult(TypeName initiator, TypeName member,IReadOnlyCollection<TypeAssociation> candidates) : base(initiator, member, candidates)
        {
        }

        public SearchingTypeResult ExclusiveWithNetComparer()
        {
            if (Result != SearchingResults.Suspense) return this;// || WithMember

            var result = new List<TypeAssociation>();
            ExclusiveWithNetComparer(Candidates, Initiator, f => f.Name, (candidate, index, isClear) => { if (isClear) result.Clear(); else result.Add(candidate); });
            return new SearchingTypeResult(Initiator, Member, result);
        }     
    }
}
