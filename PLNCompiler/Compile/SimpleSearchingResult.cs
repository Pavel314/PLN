using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile
{
    public enum SearchingResults { OK, Suspense, NotFound }

    public class SimpleSearchingResult<TInitiator, TCandidate> : FindResult<TInitiator, TCandidate>
    {
        public SearchingResults Result { get; private set; }

        public SimpleSearchingResult(TInitiator initiator, IReadOnlyCollection<TCandidate> candidates) : base(initiator, candidates)
        {
            if (Candidates != null)
                switch (candidates.Count)
                {
                    case 1: Result = SearchingResults.OK; break;
                    default: Result = SearchingResults.Suspense; break;
                }
            else
                Result = SearchingResults.NotFound;
            IsSuccessful = Result == SearchingResults.OK;
        }
        protected override string GetAdditionalString()
        {
            return string.Format("SearchingResults={0}", Result);
        }
    }
}
