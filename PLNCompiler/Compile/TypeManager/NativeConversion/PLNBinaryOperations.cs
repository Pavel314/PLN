using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace PLNCompiler.Compile.TypeManager.NativeConversion
{
    public static class PLNBinaryOperations
    {

        public static IReadOnlyDictionary<BinaryOperation, IReadOnlyDictionary<Type, ConversionInfo>> Operations;

        static PLNBinaryOperations()
        {
           Operations = new Dictionary<BinaryOperation, IReadOnlyDictionary<Type, ConversionInfo>>
            {
                {BinaryOperation.Plus,new Dictionary<Type, ConversionInfo>{
                    { typeof(Int32), new ConversionInfo(typeof(Int32), NativeFunction.Add_Ovf) },
                    { typeof(UInt32), new ConversionInfo(typeof(UInt32), NativeFunction.Add_Ovf_Un) },
                    { typeof(Int64), new ConversionInfo(typeof(Int64), NativeFunction.Add_Ovf) },
                    { typeof(UInt64), new ConversionInfo(typeof(UInt64), NativeFunction.Add_Ovf_Un) },
                    { typeof(Single), new ConversionInfo(typeof(Single), NativeFunction.Add) },
                    { typeof(Double), new ConversionInfo(typeof(Double), NativeFunction.Add) },
                    {typeof(String),new ConversionInfo(typeof(String), NativeFunction.Str_Conc) },
                    {typeof(Char),new ConversionInfo(typeof(String),NativeFunction.Str_Conc,NativeFunction.Chr_Str) }
                } },
                {BinaryOperation.Minus,new Dictionary<Type, ConversionInfo>{
                    { typeof(Int32), new ConversionInfo(typeof(Int32), NativeFunction.Sub_Ovf) },
                    { typeof(UInt32), new ConversionInfo(typeof(UInt32), NativeFunction.Sub_Ovf_Un) },
                    { typeof(Int64), new ConversionInfo(typeof(Int64), NativeFunction.Sub_Ovf) },
                    { typeof(UInt64), new ConversionInfo(typeof(UInt64), NativeFunction.Sub_Ovf_Un) },
                    { typeof(Single), new ConversionInfo(typeof(Single), NativeFunction.Sub) },
                    { typeof(Double), new ConversionInfo(typeof(Double), NativeFunction.Sub) }
                } },
                {BinaryOperation.Mul,new Dictionary<Type, ConversionInfo>{
                    { typeof(Int32), new ConversionInfo(typeof(Int32), NativeFunction.Mul_Ovf) },
                    { typeof(UInt32), new ConversionInfo(typeof(UInt32), NativeFunction.Mul_Ovf_Un) },
                    { typeof(Int64), new ConversionInfo(typeof(Int64), NativeFunction.Mul_Ovf) },
                    { typeof(UInt64), new ConversionInfo(typeof(UInt64), NativeFunction.Mul_Ovf_Un) },
                    { typeof(Single), new ConversionInfo(typeof(Single), NativeFunction.Mul) },
                    { typeof(Double), new ConversionInfo(typeof(Double), NativeFunction.Mul) }
                } },
                {BinaryOperation.Div,new Dictionary<Type, ConversionInfo>{
                    { typeof(Int32), new ConversionInfo(typeof(Double), NativeFunction.Div, NativeFunction.Conv_R8) },
                    { typeof(UInt32), new ConversionInfo(typeof(Double), NativeFunction.Div,NativeFunction.Conv_R8) },
                    { typeof(Int64), new ConversionInfo(typeof(Double), NativeFunction.Div, NativeFunction.Conv_R8) },
                    { typeof(UInt64), new ConversionInfo(typeof(Double), NativeFunction.Div, NativeFunction.Conv_R8) },
                    { typeof(Single), new ConversionInfo(typeof(Single), NativeFunction.Div) },
                    { typeof(Double), new ConversionInfo(typeof(Double), NativeFunction.Div) }
                } },
                {BinaryOperation.Equally,new Dictionary<Type, ConversionInfo>{
                    { typeof(Int32), new ConversionInfo(typeof(Boolean), NativeFunction.Ceq) },
                    { typeof(UInt32), new ConversionInfo(typeof(Boolean), NativeFunction.Ceq) },
                    { typeof(Int64), new ConversionInfo(typeof(Boolean), NativeFunction.Ceq) },
                    { typeof(UInt64), new ConversionInfo(typeof(Boolean), NativeFunction.Ceq) },
                    { typeof(Single), new ConversionInfo(typeof(Boolean), NativeFunction.Ceq) },
                    { typeof(Double), new ConversionInfo(typeof(Boolean), NativeFunction.Ceq) },
                    { typeof(Boolean), new ConversionInfo(typeof(Boolean), NativeFunction.Ceq) },
                    { typeof(Char), new ConversionInfo(typeof(Boolean), NativeFunction.Ceq) },
                    { typeof(String), new ConversionInfo(typeof(Boolean), NativeFunction.Str_Eql) }
                } },
                {BinaryOperation.NotEqually,new Dictionary<Type, ConversionInfo>{
                    { typeof(Int32), new ConversionInfo(typeof(Boolean), NativeFunction.Cneq) },
                    { typeof(UInt32), new ConversionInfo(typeof(Boolean), NativeFunction.Cneq) },
                    { typeof(Int64), new ConversionInfo(typeof(Boolean), NativeFunction.Cneq) },
                    { typeof(UInt64), new ConversionInfo(typeof(Boolean), NativeFunction.Cneq) },
                    { typeof(Single), new ConversionInfo(typeof(Boolean), NativeFunction.Cneq) },
                    { typeof(Double), new ConversionInfo(typeof(Boolean), NativeFunction.Cneq) },
                    { typeof(Boolean), new ConversionInfo(typeof(Boolean), NativeFunction.Cneq) },
                    { typeof(Char), new ConversionInfo(typeof(Boolean), NativeFunction.Cneq) },
                    {typeof(String),new ConversionInfo(typeof(Boolean),NativeFunction.Str_InEql) }
                } },
                //Greate Less GreateEql LessEql
                 {BinaryOperation.Great,new Dictionary<Type, ConversionInfo>{
                    { typeof(Int32), new ConversionInfo(typeof(Boolean), NativeFunction.Cgt) },
                    { typeof(UInt32), new ConversionInfo(typeof(Boolean), NativeFunction.Cgt_Un) },
                    { typeof(Int64), new ConversionInfo(typeof(Boolean), NativeFunction.Cgt) },
                    { typeof(UInt64), new ConversionInfo(typeof(Boolean), NativeFunction.Cgt_Un) },
                    { typeof(Single), new ConversionInfo(typeof(Boolean), NativeFunction.Cgt) },
                    { typeof(Double), new ConversionInfo(typeof(Boolean), NativeFunction.Cgt) },
                } },
                {BinaryOperation.Less,new Dictionary<Type, ConversionInfo>{
                    { typeof(Int32), new ConversionInfo(typeof(Boolean), NativeFunction.Clt) },
                    { typeof(UInt32), new ConversionInfo(typeof(Boolean), NativeFunction.Clt_Un) },
                    { typeof(Int64), new ConversionInfo(typeof(Boolean), NativeFunction.Clt) },
                    { typeof(UInt64), new ConversionInfo(typeof(Boolean), NativeFunction.Clt_Un) },
                    { typeof(Single), new ConversionInfo(typeof(Boolean), NativeFunction.Clt) },
                    { typeof(Double), new ConversionInfo(typeof(Boolean), NativeFunction.Clt) },
                } },
                {BinaryOperation.GreatEqls,new Dictionary<Type, ConversionInfo>{
                    { typeof(Int32), new ConversionInfo(typeof(Boolean), NativeFunction.Cget) },
                    { typeof(UInt32), new ConversionInfo(typeof(Boolean), NativeFunction.Cget_Un) },
                    { typeof(Int64), new ConversionInfo(typeof(Boolean), NativeFunction.Cget) },
                    { typeof(UInt64), new ConversionInfo(typeof(Boolean), NativeFunction.Cget_Un) },
                    { typeof(Single), new ConversionInfo(typeof(Boolean), NativeFunction.Cget) },
                    { typeof(Double), new ConversionInfo(typeof(Boolean), NativeFunction.Cget) },
                } },
                {BinaryOperation.LessEqls,new Dictionary<Type, ConversionInfo>{
                    { typeof(Int32), new ConversionInfo(typeof(Boolean), NativeFunction.Clet) },
                    { typeof(UInt32), new ConversionInfo(typeof(Boolean), NativeFunction.Clet_Un) },
                    { typeof(Int64), new ConversionInfo(typeof(Boolean), NativeFunction.Clet) },
                    { typeof(UInt64), new ConversionInfo(typeof(Boolean), NativeFunction.Clet_Un) },
                    { typeof(Single), new ConversionInfo(typeof(Boolean), NativeFunction.Clet) },
                    { typeof(Double), new ConversionInfo(typeof(Boolean), NativeFunction.Clet) },
                } },
                //And,Or,Xor
                  {BinaryOperation.And,new Dictionary<Type, ConversionInfo>{
                    { typeof(Int32), new ConversionInfo(typeof(Int32), NativeFunction.And) },
                    { typeof(UInt32), new ConversionInfo(typeof(UInt32), NativeFunction.And) },
                    { typeof(Int64), new ConversionInfo(typeof(Int64), NativeFunction.And) },
                    { typeof(UInt64), new ConversionInfo(typeof(UInt64), NativeFunction.And) },
                    { typeof(Boolean), new ConversionInfo(typeof(Boolean), NativeFunction.And) },
                  } },
                  {BinaryOperation.Or,new Dictionary<Type, ConversionInfo>{
                    { typeof(Int32), new ConversionInfo(typeof(Int32), NativeFunction.Or) },
                    { typeof(UInt32), new ConversionInfo(typeof(UInt32), NativeFunction.Or) },
                    { typeof(Int64), new ConversionInfo(typeof(Int64), NativeFunction.Or) },
                    { typeof(UInt64), new ConversionInfo(typeof(UInt64), NativeFunction.Or) },
                    { typeof(Boolean), new ConversionInfo(typeof(Boolean), NativeFunction.Or) },
                  } },
                 {BinaryOperation.Xor,new Dictionary<Type, ConversionInfo>{
                    { typeof(Int32), new ConversionInfo(typeof(Int32), NativeFunction.Xor) },
                    { typeof(UInt32), new ConversionInfo(typeof(UInt32), NativeFunction.Xor) },
                    { typeof(Int64), new ConversionInfo(typeof(Int64), NativeFunction.Xor) },
                    { typeof(UInt64), new ConversionInfo(typeof(UInt64), NativeFunction.Xor) },
                    { typeof(Boolean), new ConversionInfo(typeof(Boolean), NativeFunction.Xor) },
                  } },
                    {BinaryOperation.LShift,new Dictionary<Type, ConversionInfo>{
                    { typeof(Int32), new ConversionInfo(typeof(Int32), NativeFunction.shl) },
                    { typeof(Int64), new ConversionInfo(typeof(Int64), NativeFunction.shl) },
                  } },
                    {BinaryOperation.RShift,new Dictionary<Type, ConversionInfo>{
                    { typeof(Int32), new ConversionInfo(typeof(Int32), NativeFunction.shr) },
                    { typeof(Int64), new ConversionInfo(typeof(Int64), NativeFunction.shr) },
                  } },
                    //Div Mod
               {BinaryOperation.DivTrunc,new Dictionary<Type, ConversionInfo>{
                    { typeof(Int32), new ConversionInfo(typeof(Int32), NativeFunction.Div) },
                    { typeof(UInt32), new ConversionInfo(typeof(UInt32), NativeFunction.Div) },
                    { typeof(Int64), new ConversionInfo(typeof(Int64), NativeFunction.Div) },
                    { typeof(UInt64), new ConversionInfo(typeof(UInt64), NativeFunction.Div) },
                  } },
                 {BinaryOperation.Mod,new Dictionary<Type, ConversionInfo>{
                    { typeof(Int32), new ConversionInfo(typeof(Int32), NativeFunction.Rem) },
                    { typeof(UInt32), new ConversionInfo(typeof(UInt32), NativeFunction.Rem_Un) },
                    { typeof(Int64), new ConversionInfo(typeof(Int64), NativeFunction.Rem) },
                    { typeof(UInt64), new ConversionInfo(typeof(UInt64), NativeFunction.Rem_Un) },
                  } },
            };
            //TODO DivMathRound, DivBankRound;
        }
    }
}
