using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using PLNCompiler.Compile.Errors;

namespace PLNCompiler
{
    public static class StringHelper
    {
        public static CultureInfo Culture = CultureInfo.InvariantCulture;

        public static readonly StringComparer PLNComparer = StringComparer.OrdinalIgnoreCase;

        public static readonly StringComparer NetComparer = StringComparer.Ordinal;

        public static readonly Encoding Encoding = Encoding.UTF8;

        public static readonly ITypeToString PLNTypeToString = new PLNTypeToString();

        public static readonly PLNCompiler.Syntax.TreeConverter.DirectiveInfo PLNDefaultDirectirs = new Syntax.TreeConverter.DirectiveInfo(Syntax.TreeConverter.ApplicationKind.Console, true,string.Empty);

        public static readonly Syntax.ConstantParser PLNConstantParser = new Syntax.ConstantParser(
            new Syntax.ConstantParserSettings(
                NumberStyles.AllowExponent,
                NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
                Culture));


        public static readonly string PLNTypeDelimiter = ".";
        public static readonly string NetTypeDelimiter = new string(Type.Delimiter, 1);
        public static readonly string NestedDelimiter = "+";
        public static readonly char GenericDelimiter = '`';
        public const string NotUsedChar = "$";

        public static readonly IReadOnlyDictionary<TypeName, Type> PLNDefualtTypes = new Dictionary<TypeName, Type>()
        {
            {new TypeName(new string[1]{"БЦелое8" },0),typeof(Byte) },
            {new TypeName(new string[1]{"Байт" },0),typeof(Byte) },
            {new TypeName(new string[1]{"Целое8" },0),typeof(SByte) },

            {new TypeName(new string[1]{"Целое16" },0),typeof(Int16) },
            {new TypeName(new string[1]{"БЦелое16" },0),typeof(UInt16) },

             {new TypeName(new string[1]{"Целое32" },0),typeof(Int32) },
            {new TypeName(new string[1]{"Целое" },0),typeof(Int32) },
            {new TypeName(new string[1]{"БЦелое32" },0),typeof(UInt32) },
            {new TypeName(new string[1]{"БЦелое" },0),typeof(UInt32) },

            {new TypeName(new string[1]{"Целое64" },0),typeof(Int64) },
            {new TypeName(new string[1]{"БЦелое64" },0),typeof(Int32) },

            {new TypeName(new string[1]{"Вещественное" },0),typeof(Single) },
            {new TypeName(new string[1]{"Дробное" },0),typeof(Double) },
            {new TypeName(new string[1]{"Логическое" },0),typeof(Boolean) },
            {new TypeName(new string[1]{"Булевый" },0),typeof(Boolean) },
            {new TypeName(new string[1]{"Булево" },0),typeof(Boolean) },

            {new TypeName(new string[1]{"Строка" },0),typeof(String) },
            {new TypeName(new string[1]{"Символ" },0),typeof(Char) },

            {new TypeName(new string[1]{"Десятичный" },0),typeof(Decimal) },
            {new TypeName(new string[1]{"Объект" },0),typeof(Object) }
             //{new TypeName(new string[1]{"НЦелое" },0),typeof(System.Numerics.BigInteger) },
           //  {new TypeName(new string[1]{"НЦелое" },0),typeof(System.Numerics.Complex) }
        };

        public static string GenerateImpossibleName(string correctName)
        {
            return NotUsedChar + correctName;
        }

        public static string GetFullNameWithoutGeneric(this Type t)
        {
            string name = t.FullName;
            if (!t.IsGenericTypeDefinition) return name;
            int index = name.IndexOf(GenericDelimiter);
            return index == -1 ? name : name.Substring(0, index);
        }
        public static string GetFullNameWithoutConstructedGeneric(this Type t)
        {
            string name = t.FullName;
            if (!t.IsConstructedGenericType) return name;
            int index = name.IndexOf(GenericDelimiter);
            return index == -1 ? name : name.Substring(0, index);
        }
        public static string GetGenericFullNameIgnore(this Type t)
        {
            string name = t.FullName;
            if (!t.IsConstructedGenericType && !t.IsGenericTypeDefinition) return name;
            int index = name.IndexOf(GenericDelimiter);
            return index == -1 ? name : name.Substring(0, index);
        }

        public static string GetNameWithoutGeneric(this Type t)
        {
            string name = t.Name;
            if (!t.IsGenericTypeDefinition) return name;
            int index = name.IndexOf(GenericDelimiter);
            return index == -1 ? name : name.Substring(0, index);
        }

        public static string GetGenericNameIgnore(this Type t)
        {
            string name = t.Name;
            if (!t.IsConstructedGenericType && !t.IsGenericTypeDefinition) return name;
            int index = name.IndexOf(GenericDelimiter);
            return index == -1 ? name : name.Substring(0, index);
        }

    }
}
