using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax
{
    public enum ConstKind { IntNumber, FloatNumber }
    public class Constant
    {
        public ConstKind Kind { get; private set; }
        public string Value { get; private set; }
        public Constant(ConstKind kind, string value)
        {
            Kind = kind;
            Value = value;
        }
    }
}
