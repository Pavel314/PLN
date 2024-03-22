using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Syntax;
using PLNCompiler.Syntax.Optimization;
using PLNCompiler.Compile.PLNReflection;
using PLNCompiler.Compile.PLNReflection.TypeFinder;
using PLNCompiler.Compile.PLNReflection.AssemblyLoader;
using PLNCompiler.Compile.TypeManager;
using PLNCompiler.Compile.PLNReflection.MemberSelector;

namespace PLNCompiler.Compile.Errors
{
    //!TODO Errors Code

    public static class SemanticErrors
    {
        #region TypeErrors
        public class TypeNotFound : SemanticError
        {
            public TypeName NotFoundType { get; private set; }
            public TypeNotFound(Location location, TypeName notFoundType) :
                base(0, location, String.Format("Тип {0} не найден(Не подключено нужное пространство имён?)", notFoundType))
            {
                NotFoundType = notFoundType;
            }
        }

        public class NestedTypeNotFound : SemanticError
        {
            public Type ParentType { get; private set; }
            public TypeName NotFoundType { get; private set; }

            public NestedTypeNotFound(Location location, Type parenType, TypeName notFoundType) :
                base(0, location, String.Format("Вложенный тип {0} внутри типа {1} не найден", notFoundType, parenType))
            {
                ParentType = parenType;
                NotFoundType = notFoundType;
            }
        }

        public class TypeSuspense : SemanticError
        {
            public IReadOnlyList<Type> Types { get; private set; }
            public TypeSuspense(Location location, IReadOnlyList<Type> types) :
                base(1, location, string.Format("Неопределённость между типами: {0} (Попробуйте использовать полное имя и точный регистр)", types.Interlock(f => f.FullName)))
            { Types = types; }
        }

        public class TypeWithMember : SemanticError
        {
            public Type CorrectType { get; private set; }
            public TypeName FindType { get; private set; }

            public TypeWithMember(Location location, Type correctType, TypeName findType) :
                base(2, location, String.Format("Данный объект не является типом: {0} (Имелось ввиду: {1} ?)", findType, correctType))
            {
                CorrectType = correctType;
                FindType = findType;
            }
        }


        public abstract class BinaryOperatorError : SemanticError
        {
            public TypeManager.BinaryInitiator Initiator { get; private set; }
            public BinaryOperatorError(Location location, int code, TypeManager.BinaryInitiator initiator, string message) : base(code, location, message)
            {
                Initiator = initiator;
            }
        }

        public class CanNotFindGeneralType : BinaryOperatorError
        {
            public CanNotFindGeneralType(Location location, TypeManager.BinaryInitiator initiator) :
                base(location, 0, initiator, string.Format("Не удалось найти общий тип для выполнянеия бинирной операции: {0}, для типов: {1} и {2}", initiator.Operation, StringHelper.PLNTypeToString.ToString(initiator.Left.Type), StringHelper.PLNTypeToString.ToString(initiator.Right.Type)))
            {
            }
        }

        public class BinaryOperationCanNotBeApplied : BinaryOperatorError
        {
            public BinaryOperationCanNotBeApplied(Location location, TypeManager.BinaryInitiator initiator) :
                base(location, 0, initiator, string.Format("Бинарная операция: {0} неприменима для типов: {1} и {2}", initiator.Operation, 
                    StringHelper.PLNTypeToString.ToString(initiator.Left.Type) , StringHelper.PLNTypeToString.ToString(initiator.Right.Type)))
            {

            }
        }

        public class UnaryOperationCanNotBeApplied : SemanticError
        {
            public TypeManager.UnaryInitiator Initiator { get; private set; }
            public UnaryOperationCanNotBeApplied(Location location, TypeManager.UnaryInitiator initiator) :
                base(0, location, string.Format("Унарная операция: {0} неприменима для типа: {1}", initiator.Operation, initiator.Right.Type))
            {
                Initiator = initiator;
            }
        }

        public class TypeCannotBeInExpression : SemanticError
        {
            public Type Type { get; private set; }
            public TypeCannotBeInExpression(Location location, Type type) :
                base(0, location, String.Format("Тип({0}) не может участвовать в выражении", type))
            {
                Type = type;
            }
        }

        public class StaticIndexersAreImpossible : SemanticError
        {
            public Type Type { get; private set; }
            public StaticIndexersAreImpossible(Location location, Type type) :
                base(0, location, String.Format("Статические индексы невозможны(Тип: {0})", type))
            {
                Type = type;
            }
        }

        public enum TypeShouldBeBoolKind {If,While,DoWhile }

        public class TypeShouldBeBool : SemanticError
        {
            public Type Type { get; private set; }
            public TypeShouldBeBoolKind Kind { get; private set; }
            private TypeShouldBeBool(Location location, Type type, TypeShouldBeBoolKind kind) : base(0, location, GenerateErrorString(type,kind))
            {
                Type = type;
            }

            public static TypeShouldBeBool IfKind(Location location, Type type) => new TypeShouldBeBool(location, type, TypeShouldBeBoolKind.If);
            public static TypeShouldBeBool WhileKind(Location location, Type type) => new TypeShouldBeBool(location, type, TypeShouldBeBoolKind.While);
            public static TypeShouldBeBool DoWhileKind(Location location, Type type) => new TypeShouldBeBool(location, type, TypeShouldBeBoolKind.DoWhile);

            private static string GenerateErrorString(Type type, TypeShouldBeBoolKind kind)
            {
                string name = null;
                switch (kind)
                {
                    case TypeShouldBeBoolKind.While: case TypeShouldBeBoolKind.DoWhile: name = "данного цикла"; break;
                    case TypeShouldBeBoolKind.If: name = "условного оператора"; break;
                    default: throw new PresentVariantNotImplementedException(typeof(TypeShouldBeBoolKind));
                }

                return String.Format("Тип данных для: {0} должен быть логический(Встречен: {1})", name,StringHelper.PLNTypeToString.ToString(type));
            }
        }

        public class LabelArleadyInstalled:SemanticError
        {
            public string Name { get; private set; }

            public LabelArleadyInstalled(Location location,string name):
                base(0,location,String.Format("Метка: {0} уже установлена",name))
            {
                Name = name;
            }
        }

        public class LabelsNotFound:SemanticError
        {
            public IReadOnlyDictionary<string,IReadOnlyList<Location>> Labels { get; private set; }

            public LabelsNotFound(Location location, IReadOnlyDictionary<string, IReadOnlyList<Location>> labels):base(0,location,GenerateErrorString(labels))
            {

            }
            public LabelsNotFound(IReadOnlyDictionary<string, IReadOnlyList<Location>> labels) : this(labels[labels.Keys.First()][0], labels)
            {

            }
            private static string GenerateErrorString(IReadOnlyDictionary<string, IReadOnlyList<Location>> labels)
            {
                var keys = labels.Keys;
                if (labels.Count == 1)
                {
                        return string.Format("В текущем контексте метка: {0} не найдена", keys.First());
                }
                var builder = new StringBuilder();
                builder.Append("В текущем контексте используемые метки:");
                foreach (var label in keys)
                {
                    builder.AppendFormat(" {0},", label);
                }
                builder.Remove(builder.Length - 1, 1);
                builder.Append(" не найдены");
                return builder.ToString();
            }
        }

        public class LoopSpecialNumberShouldBeInt32 : SemanticError
        {
            public Semantic.TypedConstant Value { get; private set; }
            public LoopSpecialNumberShouldBeInt32(Semantic.TypedConstant value,Location location):
                base(0,location,string.Format("Не удалось распознать число как тип Int32(Встречено: {0})",value.Value))
            {
                Value = value;
            }
        }

        public class LoopSpecialNumberDeepError:SemanticError
        {
            public Int32 ParentLevel { get; private set; }
            public Int32 MaxParentLevel { get; private set; }

            public LoopSpecialNumberDeepError(Int32 parentLevel, Int32 maxParentLevel,Location location):
                base(0,location,string.Format("Не существует родительского цикла на уровни: {0}(Максимальный уровень: {1})", parentLevel,maxParentLevel))
            {
                MaxParentLevel = maxParentLevel;
                ParentLevel = parentLevel;
            }
        }

        public class LoopSpecialStatementOutLoop : SemanticError
        {
            public LoopSpecialStatementOutLoop(Location location) : base(0, location,"Операторы влияющие на ход выполнения цикла, должны находится внутри цикла")
            {
            }
        }

        public abstract class UnsuitableRefArgumentError : SemanticError
        {
            public UnsuitableRefArgumentError(int errorCode, Location location, string message) : base(errorCode, location, message)
            {
            }
        }

        public abstract class FieldCanNotBePassedByRef: UnsuitableRefArgumentError
        {
            public FieldInfo Field { get; private set; }
            public FieldCanNotBePassedByRef(int errorCode,Location location,FieldInfo field, string message) : base(errorCode, location, message)
            {
                Field = field;
            }

        }

        public class ConstantCanNotBePassedByRef: FieldCanNotBePassedByRef
        {
            public ConstantCanNotBePassedByRef(Location location, FieldInfo field):
                base(0,location,field,string.Format("Константное поле: {0} не может быть передано по ссылке", field.Name))
            {

            }
        }

        public class ReadOnlyFieldCanNotBePassedByRef : FieldCanNotBePassedByRef
        {
            public ReadOnlyFieldCanNotBePassedByRef(Location location, FieldInfo field):
                base(0,location,field,string.Format("Доступное поле только на чтение: {0} не может быть передано по ссылке",field.Name))
            {

            }
        }

        public class PropertyCanNotBePassedByRef : UnsuitableRefArgumentError
        {
            public PropertyInfo Property { get; private set; }

            public PropertyCanNotBePassedByRef(Location location, PropertyInfo property) : 
                base(0, location,  string.Format("Свойство: {0} не может быть передано по ссылке",property.Name ))
            {
                Property = property;
            }
        }


        #endregion







        #region Member Not Found
        public abstract class MemberNotFound : SemanticError
        {
            public Type Type { get; private set; }

            public MemberNotFound(int code,Location location,Type type, string message) : base(code,location, message)
            {
                Type = type;
            }
        }

        public abstract class MethodNotFound : MemberNotFound
        {
            public string Name { get; private set; }
            public IReadOnlyList<Type> Arguments { get; private set; }
            public MethodNotFound(Location location, Type type, string name, IReadOnlyList<Type> arguments,string message):base(0,location,type,message)
            {
                Name = name;
                Arguments = arguments;
            }

            protected static string GenerateArgumentString(IReadOnlyList<Type> arguments)
            {
                if (arguments.IsNullOrEmpty()) return null;
                var builder = new StringBuilder();
                foreach (var argument in arguments)
                {
                    builder.Append(argument.ToString() + ",");
                }
                return builder.Remove(builder.Length - 1, 1).ToString();
            }
        }

        public class InstanceMethodNotFound : MethodNotFound
        {
            public InstanceMethodNotFound(Location location, Type type, string name, IReadOnlyList<Type> arguments) : 
                base(location, type, name, arguments, 
                    String.Format("Не удалось найти метод {0}({1}) внутри типа {2}",
                        name,GenerateArgumentString(arguments),type))
            {
            }
        }

        public class StaticMethodNotFound : MethodNotFound
        {
            public StaticMethodNotFound(Location location, Type type, string name, IReadOnlyList<Type> arguments) :
                base(location, type, name, arguments,
                    String.Format("Не удалось найти статический метод {0}({1}) внутри типа {2}",
                        name, GenerateArgumentString(arguments), type))
            {
            }

            protected StaticMethodNotFound(Location location, Type type, string name, IReadOnlyList<Type> arguments, string message) :base(location,type,name,arguments,message)
            {
            }

        }


        public class ConstructorNotFound : StaticMethodNotFound
        {
            public ConstructorNotFound(Location location, Type type, IReadOnlyList<Type> arguments) : 
                base(location, type, ConstructorInfo.ConstructorName, arguments,
                    String.Format("Не удалось найти конструктор ({0}) внутри типа {1}",
                        GenerateArgumentString(arguments), type))
            {
            }
        }

        public abstract class StringMemberNotFound : MemberNotFound
        {
            public IMemberHeader Header { get; private set; }

            public StringMemberNotFound(int code, Location location, Type type, IMemberHeader header,string message):base(code,location,type,message)
            {
                Header = header;
            }
        }

        public class StaticStringMemberNotFound : StringMemberNotFound
        {
            public StaticStringMemberNotFound(Location location, Type type, IMemberHeader header) :
                base(0, location, type, header, String.Format("Статический член: {0} внутри типа: {1} не найден", header.Name, type))
            {

            }
        }

        public class InstanceStringMemberNotFound : StringMemberNotFound
        {
            public InstanceStringMemberNotFound(Location location, Type type, IMemberHeader header) :
                base(0, location, type, header, String.Format("Статический член: {0} внутри типа: {1} не найден", header.Name, type))
            {

            }
        }

        #endregion







        #region DirectiveErrors
        public abstract class DirectiveError : SemanticError
        {
            public Syntax.SyntaxTree.DirectiveNode DirectiveNode { get; private set; }

            public DirectiveError(int code, Syntax.SyntaxTree.DirectiveNode directiveNode, string message) : base(code, directiveNode.Location, message)
            {
                DirectiveNode = directiveNode;
            }
        }

        public class DirectiveArleadyUsing: DirectiveError
        {
            public DirectiveArleadyUsing(Syntax.SyntaxTree.DirectiveNode directiveNode) : 
                base(0,directiveNode,string.Format("Директива: {0}, уже упомянута", directiveNode.Spelling))
            {
              
            }
        }
        #endregion







        
        #region CastErrors
        public abstract class InvalidCast : SemanticError
        {
            public Type PrimaryType { get; private set; }
            public Type TargetType { get; private set; }

            public InvalidCast(Location location, int code, Type primaryType, Type targetType, string message) : base(code, location, message)
            {
                PrimaryType = primaryType;
                TargetType = targetType;
            }
        }

        public class InvalidImplicitCast : InvalidCast
        {
            public InvalidImplicitCast(Location location, Type primaryType, Type targetType) :
                base(location, 0, primaryType, targetType, string.Format("Не удалось выполнить неявное преобразование от типа: {0} к целевому типу: {1}", StringHelper.PLNTypeToString.ToString(primaryType), StringHelper.PLNTypeToString.ToString(targetType)))
            {
            }
        }

        public class InvalidExplicitCast : InvalidCast
        {
            public InvalidExplicitCast(Location location, Type primaryType, Type targetType) :
                base(location, 0, primaryType, targetType, string.Format("Не удалось выполнить явное преобразование от типа: {0} к целевому типу: {1}", StringHelper.PLNTypeToString.ToString(primaryType), StringHelper.PLNTypeToString.ToString(targetType)))
            {
            }
        }

        //public class InvalidExplicictCast : InvalidCast
        //{
        //    public InvalidExplicictCast(Location location, Type primaryType, Type targetType) :
        //        base(location, 0, primaryType, targetType, string.Format("Не удалось выполнить явное преобразование от типа: {0} к целевлому типу: {1}", StringHelper.PLNTypeToString.ToString(primaryType), StringHelper.PLNTypeToString.ToString(targetType)))
        //    {
        //    }
        //}



        public class InvalidIsCastType : InvalidCast
        {
            public InvalidIsCastType(Location location, Type primaryType, Type targetType) :
                base(location, 0, primaryType, targetType, string.Format("Некорректная корреляция между типами: {0} и {1}, для выполнения операции безопасного приведения", StringHelper.PLNTypeToString.ToString(primaryType), StringHelper.PLNTypeToString.ToString(targetType)))
            {
            }
        }

        public class InvalidAsCastTypeBase : InvalidCast
        {
            public InvalidAsCastTypeBase(Location location, int code, Type primaryType, Type targetType, string message) : base(location, code, primaryType, targetType, message)
            {

            }
        }

        public class InvalidAsCastType : InvalidAsCastTypeBase
        {
            public InvalidAsCastType(Location location, Type primaryType, Type targetType) :
                base(location, 0, primaryType, targetType, string.Format("Некорректная корреляция между типами: {0} и {1}, для выполнения операции безопасного приведения", StringHelper.PLNTypeToString.ToString(primaryType), StringHelper.PLNTypeToString.ToString(targetType)))
            {
            }
        }

        public class InvalidAsTargetType : InvalidAsCastTypeBase
        {
            public InvalidAsTargetType(Location location, Type primaryType, Type targetType) :
                base(location, 0, primaryType, targetType, string.Format("Для выполнения операции безопасного приведения, тип: {0} должен быть ссылочным", StringHelper.PLNTypeToString.ToString(targetType)))
            {
            }
        }
        #endregion










        #region ConstantErrors
        public class ConstantNotBeParsed : SemanticError
        {
            public ParseConstantResults Results { get; private set; }
            public string Constant { get; private set; }

            public ConstantNotBeParsed(Location location, ParseConstantResults results, string constant) : base(0, location, GenerateErrorString(results, constant))
            {
                Results = results;
                Constant = constant;
            }

            private static string GenerateErrorString(ParseConstantResults results, string constant)
            {
                switch (results)
                {
                    case ParseConstantResults.IntError: return string.Format("Целое число: {0} не было распознано", constant);
                    case ParseConstantResults.FloatError: return string.Format("Вещественное число: {0} не было распознано", constant);
                    default: throw new PresentVariantNotImplementedException(typeof(ConstKind));
                }
            }
        }

        public class ConstantCanNotAssign : SemanticError
        {
            public ParsableConstant Constant { get; private set; }
            public Type TargetType { get; private set; }

            public ConstantCanNotAssign(Location location, ParsableConstant constant, Type targetType) : base(0, location, GenerateErrorString(constant, targetType))
            {
                Constant = constant;
                TargetType = targetType;
            }

            private static string GenerateErrorString(ParsableConstant constant, Type targetType)
            {
                if (constant.Kind == ParsableConstantKind.Null)
                    return string.Format("Не удалось преобразовать нулевою ссылку к типы: {0}", targetType);
                var value = constant.Value;
                if (constant.Kind == ParsableConstantKind.Int64)
                {
                    var i64 = (Int64)value;
                    if (i64 >= Int32.MinValue && i64 <= Int32.MaxValue)
                        value = (Int32)i64;
                }
                return string.Format("Не удалось преобразовать константу: {0} типа {1} к целевому типы: {2}", value, value.GetType(), targetType);
            }
        }


        public enum OperationErrorKind { InvalidOperation, Overflow }

        public abstract class ConstantOpperationError:SemanticError
        {
            public OperationErrorKind ErrorKind { get; private set; }
            public ConstantOpperationError(int errorCode, Location location, string message,OperationErrorKind errorKind) : base(errorCode, location,message)
            {
                ErrorKind = errorKind;
            }

            public static OperationErrorKind InterpreterResults2OperationErrorKind(ConstantInterpreterResults result)
            {
                switch (result)
                {
                    case ConstantInterpreterResults.InvalidOperation:return OperationErrorKind.InvalidOperation;
                    case ConstantInterpreterResults.OverflowError:return OperationErrorKind.Overflow;
                    default:throw new ArgumentException();
                }
            }

        }

        public class ConstantUnaryOpperationError : ConstantOpperationError
        {
            public Syntax.Optimization.UnaryInitiator Initiator { get; private set; }

            public ConstantUnaryOpperationError(Location location, Syntax.Optimization.UnaryInitiator initiator, OperationErrorKind errorKind) : base(0, location, GenerateErrorString(initiator, errorKind), errorKind)
            {
                Initiator = initiator;
            }

            private static string GenerateErrorString(Syntax.Optimization.UnaryInitiator initiator, OperationErrorKind errorKind)
            {
                switch (errorKind)
                {
                    case OperationErrorKind.InvalidOperation:
                        return string.Format("Невозможно применить унарную операцию: {0} для константы: {1}",initiator.Operation, initiator.Right.Value);
                    case OperationErrorKind.Overflow:
                        return string.Format("При выполнение унарный операции: {0} для константы {1} произошло переполнение",initiator.Operation, initiator.Right.Value);
                    default: throw new PresentVariantNotImplementedException(typeof(OperationErrorKind));
                }
            }
        }

        public class ConstantBinaryOpperationError : ConstantOpperationError
        {
            public Syntax.Optimization.BinaryInitiator Initiator { get; private set; }

            public ConstantBinaryOpperationError(Location location, Syntax.Optimization.BinaryInitiator initiator, OperationErrorKind errorKind):base(0,location, GenerateErrorString(initiator,errorKind),errorKind)
            {
                Initiator = initiator;
            }

            private static string GenerateErrorString(Syntax.Optimization.BinaryInitiator initiator, OperationErrorKind errorKind)
            {
                switch (errorKind)
                {
                    case OperationErrorKind.InvalidOperation: return string.Format("Невозможно применить бинарную операцию: {0} для констант: {1} и {2}",
                        initiator.Operation,initiator.Left.Value,initiator.Right.Value);
                    case OperationErrorKind.Overflow: return string.Format("При выполнение бинарный операции: {0} для констант: {1} и {2} произошло переполнение",
                        initiator.Operation, initiator.Left.Value, initiator.Right.Value);
                    default: throw new PresentVariantNotImplementedException(typeof(OperationErrorKind));
                }
            }
        }

    
        #endregion








        #region VariableErrors
        public class VariableNotFound : SemanticError
        {
            public string VariableName;

            public VariableNotFound(Location location, string variableName) : base(0, location,
                String.Format("В текущем контексте переменная: {0} не существует", variableName))
            {
                VariableName = variableName;
            }
        }

        public class VariableCanNotHasGenerics : SemanticError
        {
            public string VariableName;

            public VariableCanNotHasGenerics(Location location, string variableName) : base(0, location,
                String.Format("Переменная не может иметь универсальные параметры(Имя:{0})", variableName))
            {
                VariableName = variableName;
            }
        }

        public class VariableArleadyDefine:SemanticError
        {
            public string VariableName;

            public VariableArleadyDefine(Location location, string variableName) : base(0, location,
                String.Format("В текущем контексте переменная: {0} уже определена", variableName))
            {
                VariableName = variableName;
            }
        }

        #endregion








        #region NamespaceErrors
        public class NamespaceNotFound : SemanticError
        {
            public NameManagerReadOnly NotFoundNamespace { get; private set; }
            public NamespaceNotFound(Location location, NameManagerReadOnly notFoundNamespace) :
                base(3, location, String.Format("Пространство имён {0} не найдено", notFoundNamespace))
            {
                NotFoundNamespace = notFoundNamespace;
            }
        }

        public class NamespaceSuspense : SemanticError
        {
            public IReadOnlyList<NameManagerReadOnly> Namespaces { get; private set; }
            public NamespaceSuspense(Location location, IReadOnlyList<NameManagerReadOnly> namemspaces) :
                base(4, location, string.Format("Неопределённость между пространствами имён: {0}(Попробуйте использовать точный регистр)", namemspaces.Interlock(f => f.ToString())))
            { Namespaces = namemspaces; }
        }

        public class NamespaceWithMember : SemanticError
        {
            public NameManagerReadOnly CorrectNamespace { get; private set; }
            public NameManagerReadOnly FindNamespace { get; private set; }

            public NamespaceWithMember(Location location, NameManagerReadOnly correctNamespace, NameManagerReadOnly findNamespace) :
                base(5, location, String.Format("Данный объект не является пространством имён: {0}(Имелось ввиду: {1} ?)",findNamespace,correctNamespace))
            {
                CorrectNamespace = correctNamespace;
                FindNamespace = findNamespace;
            }
        }

        public class NamespaceAlreadyUsing:SemanticError
        {
            public NameManagerReadOnly Namespace { get; private set; }

            public NamespaceAlreadyUsing(Location location,NameManagerReadOnly namespace_):
                base(6,location,String.Format("Пространство имен {0} уже подключено",namespace_))
            {
                Namespace = namespace_;
            }
        }
        #endregion








        #region GenericErrors
        public class GenericArgumentNotVerify : SemanticError
        {
            public IReadOnlyList<GenericArgumentError> ErrorArguments { get; private set; }
            public Type Initiator { get; private set; }
            public GenericArgumentNotVerify(Location location, Type initiator, IReadOnlyList<GenericArgumentError> errorArguments):base(7,location,GenerateErrorString(initiator,errorArguments))
            {
                Initiator = initiator;
                ErrorArguments = errorArguments;
            }

            private static string GenerateErrorString(Type initiator, IReadOnlyList<GenericArgumentError> errorArguments)
            {
                var builder = new StringBuilder();
                builder.AppendFormat("Для типа {0} не были удовлетворены все ограничения на универсальные параметры:", initiator);

                foreach (var argument in errorArguments)
                {
                    builder.AppendLine();
                    builder.AppendFormat("Тип {0} не удовлетворил ограничения: {1}", argument.InitiatorType, GenerateArgumentString(argument));
                }

                return builder.ToString();
            }

            private static string GenerateArgumentString(GenericArgumentError argument)
            {
                var flagString = GenerateFlagsString(argument.NotVerifyFlags);
                var typesString = argument.TargetTypes.Interlock(f => f.ToString());

                if (!typesString.IsNullOrEmpty())
                {
                    typesString = typesString.Insert(0, "Быть производным от типов: ");
                    if (!flagString.IsNullOrEmpty())
                        flagString = flagString.Insert(0, " а также");
                }
                return String.Format("{0}{1}",typesString, flagString);
            }

            private static string GenerateFlagsString(GenericParameterAttributes flags)
            {
                if (flags == GenericParameterAttributes.None) return null;
                var builder = new StringBuilder();
                if (flags.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint))
                {
                    if (builder.Length > 0) builder.Append(", ");
                    builder.Append("Тип должен быть не типом значения");
                }
                if (flags.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint))
                {
                    if (builder.Length > 0) builder.Append(", ");
                    builder.Append("Тип должен быть ссылочным");
                }
                if (flags.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint))
                {
                    if (builder.Length > 0) builder.Append(", ");
                    builder.Append("Тип должен содержать конструктор без параметров");
                }
                    return builder.ToString();
           }

        }
        #endregion








        #region ReferenceErrors
        public class ReferenceArleadyImport : SemanticError
        {
            public Assembly Assembly { get; private set; }

            public ReferenceArleadyImport(Location location, Assembly assembly):base(8,location,String.Format("Сборка \"{0}\" уже импортирована", assembly))
            {
                Assembly = assembly;
            }
        }

        public abstract class ReferenceException : SemanticError
        {
            public AssemblyLoadHeader Header { get; private set; }
            public ReferenceException(int code, Location location, AssemblyLoadHeader header, string message) : base(code, location, message)
            {
                Header = header;
            }
        }

        public class ReferenceNotFound : ReferenceException
        {
            public ReferenceNotFound(Location location, AssemblyLoadHeader header) : base(9, location,header, String.Format("Сборка \"{0}\" не найдена", header.Name))
            {
            }
        }

        public class ReferenceNullName : ReferenceException
        {
            public ReferenceNullName(Location location, AssemblyLoadHeader header) : base(10, location,header,"Имя сборки не может быть нулевым")
            {
            }
        }

        public class ReferenceIncorrectFile : ReferenceException
        {
            public ReferenceIncorrectFile(Location location,AssemblyLoadHeader header ) : base(11, location,header,String.Format("Файл сборки \"{0}\" некорректен", header.Name))
            {
            }
        }

        public class ReferenceBadeImageFormat : ReferenceException
        {
            public BadImageFormatException Exception { get; private set; }
            public ReferenceBadeImageFormat(Location location, AssemblyLoadHeader header, BadImageFormatException exception) : base(12, location,header, exception.Message)
            {
                Exception = exception;
            }
        }

        public class ReferencePathTooLong : ReferenceException
        {
            public System.IO.PathTooLongException Exception { get; private set; }
            public ReferencePathTooLong(Location location, AssemblyLoadHeader header, System.IO.PathTooLongException exception) : base(13, location, header, exception.Message)
            {
                Exception = exception;
            }
        }
        #endregion








        #region Auxiliary methods for obtaining errors
        public static SemanticError Get(NamespaceArleadyUsingEventArgs namespaceArleadyUsingEventArgs)
        {
            return new NamespaceAlreadyUsing(namespaceArleadyUsingEventArgs.Location, namespaceArleadyUsingEventArgs.ArleadyUsingName);
        }

        public static SemanticError Get(Location location, AssemblyLoadResult assemblyResult)
        {
            CallExceptionIfIsSuccessful(assemblyResult);
            var header = assemblyResult.Initiator;
            switch (assemblyResult.Result)
            {
                case AssemblyResults.BadImageFormatException: return new ReferenceBadeImageFormat(location, header, ((BadImageFormatException)assemblyResult.Exception));
                case AssemblyResults.PathTooLongException: return new ReferencePathTooLong(location, header, ((System.IO.PathTooLongException)assemblyResult.Exception));
                case AssemblyResults.FileLoadException: return new ReferenceIncorrectFile(location, header);
                case AssemblyResults.FileNotFoundException:
                case AssemblyResults.InGacNotFound:
                case AssemblyResults.GACNotExists: return new ReferenceNotFound(location, header);
                case AssemblyResults.ArgumentException: return new ReferenceNullName(location, header);
                default: throw new PresentVariantNotImplementedException(typeof(AssemblyResults));
            }

        }

        public static SemanticError Get(Location location, GenericVerifyResult genericVerifyResult)
        {
            CallExceptionIfIsSuccessful(genericVerifyResult);
            return new GenericArgumentNotVerify(location, genericVerifyResult.Initiator, genericVerifyResult.ErrorArguments);
        }

       public  static SemanticError Get(Location location, InterpretBinaryResult interpResult)
        {
            CallExceptionIfIsSuccessful(interpResult);

            switch (interpResult.Result)
            {
                case InterpretBinaryResults.CanNotFindGeneralType:return new CanNotFindGeneralType(location, interpResult.Initiator);
                case InterpretBinaryResults.CanNotBeApplied:return new BinaryOperationCanNotBeApplied(location,interpResult.Initiator);
                default: throw new NotImplementedException();
            }
        }

       public static SemanticError Get(Location location, InterpretUnaryResult interpResult)
        {
            CallExceptionIfIsSuccessful(interpResult);
            return new UnaryOperationCanNotBeApplied(location, interpResult.Initiator);
        }

        public static SemanticError Get(Location location, ParseConstantResult parseResult)
        {
            CallExceptionIfIsSuccessful(parseResult);
            return new ConstantNotBeParsed(location, parseResult.Result, parseResult.Initiator.Value);
        }


        //public static SemanticError Get(Location location, AssignConstantResult assignResult)
        //{
        //    CallExceptionIfIsSuccessful(assignResult);
        //    return new ConstantCanNotAssign(location, assignResult.Initiator, assignResult.TargetType);
        //}

        public static SemanticError Get(Location location, ConstantInterpreterResultUnary result)
        {
            CallExceptionIfIsSuccessful(result);
            return new ConstantUnaryOpperationError(location, result.Initiator, ConstantOpperationError.InterpreterResults2OperationErrorKind(result.Result));
        }

        public static SemanticError Get(Location location, ConstantInterpreterResultBinary result)
        {
            CallExceptionIfIsSuccessful(result);
            return new ConstantBinaryOpperationError(location, result.Initiator, ConstantOpperationError.InterpreterResults2OperationErrorKind(result.Result));
        }



        public static SemanticError Get(Location location, SearchingTypeResult searchingTypeResult)
        {
            CallExceptionIfIsSuccessful(searchingTypeResult);
            switch (searchingTypeResult.Result)
            {
                case SearchingResults.NotFound: return new TypeNotFound(location, searchingTypeResult.Initiator);
                case SearchingResults.Suspense: return new TypeSuspense(location, searchingTypeResult.Candidates.Select(f => f.Type).ToArray());
                case SearchingResults.OK:
                    {
                        if (searchingTypeResult.WithMember)
                            return new TypeWithMember(location, searchingTypeResult.Candidates.First().Type, searchingTypeResult.Initiator);
                        throw new InternalCompilerException();
                    }
                default: throw new PresentVariantNotImplementedException(typeof(SearchingResults));
            }
        }

        public static SemanticError Get(Location location, FindNestedTypeResult findNestedTypeResult)
        {
            CallExceptionIfIsSuccessful(findNestedTypeResult);
            switch (findNestedTypeResult.Result)
            {
                case SearchingResults.NotFound:
                    {
                        return new NestedTypeNotFound(location, findNestedTypeResult.ParentType.Type, findNestedTypeResult.ShortInitiator);
                    }

                case SearchingResults.Suspense: return new TypeSuspense(location, findNestedTypeResult.Candidates.Select(f => f.Type).ToArray());
                case SearchingResults.OK:
                    {
                        if (findNestedTypeResult.WithMember)
                            return new NestedTypeNotFound(location, findNestedTypeResult.Candidates.First().Type, findNestedTypeResult.Initiator);
                        throw new InternalCompilerException();
                    }
                default: throw new PresentVariantNotImplementedException(typeof(SearchingResults));
            }
        }

        public static SemanticError Get(Location location, SelectStringMemberResult selectResult,Type geneticType)
        {
            CallExceptionIfIsSuccessful(selectResult);
            switch (selectResult.Result)
            {
                case SearchingResults.NotFound:
                    {

                        return new StaticStringMemberNotFound(location, geneticType, selectResult.Initiator.Header);
                    }
                case SearchingResults.Suspense:
                    {
                        throw new NotImplementedException();
                    }
                default: throw new PresentVariantNotImplementedException(typeof(SearchingResults));
            }

        }

        public static SemanticError Get(Location location, SearechingNamespaceResult searchingNamespaceResult)
        {
            CallExceptionIfIsSuccessful(searchingNamespaceResult);
            switch (searchingNamespaceResult.Result)
            {
                case SearchingResults.Suspense: return new NamespaceSuspense(location, searchingNamespaceResult.Candidates.ToArray());
                case SearchingResults.NotFound: return new NamespaceNotFound(location, searchingNamespaceResult.Initiator);
                case SearchingResults.OK:
                    {
                        if (searchingNamespaceResult.WithMember)
                            return new NamespaceWithMember(location, searchingNamespaceResult.Candidates.First(), searchingNamespaceResult.Initiator);
                        throw new InternalCompilerException();
                    }
                default: throw new PresentVariantNotImplementedException(typeof(SearchingResults));
            }
        }

        public static SemanticError Get(Location location,AsCastResult castResult)
        {
            CallExceptionIfIsSuccessful(castResult);
            switch (castResult.Result)
            {
                case AsResult.TargetTypeIsValueType:return new InvalidAsTargetType(location, castResult.Initiator.Type1, castResult.Initiator.Type2);
                case AsResult.CannotBeApplied:return new InvalidAsTargetType(location, castResult.Initiator.Type1, castResult.Initiator.Type2);
                default: throw new PresentVariantNotImplementedException(typeof(AsResult));
            }
        }

        public static SemanticError Get(Location location, IsCastResult castResult)
        {
            CallExceptionIfIsSuccessful(castResult);
            return new InvalidIsCastType(location, castResult.Initiator.Type1, castResult.Initiator.Type2);
        }

            private static void CallExceptionIfIsSuccessful<T>(Result<T> result)
        {
#if DEBUG
            if (result.IsSuccessful) throw new ArgumentException("result is IsSuccessful");
#endif
        }
        #endregion
    }

    //    public static class SemanticErrors
    //    {
    //        public class TypeNotFound : SemanticError
    //        {
    //            public TypeNotFound(string source, Location location) : base(String.Format("Тип не найден: {0}", source), 0,location) { }
    //        }

    //        public class ThisIsObjectNotType : SemanticError
    //        {
    //            public ThisIsObjectNotType(string source, Location location) : base(String.Format("Данный объект не является типом: {0}", source), 1,location) { }
    //        }

    //        public class TypeSuspense : SemanticError
    //        {
    //            public IReadOnlyList<Type> Types { get; private set; }
    //            public TypeSuspense(IReadOnlyList<Type> types, Location location) : base(String.Format("Неопределённость между типами: {0}", GenStr(types)), 2,location) { Types = types; }
    //            public static string GenStr(IReadOnlyList<Type> Types)
    //            {
    //                string res = string.Empty;
    //                for (int i = 0; i < Types.Count - 1; i++)
    //                    res += Types[i].FullName + ",";
    //                res += Types[Types.Count - 1].FullName;
    //                return res;
    //            }
    //        }

    //        public class VarArleadyDefine : SemanticError
    //        {
    //            public VarArleadyDefine(string source,Location location) : base(String.Format("Переменная уже определена: {0}", source), 3,location) { }
    //        }

    //        public class VarUseForType : SemanticError
    //        {
    //            public VarUseForType(string source, Location location) : base(String.Format("Данное имя уже зарезервировано именим типа: {0}", source), 4, location) { }
    //        }

    //        public class VarNotDefine : SemanticError
    //        {
    //            public VarNotDefine(string source, Location location) : base(string.Format("Переменная не определена: {0}", source), 5, location) { }
    //        }
    //        public class IntNumberNotBeParsed : SemanticError
    //        {
    //            public IntNumberNotBeParsed(string source, Location location) : base(string.Format("Не удалось распознать действительное число: {0}", source), 6, location) { }
    //        }
    //        public class FloatNumberNotBeParsed : SemanticError
    //        {
    //            public FloatNumberNotBeParsed(string source, Location location) : base(string.Format("Не удалось распознать вещественное число: {0}", source), 7, location) { }
    //        }
    //        public class UnaryOperationCanNotBeApplied : SemanticError
    //        {
    //            public UnaryOperationCanNotBeApplied(string type, string unop, Location location) : base(string.Format("Унарная опперация:{1} не применима к типу: {0}", type, unop), 8, location) { }
    //        }
    //      
    //        public class BinaryOverflow : SemanticError
    //        {
    //            public BinaryOverflow(string value1, string value2,BinaryOperation operation, Location location) :
    //                base(string.Format("При выполнение бинарной операции: {2} для значений {0} и {1} произошло переполнение", value1, value2, operation), 10, location)
    //            { }
    //        }
    //        public class UnkownIdentifer : SemanticError
    //        {
    //            public UnkownIdentifer(string id, Location location) : base(string.Format("Неизвестное имя: {0}", id), 11, location) { }
    //        }
    //        public class InvalidImplicitConvert : SemanticError
    //        {
    //            public InvalidImplicitConvert(string primType, string targType, Location location) : base(string.Format("Не удалось выполнить неявное преобразование от типа:{0} к целевому типу:{1}", primType, targType), 12, location) { }
    //        }
    //        public class InvalidConstantImplicitConvert : SemanticError
    //        {
    //            public InvalidConstantImplicitConvert(string primType, string targType, string value, Location location) : base(string.Format("Не удалось выполнить неявное преобразование константы:{0} от типа:{1} к целевому типу:{2}", value, primType, targType), 13, location) { }
    //        }






    //        public static SemanticError Get(Location location, PLNReflection.GenericVerifyResult genericVerifyResult)
    //        {
    //            CallExceptionIfIsSuccessful(genericVerifyResult);
    //            return null;
    //        }
    //        public static SemanticError Get(Location location, PLNReflection.TypeFinder.SearchingTypeResult searchingTypeResult)
    //        {
    //            CallExceptionIfIsSuccessful(searchingTypeResult);
    //            return null;
    //        }

    //        public static SemanticError Get(Location location, PLNReflection.TypeFinder.SearechingNamespaceResult searchingNamespaceResult)
    //        {
    //            CallExceptionIfIsSuccessful(searchingNamespaceResult);
    //            return null;
    //        }

    //        private static void CallExceptionIfIsSuccessful<T>(Result<T> result)
    //        {
    //#if DEBUG
    //            if (result.IsSuccessful) throw new ArgumentException("result is IsSuccessful");
    //#endif
    //        }


    //    }

}
