using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Tests
{
    [Flags]
    public enum BB
    { a,b}

    public static class cll
    {
        public static int a;
    }

    public static class cl
    {
        public static void v(ref Int64 a)
        {
            a = 99;
        }
    }

    public static class refArg
    {
       public class a
        {
            public class gen<T> { 
}
        }

    }

    public static class defPrms
    {
        public static void def(object obj)
        {
            Console.WriteLine(obj);
        }
    }


    public class fun
    {
        public static bool f()
        {
            Console.WriteLine("called");
            return true;
        }

    }


    public class dec
    {
        public const decimal a = 9;

    }


    public class Simple<T>
    {
        public class Nested
        {

        }
    }

    public class Outer<T>
    {
       public class Inner<U>
        {
            public static int a;

        }
        public class Inner
        {
            public static int a;
            public static int A;
        }


    }

    public class GenConst<T> where T : IComparable
    {
        public class nes<T> where T : IComparable
        {
        }
    }
        public  class NesType<TT>
    {
        public class NesType1
        {
            public class NesType11
            {
                public class NesType111
                {

                }
            }

        }


        public class NesType2
        {
            public class NesType21
            {
            }
            public class NesType22
            {
                public class NesType221<T>
                {
                }
            }
            public class nesType22
            {
            }
            }

        public class NesType3
        {
            public class NesType31
            {
                public class NesType311
                {
                }
            }
            public class NesType32
            {
            }
            public class NesType33
            {
            }
        }
    }
}
