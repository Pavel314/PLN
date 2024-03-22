using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace PLNCompiler.Compile.TypeManager
{
    //public sealed class NativeFunction
    //{
    //    private AddOnlyList<OpCode> OpCodes;

    //    public NativeFunction(IEnumerable<OpCode> opCodes)
    //    {
    //        if (opCodes.IsNullOrEmpty())
    //            throw new ArgumentException("NativeFunction not contains OpCodes or null");

    //        OpCodes = new AddOnlyList<OpCode>(opCodes);
    //    }

    //    public void Add(OpCode code)
    //    {
    //        OpCodes.Add(code);
    //    }

    //    public void AddRange(IEnumerable<OpCode> opCodes)
    //    {
    //        OpCodes.AddRange(opCodes);
    //    }

    //    public void Emit(ILGenerator il)
    //    {
    //        foreach (var inst in OpCodes)
    //            il.Emit(inst);
    //    }

    //    public NativeFunction(params OpCode[] OpCodes) : this((IEnumerable<OpCode>)OpCodes)
    //    {
    //    }

    //    public NativeFunction(NativeFunction function):this(function.OpCodes)
    //    {

    //    }
    //}


    public sealed class NativeFunction
    {
        private IEnumerable<OpCode> opCodes;
        private Action<ILGenerator> opCodeAction;
        private OpCode oneOpCode;

        public NativeFunction(Action<ILGenerator> opCodeAction)
        {
            if (ReferenceEquals(opCodeAction, null))
                throw new ArgumentNullException("opCodeAction");
            opCodes = null;
            this.opCodeAction = opCodeAction;
        }
        public NativeFunction(IEnumerable<OpCode> opCodes)
        {
            if (opCodes.IsNullOrEmpty())
                throw new ArgumentException("NativeFunction not contains OpCodes or null");
            opCodeAction = null;
            this.opCodes = opCodes;
        }
        public NativeFunction(OpCode oneOpCode)
        {
            this.oneOpCode = oneOpCode;
        }


        public void Emit(ILGenerator il)
        {
            if (opCodes != null)
                foreach (var inst in opCodes)
                    il.Emit(inst);
            else
                if (opCodeAction != null)
                opCodeAction.Invoke(il);
            else
                il.Emit(oneOpCode);
        }

        public static readonly NativeFunction Conv_Ovf_I1 = new NativeFunction(OpCodes.Conv_Ovf_I1);
        public static readonly NativeFunction Conv_Ovf_I1_Un = new NativeFunction(OpCodes.Conv_Ovf_I1_Un);
        public static readonly NativeFunction Conv_Ovf_I2 = new NativeFunction(OpCodes.Conv_Ovf_I2);
        public static readonly NativeFunction Conv_Ovf_I2_Un = new NativeFunction(OpCodes.Conv_Ovf_I2_Un);
        public static readonly NativeFunction Conv_Ovf_I4 = new NativeFunction(OpCodes.Conv_Ovf_I4);
        public static readonly NativeFunction Conv_Ovf_I4_Un = new NativeFunction(OpCodes.Conv_Ovf_I4_Un);
        public static readonly NativeFunction Conv_Ovf_I8 = new NativeFunction(OpCodes.Conv_Ovf_I8);
        public static readonly NativeFunction Conv_Ovf_I8_Un = new NativeFunction(OpCodes.Conv_Ovf_I8_Un);

        public static readonly NativeFunction Conv_Ovf_U1 = new NativeFunction(OpCodes.Conv_Ovf_U1);
        public static readonly NativeFunction Conv_Ovf_U1_Un = new NativeFunction(OpCodes.Conv_Ovf_U1_Un);
        public static readonly NativeFunction Conv_Ovf_U2 = new NativeFunction(OpCodes.Conv_Ovf_U2);
        public static readonly NativeFunction Conv_Ovf_U2_Un = new NativeFunction(OpCodes.Conv_Ovf_U2_Un);
        public static readonly NativeFunction Conv_Ovf_U4 = new NativeFunction(OpCodes.Conv_Ovf_U4);
        public static readonly NativeFunction Conv_Ovf_U4_Un = new NativeFunction(OpCodes.Conv_Ovf_U4_Un);
        public static readonly NativeFunction Conv_Ovf_U8 = new NativeFunction(OpCodes.Conv_Ovf_U8);
        public static readonly NativeFunction Conv_Ovf_U8_Un = new NativeFunction(OpCodes.Conv_Ovf_U8_Un);

        public static readonly NativeFunction Conv_I1 = new NativeFunction(OpCodes.Conv_I1);
        public static readonly NativeFunction Conv_I2 = new NativeFunction(OpCodes.Conv_I2);
        public static readonly NativeFunction Conv_I4 = new NativeFunction(OpCodes.Conv_I4);
        public static readonly NativeFunction Conv_I8 = new NativeFunction(OpCodes.Conv_I8);

        public static readonly NativeFunction Conv_U1 = new NativeFunction(OpCodes.Conv_U1);
        public static readonly NativeFunction Conv_U2 = new NativeFunction(OpCodes.Conv_U2);
        public static readonly NativeFunction Conv_U4 = new NativeFunction(OpCodes.Conv_U4);
        public static readonly NativeFunction Conv_U8 = new NativeFunction(OpCodes.Conv_U8);

        public static readonly NativeFunction Conv_R4 = new NativeFunction(OpCodes.Conv_R4);
        public static readonly NativeFunction Conv_R8 = new NativeFunction(OpCodes.Conv_R8);

        public static readonly NativeFunction Box = new NativeFunction(OpCodes.Box);

        public static readonly NativeFunction Add = new NativeFunction(OpCodes.Add);
        public static readonly NativeFunction Add_Ovf = new NativeFunction(OpCodes.Add_Ovf);
        public static readonly NativeFunction Add_Ovf_Un = new NativeFunction(OpCodes.Add_Ovf_Un);

        public static readonly NativeFunction Sub = new NativeFunction(OpCodes.Sub);
        public static readonly NativeFunction Sub_Ovf = new NativeFunction(OpCodes.Sub_Ovf);
        public static readonly NativeFunction Sub_Ovf_Un = new NativeFunction(OpCodes.Sub_Ovf_Un);

        public static readonly NativeFunction Div = new NativeFunction(OpCodes.Div);

        public static readonly NativeFunction Mul = new NativeFunction(OpCodes.Mul);
        public static readonly NativeFunction Mul_Ovf = new NativeFunction(OpCodes.Mul_Ovf);
        public static readonly NativeFunction Mul_Ovf_Un = new NativeFunction(OpCodes.Mul_Ovf_Un);

        public static readonly NativeFunction Ceq = new NativeFunction(OpCodes.Ceq);
        public static readonly NativeFunction Cneq = new NativeFunction(new OpCode[] { OpCodes.Ceq, OpCodes.Ldc_I4_0, OpCodes.Ceq });
        public static readonly NativeFunction Cgt = new NativeFunction(OpCodes.Cgt);
        public static readonly NativeFunction Cgt_Un = new NativeFunction(OpCodes.Cgt_Un);
        public static readonly NativeFunction Clt = new NativeFunction(OpCodes.Clt);
        public static readonly NativeFunction Clt_Un = new NativeFunction(OpCodes.Clt_Un);

        public static readonly NativeFunction Cget = new NativeFunction(new OpCode[] { OpCodes.Clt, OpCodes.Ldc_I4_0, OpCodes.Ceq });
        public static readonly NativeFunction Cget_Un = new NativeFunction(new OpCode[] { OpCodes.Clt_Un, OpCodes.Ldc_I4_0, OpCodes.Ceq });
        public static readonly NativeFunction Clet = new NativeFunction(new OpCode[] { OpCodes.Cgt, OpCodes.Ldc_I4_0, OpCodes.Ceq });
        public static readonly NativeFunction Clet_Un = new NativeFunction(new OpCode[] { OpCodes.Cgt_Un, OpCodes.Ldc_I4_0, OpCodes.Ceq });

        public static readonly NativeFunction And = new NativeFunction(OpCodes.And);
        public static readonly NativeFunction Or = new NativeFunction(OpCodes.Or);
        public static readonly NativeFunction Xor = new NativeFunction(OpCodes.Xor);

        public static readonly NativeFunction shl = new NativeFunction(OpCodes.Shl);
        public static readonly NativeFunction shr = new NativeFunction(OpCodes.Shr);

        public static readonly NativeFunction Rem = new NativeFunction(OpCodes.Rem);
        public static readonly NativeFunction Rem_Un = new NativeFunction(OpCodes.Rem_Un);

        public static readonly NativeFunction Neq = new NativeFunction(OpCodes.Neg);
        public static readonly NativeFunction Not = new NativeFunction(OpCodes.Not);
        public static readonly NativeFunction Bool_Not = new NativeFunction(new OpCode[] {OpCodes.Ldc_I4_0,OpCodes.Ceq });

        public static readonly NativeFunction Chr_Str = new NativeFunction(f=>f.EmitCall(OpCodes.Call,CodeGeneration.MethodGenerator.CharToString,null));
        public static readonly NativeFunction Str_Conc = new NativeFunction(F => F.EmitCall(OpCodes.Call, CodeGeneration.MethodGenerator.StringConcat, null));
        public static readonly NativeFunction Str_Eql = new NativeFunction(F => F.EmitCall(OpCodes.Call, CodeGeneration.MethodGenerator.StringEquality, null));
        public static readonly NativeFunction Str_InEql = new NativeFunction(F => F.EmitCall(OpCodes.Call, CodeGeneration.MethodGenerator.StringInequality, null));

        public static readonly NativeFunction Pop_PushI1 = new NativeFunction(new OpCode[] { OpCodes.Pop, OpCodes.Ldc_I4_1 });

        public static NativeFunction GenISInst(Type target)
        {
            return new NativeFunction(f => { f.Emit(OpCodes.Isinst, target); f.Emit(OpCodes.Ldnull); f.Emit(OpCodes.Cgt_Un); });
        }

        public static NativeFunction GenASInst(Type target)
        {
            return new NativeFunction(f => { f.Emit(OpCodes.Isinst, target);  });
        }
    }
}
