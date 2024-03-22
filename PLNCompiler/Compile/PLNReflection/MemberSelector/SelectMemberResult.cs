using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PLNCompiler.Compile.PLNReflection.MemberSelector
{
    public struct HeaderInitiator<THeader> where THeader:IMemberHeader
    {
        public readonly Type Type;
        public readonly THeader Header;

        public HeaderInitiator(Type type, THeader header)
        {
            Type = type;
            Header = header;
        }

    }


   public abstract class SelectMemberResult<THeader,TCandidate>: SimpleSearchingResult<HeaderInitiator<THeader>,TCandidate> where THeader:IMemberHeader where TCandidate : MemberInfo
    {
        public SelectMemberResult(HeaderInitiator<THeader> initiator, IReadOnlyCollection<TCandidate> candidates) : base(initiator, candidates)
        {
        }

        protected static void ExclusiveWithNetComparer(IEnumerable<TCandidate> candidates, HeaderInitiator<THeader> initiator, Action<TCandidate> returnFunction)
        {
            foreach (var candidate in candidates)
            {
                if (StringHelper.NetComparer.EqualsStr(candidate.Name, initiator.Header.Name))
                    returnFunction(candidate);
            }
        }

    }
}
