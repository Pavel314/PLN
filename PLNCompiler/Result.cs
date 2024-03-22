using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler
{
    public class Result<TInitiator>
    {
        public TInitiator Initiator { get; private set; }
        public bool IsSuccessful { get; protected set; }

        protected Result(TInitiator initiator, bool isSuccessful)
        {
            Initiator = initiator;
            IsSuccessful = isSuccessful;
        }

        protected Result(TInitiator initiator) : this(initiator, false)
        {
        }
    }
}
