using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Semantic;
using PLNCompiler.Syntax.SyntaxTree;
using PLNCompiler.Syntax.Optimization;
using PLNCompiler.Compile.Errors;
using PLNCompiler.Compile.TypeManager;
using System.Reflection;
using PLNCompiler.Compile.PLNReflection;
using PLNCompiler.Compile.PLNReflection.TypeFinder;
using PLNCompiler.Compile.PLNReflection.MemberSelector;

//При генерации блока вызовать не VisitBlockNode VisitStatement

namespace PLNCompiler.Syntax.TreeConverter
{
    public class PLNTreeConverter : ITreeConverter, IStatementVisitor, IExpressionVisitor
    {

        private void VisitStatement(StatementNode node)
        {
            var parent = semanticBlock;
            node.Visit(this);
            if (node is MemberNode)
            {
                semanticBlock.Statements.Add(new Semantic.SemanticTree.CallStaticMethodNode(semanticMember));

                semanticMember = null;
                semanticLastMember = null;
              //  semanticVoidType = null;
                semanticExpression = null;
                semanticFindMode = FindMode.Unkown;
            } else
                if (node is BlockNode blockNode) semanticBlock = parent;
        }

        private int deep = 0;
        public void VisitBlockNode(BlockNode node)
        {
            deep++;
            var waitlabelsCopy = waitLabels;
            waitLabels = new WaitLabelsDictionary();

            if (!ReferenceEquals(semanticBlock, null))
            {
                var childBlock = new Semantic.SemanticTree.BlockNode();
                semanticBlock.Statements.Add(childBlock);
                semanticBlock = childBlock;
            }
            else
                semanticBlock = new Semantic.SemanticTree.BlockNode();

            foreach (var statement in node.Statements)
            {
                VisitStatement(statement);
            }
            variablesStack.Pops(semanticBlock.Statements.VariablesCount);
            labelsStack.Pops(semanticBlock.Statements.LabelsCount);

            var newLabels = waitLabels;
            waitLabels = waitlabelsCopy;
            waitLabels.AddUniqueRange(newLabels);
            deep--;
            if (deep == 0 && waitLabels.Count != 0)
            {
              throw new SemanticErrors.LabelsNotFound(waitLabels.ToDictionary(f => f.Key, f => (IReadOnlyList<Location>)f.Value.Location, waitLabels.Comparer));
            }
        }



        private void callExceptionIfLocalVariableNameExists(LocalWrapper a,IdentiferNode b)
        {
            if (StringHelper.PLNComparer.EqualsStr(a.Name, b.Name))
                throw new SemanticErrors.VariableArleadyDefine(b.Location, b.Name);
        }

        private void addVariable(Semantic.SemanticTree.VarDefineNode varDefineNode,Type type,IdentiferNode variable)
        {
            var wrapper = new LocalWrapper(type, variable.Name);
            varDefineNode.Variables.Add(wrapper);
            variablesStack.Push(wrapper);
        }

        public void VisitVarDefineNode(VarDefineNode node)
        {
            var semanticVarDefNode = new Semantic.SemanticTree.VarDefineNode();
            var type = TypeGenerator.GenerateType(node.TypeNode);
            foreach (var variable in node.Variables)
            {
                foreach (var stackVariable in variablesStack)
                {

                    callExceptionIfLocalVariableNameExists(stackVariable, variable);
                }
                addVariable(semanticVarDefNode, type, variable);

            }
            semanticBlock.Statements.Add(semanticVarDefNode);

            //foreach (var stackVariable in variablesStack)
            //{
            //    foreach (var variable in node.Variables)
            //    {
            //        callExceptionIfLocalVariableNameExists(stackVariable, variable);
            //    }
            //}
            //var semanticVarDefNode = new Semantic.SemanticTree.VarDefineNode();
            //var type = TypeGenerator.GenerateType(node.TypeNode);
            //foreach (var variable in node.Variables)
            //{
            //    addVariable(semanticVarDefNode, type, variable);
            //}

            //semanticBlock.Statements.Add(semanticVarDefNode);
        }


        private Semantic.SemanticTree.ExpressionNode makeRightAssign(ExpressionNode rightExpression, Location  location, ref Type type)
        {
            ConstNode unParsableConstantNode = rightExpression as ConstNode;
            ParsableConstNode constantNode = null;

            if (unParsableConstantNode != null)
                constantNode = ParseConstant(unParsableConstantNode);
            else
                constantNode = rightExpression as ParsableConstNode;


            if (constantNode == null)
            {
                rightExpression.Visit(this);
                if (type == null)
                {
                    type = semanticExpressionType;
                }
                else
                { 
                    CastTypeResult castResult = TypeConversion.ImplicitCast(semanticExpressionType, type);
                    if (!castResult.IsSuccessful) throw new SemanticErrors.InvalidImplicitCast(location, castResult.Initiator.Type1, castResult.Initiator.Type2);
                    if (castResult.NativeFunction != null) semanticExpression.NativeFunctions.Add(castResult.NativeFunction);
                }
                return semanticExpression;
            }

            if (type==null)
            {
                TypedConstant tConst = constantNode.Constant.ToTypedConstant();
                if (!tConst.IsNull)
                    type = tConst.Type;
                else
                   type = PLNTypeConversion.OBJECT;
                return new Semantic.SemanticTree.ConstantNode(tConst);
            }


            TypedConstant typedConstant = TypeConversion.AssignConstant(constantNode.Constant, type);
            NativeFunction convFunction = null;
            if (typedConstant == null)
            {
                typedConstant = constantNode.Constant.ToTypedConstant();
                CastTypeResult castTypeResult = TypeConversion.ImplicitCast(typedConstant.Type, type);
                if (!castTypeResult.IsSuccessful) throw new SemanticErrors.ConstantCanNotAssign(constantNode.Location, constantNode.Constant, type);
                convFunction = castTypeResult.NativeFunction;
            }

            return new Semantic.SemanticTree.ConstantNode(typedConstant, convFunction);
        }

        public void VisitAssignNode(AssignNode node)
        {
            if (node.AssignType != AssignKind.Assign) throw new NotImplementedException();
            ExpressionNode rightExpression = node.Expression;
            if (ExpressionOptimizer != null) rightExpression = ExpressionOptimizer.Optimize(rightExpression);

            var varNode = node.Assignable as FieldNode;
            if (varNode == null) throw new NotImplementedException();

            LocalWrapper localWrapper = FindLocalVariable(varNode.ToVariable());

            Type type = localWrapper.Type;
            semanticBlock.Statements.Add(new Semantic.SemanticTree.VarAssignNode(localWrapper, makeRightAssign(rightExpression, node.Location, ref type)));

            //ConstNode unParsableConstantNode = rightExpression as ConstNode;
            //ParsableConstNode constantNode = null;
            //Semantic.SemanticTree.ExpressionNode targetExpression = null;


            //if (unParsableConstantNode != null)
            //    constantNode = ParseConstant(unParsableConstantNode);
            //else
            //    constantNode = rightExpression as ParsableConstNode;


            //if (constantNode == null)
            //{
            //    rightExpression.Visit(this);
            //    CastTypeResult castResult = TypeConversion.ImplicitCast(semanticExpressionType, localWrapper.Type);
            //    if (!castResult.IsSuccessful) throw new SemanticErrors.InvalidImplicitCast(node.Location, castResult.Initiator.Type1, castResult.Initiator.Type2);
            //    if (castResult.NativeFunction != null) semanticExpression.NativeFunctions.Add(castResult.NativeFunction);

            //    targetExpression = semanticExpression;
            //}
            //else
            //{
            //    TypedConstant typedConstant = TypeConversion.AssignConstant(constantNode.Constant, localWrapper.Type);
            //    if (typedConstant == null)
            //    {
            //        typedConstant = constantNode.Constant.ToTypedConstant();
            //        CastTypeResult castTypeResult = TypeConversion.ImplicitCast(typedConstant.Type, localWrapper.Type);
            //        if (castTypeResult.IsSuccessful)
            //            targetExpression = new Semantic.SemanticTree.ConstantNode(typedConstant, castTypeResult.NativeFunction);
            //        else
            //            throw new SemanticErrors.ConstantCanNotAssign(constantNode.Location, constantNode.Constant, localWrapper.Type);
            //    }
            //    else
            //        targetExpression = new Semantic.SemanticTree.ConstantNode(typedConstant);
            //}

        }


        private void ifVariableExistsException(IdentiferNode node)
        {
            foreach (var stackVariable in variablesStack)
            {
                callExceptionIfLocalVariableNameExists(stackVariable, node);
            }
        }

        public void VisitVarDefineAssignNode(VarDefineAssignNode node)
        {
            ifVariableExistsException(node.Variable);

            Type type = TypeGenerator.GenerateType(node.TypeNode);

            LocalWrapper wrapper = new LocalWrapper(type,node.Variable.Name);
            variablesStack.Push(wrapper);
            semanticBlock.Statements.Add(new Semantic.SemanticTree.VarDefineNode(wrapper));

            ExpressionNode rightExpression = node.Expression;
            if (ExpressionOptimizer != null) rightExpression = ExpressionOptimizer.Optimize(rightExpression);

            semanticBlock.Statements.Add(new Semantic.SemanticTree.VarAssignNode(wrapper, makeRightAssign(rightExpression, node.Location, ref type)));
        }



        public void VisitVarDefineAssignAutoNode(VarDefineAssignAutoNode node)
        {
            ifVariableExistsException(node.Variable);

            ExpressionNode rightExpression = node.Expression;
            if (ExpressionOptimizer != null) rightExpression = ExpressionOptimizer.Optimize(rightExpression);

            Type type = null;
           var expression = makeRightAssign(rightExpression, node.Location, ref type);

            LocalWrapper wrapper = new LocalWrapper(type, node.Variable.Name);
            variablesStack.Push(wrapper);
            semanticBlock.Statements.Add(new Semantic.SemanticTree.VarDefineNode(wrapper));
            semanticBlock.Statements.Add(new Semantic.SemanticTree.VarAssignNode(wrapper, expression));
        }



        public void VisitParsableConstantNode(ParsableConstNode node)
        {
            TypedConstant typedConstant = node.Constant.ToTypedConstant();
            semanticExpressionType = typedConstant.Type;
            semanticExpression = new Semantic.SemanticTree.ConstantNode(typedConstant);

        }



        public void VisitUnaryNode(UnaryNode node)
        {
            node.Right.Visit(this);
            InterpretUnaryResult interpResult = TypeConversion.InterpretUnary(node.Operation, new TypedExpression(semanticExpression, semanticExpressionType));
            if (!interpResult.IsSuccessful)
            {
                throw SemanticErrors.Get(node.Location, interpResult);
            }
            semanticExpressionType = interpResult.InterpType;
            semanticExpression = new Semantic.SemanticTree.UnaryNode(node.Operation, semanticExpression);
            semanticExpression.NativeFunctions.Add(interpResult.NativeFunction);  
        }



        public void VisitBinaryNode(BinaryNode node)
        {
                node.Left.Visit(this);
                var left = semanticExpression;
                var leftType = semanticExpressionType;
                node.Right.Visit(this);
                InterpretBinaryResult interpResult = TypeConversion.InterpretBinary(node.Opration, new TypedExpression(left, leftType), new TypedExpression(semanticExpression, semanticExpressionType));
                if (!interpResult.IsSuccessful)
                    throw SemanticErrors.Get(node.Location, interpResult);
                semanticExpressionType = interpResult.InterpType;
                semanticExpression = new Semantic.SemanticTree.BinaryNode(node.Opration,semanticExpressionType==typeof(bool),interpResult.NativeFunction, left, semanticExpression);
        }



        public void VisitNewUnaryNode(NewUnaryNode node)
        {
            Type type = TypeGenerator.GenerateType(node.Type);
            PLNCompiler.Semantic.SemanticTree.CreateObjectNode createObjectNode = null;
            ConstructorInfo constructor = null ;
            if (node.Kind == NewUnaryKind.Object)
            {
                var processArgumensResult = ProcessMethodFactArguments(node.Arguments);
                var arguments = processArgumensResult.Arguments;
                var argumentTypes = processArgumensResult.ArgumentTypes;
                constructor = type.GetConstructor(argumentTypes);
                if (constructor is null)
                    throw new SemanticErrors.ConstructorNotFound(node.Location, type, argumentTypes);
                MakeParametersConvension(constructor, arguments);
                createObjectNode = new Semantic.SemanticTree.CreateObjectNode(type, constructor,arguments);
                semanticExpression = new Semantic.SemanticTree.NewUnaryNode(createObjectNode);
                semanticExpressionType = type;
                semanticMember = createObjectNode;
            //    semanticFindMode = FindMode.Instance;
                return;
            } 


            if (!node.HasArguments()) throw new Exception("Нет аргументов");

            {
                Type elementType = type;
                type = type.MakePLNArrayType(node.Arguments.Count);
                if (node.Arguments.Count != type.GetArrayRank())
                    throw new Exception("Не то число индексов");
                // semanticVoidType = type;
                semanticExpressionType = type;
                var arguments = new TypedExpression[node.Arguments.Count];
                var argumentTypes = new Type[node.Arguments.Count];
                Type oldSemanticExprType = semanticExpressionType;
                int i = 0;
                foreach (var argument in node.Arguments)
                {
                    argumentTypes[i] = elementType;
                    semanticExpressionType = elementType;
                    argument.Visit(this);
                    arguments[i] = new TypedExpression(semanticExpression, elementType);
                    i++;
                }
                semanticExpressionType = oldSemanticExprType;

                constructor = semanticExpressionType.GetConstructor(argumentTypes);
                if (constructor is null)
                    throw new SemanticErrors.ConstructorNotFound(node.Location, type, argumentTypes);
                semanticExpressionType = type;
                //  semanticVoidType = type;
                semanticFindMode = FindMode.Unkown;
                createObjectNode = new PLNCompiler.Semantic.SemanticTree.CreateObjectNode(type, constructor, arguments);
                semanticExpression = new PLNCompiler.Semantic.SemanticTree.NewUnaryNode(createObjectNode);
                semanticMember = createObjectNode;
                // semanticBlock.Statements.Add(new PLN.Semantic.SemanticTree.CreateObjectNode(type,constructor,null));
                // throw new NotImplementedException();
            }
        }

        //private Tuple<TypedExpression[],Type[]> ProcessArguments(ExpressionCollection methodArguemts)
        //{
        //    TypedExpression[] arguments = new TypedExpression[methodArguemts.Count];
        //    Type[] argumentTypes = new Type[methodArguemts.Count];
        //    int i = 0;
        //    foreach (var argument in methodArguemts)
        //    {
        //        var oldSemanticLastMember = semanticLastMember;
        //        var oldSemanticMember = semanticMember;

        //        argument.Visit(this);

        //        semanticLastMember = oldSemanticLastMember;
        //        semanticMember = oldSemanticMember;

        //        arguments[i] = new TypedExpression(semanticExpression, semanticExprType);
        //        argumentTypes[i] = semanticExprType;
        //        i++;
        //    }
        //    return new Tuple<TypedExpression[], Type[]>(arguments, argumentTypes);
        //}

            protected virtual System.Reflection.MethodInfo GetMethod(Type mainType,string name,BindingFlags flags,Type[] types)
        {
            foreach (var type in types)
            {
                if (type == typeof(void))
                    return null;
            }
            return mainType.GetMethod(name, flags, null, types, null);
        }

        public void VisitMethodNode(MethodNode node)
        {
            FindMode mode = semanticFindMode;

            var processArgumensResult = ProcessMethodFactArguments(node.Arguments);
            var arguments = processArgumensResult.Arguments;
            var argumentTypes = processArgumensResult.ArgumentTypes;

            System.Reflection.MethodInfo method = null;
            if (mode == FindMode.Static)
            {
                //TODO
                //Char Byte Not
                // method = semanticExpressionType.GetMethod(node.Name.Name, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase, null, argumentTypes, null);
                method = GetMethod(semanticExpressionType, node.Name.Name, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase,argumentTypes);
                if (method is null)
                    throw new SemanticErrors.StaticMethodNotFound(node.Location, semanticExpressionType, node.Name.Name, argumentTypes);
                
                MakeParametersConvension(method, arguments);
                semanticMember = new Semantic.SemanticTree.StaticMethodNode(method, arguments);
                semanticFindMode = FindMode.Instance;
            }
            else
                if (mode == FindMode.Instance)
            {
                // method = semanticExpressionType.GetMethod(node.Name.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase, null, argumentTypes, null);
                method = GetMethod(semanticExpressionType, node.Name.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase, argumentTypes);
                if (method is null)
                    throw new SemanticErrors.InstanceMethodNotFound(node.Location, semanticExpressionType, node.Name.Name, argumentTypes);


                MakeParametersConvension(method, arguments);
                var instanceMethod = new Semantic.SemanticTree.InstanceMethodNode(method, arguments);
                if (semanticLastMember == null)
                {
                   // if (!method.ReturnType.IsValueType)
                     

                    semanticLastMember = instanceMethod;
                    semanticMember.Child = semanticLastMember;
                }
                else
                {
                    semanticLastMember.Child = instanceMethod;
                    semanticLastMember = semanticLastMember.Child;
                }
            }
            else
                if (mode == FindMode.Unkown)
            {
                throw new NotImplementedException();
            }
            else throw new PresentVariantNotImplementedException(typeof(FindMode));

            semanticExpressionType = method.ReturnType;
        //    semanticVoidType = method.ReturnType;
            if (node.HasChild())
                node.Child.Visit(this);
        }

        //Статические индексаторы не могут быть


        public void VisitFieldNode(FieldNode node)
        {
            if (node.GetIndexsCount() != 0) throw new NotImplementedException();
       

            if (semanticFindMode == FindMode.Unkown)
            {
                LocalWrapper localWrapper;
                if (node.GetGenericsCount() == 0 && (localWrapper = FindLocalVariable(node.Name, false)) != null)
                {
                    // ;
                    //if (localWrapper == null) goto br2;
                    semanticFindMode = FindMode.Instance;
                    //   semanticVoidType = localWrapper.Type;
                    semanticExpressionType = localWrapper.Type;

                    semanticMember = new Semantic.SemanticTree.InstanceRootMemberNode(localWrapper);
                    if (node.HasChild())
                    {
                        //  var o = semanticMember;
                        node.Child.Visit(this);
                        // semanticMember = o;
                    }
                }
                else
                {
                    ProcessStaticFieldResult processedField = ProcessStaticField(node);
                    SelectStringMemberResult selectResult = processedField.LastSelectResult;
                    if (selectResult != null)
                    {
                        if (!selectResult.IsSuccessful)
                        {
                            throw SemanticErrors.Get(processedField.LastLocation, selectResult, processedField.ResultType);
                        }
                        //MemberTypes.Field | MemberTypes.Event | MemberTypes.
                        MemberInfo foundResult = selectResult.FoundResult;

                        if (foundResult.MemberType == MemberTypes.Field)
                        {
                            FieldInfo fieldInfo = (FieldInfo)foundResult;
                            semanticExpressionType = fieldInfo.FieldType;

                            if (isRefArgument)
                            {
                                if (fieldInfo.IsLiteral)
                                    throw new SemanticErrors.ConstantCanNotBePassedByRef(processedField.LastLocation, fieldInfo);
                                if (fieldInfo.IsInitOnly)
                                    throw new SemanticErrors.ReadOnlyFieldCanNotBePassedByRef(processedField.LastLocation, fieldInfo);
                            }

                            semanticMember = new Semantic.SemanticTree.StaticFieldNode(fieldInfo, isRefArgument);

                            // semanticVoidType = fieldInfo.FieldType;
                        }
                        else
                            if (foundResult.MemberType == MemberTypes.Property)
                        {
                            PropertyInfo foundProperty = ((PropertyInfo)foundResult);
                            if (isRefArgument) throw new SemanticErrors.PropertyCanNotBePassedByRef(processedField.LastLocation, foundProperty);

                            System.Reflection.MethodInfo methodInfo = foundProperty.GetGetMethod(false);
                            if (methodInfo == null) throw new Exception("Not exists open get method");
                            semanticMember = new Semantic.SemanticTree.StaticMethodNode(methodInfo);
                            semanticExpressionType = methodInfo.ReturnType;
                            //  semanticVoidType = methodInfo.ReturnType;
                        }
                        else throw new NotImplementedException();

                        semanticFindMode = FindMode.Instance;
                    }
                    else
                    {
                        //Если здесь значит было встречено: метод или индексированное поле
                        //  semanticVoidType = processedField.ResultType;
                        semanticExpressionType = processedField.ResultType;
                        semanticFindMode = FindMode.Static;
                    }
                    if (processedField.StayMembers != null)
                    {
                        processedField.StayMembers.Visit(this);
                    }
                    //   else
                    //   semanticExprType = semanticVoidType;
                    return;
                }

            }
            else
            {
                if (semanticFindMode == FindMode.Instance)
                {
                    throw new SemanticErrors.InstanceStringMemberNotFound(node.Location, semanticExpressionType, new FieldHeader("", new MemberFlags()));
                  //  throw new NotImplementedException();
                }

                    throw new NotImplementedException();
            }
        }



        public void VisitTypeOfNode(TypeOfNode node)
        {
            Type generatedType = TypeGenerator.GenerateType(node.Argument);
            semanticExpression = new Semantic.SemanticTree.TypeOfNode(generatedType);
            //   var oldSemanticVoidType = semanticVoidType;
            var oldSemanticFindMode = semanticFindMode;
            semanticFindMode = FindMode.Instance;
            //  semanticVoidType = typeof(Type);
            semanticExpressionType = typeof(Type);
            semanticMember = new PLNCompiler.Semantic.SemanticTree.StaticMethodNode(PLNCompiler.Compile.CodeGeneration.MethodGenerator.GetTypeFromHandle, new TypedExpression[1] { new TypedExpression(semanticExpression, typeof(Type)) });
            if (node.HasChild())
                node.Child.Visit(this);

            semanticFindMode = oldSemanticFindMode;

        }



        public void VisitExpressionArgumentNode(ExpressionArgumentNode node)
        {
            bool oldIsRefArgument = isRefArgument;
            isRefArgument = false;
            node.Expression.Visit(this);
            isRefArgument = oldIsRefArgument;
        }

        private bool isRefArgument = false;

        public void VisitRefArgumentNode(RefArgumentNode node)
        {
            bool oldIsRefArgument = isRefArgument;
            isRefArgument = true;
           ((new MemberAccessNode(null, node.Field, node.Field.Location))).Visit(this);
            //  node.Field.Visit(this);
           semanticExpressionType= semanticExpressionType.MakeByRefType();
            isRefArgument = oldIsRefArgument;
        }

        //Может быть встречан в выражениях.
        public void VisitMemberAccessNode(MemberAccessNode node)
        {

            if (node.ParentExpression != null)
            {
                throw new NotImplementedException();
                //node.ParentExpression.Visit(this);
                //semanticFindMode = FindMode.Static;
                //semanticVoidType = semanticExprType;

                //node.ChildMember.Visit(this);
                //semanticLastMember = null;
                ////   semanticExprType = ((Semantic.SemanticTree.StaticFieldNode) semanticMember).Field.FieldType;
                //semanticExpression = new Semantic.SemanticTree.MemberAccessNode(semanticMember);
                //return;
            }
                //   node.ChildMember.Visit(this);
                //  semanticExpression = new Semantic.SemanticTree.VarAccessNode(semanticCurLocal);
                
                switch (node.ChildMember.Kind)
                {
                    case MemberKind.Field:
                        {
                            var fieldNode = node.ChildMember as FieldNode;
                            if (!fieldNode.HasChild())//Если нет потомка, значит это переменная
                                {
                                    if (fieldNode.GetGenericsCount() != 0) throw new SemanticErrors.VariableCanNotHasGenerics(fieldNode.Location, fieldNode.Name.Name);
                                    LocalWrapper localWrapper = FindLocalVariable(fieldNode.ToVariable());
                                    semanticExpressionType = localWrapper.Type;
                                    semanticExpression = new Semantic.SemanticTree.VarAccessNode(localWrapper,isRefArgument);
                                }
                                else
                                {//Потомок есть, значит это цепочка её надо проанализировать
                                    semanticFindMode = FindMode.Unkown;

                                    fieldNode.Visit(this);
                            semanticExpression = new Semantic.SemanticTree.MemberAccessNode(semanticMember);
                            semanticLastMember = null;
                            semanticFindMode = FindMode.Unkown;
                            semanticMember = null;

                        }
                        return;
                        }
                    case MemberKind.Method:
                        {
                            throw new NotImplementedException();
                        }
                case MemberKind.TypeOf:
                    {
                        if (node.ParentExpression != null) throw new Exception();
                        node.ChildMember.Visit(this);
                        semanticExpression = new Semantic.SemanticTree.MemberAccessNode(semanticMember);
                        semanticFindMode = FindMode.Unkown;
                        semanticLastMember = null;
                        semanticMember = null;
                        return;
                    }
                    default:throw new PresentVariantNotImplementedException(typeof(MemberKind));
                }
            
            throw new NotImplementedException();
        }


        public void VisitIfElseNode(IfElseNode ifElseNode)
        {
            ifElseNode.Expression.Visit(this);

            if (semanticExpressionType != typeof(Boolean))
            {
                throw SemanticErrors.TypeShouldBeBool.IfKind(ifElseNode.Location, semanticExpressionType);
            }

            var expression = semanticExpression;
            var copySemanticBlock = semanticBlock;
            semanticBlock = null;

            BlockNode trueBranchBlock = ifElseNode.TrueBranch.ConvertToBlock();
            
            trueBranchBlock.Visit(this);
            var trueBlock = semanticBlock;
            semanticBlock = copySemanticBlock;

            Semantic.SemanticTree.BlockNode falseBlock = null;

            if (ifElseNode.HasFalseBranch())
            {

                BlockNode falseBranchBlock = ifElseNode.FalseBranch.ConvertToBlock();
                var copySemanticBlock2 = semanticBlock;
                semanticBlock = null;
                falseBranchBlock.Visit(this);
                falseBlock = semanticBlock;
                semanticBlock = copySemanticBlock2;
            }
            semanticBlock.Statements.Add(new PLNCompiler.Semantic.SemanticTree.IfElseNode(expression, trueBlock,falseBlock));
        }

        private int loopDeep=-1;
        public void VisitLoopBodyNode(LoopNode node)
        {
            BlockNode statementBlock = node.Statement.ConvertToBlock();
            loopDeep++;
            statementBlock.Visit(this);
            loopDeep--;
        }

        public void VisitWhileLoopNode(WhileLoopNode node)
        {
            node.Expression.Visit(this);

            if (semanticExpressionType != typeof(Boolean))
            {
                throw SemanticErrors.TypeShouldBeBool.WhileKind(node.Location, semanticExpressionType);
            }

            var expression = semanticExpression;
            var copySemanticBlock = semanticBlock;
            semanticBlock = null;

            VisitLoopBodyNode(node);

            var loopBody = semanticBlock;
            semanticBlock = copySemanticBlock;

            semanticBlock.Statements.Add(new Semantic.SemanticTree.WhileLoopNode(expression,loopBody));
        }

        public void VisitDoWhileLoopNode(DoWhileLoopNode node)
        {
            node.Expression.Visit(this);

            if (semanticExpressionType != typeof(Boolean))
            {
                throw SemanticErrors.TypeShouldBeBool.DoWhileKind(node.Location, semanticExpressionType);
            }

            var expression = semanticExpression;
            var copySemanticBlock = semanticBlock;
            semanticBlock = null;

            VisitLoopBodyNode(node);

            var loopBody = semanticBlock;
            semanticBlock = copySemanticBlock;

            semanticBlock.Statements.Add(new Semantic.SemanticTree.DoWhileLoopNode(loopBody, expression));
        }

        public void VisitBuiltInLoopNode(BuiltInLoopNode builtInLoopNode)
        {
            var block = new BlockNode(builtInLoopNode.DefineVariable, builtInLoopNode.Location);
            block.Statements.Add(new WhileLoopNode(builtInLoopNode.Expression,
                builtInLoopNode.Statement.ConvertToBlockAndAdd(builtInLoopNode.Increament),
                builtInLoopNode.Location));
            VisitStatement(block);
        }

        public void VistitBreakNode(BreakNode node)
        {
            semanticBlock.Statements.Add(new Semantic.SemanticTree.BreakNode(VisitLoopSpecialStatementNode(node)));
        }

        public void VistContinueNode(ContinueNode node)
        {
            semanticBlock.Statements.Add(new Semantic.SemanticTree.ContinueNode(VisitLoopSpecialStatementNode(node)));
        }


        public void VisitLabelDefineNode(LabelDefineNode defineLabelNode)
        {
            string name = defineLabelNode.Name.Name;

           if (labelsStack.StringExists(f=>f.Name,defineLabelNode.Name.Name))
                throw new SemanticErrors.LabelArleadyInstalled(defineLabelNode.Location, name);

            if (!waitLabels.TryRemove(name,out LabelWrapper wrapper))
                wrapper = new LabelWrapper(name);

            labelsStack.Push(wrapper);
            semanticBlock.Statements.Add(new Semantic.SemanticTree.LabelDefineNode(wrapper));

            VisitStatement(defineLabelNode.Statement);
        }



        public void VisitGOTOLabelNode(GOTOLabelNode gotoLabelNode)
        {
            string name = gotoLabelNode.Label.Name;
            LabelWrapper label = null;
            foreach (var stackLabel in labelsStack)
            {
                if (StringHelper.PLNComparer.EqualsStr(name, stackLabel.Name))
                {
                    label = stackLabel;
                    break;
                }
            }

            if (label == null)
                label = waitLabels.TryAdd(name, gotoLabelNode.Label.Location);

            semanticBlock.Statements.Add(new Semantic.SemanticTree.GOTOLabelNode(label));
        }



        public void VisitExplicitCastNode(ExplicitCastNode node)
        {
            node.Expression.Visit(this);
           Type type= TypeGenerator.GenerateType(node.Type);
            CastTypeResult castResult = TypeConversion.ExplicictCast(semanticExpressionType, type);

            if (!castResult.IsSuccessful) throw new SemanticErrors.InvalidExplicitCast(node.Location, castResult.Initiator.Type1, castResult.Initiator.Type2);
            if (castResult.NativeFunction != null)
            {
                semanticExpression = new PLNCompiler.Semantic.SemanticTree.CastTypeNode(type, semanticExpression,castResult.NativeFunction);
            }
            semanticExpressionType = type;
        }



        public void VisitIsCastNode(IsCastNode node)
        {
            node.Expression.Visit(this);
            Type type = TypeGenerator.GenerateType(node.Type);
            IsCastResult castResult = TypeConversion.IsCast(semanticExpressionType, type);

            if (!castResult.IsSuccessful)
                throw SemanticErrors.Get(node.Location, castResult);

            if (castResult.NativeFunction != null)
            {
                semanticExpression = new PLNCompiler.Semantic.SemanticTree.IsCastTypeNode(type, semanticExpression, castResult.NativeFunction);
            }

            semanticExpressionType = typeof(Boolean);
        }




        public void VisitAsCastNode(AsCastNode node)
        {
            node.Expression.Visit(this);
            Type type = TypeGenerator.GenerateType(node.Type);
            AsCastResult castResult = TypeConversion.AsCast(semanticExpressionType, type);

            if (!castResult.IsSuccessful)
                throw SemanticErrors.Get(node.Location, castResult);

            if (castResult.NativeFunction != null)
            {
                semanticExpression = new PLNCompiler.Semantic.SemanticTree.AsCastTypeNode(type, semanticExpression, castResult.NativeFunction);
            }

            semanticExpressionType = type;
        }



        public void VisitOneLineCommentNode(OneLineCommentNode oneLineCommentNode)
        {
        }

  
         
        ///
        //**************************************************
        //__________________________________________________
        //                  HELP METHODS
        //__________________________________________________

        //**************************************************
        #region Help Methods

        private ProcessStaticFieldResult ProcessStaticMembers(Type type, MemberNode member)
        {
           // Type constructoredType = type;
            Type[] genericArguments = null;
            if (type.IsConstructedGenericType)
            {
                genericArguments = type.GetGenericArguments();
               // type = type.GetGenericTypeDefinition();
            }

            SelectStringMemberResult selectResult = null;
            Location lastLocation = member.Location;
            MemberInfo memberInfo = null;
            do
            {
                if (member == null)
                {
                   // if (!genericArguments.IsNullOrEmpty()) type = type.MakeGenericType(genericArguments);
                    throw new SemanticErrors.TypeCannotBeInExpression(lastLocation, type);

                }
                if (member.Kind == MemberKind.Method || (member.Kind == MemberKind.Field && member.GetIndexsCount() != 0)) break;

                selectResult = MemberSelector.SelectStringMember(type, new StringMemberHeader(member.Name.Name, MemberFlags.Static)).ExclusiveWithNetComparer();
                if (!selectResult.IsSuccessful) break;
                memberInfo = selectResult.FoundResult;
                lastLocation = member.Location;
                member = member.Child;

                if (memberInfo.MemberType != MemberTypes.NestedType) break;

                type = (Type)memberInfo;
                if (!genericArguments.IsNullOrEmpty())
                    type = type.MakeGenericType(genericArguments);
            } while (true);

          //  if (!genericArguments.IsNullOrEmpty())
           //     type = type.MakeGenericType(genericArguments);


            return new ProcessStaticFieldResult(type, member, selectResult, lastLocation);//type
            //if (selectResult != null)
            //{
            //    if (!selectResult.IsSuccessful)
            //    {
            //        throw SemanticErrors.Get(lastLocation, selectResult, type);
            //    }
            //    //MemberTypes.Field | MemberTypes.Event | MemberTypes.Property
            //    if (selectResult.FoundResult.MemberType != MemberTypes.Field) throw new NotImplementedException();
            //    FieldInfo fieldInfo = (FieldInfo)selectResult.FoundResult;
            //    semanticMember = new Semantic.SemanticTree.StaticFieldNode(fieldInfo);
            //}

            // return member;
        }


        private ProcessStaticFieldResult ProcessStaticField(FieldNode node)
        {
            TypeNameNode typeName = new TypeNameNode(node.Name, node.Generics, node.Location);
            TypeNameNode typeNameChild = typeName;

            bool isf = true;
            bool hasGenerics = node.GetGenericsCount() != 0;

            TypeNameNode tmpName = new TypeNameNode(node.Name, node.Generics, node.Location);
            TypeNameNode tmpNameChild = tmpName;

            NameManager simpleName = new NameManager { node.Name.Name };

            MemberNode nodeСhild = node.Child;
            bool hasIndexes = false;
            MemberNode lastChild = node;

            while (nodeСhild != null && nodeСhild.Kind == MemberKind.Field)
            {
                tmpNameChild.Child = new TypeNameNode(nodeСhild.Name, nodeСhild.Generics, nodeСhild.Location);
                tmpNameChild = tmpNameChild.Child;

                if (nodeСhild.GetIndexsCount() != 0)
                {
                    hasIndexes = true;
                    break;
                }


                if (nodeСhild.GetGenericsCount() != 0)
                {
                    if (isf)
                    {
                        typeName = tmpName;
                        typeNameChild = typeName;
                        isf = false;
                    }
                    else
                    {
                        typeNameChild.Child = tmpName;
                        typeNameChild = typeNameChild.Child;
                    }
                    tmpName = new TypeNameNode(nodeСhild.Name, nodeСhild.Generics, nodeСhild.Location);
                    tmpNameChild = tmpName;
                    simpleName = null;
                    hasGenerics = true;
                    lastChild = nodeСhild;
                }

                if (simpleName != null) simpleName.Add(nodeСhild.Name.Name);

                nodeСhild = nodeСhild.Child;

            }


            if (hasGenerics)
            {
                lastChild = lastChild.Child;
                Type generatedType = TypeGenerator.GenerateType(typeName);
                if (lastChild == null)
                {
                    throw new SemanticErrors.TypeCannotBeInExpression(typeName.Location, generatedType);
                }

                return ProcessStaticMembers(generatedType, lastChild);

            }
            else
            {
                SearchingTypeResult findTypeResult = TypeGenerator.TypeFinder.FindType(new TypeName(simpleName, 0)).ExclusiveWithNetComparer();

                if (findTypeResult.Result == Compile.SearchingResults.OK)
                {
                    TypeAssociation generatedType = findTypeResult.Candidates.First();
                    for (int i = 0; i < generatedType.Name.Count; i++)
                        lastChild = lastChild.Child;

                    if (lastChild == null) throw new SemanticErrors.TypeCannotBeInExpression(typeName.Location, findTypeResult.FoundResult.Type);

                    return ProcessStaticMembers(generatedType.Type, lastChild); ;
                }
                else
                    throw SemanticErrors.Get(typeName.Location, findTypeResult);
            }
        }


        private void MakeParametersConvension(MethodBase method, IEnumerable<TypedExpression> arguments)
        {
            var methodPrms = method.GetParameters();
            if (!methodPrms.IsNullOrEmpty())
            {
                int i = 0;
                foreach (var argument in arguments)
                {
                    var castResult = TypeConversion.ImplicitCast(argument.Type, methodPrms[i].ParameterType);
                    if (!castResult.IsSuccessful)
                        throw new SemanticErrors.InvalidImplicitCast(new Location(), argument.Type, methodPrms[i].ParameterType);
                    NativeFunction nativeFunction = castResult.NativeFunction;
                  //  NativeFunction nativeFunction = TypeConversion.ImplicitCast(argument.Type, methodPrms[i].ParameterType).NativeFunction;
                    if (nativeFunction != null)
                        argument.Expression.NativeFunctions.Add(nativeFunction);
                    i++;
                }
            }
        }


        private ProcessMethodFactArgumentsResult ProcessMethodFactArguments(ICollection<FactArgumentNode> nodeArguments)
        {
            Type[] argumentTypes = Type.EmptyTypes;
            TypedExpression[] arguments = null;

            if (!nodeArguments.IsNullOrEmpty())
            {
                arguments = new TypedExpression[nodeArguments.Count];
                argumentTypes = new Type[nodeArguments.Count];
                int i = 0;
                foreach (var argument in nodeArguments)
                {
                    var oldSemanticLastMember = semanticLastMember;
                    var oldSemanticMember = semanticMember;
                    var oldSemanticExpressionType = semanticExpressionType;
                    var oldSemanticFindMode = semanticFindMode;

                    semanticLastMember = null;
                    semanticMember = null;
                    semanticExpressionType = null;
                    semanticFindMode = FindMode.Unkown;

                    argument.Visit(this);

                   
                    semanticLastMember = oldSemanticLastMember;
                    semanticMember = oldSemanticMember;
                    arguments[i] = new TypedExpression(semanticExpression, semanticExpressionType);
                    argumentTypes[i] = semanticExpressionType;
                    semanticExpressionType = oldSemanticExpressionType;
                    semanticFindMode = oldSemanticFindMode;


                    i++;
                }
            }
            return new ProcessMethodFactArgumentsResult(argumentTypes, arguments);
        }


        public Int32 VisitLoopSpecialStatementNode(LoopSpecialStatementNode node)
        {
            if (loopDeep == -1) throw new SemanticErrors.LoopSpecialStatementOutLoop(node.Location);
            switch (node.Kind)
            {
                case LoopSpecialStatementNode.ConstKind.HasConstant:
                    {
                        Int32 parentLevel = 0;
                        Location parentLevelLocation = node.ConstNode.Location;
                        TypedConstant typedConstant = ParseConstant(node.ConstNode).Constant.ToTypedConstant();
                        if (typedConstant.Code != TypeCode.Int32) throw new SemanticErrors.LoopSpecialNumberShouldBeInt32(typedConstant, parentLevelLocation);
                        parentLevel = (Int32)typedConstant.Value;
                        if (loopDeep < parentLevel) throw new SemanticErrors.LoopSpecialNumberDeepError(parentLevel, loopDeep, parentLevelLocation);
                        return parentLevel;
                    }
                case LoopSpecialStatementNode.ConstKind.ApplyMax: return loopDeep;
                case LoopSpecialStatementNode.ConstKind.ApplyNear: return 0;
                default: throw new PresentVariantNotImplementedException(typeof(LoopSpecialStatementNode.ConstKind));
            }
        }


        private LocalWrapper FindLocalVariable(IdentiferNode name,bool callExceptionIfNodeFound=true)
        {
            LocalWrapper localWrapper = null;
            foreach (var variable in variablesStack)
            {
                if (StringHelper.PLNComparer.EqualsStr(variable.Name, name.Name))
                {
                    localWrapper = variable;
                    break;
                }
            }
            semanticCurLocal = localWrapper;
            if (callExceptionIfNodeFound && ReferenceEquals(localWrapper, null)) throw new SemanticErrors.VariableNotFound(name.Location, name.Name);
            return localWrapper;
        }
#endregion



        public Semantic.SemanticTree.SemanticMain GenerateSemanticTree(StatementNode node)
        {
            Reset();
            node.Visit(this);
            return new Semantic.SemanticTree.SemanticMain(DirectiveInfo, semanticBlock);
        }






        public PLNTreeConverter(DirectiveInfo directiveInfo, ITypeGenerator typeGenerator, ITypeConversion typeConversion,IMemberSelector memberSelector, IExpressionOptimizer expressionOptimizer)
        {
            DirectiveInfo = directiveInfo;
            variablesStack = new Stack<LocalWrapper>();
            labelsStack = new Stack<LabelWrapper>();
            waitLabels = new WaitLabelsDictionary();
            TypeGenerator = typeGenerator;
            TypeConversion = typeConversion;
            MemberSelector = memberSelector;
            ExpressionOptimizer = expressionOptimizer;
            Reset();
        }

        //public PLNTreeConverter(ProgramNode programNode):this()
        //{
        //}

        public static PLNTreeConverter FromProgramNode(ProgramNode programNode,string compilationPath)
        {
            DirectiveInfo directiveInfo = (new PLNDirectiveGenerator()).GenerateInfo(programNode.DirectiveSection,compilationPath);
            return new PLNTreeConverter(directiveInfo,
                new PLNTypeGenerator(programNode,directiveInfo),new PLNTypeConversion(),
                new PLNMemberSelector(null), new PLNExpressionOptimizer());
        }

        public void Reset()
        {
            variablesStack.Clear();
            labelsStack.Clear();
            waitLabels.Clear();
            semanticBlock = null;
            semanticExpression = null;
            //semanticVoidType = null;
            semanticExpressionType = null;
            semanticCurLocal = null;
            semanticMember = null;
            semanticLastMember = null;
            semanticFindMode = FindMode.Unkown;
        }
        public DirectiveInfo DirectiveInfo { get; private set; }
        public ITypeGenerator TypeGenerator { get; private set; }
        public ITypeConversion TypeConversion { get; private set; }
        public IMemberSelector MemberSelector { get; private set; }
        public IExpressionOptimizer ExpressionOptimizer { get; private set; }

        private Semantic.SemanticTree.BlockNode semanticBlock;
        private Semantic.SemanticTree.ExpressionNode semanticExpression;
        private Type semanticExpressionType;
        private Stack<LocalWrapper> variablesStack;

       // private Type semanticVoidType;
        private FindMode semanticFindMode;
        private Semantic.SemanticTree.RootMemberNode semanticMember;
        private Semantic.SemanticTree.InstanceMemberNode semanticLastMember;
        private LocalWrapper semanticCurLocal;

#region Fields for goto and labels
        private Stack<LabelWrapper> labelsStack;
        private WaitLabelsDictionary waitLabels;
#endregion

        private struct SemanticChain
        {
            public Type CurType;
            public FindMode FindMode;
        }

        private struct ProcessStaticFieldResult
        {
            public readonly Type ResultType;
            public readonly MemberNode StayMembers;
            public readonly SelectStringMemberResult LastSelectResult;
            public readonly Location LastLocation;

            public ProcessStaticFieldResult(Type resultType, MemberNode stayMembers, SelectStringMemberResult lastSelectResult, Location lastLocation)
            {
                ResultType = resultType;
                StayMembers = stayMembers;
                LastSelectResult = lastSelectResult;
                LastLocation = lastLocation;
            }
        }

        private struct ProcessMethodFactArgumentsResult
        {
           public readonly Type[] ArgumentTypes;
           public readonly IReadOnlyCollection<TypedExpression> Arguments;

            public ProcessMethodFactArgumentsResult(Type[] argumentTypes, IReadOnlyCollection<TypedExpression> arguments)
            {
                ArgumentTypes = argumentTypes;
                Arguments = arguments;
            }
        }

        private enum FindMode {Unkown, Static,Instance }

        public void VisitConstantNode(ConstNode node)
        {
            VisitParsableConstantNode(ParseConstant(node));
        }


        public static ParsableConstNode ParseConstant(ConstNode node)
        {
            var parseResult = StringHelper.PLNConstantParser.Parse(node.Constant);
            if (!parseResult.IsSuccessful)
                throw SemanticErrors.Get(node.Location, parseResult);
            return new ParsableConstNode(parseResult.Constant, node.Location);
        }

      
    }

}
