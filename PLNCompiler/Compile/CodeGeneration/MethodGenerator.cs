using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Reflection;

namespace PLNCompiler.Compile.CodeGeneration
{
    public class MethodGenerator
    {
        public static readonly MethodInfo ReadLineMethod = typeof(Console).GetMethod("ReadLine");
        public static readonly MethodInfo StringConcat = typeof(String).GetMethod("Concat", new Type[2] { typeof(string), typeof(string) });
        public static readonly MethodInfo StringEquality = typeof(String).GetMethod("op_Equality", new Type[2] { typeof(string), typeof(string) });
        public static readonly MethodInfo StringInequality = typeof(String).GetMethod("op_Inequality", new Type[2] { typeof(string), typeof(string) });
        public static readonly MethodInfo CharToString = typeof(Char).GetMethod("ToString", new Type[1] { typeof(char) });
        public static readonly MethodInfo MathRound = typeof(Math).GetMethod("Round", new Type[1] { typeof(double) });
        public static readonly MethodInfo CovertToInt32 = typeof(Convert).GetMethod("ToInt32", new Type[1] { typeof(double) });
        public static readonly MethodInfo CovertToInt64 = typeof(Convert).GetMethod("ToInt64", new Type[1] { typeof(double) });
        public static readonly MethodInfo CovertToUInt32 = typeof(Convert).GetMethod("ToUInt32", new Type[1] { typeof(double) });
        public static readonly MethodInfo CovertToUInt64 = typeof(Convert).GetMethod("ToUInt64", new Type[1] { typeof(double) });
        public static readonly MethodInfo GetTypeFromHandle = typeof(Type).GetMethod("GetTypeFromHandle", new Type[1] { typeof(RuntimeTypeHandle) });

        public ILGenerator IL { get; private set; }
        public MethodBuilder MethodBuilder { get; private set; }

        public MethodGenerator(MethodBuilder header,bool emitSymInfo)
        {
            variables = new Stack<LocalBuilder>();
            loopLabelsStack = new Stack<LoopLabels>();
            MethodBuilder = header;
            EmitSymInfo = emitSymInfo;
            IL = header.GetILGenerator();
        }

        public void BeginScope()
        {
            IL.BeginScope();
        }

        public void EndScop(int popVarsCount)
        {
            variables.Pops(popVarsCount);
            IL.EndScope();
        }

        public void WriteTypeOf(Type type)
        {
            IL.Emit(OpCodes.Ldtoken, type);
            IL.Emit(OpCodes.Call, GetTypeFromHandle);
        }

        public void WriteNop()
        {
            IL.Emit(OpCodes.Nop);
        }
        public void WriteRet()
        {
            IL.Emit(OpCodes.Ret);
        }

        public LocalBuilder AddVariable(Type variableType, string name)
        {
            LocalBuilder varible = IL.DeclareLocal(variableType, false);
            if (EmitSymInfo)
            varible.SetLocalSymInfo(name);
            variables.Push(varible);
            return varible;
        }

        public LocalBuilder AddTempVariable(Type variableType)
        {
            LocalBuilder varible = IL.DeclareLocal(variableType, false);

            return varible;
        }

        public void WriteReadLine()
        {
            IL.EmitCall(OpCodes.Call, ReadLineMethod, null);
            IL.Emit(OpCodes.Pop);
        }

        public void GenIF(Action GenTrueBody)
        {
            var truelabel = IL.DefineLabel();
            var exitlabel = IL.DefineLabel();
            IL.Emit(OpCodes.Brtrue, truelabel);
            IL.Emit(OpCodes.Br, exitlabel);

            IL.MarkLabel(truelabel);
            GenTrueBody();

            IL.MarkLabel(exitlabel);
        }
     //   public void GenIF(RequireIFBody GenTrueBody, RequireIFBody GenFalseBody)
       public void GenIF(Action GenTrueBody, Action GenFalseBody)
        {

            var truelabel = IL.DefineLabel();
            var falselabel = IL.DefineLabel();
            var exitlabel = IL.DefineLabel();

            IL.Emit(OpCodes.Brtrue, truelabel);
            IL.Emit(OpCodes.Br, falselabel);

            IL.MarkLabel(truelabel);
            GenTrueBody();
            IL.Emit(OpCodes.Br, exitlabel);

            IL.MarkLabel(falselabel);
            GenFalseBody();

            IL.MarkLabel(exitlabel);

        }
        public void GenWhileLoop(Action genExpression, Action genBody)
        {
            var exitLabel = IL.DefineLabel();
            var bodyLabel = IL.DefineLabel();
            IL.MarkLabel(bodyLabel);

            genExpression();
            IL.Emit(OpCodes.Brfalse, exitLabel);

            loopLabelsStack.Push(new LoopLabels(bodyLabel, exitLabel));
            genBody();
            loopLabelsStack.Pop();

            IL.Emit(OpCodes.Br, bodyLabel);
            IL.MarkLabel(exitLabel);
        }

        public void GenDoWhileLoop(Action genBody, Action genExpression)
        {
            var exitLabel = IL.DefineLabel();
            var bodyLabel = IL.DefineLabel();

            IL.MarkLabel(bodyLabel);

            loopLabelsStack.Push(new LoopLabels(bodyLabel, exitLabel));
            genBody();
            loopLabelsStack.Pop();

            genExpression();
            IL.Emit(OpCodes.Brfalse, exitLabel);
            IL.Emit(OpCodes.Br, bodyLabel);
            IL.MarkLabel(exitLabel);
        }

        public void MakeBreak(int parentLevel)
        {
            int i = 0;
            foreach (var pair in loopLabelsStack)
            {
                if (i == parentLevel)
                {
                    IL.Emit(OpCodes.Br, pair.ExitLabel);
                    break;
                }
                i++;
            }
        }

        public void MakeContinue(int parentLevel)
        {
            int i = 0;
            foreach (var pair in loopLabelsStack)
            {
                if (i == parentLevel)
                {
                    IL.Emit(OpCodes.Br, pair.Bodylabel);
                    break;
                }
                i++;
            }
        }

        //public void GenLoop(int iters, RequireLoopBody loopBodyGenerator)
        public void GenLoop(int iters,Action<ILGenerator,LocalBuilder,Label,Label> loopBodyGenerator)
        {
            //if Iters<0 not call loop or abs?

            var i = IL.DeclareLocal(typeof(Int32));
            var beginlabel = IL.DefineLabel();
            var exitlabel = IL.DefineLabel();

            IL.Emit(OpCodes.Ldc_I4, iters);
            IL.Emit(OpCodes.Stloc, i);



            IL.MarkLabel(beginlabel);
            IL.Emit(OpCodes.Ldloc, i);
            IL.Emit(OpCodes.Ldc_I4_0);
            IL.Emit(OpCodes.Ble, exitlabel);

            //loopBodyGenerator(this, new RequireLoopBodyEventArgs(IL, i, beginlabel, exitlabel));
            loopBodyGenerator(IL, i, beginlabel, exitlabel);

            IL.Emit(OpCodes.Ldloc, i);
            IL.Emit(OpCodes.Ldc_I4_1);
            IL.Emit(OpCodes.Sub);
            IL.Emit(OpCodes.Stloc, i);

            IL.Emit(OpCodes.Br, beginlabel);
            IL.MarkLabel(exitlabel);
        }

        private Stack<LocalBuilder> variables;
        private Stack<LoopLabels> loopLabelsStack;

        public IEnumerable<LocalBuilder> Variables
        {
            get
            {
                return variables;
            }
        } 

        public bool EmitSymInfo { get; private set; }

        public delegate void RequireIFBody(object e, RequireIFBodyEventArgs args);
        public delegate void RequireLoopBody(object e, RequireLoopBodyEventArgs args);

        public struct LoopLabels
        {
            public readonly Label Bodylabel;
            public readonly Label ExitLabel;

            public LoopLabels(Label bodylabel, Label exitLabel)
            {
                Bodylabel = bodylabel;
                ExitLabel = exitLabel;
            }
        }
    }
}
