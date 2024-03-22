using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile
{
 
    public class FindResult<TInitiator, TCandidate>: Result<TInitiator>
    {
        public IReadOnlyCollection<TCandidate> Candidates { get; private set; }
        private readonly TCandidate First;

        protected FindResult(TInitiator initiator, IReadOnlyCollection<TCandidate> candidates):base(initiator)
        {
            if (candidates.IsNullOrEmpty()) candidates = null; else First = candidates.First();
            Candidates = candidates;
        }
        public TCandidate FoundResult
        {
            get
            {
                if (!IsSuccessful)
                    throw new InvalidOperationException("IsSuccessfully != true");
                return First;
            }
        }

        public string CandidatesToString()
        {
            string stringCandidates = string.Empty;
            if (!Candidates.IsNullOrEmpty())
            {
                foreach (var candidate in Candidates)
                {
                    stringCandidates += candidate.ToString() + ", ";
                }
                stringCandidates = stringCandidates.Remove(stringCandidates.Length - 2, 2);
            }
            return "{" + stringCandidates + "}";
        }

        protected virtual string GetAdditionalString()
        {
            return null;
        }

        public override string ToString()
        {
            return string.Format("Initiator={0} IsSuccessfully={1} {2}\n{3}", Initiator,IsSuccessful, GetAdditionalString(), CandidatesToString());
        }
    }
}
