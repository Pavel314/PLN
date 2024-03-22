using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler
{
    [Serializable]
    public class PresentVariantNotImplementedException : InternalCompilerException
    {
        protected PresentVariantNotImplementedException() : this(ERROR_MESSAGE) { }
        protected PresentVariantNotImplementedException(string errconst) : base(string.Format("{0} in {1}", ERROR_MESSAGE, errconst)) { }
        protected PresentVariantNotImplementedException(string errconst, Exception inner) : base(string.Format("{0} in {1}", ERROR_MESSAGE, errconst), inner) { }

        public PresentVariantNotImplementedException(Type type, Exception inner) : this(type.ToString(), inner) { }
        public PresentVariantNotImplementedException(Type type) : this(type.ToString()) { }

        public const string ERROR_MESSAGE = "Present Variant Not Implemented";

    }
}
