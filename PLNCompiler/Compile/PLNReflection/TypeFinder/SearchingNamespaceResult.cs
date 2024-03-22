using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.PLNReflection.TypeFinder
{
  public enum SearechingNamespaceResults {OK,NotFound,WithMember }

    public sealed class SearechingNamespaceResult : SearchingMemberResult<NameManagerReadOnly,NameManagerReadOnly,NameManagerReadOnly>
    {
        public SearechingNamespaceResult(NameManagerReadOnly initiator, NameManagerReadOnly member, IReadOnlyCollection<NameManagerReadOnly> candidates) : base(initiator,member, candidates)
        {
        }

        public int ExclusiveWithNetComparer()
        {
#if DEBUG
            if (Result != SearchingResults.Suspense || WithMember) throw new InvalidOperationException("Inncorect current state");
#endif
            var result = new UniqueCollection<int>();
            ExclusiveWithNetComparer(Candidates,Initiator, f => f, (candiadte,index, isClear) => { if (isClear) result.Clear(); else result.Add(index); });
            if (result.Count == 1) return result.First();

            return -1;
            //Int32 matchesMax = Int32.MinValue;
            //Int32 index = 0;
            //foreach (var candidate in Candidates)
            //{
            //    if (candidate.Count > Initiator.Count) continue;
            //    Int32 score = candidate.Count;
            //    Int32 mathes = 0;
            //    for (int i = 0; i < candidate.Count; i++)
            //    {
            //        if (StringHelper.NetComparer.Equals(candidate[i], Initiator[i]))
            //            mathes += score;
            //        score--;
            //    }
            //    if (mathes >= matchesMax)
            //    {
            //        if (mathes > matchesMax)
            //        {
            //            matchesMax = mathes;
            //            result.Clear();
            //        }
            //        result.Add(index);
            //    }
            //    index++;
            //}


        }
    }
}


//var result = new UniqueCollection<int>();
//int i = 0;
//            foreach (var candidate in Candidates)
//            {
//                if (candidate.Equals(Initiator, StringHelper.NetComparer))
//                    result.Add(i);
//                i++;
//            }
//            if (result.Count == 1) return result.First();