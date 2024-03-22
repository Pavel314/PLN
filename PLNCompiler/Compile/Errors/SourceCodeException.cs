using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using PLNCompiler.Syntax;

namespace PLNCompiler.Compile.Errors
{
    [Serializable]
    public abstract class SourceCodeException:PLNException
    {
        public ErrorCode ErrorCode { get; private set; }
        public Location Location { get; private set; }
        public SourceCodeException(ErrorCode errorCode, Location location)  : this(errorCode, location,null) { }
        public SourceCodeException(ErrorCode errorCode, Location location, string message) : this(errorCode,location,message,null) { }
        public SourceCodeException(ErrorCode errorCode,Location location, string message, Exception inner) : base(message, inner)
        {
            ErrorCode = errorCode;
            Location = location;
        }
        //TODO Исклюючения и сериализация
        protected SourceCodeException(SerializationInfo info,StreamingContext context) : base(info, context) { }
    }
}
