using System;
using System.Runtime.Serialization;

namespace PLNCompiler
{
    [Serializable]
    public class PLNException : Exception
    {
        public PLNException() { }
        public PLNException(string message) : base(message) { }
        public PLNException(string message, Exception inner) : base(message, inner) { }
        protected PLNException(SerializationInfo info,StreamingContext context) : base(info, context) { }
    }
}
