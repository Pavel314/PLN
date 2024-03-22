//#define DEEP_CONSTANT_OPTIMIZATION

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Syntax;

namespace PLNCompiler.Syntax.Optimization
{
   public class PLNConstantInterpreter: IConstantInterpreter
    {
        public virtual ConstantInterpreterResultUnary Compute(ParsableConstant constant, UnaryOperation operation)
        {
            var initiator = new UnaryInitiator(constant, operation);
            switch (constant.Kind)
            {
                case ParsableConstantKind.Int64: return DoUnary(initiator, (Int64)constant.Value);
                case ParsableConstantKind.UInt64: return DoUnary(initiator, (UInt64)constant.Value);
                case ParsableConstantKind.Double: return DoUnary(initiator, (Double)constant.Value);
                case ParsableConstantKind.Boolean:return DoUnary(initiator, (Boolean)constant.Value);
                default:
                    return new ConstantInterpreterResultUnary(initiator, ConstantInterpreterResults.InvalidOperation, null);
            }
        }

        public virtual ConstantInterpreterResultBinary Compute(ParsableConstant left, ParsableConstant right, BinaryOperation operation)
        {
            var initiator = new BinaryInitiator(left, right, operation);

            #region left kind equivalent right kind
            if (left.Kind == ParsableConstantKind.Int64)
            {
                var l = (Int64)left.Value;
                if (right.Kind == ParsableConstantKind.Int64)
                    return DoBinary(initiator,l , (Int64)right.Value);

                if (right.Kind == ParsableConstantKind.Double)
                {
                    return DoBinary(initiator, (Double)l, (Double)right.Value);
                }

                if (right.Kind == ParsableConstantKind.UInt64)
                {
                    var r = (UInt64)right.Value;
                    if (l >= 0)
                        return DoBinary(initiator,(UInt64)l, r);
                }
            } else

            if (left.Kind == ParsableConstantKind.UInt64)
            {
                var l = (UInt64)left.Value;
                if (right.Kind == ParsableConstantKind.UInt64)
                    return DoBinary(initiator, l, (UInt64)right.Value);

                if (right.Kind == ParsableConstantKind.Double)
                    return DoBinary(initiator, (Double)l, (Double)right.Value);

                if (right.Kind == ParsableConstantKind.Int64)
                {
                    var r = (Int64)right.Value;
                    if (r >= 0)
                        return DoBinary(initiator, l, (UInt64)r);
                }
            } else

            if (left.Kind == ParsableConstantKind.Double)
            {
                var l = (Double)left.Value;
                if ( right.Kind == ParsableConstantKind.Double)
                    return DoBinary(initiator, l, (Double)right.Value);

                if (right.Kind == ParsableConstantKind.UInt64)
                    return DoBinary(initiator, l, (Double)(UInt64)right.Value);

                if (right.Kind == ParsableConstantKind.Int64)
                    return DoBinary(initiator, l, (Double)(Int64)right.Value);
            } else


            if (left.Kind == ParsableConstantKind.Boolean)
            {
                var l = (Boolean)left.Value;
                if (right.Kind== ParsableConstantKind.Boolean)
                return DoBinary(initiator,l , (Boolean)right.Value);
            } else

            if (left.Kind == ParsableConstantKind.String)
            {
                var l = (String)left.Value;
                if (right.Kind == ParsableConstantKind.String)
                    return DoBinary(initiator, l, (String)right.Value);
                if (right.Kind == ParsableConstantKind.Char)
                    return DoBinary(initiator, l,Char.ToString((Char)right.Value));
            } else

            if (left.Kind == ParsableConstantKind.Char)
            {
                var l = Char.ToString((Char)left.Value);
                if (right.Kind == ParsableConstantKind.String)
                    return DoBinary(initiator, l, (String)right.Value);
                if (right.Kind == ParsableConstantKind.Char)
                    return DoBinary(initiator, l, Char.ToString((Char)right.Value));
            }

            #endregion
            return new ConstantInterpreterResultBinary(initiator, ConstantInterpreterResults.InvalidOperation, null);
            //else
            //if (left.Kind == ParsableConstantKind.Int64 && right.Kind == ParsableConstantKind.UInt64)
            //{

            //}
            //else
            //if (left.Kind == ParsableConstantKind.UInt64 && right.Kind == ParsableConstantKind.Int64)
            //{

            //}
            //else
            ////Strings
            //if (left.Kind == ParsableConstantKind.String && right.Kind == ParsableConstantKind.Char)
            //{
            //    var l = (String)left.Value;
            //    var r = new string((Char)right.Value, 1);

            //}
            //else
            //if (left.Kind == ParsableConstantKind.Char && right.Kind == ParsableConstantKind.String)
            //{
            //    var l = new string((Char)left.Value, 1);
            //    var r = (String)right.Value;
            //}

            //5.0 I64 2 U64

            // ParsableConstantKind.Int64 (bits Int64 Int64)
            //ParsableConstantKind.UInt64  (Int64>0->Uint64)(Uint64 can not Int64)(bits UInt64 UInt64)
            //ParsableConstantKind.Double ( (Int64 Uint64)->Double)


            //ParsableConstantKind.String
            //ParsableConstantKind.Char (Char->ToString)
            //ParsableConstantKind.Boolean
            //return new ConstantInterpreterResultBinary(initiator, ConstantInterpreterResults.InvalidOperation, null);

            //  throw new InternalCompilerException();
        }

        #region Binary opeartion computer
        private ConstantInterpreterResultBinary DoBinary(BinaryInitiator initiator, UInt64 left, UInt64 right)
        {
            try
            {
                checked
                {
                    switch (initiator.Operation)
                    {
                        case BinaryOperation.Plus: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left + right));
                        case BinaryOperation.Minus: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left - right));
                        case BinaryOperation.Mul: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left * right));
                        case BinaryOperation.Div: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant((Double)left / right));
                        case BinaryOperation.Equally: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left == right));
                        case BinaryOperation.NotEqually: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left != right));
                        case BinaryOperation.Great: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left > right));
                        case BinaryOperation.Less: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left < right));
                        case BinaryOperation.GreatEqls: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left >= right));
                        case BinaryOperation.LessEqls: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left <= right));
                        case BinaryOperation.DivTrunc: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left / right));
                        case BinaryOperation.Mod: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left % right));
                        case BinaryOperation.LShift:
                            {
                                if ( right <= Int32.MaxValue)
                                    return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left << (Int32)right));
                                return new ConstantInterpreterResultBinary(initiator, ConstantInterpreterResults.InvalidOperation, null);
                            }
                        case BinaryOperation.RShift:
                            {
                                if (right <= Int32.MaxValue)
                                    return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left >> (Int32)right));
                                return new ConstantInterpreterResultBinary(initiator, ConstantInterpreterResults.InvalidOperation, null);
                            }
                        default:
                            return new ConstantInterpreterResultBinary(initiator, ConstantInterpreterResults.InvalidOperation, null);
                    }
                }
            }
            catch (OverflowException)
            {
                return new ConstantInterpreterResultBinary(initiator, ConstantInterpreterResults.OverflowError, null);
            }
        }

        private ConstantInterpreterResultBinary DoBinary(BinaryInitiator initiator, Int64 left, Int64 right)
        {
            try
            {
                checked
                {
                    switch (initiator.Operation)
                    {
                        case BinaryOperation.Plus: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left + right));
                        case BinaryOperation.Minus: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left - right));
                        case BinaryOperation.Mul: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left * right));
                        case BinaryOperation.Div: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant((Double)left / right));
                        case BinaryOperation.Equally: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left == right));
                        case BinaryOperation.NotEqually: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left != right));
                        case BinaryOperation.Great: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left > right));
                        case BinaryOperation.Less: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left < right));
                        case BinaryOperation.GreatEqls: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left >= right));
                        case BinaryOperation.LessEqls: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left <= right));
                        case BinaryOperation.And: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left & right));
                        case BinaryOperation.Or: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left | right));
                        case BinaryOperation.Xor: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left ^ right));
                        case BinaryOperation.DivTrunc: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left / right));
                        case BinaryOperation.Mod: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left % right));
                        case BinaryOperation.LShift:
                            {
                                if (right>=Int32.MinValue && right<=Int32.MaxValue)
                                    return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left << (Int32)right));
                                return new ConstantInterpreterResultBinary(initiator, ConstantInterpreterResults.InvalidOperation, null);
                            }
                        case BinaryOperation.RShift:
                            {
                                if (right >= Int32.MinValue && right <= Int32.MaxValue)
                                    return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left >> (Int32)right));
                                return new ConstantInterpreterResultBinary(initiator, ConstantInterpreterResults.InvalidOperation, null);
                            }
                        default:
                            return new ConstantInterpreterResultBinary(initiator, ConstantInterpreterResults.InvalidOperation, null);
                    }
                }
            } catch (OverflowException)
            {
                return new ConstantInterpreterResultBinary(initiator, ConstantInterpreterResults.OverflowError, null);
            }
        }

        private ConstantInterpreterResultBinary DoBinary(BinaryInitiator initiator, Double left, Double right)
        {
            switch (initiator.Operation)
            {
                case BinaryOperation.Plus: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left + right));
                case BinaryOperation.Minus: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left - right));
                case BinaryOperation.Mul: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left * right));
                case BinaryOperation.Div: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left / right));
                case BinaryOperation.Equally: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left == right));
                case BinaryOperation.NotEqually: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left != right));
                case BinaryOperation.Great: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left > right));
                case BinaryOperation.Less: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left < right));
                case BinaryOperation.GreatEqls: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left >= right));
                case BinaryOperation.LessEqls: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left <= right));
                default:
                    return new ConstantInterpreterResultBinary(initiator, ConstantInterpreterResults.InvalidOperation, null);
            }
        }

        private ConstantInterpreterResultBinary DoBinary(BinaryInitiator initiator, String left, String right)
        {
            switch (initiator.Operation)
            {
                case BinaryOperation.Plus: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left+right));
                case BinaryOperation.Equally: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left == right));
                case BinaryOperation.NotEqually: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left != right));
                default:
                    return new ConstantInterpreterResultBinary(initiator, ConstantInterpreterResults.InvalidOperation, null);
            }
        }

        private ConstantInterpreterResultBinary DoBinary(BinaryInitiator initiator, Boolean left,Boolean right)
        {
            switch (initiator.Operation)
            {
                case BinaryOperation.Equally: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left == right));
                case BinaryOperation.NotEqually: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left != right));
                case BinaryOperation.And: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left & right));
                case BinaryOperation.Or: return new ConstantInterpreterResultBinary(initiator, new ParsableConstant(left | right));
                default:
                    return new ConstantInterpreterResultBinary(initiator, ConstantInterpreterResults.InvalidOperation, null);
            }
        }
        #endregion



        #region Unary opeartion computer
        private ConstantInterpreterResultUnary  DoUnary(UnaryInitiator initiator, Int64 value)
        {
            switch (initiator.Operation)
            {
                case UnaryOperation.Inverse: return new ConstantInterpreterResultUnary(initiator, new ParsableConstant(~value));
                case UnaryOperation.Minus: return new ConstantInterpreterResultUnary(initiator, new ParsableConstant(-value));
                case UnaryOperation.Plus: return new ConstantInterpreterResultUnary(initiator, new ParsableConstant(value));
                default:
                    return new ConstantInterpreterResultUnary(initiator, ConstantInterpreterResults.InvalidOperation, null);
            }
        }

        private ConstantInterpreterResultUnary DoUnary(UnaryInitiator initiator, UInt64 value)
        {
            switch (initiator.Operation)
            {
                case UnaryOperation.Inverse: return new ConstantInterpreterResultUnary(initiator, new ParsableConstant(~value));
                case UnaryOperation.Plus: return new ConstantInterpreterResultUnary(initiator, new ParsableConstant(value));
                default:
                    return new ConstantInterpreterResultUnary(initiator, ConstantInterpreterResults.InvalidOperation, null);
            }
        }

        private ConstantInterpreterResultUnary DoUnary(UnaryInitiator initiator, Double value)
        {
            switch (initiator.Operation)
            {
                case UnaryOperation.Minus: return new ConstantInterpreterResultUnary(initiator, new ParsableConstant(-value));
                case UnaryOperation.Plus: return new ConstantInterpreterResultUnary(initiator, new ParsableConstant(value));
                default:
                    return new ConstantInterpreterResultUnary(initiator, ConstantInterpreterResults.InvalidOperation, null);
            }
        }

        private ConstantInterpreterResultUnary DoUnary(UnaryInitiator initiator, Boolean value)
        {
            switch (initiator.Operation)
            {
                case UnaryOperation.Inverse: return new ConstantInterpreterResultUnary(initiator, new ParsableConstant(!value));
                default:
                    return new ConstantInterpreterResultUnary(initiator, ConstantInterpreterResults.InvalidOperation, null);
            }
        }
        #endregion
    }
}
