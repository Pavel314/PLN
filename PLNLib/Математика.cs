using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNLib
{
 public static class Математика
    {
        public const double ПИ = Math.PI;
        public const double Е = Math.E;

        public static double Синус(double значение)
        {
            return Math.Sin(значение);
        }

        public static double Косинус(double значение)
        {
            return Math.Cos(значение);
        }

        public static double Тангенс(double значение)
        {
            return Math.Tan(значение);
        }

        public static double Котангенс(double значение)
        {
            return  Math.Cos(значение)/Math.Sin(значение);
        }

        public static double ГСинус(double значение)
        {
            return Math.Sinh(значение);
        }

        public static double ГКосинус(double значение)
        {
            return Math.Cosh(значение);
        }

        public static double АСинус(double значение)
        {
            return Math.Asin(значение);
        }

        public static double АКосинус(double значение)
        {
            return Math.Acos(значение);
        }

        public static double АТангенс(double значение)
        {
           return Math.Atan(значение);
        }

        public static double Степень(double основание, double степень)
        {
            return Math.Pow(основание, степень);
        }

        public static double Корень(double основание, double степень)
        {
            return Math.Pow(основание, 1.0 / степень);
        }

        public static double Корень2(double основание)
        {
            return Math.Sqrt(основание);
        }

        public static double Логарифм(double значение, double основание)
        {
            return Math.Log(значение, основание);
        }

        public static double Логарифм(double значение)
        {
            return Math.Log10(значение);
        }

        public static int  Усечь(double значение)
        {
            return Convert.ToInt32(Math.Truncate(значение));
        }

        public static double Усечь(double значение,int после_запятой)
        {
            double a = Math.Pow(10, после_запятой);
            return Math.Truncate(значение*a)/a;
        }

        public static int ОкруглитьМ(double значение)
        {
            return Convert.ToInt32(Math.Round(значение,MidpointRounding.AwayFromZero));
        }

        public static int ОкруглитьМ(double значение, int после_запятой)
        {
            return Convert.ToInt32(Math.Round(значение,после_запятой, MidpointRounding.AwayFromZero));
        }

        public static int Округлить(double значение)
        {
            return ОкруглитьП(значение);
        }

        public static int Округлить(double значение, int после_запятой)
        {
            return ОкруглитьП(значение, после_запятой);
        }

        public static int ОкруглитьП(double значение)
        {
            return Convert.ToInt32(Math.Round(значение, MidpointRounding.ToEven));
        }

        public static int ОкруглитьП(double значение, int после_запятой)
        {
            return Convert.ToInt32(Math.Round(значение, после_запятой, MidpointRounding.ToEven));
        }

        public static double Случайное()
        {
            return random.NextDouble();
        }

        public static int СлучайноеЦелое()
        {
            return random.Next();
        }

        public static int Случайное(int min,int max)
        {
            return random.Next(min, max);
        }

        public static int Случайное(int max)
        {
            return random.Next(max);
        }

        private static Random random = new Random();
    }
}
