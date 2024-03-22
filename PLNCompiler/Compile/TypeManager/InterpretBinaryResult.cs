using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.TypeManager
{
    public enum InterpretBinaryResults { CanNotFindGeneralType,TwoGeneralTypes,CanNotBeApplied, OK}

    public class InterpretBinaryResult : InterpretResult<BinaryInitiator>
    {
        public InterpretBinaryResults Result { get; private set; }

        private TypePair generalTypes;
        public TypePair GeneralTypes
        {
            get
            {
                if (Result != InterpretBinaryResults.TwoGeneralTypes)
                    throw new InvalidOperationException("Results != InterpretBinaryResults.TwoGeneralTypes");
                return generalTypes;
            }
        }

        protected InterpretBinaryResult(BinaryInitiator initiator, TypePair generalTypes, InterpretBinaryResults result, 
            Type interpType, NativeFunction nativeFunction) :
            base(initiator,result==InterpretBinaryResults.OK, interpType, nativeFunction)
        {
           this.generalTypes = generalTypes;
            Result = result;
        }

        public static InterpretBinaryResult CreateSuccesful(BinaryInitiator initiator, Type interpType, NativeFunction nativeFunction)
        {
            return new InterpretBinaryResult(initiator,new TypePair(), InterpretBinaryResults.OK, interpType, nativeFunction);
        }

        public static InterpretBinaryResult CanNotFindGeneralType(BinaryInitiator initiator)
        {
            return new InterpretBinaryResult(initiator, new TypePair(), InterpretBinaryResults.CanNotFindGeneralType, null, null);
        }

        public static InterpretBinaryResult TwoGeneralTypes(BinaryInitiator initiator, TypePair generalTypes)
        {
            return new InterpretBinaryResult(initiator, generalTypes, InterpretBinaryResults.TwoGeneralTypes, null, null);
        }

        public static InterpretBinaryResult CanNotBeApplied(BinaryInitiator initiator)
        {
            return new InterpretBinaryResult(initiator, new TypePair(), InterpretBinaryResults.CanNotBeApplied, null, null);
        }
    }
}
