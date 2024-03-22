using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace PLNCompiler.Compile.TypeManager.NativeConversion
{
    public enum NativeTypeKind { Sign, UnSign, Float }
    public struct NativeTypeInfo
    {
        public readonly OpCode Ovf_Code;
        public readonly OpCode Ovf_Un_Code;
        public readonly OpCode Code;
        public readonly NativeTypeKind Kind;
        public readonly int Size;
        public readonly int Log;

        public NativeTypeInfo(OpCode code, OpCode ovf_Code, OpCode ovf_Un_Code, int size,int log, NativeTypeKind kind)
        {
            Code = code;
            Ovf_Code = ovf_Code;
            Ovf_Un_Code = ovf_Un_Code;
            Size = size;
            Kind = kind;
            Log = log;
#if DEBUG
            if (Math.Log(size, 2) != log) throw new ArgumentException("log");
#endif
        }
    }
}
