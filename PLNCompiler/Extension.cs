using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Syntax;
using System.Reflection;

namespace PLNCompiler
{
   public static class Extension
    {
        public static int IndexOf(this string inSearech, string value, IEqualityComparer<string> comparer)
        {
            if (value.IsNull()) throw new ArgumentNullException("item");
            if (comparer.IsNull()) throw new ArgumentNullException("comparer");
            int i = 0;
            while (i < inSearech.Length)
            {
                if (i + value.Length > inSearech.Length) return -1;
                if (comparer.Equals(value, inSearech.Substring(i, value.Length))) return i;
                i++;
            }
            return -1;
        }
        public static bool Contains(this string inSearech, string value, IEqualityComparer<string> comparer)
        {
            return IndexOf(inSearech, value, comparer) >= 0;
        }



        public static bool IsNullOrEmpty<T>(this IEnumerable<T> items)
        {
            return ReferenceEquals(items,null) || !items.Any();
        }

        public static bool IsNull(this object obj)
        {
            return ReferenceEquals(obj, null);
        }
        public static TypeCode ToTypeCode(this Type type)
        {
            return Type.GetTypeCode(type);
        }

        public static void RemoveLast<T>(this IList<T> list)
        {
            list.RemoveAt(list.Count - 1);
        }

        public static string TrimLast2Char(this string value)
        {
            return value.Remove(value.Length - 2, 2);
        }
        public static string TrimLastChars(this string value,int count)
        {
            return value.Remove(value.Length - count, count);
        }

        public static string TrimFLChar(this string value)
        {
            if (value.Length == 2) return string.Empty;
            return value.Substring(1, value.Length - 2);
        }

        public static string Interlock<T>(this IEnumerable<T> items,Func<T,string> toString, string separator=", ")
        {
            if (items.IsNullOrEmpty()) return null;
            StringBuilder builder = new StringBuilder();
            foreach (var item in items)
                builder.Append(toString(item) + separator);
            return builder.Remove(builder.Length - separator.Length, separator.Length).ToString();
        }

        public static void Pushs<T>(this Stack<T> stack,IEnumerable<T> items)
        {
            foreach (var item in items)
                stack.Push(item);
        }

        public static void Pops<T>(this Stack<T> stack,int count)
        {
            for (int i = 0; i < count; i++)
                stack.Pop();
        }

        public static bool EqualsStr(this IEqualityComparer<string> stringComparer,string x,string y)
        {
            return stringComparer.Equals(x, y);
        }

        public static bool ReturnIsVoid(this MethodInfo methodInfo)
        {
            return methodInfo.ReturnType == typeof(void);
        }

        public static bool StringExists<T>(this IEnumerable<T> items, Func<T, string> getString,string testString)
        {
            foreach (var item in items)
            {
                if (StringHelper.PLNComparer.EqualsStr(getString(item), testString))
                    return true;
            }
            return false;
        }

   //     public static string ToStringPLN

        public static Type MakePLNArrayType(this Type type, int rank)
        {
            if (rank == 1)
                return type.MakeArrayType();
            return type.MakeArrayType(rank);
        }

        public static bool ImplementInterface(this Type type, Type ifaceType)
        {
            Type[] interfaces = type.GetInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
                if (interfaces[i] == ifaceType)
                    return true;
            return false;
        }

        //public static bool IsStringChar(this ParsableConstantKind kind)
        //{
        //    return (kind == ParsableConstantKind.String || kind == ParsableConstantKind.Char);
        //}

        //public static bool IsInt64UInt64(this ParsableConstantKind kind)
        //{
        //    return (kind == ParsableConstantKind.Int64 || kind == ParsableConstantKind.UInt64);
        //}

        //public static Stack<T> Reverse<T>(this Stack<T> Stack)
        //{
        //    var res = new Stack<T>();
        //    while (Stack.Count > 0) res.Push(Stack.Pop());
        //    return res;
        //}

    }
}
