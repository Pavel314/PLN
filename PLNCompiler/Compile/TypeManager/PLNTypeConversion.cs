using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using PLNCompiler.Semantic;
using PLNCompiler.Syntax;
using PLNCompiler.Compile.PLNReflection.MemberSelector;
using PLNCompiler.Compile.TypeManager.NativeConversion;

namespace PLNCompiler.Compile.TypeManager
{



    public class PLNTypeConversion : ITypeConversion,IMassCalculator
    {
        public static readonly Type OBJECT = typeof(object);


        public TypedConstant AssignConstant(ParsableConstant constant, Type target)
        {
            var targetCode = target.ToTypeCode();

            switch (constant.Kind)
            {
                case ParsableConstantKind.UInt64:
                    {
                        var value = (UInt64)constant.Value;
                        switch (targetCode)
                        {
                            case TypeCode.UInt64: return  new TypedConstant(value);
                            case TypeCode.Double: return new TypedConstant((Double)value);
                            case TypeCode.Single: return new  TypedConstant((Single)value);
                        }
                    }
                    break;

                case ParsableConstantKind.Int64:
                    {
                        var value = (Int64)constant.Value;
                        switch (targetCode)
                        {
                            case TypeCode.UInt64: if (value >= (Int64)UInt64.MinValue) return  new TypedConstant((UInt64)value); break;
                            case TypeCode.Int64: return new TypedConstant(value);
                            case TypeCode.UInt32: if (value >= UInt32.MinValue && value <= UInt32.MaxValue) return new TypedConstant((UInt32)value); break;
                            case TypeCode.Int32: if (value >= Int32.MinValue && value <= Int32.MaxValue) return  new TypedConstant((Int32)value); break;
                            case TypeCode.UInt16: if (value >= UInt16.MinValue && value <= UInt16.MaxValue) return  new TypedConstant((UInt16)value); break;
                            case TypeCode.Int16: if (value >= Int16.MinValue && value <= Int16.MaxValue) return new TypedConstant((Int16)value); break;
                            case TypeCode.Byte: if (value >= Byte.MinValue && value <= Byte.MaxValue) return  new TypedConstant((Byte)value); break;
                            case TypeCode.SByte: if (value >= SByte.MinValue && value <= SByte.MaxValue) return  new TypedConstant((SByte)value); break;
                            case TypeCode.Double: return  new TypedConstant((Double)value);
                            case TypeCode.Single: return  new TypedConstant((Single)value);
                        }
                        break;
                    }

                case ParsableConstantKind.Double:
                    {
                        var value = (Double)constant.Value;
                        switch (targetCode)
                        {
                            case TypeCode.Single: if (value >= Single.MinValue && value <= Single.MaxValue) return new TypedConstant((Single)value); break;
                            case TypeCode.Double: return new TypedConstant(value);
                        }
                        break;
                    }

                case ParsableConstantKind.String:
                    {
                        var value = (String)constant.Value;
                        switch (targetCode)
                        {
                            case TypeCode.String: return new TypedConstant(value);
                        }
                        break;
                    }

                case ParsableConstantKind.Char:
                    {
                        var value = (Char)constant.Value;
                        switch (targetCode)
                        {
                            case TypeCode.Char: return new TypedConstant(value);
                            case TypeCode.String: return new TypedConstant(new string(value, 1));
                        }
                        break;
                    }
                case ParsableConstantKind.Boolean:
                    {
                        var value = (Boolean)constant.Value;
                        switch (targetCode)
                        {
                            case TypeCode.Boolean: return  new TypedConstant(value);
                        }
                        break;
                    }
                case ParsableConstantKind.Null:
                    {
                        switch (targetCode)
                        {
                            case TypeCode.Object: return TypedConstant.CreateForNull();
                            case TypeCode.String: return TypedConstant.CreateForNull();
                        }
                        break;
                    }
                default: throw new PresentVariantNotImplementedException(typeof(ParsableConstantKind));
            }
            return null;
        }



        public CastTypeResult ImplicitCast(Type primary, Type target)
        {
            TypePair typePair = new TypePair(primary, target);

            if (primary == target)
                return ImplicitCastResult.CreateSuccessful(typePair, null);

            if (target==null)
                return new ImplicitCastResult(typePair, false, null);

            if (primary == null)
            {
                if (!target.IsValueType)
                return ImplicitCastResult.CreateSuccessful(typePair, null);
                return new ImplicitCastResult(typePair, false, null);
            }

            if (primary == typeof(void))
                return new ImplicitCastResult(typePair, false, null);

            if (PLNImplicitCast.Rules.TryGetValue(primary, out IReadOnlyDictionary<Type, CastInfo> toConvTypes))
            {
                if (toConvTypes.TryGetValue(target,out CastInfo info))
                {
                    return ImplicitCastResult.CreateSuccessful(typePair, info.NativeFunction);
                }
              //  return new ImplicitCastResult(typePair, false, null);
            }




            if (primary.IsValueType && (target == OBJECT || (target.IsInterface && primary.ImplementInterface(target))))
            {
                return new ImplicitCastResult(typePair, true, new NativeFunction(f=>f.Emit(OpCodes.Box,primary)));
            }

            if (target.IsAssignableFrom(primary))
            {
                return ImplicitCastResult.CreateSuccessful(typePair,null);
            }

            return new ImplicitCastResult(typePair, false, null);
        }



        public CastTypeResult ExplicictCast(Type primary, Type target)
        {
            TypePair typePair = new TypePair(primary, target);
            if (primary == target)
                return ExplicictCastResult.CreateSuccessful(typePair, null);

            if (primary==null)
                return ImplicitCast(primary, target);

            if (PLNExplicitCast.Rules.TryGetValue(primary, out IReadOnlyDictionary<Type, CastInfo> toConvTypes))
            {
                if (toConvTypes.TryGetValue(target, out CastInfo info))
                {
                    return ExplicictCastResult.CreateSuccessful(typePair, info.NativeFunction);
                }
            }
            if (primary.IsAssignableFrom(target))
            {
                if (target.IsValueType)
                {
                    return new ExplicictCastResult(typePair, true, new NativeFunction(f => f.Emit(OpCodes.Unbox_Any, target)));
                }
                return new ExplicictCastResult(typePair, true, new NativeFunction(f => f.Emit(OpCodes.Castclass, target)));
            }
            return ImplicitCast(primary,target);
        }



        public IsCastResult IsCast(Type primary, Type target)
        {
            TypePair typePair = new TypePair(primary, target);
            if (primary == target)
                return IsCastResult.CreateSuccessful(typePair, NativeFunction.Pop_PushI1);

            if (primary.IsAssignableFrom(target))
                return IsCastResult.CreateSuccessful(typePair,NativeFunction.GenISInst(target));
            if (target.IsAssignableFrom(primary))
                return IsCastResult.CreateSuccessful(typePair,NativeFunction.Pop_PushI1);

            return new IsCastResult(typePair, false, null);
        }


        public AsCastResult AsCast(Type primary, Type target)
        {
            TypePair typePair = new TypePair(primary, target);
            if (target.IsValueType)
                return new AsCastResult(typePair, AsResult.TargetTypeIsValueType, null);

            if (primary == target)
                return AsCastResult.CreateSuccessful(typePair,null);

            if (primary.IsAssignableFrom(target))
                return AsCastResult.CreateSuccessful(typePair, NativeFunction.GenASInst(target));
            if (target.IsAssignableFrom(primary))
                return AsCastResult.CreateSuccessful(typePair, null);

            return new AsCastResult(typePair, AsResult.CannotBeApplied, null);
        }


        public InterpretBinaryResult InterpretBinary(BinaryOperation operation,TypedExpression left, TypedExpression right)
        {

            BinaryInitiator initiator = new BinaryInitiator(operation,left, right);

            Type leftType = left.Type;
            Type rightType = right.Type;

            if (leftType == rightType && leftType == null)
            {
                if (operation == BinaryOperation.Equally)
                    return InterpretBinaryResult.CreateSuccesful(initiator, typeof(Boolean), NativeFunction.Ceq);
                if (operation == BinaryOperation.NotEqually)
                    return InterpretBinaryResult.CreateSuccesful(initiator, typeof(Boolean), NativeFunction.Cneq);
            }

            if (UnnormalizedTypes.Contains(leftType))
            {
                left.Expression.NativeFunctions.Add(NativeFunction.Conv_I4);
                leftType = typeof(Int32);
            }

            if (UnnormalizedTypes.Contains(rightType))
            {
                right.Expression.NativeFunctions.Add(NativeFunction.Conv_I4);
                rightType = typeof(Int32);
            }

            //if (leftType==null || rightType == null && (operation==BinaryOperation.Equally || operation==BinaryOperation.NotEqually))
            //{
            //    if (leftType==null && rightType == null)
            //    {
            //        if (operation == BinaryOperation.Equally)
            //            return InterpretBinaryResult.CreateSuccesful(initiator, typeof(Boolean), NativeFunction.Ceq);
            //        if (operation == BinaryOperation.NotEqually)
            //            return InterpretBinaryResult.CreateSuccesful(initiator, typeof(Boolean), NativeFunction.Cneq);
            //    }

            //    if (leftType==null && !rightType.IsValueType)
            //    {
            //        if (operation==BinaryOperation.Equally)
            //            return InterpretBinaryResult.CreateSuccesful(initiator, typeof(Boolean), NativeFunction.Ceq);
            //        if (operation == BinaryOperation.NotEqually)
            //            return InterpretBinaryResult.CreateSuccesful(initiator, typeof(Boolean), NativeFunction.Cneq);
            //    }

            //    if (rightType==null && !leftType.IsValueType)
            //    {
            //        if (operation == BinaryOperation.Equally)
            //            return InterpretBinaryResult.CreateSuccesful(initiator, typeof(Boolean), NativeFunction.Ceq);
            //        if (operation == BinaryOperation.NotEqually)
            //            return InterpretBinaryResult.CreateSuccesful(initiator, typeof(Boolean), NativeFunction.Cneq);
            //    }
            //}
           // if (leftType==rightType)

            if (leftType != rightType)
            {
                CastTypeResult rightLeftCast = ImplicitCast(rightType, leftType);
                CastTypeResult leftRightCast = ImplicitCast(leftType, rightType);

                if (rightLeftCast.IsSuccessful && !leftRightCast.IsSuccessful)
                {
                    rightType = leftType;
                    if (rightLeftCast.NativeFunction != null)
                        right.Expression.NativeFunctions.Add(rightLeftCast.NativeFunction);
                }
                else

                if (!rightLeftCast.IsSuccessful && leftRightCast.IsSuccessful)
                {
                    leftType = rightType;
                    if (leftRightCast.NativeFunction != null)
                        left.Expression.NativeFunctions.Add(leftRightCast.NativeFunction);
                }
                else
                {
                    if (rightLeftCast.IsSuccessful && leftRightCast.IsSuccessful)
                        return InterpretBinaryResult.TwoGeneralTypes(initiator, new TypePair(leftType, rightType));
                    else

                        return InterpretBinaryResult.CanNotFindGeneralType(initiator);
                }
            }

            if (!PLNBinaryOperations.Operations[operation].TryGetValue(leftType, out ConversionInfo conversionInfo))
            {
                if (!leftType.IsValueType)
                {
                    if (operation == BinaryOperation.Equally)
                        return InterpretBinaryResult.CreateSuccesful(initiator, typeof(Boolean), NativeFunction.Ceq);
                    if (operation == BinaryOperation.NotEqually)
                        return InterpretBinaryResult.CreateSuccesful(initiator, typeof(Boolean), NativeFunction.Cneq);
                }

                //if (leftType == rightType && !leftType.IsValueType && !rightType.IsValueType)
                //{
                //    if (operation == BinaryOperation.Equally)
                //        return InterpretBinaryResult.CreateSuccesful(initiator, typeof(Boolean), NativeFunction.Ceq);
                //    if (operation == BinaryOperation.NotEqually)
                //        return InterpretBinaryResult.CreateSuccesful(initiator, typeof(Boolean), NativeFunction.Cneq);
                //}
                return InterpretBinaryResult.CanNotBeApplied(initiator);
            }
            if (conversionInfo.LeftRightFunc != null)
            {
                left.Expression.NativeFunctions.Add(conversionInfo.LeftRightFunc);
                right.Expression.NativeFunctions.Add(conversionInfo.LeftRightFunc);
            }
                return InterpretBinaryResult.CreateSuccesful(initiator,conversionInfo.ResultType,conversionInfo.OperatorFunction);
        }

   

        public InterpretUnaryResult InterpretUnary(UnaryOperation operation, TypedExpression right)
        {
            UnaryInitiator initiator = new UnaryInitiator(operation,right);
            Type rightType = right.Type;
            if (UnnormalizedTypes.Contains(rightType))
            {
                right.Expression.NativeFunctions.Add(NativeFunction.Conv_I4);
                rightType = typeof(Int32);
            }

            if (!PLNUnaryOperations.Operations[operation].TryGetValue(rightType, out ConversionInfo conversionInfo))
            {
                return InterpretUnaryResult.CanNotBeApplied(initiator);
            }

            if (conversionInfo.LeftRightFunc != null)
            {
                right.Expression.NativeFunctions.Add(conversionInfo.LeftRightFunc);
            }

            return InterpretUnaryResult.CreateSuccesful(initiator, conversionInfo.ResultType, conversionInfo.OperatorFunction);
        }



        public int CalculateMass(Type primary, Type target)
        {
            throw new NotImplementedException();
        }
    }
}