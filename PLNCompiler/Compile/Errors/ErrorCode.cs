using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.Errors
{
    public enum ErrorType { Lexical = 0, Syntax = 1, Semantric = 2 }
    public struct ErrorCode
    {
        public readonly ErrorType Type;
        public readonly int Code;

        public ErrorCode(ErrorType type, int code)
        {
            Type = type;
            Code = code;
        }

        public override string ToString()
        {
            return string.Format("P{0}{1}", ((int)Type), Code);
        }

        public static string ErrorTypeToString(ErrorType type)
        {
            switch (type)
            {
                case ErrorType.Lexical: return "Лексическая";
                case ErrorType.Syntax: return "Синтаксическая";
                case ErrorType.Semantric: return "Семантическая";
                default: throw new PresentVariantNotImplementedException(typeof(ErrorType));
            }
        }
    }
}
