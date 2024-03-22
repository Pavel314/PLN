using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.PLNReflection.TypeFinder
{

    public abstract class SearchingMemberResult<TInitiator, TCandidate, TMember>:SimpleSearchingResult<TInitiator,TCandidate> where TInitiator:NameManagerReadOnly where TMember:NameManagerReadOnly
    {
        public TMember  Member { get; private set; }
        public bool WithMember { get; private set; }

        public SearchingMemberResult(TInitiator initiator, TMember member,IReadOnlyCollection<TCandidate> candidates) : base(initiator, candidates)
        {
            WithMember = !member.IsNullOrEmpty();
            if (!WithMember) member = null;
            Member = member;
            IsSuccessful = Result == SearchingResults.OK && !WithMember;
        }

       protected static void ExclusiveWithNetComparer(IEnumerable<TCandidate> candidates,NameManagerReadOnly initiator, Func<TCandidate,NameManagerReadOnly> GetNameManager,Action<TCandidate,int,bool> returnFunction) 
        {
            Int32 matchesMax = Int32.MinValue;
            Int32 index = 0;
            foreach (var candidate in candidates)
            {
                var nameManager = GetNameManager(candidate);
                 if (nameManager.Count > initiator.Count) continue;
               

             //   if (nameManager.Count < initiator.Count) continue;
               // var min = initiator.Count;
                Int32 score = nameManager.Count;
                Int32 mathes = 0;
                for (int i = 0; i < nameManager.Count; i++)
                {
                    if (StringHelper.NetComparer.Equals(nameManager[i],initiator[i]))
                        mathes += score;
                    score--;
                }
                if (mathes >= matchesMax)
                {
                    if (mathes > matchesMax)
                    {
                        matchesMax = mathes;
                        returnFunction(candidate, -1, true);
                    }
                    returnFunction(candidate, index,false);
                }
                index++;
            }

        }

        //public class SearchingMemberResult<TInitiator,TCandidate,TMember> : SimpleSearchingResult<TInitiator, TCandidate>  where TInitiator:NameManagerReadOnly where TMember : NameManagerReadOnly
        //{
        //    public TMember Member { get; private set; }
        //    public bool WithMember { get; private set; }

        //    public SearchingMemberResult(TInitiator initiator, TMember member, IReadOnlyCollection<TCandidate> candidates) : base(initiator, candidates)
        //    {
        //        WithMember = !member.IsNullOrEmpty();
        //        if (!WithMember) member = null;
        //        Member = member;
        //        IsSuccessful= Result == SearchingResults.OK && !WithMember; 
        //    }

        //  public abstract SearchingMemberResult<TInitiator, TCandidate> ExclusiveWithNetComparer();

        protected override string GetAdditionalString()
        {
            return String.Format("{0} WithMember={1}", base.GetAdditionalString(), WithMember);
        }
    }
}
