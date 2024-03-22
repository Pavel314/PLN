//#define ILDebug
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Semantic;
using PLNCompiler.Semantic.SemanticTree;

namespace PLNCompiler.Compile.CodeGeneration
{
    public class PLNCodeGenerator : IModuleGenerator, ISemanticVisitor
    {
        public void VisitBlockNode(BlockNode node)
        {
            currentMethod.BeginScope();
            foreach (var statement in node.Statements)
                statement.Visit(this);
            currentMethod.EndScop(node.Statements.VariablesCount);
        }

        public void VisitVarDefineNode(VarDefineNode node)
        {
            foreach (var variable in node.Variables)
                variable.SetLocal(currentMethod.AddVariable(variable.Type, variable.Name));
        }

        public void VisitVarAssignNode(VarAssignNode varAssignNode)
        {
            varAssignNode.Expression.Visit(this);
            currentMethod.IL.Emit(OpCodes.Stloc, varAssignNode.Variable.Local);
#if ILDebug
            currentMethod.IL.EmitWriteLine(varAssignNode.Variable.Local);
            Console.WriteLine(string.Format("Define variable: {0}", varAssignNode.Variable.Name));
#endif
        }

       

        public void VisitConstantNode(ConstantNode constantNode)
        {
            ConstantPusher.Push(constantNode.Constant);
            constantNode.EmitNativeFunctions(currentMethod.IL);
#if ILDebug
            Console.WriteLine(string.Format("Push constant: {0}", constantNode.Constant.Value));
#endif
        }

        public void VisitVarAccessNode(VarAccessNode node)
        {
            OpCode ldCode = OpCodes.Ldloc;
            if (node.IsRef)
                ldCode = OpCodes.Ldloca;

            currentMethod.IL.Emit(ldCode, node.Variable.Local);
            node.EmitNativeFunctions(currentMethod.IL);
        }

        public void VisitCallStaticMethodNode(CallStaticMethodNode node)
        {
            node.Child.Visit(this);
            if (!lastMethodReturnIsVoid)
                currentMethod.IL.Emit(OpCodes.Pop);
        }


        public void VisitUnaryNode(UnaryNode unaryNode)
        {
            unaryNode.Expression.Visit(this);
            unaryNode.EmitNativeFunctions(currentMethod.IL);
        }

        public void VisitNewUnaryNode(NewUnaryNode newUnaryNode)
        {
            newUnaryNode.Child.Visit(this);
        }

        public void VisitBinaryNode(BinaryNode binaryNode)
        {
            if (binaryNode.Operation == BinaryOperation.Or && binaryNode.LeftRightIsBool)
            {
                binaryNode.Left.Visit(this);
                Label a= currentMethod.IL.DefineLabel();
                currentMethod.IL.Emit(OpCodes.Brtrue,a);
                currentMethod.IL.Emit(OpCodes.Ldc_I4_0);

                binaryNode.Right.Visit(this);

                binaryNode.OperatorFuction.Emit(currentMethod.IL);

                Label b = currentMethod.IL.DefineLabel();
                currentMethod.IL.Emit(OpCodes.Br, b);
                currentMethod.IL.MarkLabel(a);

                currentMethod.IL.Emit(OpCodes.Ldc_I4_1);
                currentMethod.IL.MarkLabel(b);
                binaryNode.EmitNativeFunctions(currentMethod.IL);
            }
            else
            {

                binaryNode.Left.Visit(this);
                binaryNode.Right.Visit(this);
                binaryNode.OperatorFuction.Emit(currentMethod.IL);
                binaryNode.EmitNativeFunctions(currentMethod.IL);
            }
        }

        public void VisitMemberAccessNode(MemberAccessNode memberAccessNode)
        {
            if (memberAccessNode.Root.Kind == RootKind.StaticField)
            {
                StaticFieldNode field = (StaticFieldNode)memberAccessNode.Root;
                if (field.Field.Attributes.HasFlag(FieldAttributes.Literal))
                {
                    object constantValue = field.Field.GetRawConstantValue();
                    Type constantType = constantValue.GetType();

                    ConstantPusher.Push(constantValue,constantType);
                    if (field.HasChild())
                    {
                        if (constantType.IsValueType)
                        {
                            LocalWrapper local = new LocalWrapper(constantType, null);
                            local.SetLocal(currentMethod.AddTempVariable(local.Type));
                            currentMethod.IL.Emit(OpCodes.Stloc,local.Local);
                            VisitInstanceRootMemberNode(new InstanceRootMemberNode(local,field.Child));
                            //LocalBuilder variable = currentMethod.AddTempVariable(constantType);
                            //currentMethod.IL.Emit(OpCodes.Stloc, variable);

                            //OpCode ldCode = OpCodes.Ldloca;

                            //if (variable.LocalIndex>=0 && variable.LocalIndex<=255)
                            //    ldCode = OpCodes.Ldloca_S;

                            //currentMethod.IL.Emit(ldCode, variable);
                        } else
                        //VisitRoot(field,constantType);
                        field.Child.Visit(this);
                    }
                        memberAccessNode.EmitNativeFunctions(currentMethod.IL);
#if ILDebug
                    Console.WriteLine("Push constant field:(Name: {0}, Value: {1})", field.Field,field.Field.GetRawConstantValue());
#endif
                    return;
                }
            }
            else
             if (memberAccessNode.Root.Kind == RootKind.StaticMethod && memberAccessNode.Root.HasChild())
            {
                StaticMethodNode staticMethodNode = (StaticMethodNode)memberAccessNode.Root;
                if (staticMethodNode.Method.ReturnType.IsValueType)
                {
                    foreach (var argument in staticMethodNode.Arguments)
                    {
                        argument.Expression.Visit(this);
                    }
                    currentMethod.IL.EmitCall(OpCodes.Call, staticMethodNode.Method, null);
                    LocalBuilder variable = currentMethod.AddTempVariable(staticMethodNode.Method.ReturnType);
                    currentMethod.IL.Emit(OpCodes.Stloc, variable);
                    currentMethod.IL.Emit(OpCodes.Ldloca_S, variable);
                    staticMethodNode.Child.Visit(this);
                    return;
                }
            }
            memberAccessNode.Root.Visit(this);
            memberAccessNode.EmitNativeFunctions(currentMethod.IL);
        }

        public void VisitInstanceMethodNode(InstanceMethodNode instanceMethodNode)
        {
            foreach (var argument in instanceMethodNode.Arguments)
            {
                argument.Expression.Visit(this);
            }
           // OpCode callCode = OpCodes.Call;
         //   if (instanceMethodNode.Method.DeclaringType==)
               OpCode callCode = OpCodes.Callvirt;

            if (instanceMethodNode.Method.ReturnType.IsValueType && instanceMethodNode.HasChild())
                currentMethod.IL.Emit(OpCodes.Box, instanceMethodNode.Method.ReturnType);

            currentMethod.IL.EmitCall(callCode, instanceMethodNode.Method, null);

            lastMethodReturnIsVoid = instanceMethodNode.Method.ReturnIsVoid() ;

            if (instanceMethodNode.HasChild())
            {
                
                instanceMethodNode.Child.Visit(this);
            }
        }

        public void VisitStaticFieldNode(StaticFieldNode staticFieldNode)
        {
            OpCode ldCode = OpCodes.Ldsfld;
            if (staticFieldNode.IsRef)
                ldCode = OpCodes.Ldsflda;
    
            currentMethod.IL.Emit(ldCode, staticFieldNode.Field);
        }

        public void VisitStaticMethodNode(StaticMethodNode staticMethodNode)
        {
            foreach (var argument in staticMethodNode.Arguments)
            {
                argument.Expression.Visit(this);
            }
            VisitRoot(staticMethodNode,staticMethodNode.Method.ReturnType);
            currentMethod.IL.EmitCall(OpCodes.Call, staticMethodNode.Method, null);
            lastMethodReturnIsVoid = staticMethodNode.Method.ReturnIsVoid();
            if (staticMethodNode.HasChild())
                staticMethodNode.Child.Visit(this);
        }


        public void VisitTypeOfNode(TypeOfNode node)
        {
            currentMethod.IL.Emit(OpCodes.Ldtoken, node.ArgumentType);
          //  currentMethod.WriteTypeOf(node.ArgumentType);
            node.EmitNativeFunctions(currentMethod.IL);

        }

        public void VisitCreateObjectNode(CreateObjectNode createObjectNode)
        {
            foreach (var argument in createObjectNode.Arguments)
                argument.Expression.Visit(this);

            currentMethod.IL.Emit(OpCodes.Newobj, createObjectNode.Constructor);
        }

  

        public void VisitIfElseNode(IfElseNode ifElseNode)
        {
            ifElseNode.Expression.Visit(this);

            if (!ifElseNode.HasFalseBranch())
            {
                currentMethod.GenIF(() => { ifElseNode.TrueBranch.Visit(this); });
            }
            else
                currentMethod.GenIF(() => { ifElseNode.TrueBranch.Visit(this); }, () => { ifElseNode.FalseBranch.Visit(this); });
        }

        public void VisitWhileLoopNode(WhileLoopNode whileLoopNode)
        {
            currentMethod.GenWhileLoop(() => { whileLoopNode.Expression.Visit(this); }, () => { whileLoopNode.Statement.Visit(this); });
        }

        public void VisitDoWhileLoopNode(DoWhileLoopNode doWhileLoopNode)
        {
            currentMethod.GenDoWhileLoop(() => { doWhileLoopNode.Statement.Visit(this); }, () => { doWhileLoopNode.Expression.Visit(this); });
        }

        public void VisitBreakNode(BreakNode node)
        {
            currentMethod.MakeBreak(node.ParentLevel);
        }

        public void VisitContinueNode(ContinueNode node)
        {
            currentMethod.MakeContinue(node.ParentLevel);
        }

        public void VisitLabelDefineNode(LabelDefineNode labelDefineNode)
        {
            if (labelDefineNode.Label.Label == null)
                labelDefineNode.Label.SetLabel(currentMethod.IL.DefineLabel());
            currentMethod.IL.MarkLabel(labelDefineNode.Label.Label.Value);
        }

        public void VisitGOTOLabelNode(GOTOLabelNode gotoLabelNode)
        {
            if (gotoLabelNode.Label.Label==null)
                gotoLabelNode.Label.SetLabel(currentMethod.IL.DefineLabel());
            //Console.WriteLine(gotoLabelNode.Label == null);
            currentMethod.IL.Emit(OpCodes.Br, gotoLabelNode.Label.Label.Value);
        }

        private void VisitRoot(RootMemberNode root,Type type)
        {
            if (!type.IsValueType) return;
           if (root.HasChild())
            {
                InstanceMethodNode methodNode = root.Child as InstanceMethodNode;
                if (ReferenceEquals(methodNode,null) || methodNode.Method.ReturnIsVoid()) return;
               if (methodNode.Method.DeclaringType != type)
                    currentMethod.IL.Emit(OpCodes.Box, type);
                //if (!methodNode.Method.ReturnType.IsValueType)
                //  currentMethod.IL.Emit(OpCodes.Box, type);
            }
        }

        public void VisitInstanceRootMemberNode(InstanceRootMemberNode node)
        {
            OpCode ldCode = OpCodes.Ldloc;
            if (!node.Local.Type.IsEnum && node.Local.Type.IsValueType) ldCode = OpCodes.Ldloca;
            currentMethod.IL.Emit(ldCode, node.Local.Local);
            VisitRoot(node,node.Local.Type);
            node.Child.Visit(this);
        }

        public void VisitCastTypeNode(CastTypeNode castTypeNode)
        {
            castTypeNode.Expression.Visit(this);
            castTypeNode.EmitNativeFunctions(currentMethod.IL);
        }

        public void VisitIsCastTypeNode(IsCastTypeNode node)
        {
            node.Expression.Visit(this);
            node.EmitNativeFunctions(currentMethod.IL);
        }

        public void VisitAsCastTypeNode(AsCastTypeNode node)
        {
            node.Expression.Visit(this);
            node.EmitNativeFunctions(currentMethod.IL);
        }


        public ModuleGeneratorResult GenerateModule(AssemblyBuilder assemblyBuilder,ModuleSetting moduleSetting, SemanticNode node)
        {
            
           moduleBuilder = assemblyBuilder.DefineDynamicModule(moduleSetting.FileName, moduleSetting.FileName, moduleSetting.EmitSymInfo);
           mainClass = moduleBuilder.DefineType(moduleSetting.MainClassName, TypeAttributes.Class | TypeAttributes.Public);
         var  mainMethod = new MethodGenerator(mainClass.DefineMethod(moduleSetting.MainMethodName, MethodAttributes.Public | MethodAttributes.Static),moduleSetting.EmitSymInfo);
            currentMethod = mainMethod;
            ConstantPusher.IL = currentMethod.IL;
            Reset();
            node.Visit(this);
#if ILDebug
            currentMethod.WriteReadLine();
#endif
            mainMethod.IL.Emit(OpCodes.Ret);
            return new ModuleGeneratorResult(moduleBuilder, mainClass.CreateType(), mainMethod.MethodBuilder); 
        }

        public PLNCodeGenerator()
        {
  
            ConstantPusher = new ConstantPusher();
            methods = new List<MethodGenerator>();
            Reset();
        }

        private void Reset()
        {
            lastMethodReturnIsVoid = false;
            methods.Clear();
        }

       

        private bool lastMethodReturnIsVoid;

        private ConstantPusher ConstantPusher;
        private ModuleBuilder moduleBuilder;
        private TypeBuilder mainClass;
     //   private MethodGenerator mainMethod;
        private List<MethodGenerator> methods;
        private MethodGenerator currentMethod;

    }
}
