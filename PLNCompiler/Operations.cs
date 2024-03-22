namespace PLNCompiler
{
    public enum BinaryOperation{Plus, Minus, Mul,Div,Mod,DivTrunc, DivMathRound, DivBankRound,Equally, NotEqually, Great, GreatEqls, Less,LessEqls,And,Or,Xor,LShift,RShift};

    public enum UnaryOperation { Plus,Minus,Inverse };

    public enum AssignKind { Assign, AssignPlus, AssignMinus, AssignMult, AssignDivide };
}
