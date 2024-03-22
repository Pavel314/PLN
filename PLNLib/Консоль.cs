using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNLib
{
    public static class Консоль
    {
     

        public static void Вывод(object значение)
        {
            Console.WriteLine(значение);
        }
        public static void Вывод(string формат, object значение)
        {
            Console.WriteLine(формат, значение);
        }
        public static void Вывод(string формат, object значение0, object значение1)
        {
            Console.WriteLine(формат, значение0, значение1);
        }
        public static void Вывод(string формат, object значение0, object значение1, object значение2)
        {
            Console.WriteLine(формат, значение0, значение1, значение2);
        }
        public static void Вывод(string формат, object значение0, object значение1, object значение2, object значение3)
        {
            Console.WriteLine(формат, значение0, значение1, значение2, значение3);
        }
        public static void Вывод(string формат, object[] значения)
        {
            Console.WriteLine(формат, значения);
        }

        public static void Вывод()
        {
            Console.WriteLine();
        }

        public static string Ввод()
        {
            return Console.ReadLine();
        }

        public static Int32 ВводЦелое()
        {
            return Convert.ToInt32(Console.ReadLine());
        }

        public static Double ВводДробное()
        {
            return Convert.ToDouble(Console.ReadLine());
        }


        public static int ВводСимвол()
        {
            return Console.Read();
        }

        public static ConsoleKeyInfo ВводКнопка()
        {
            return Console.ReadKey();
        }
        public static ConsoleKeyInfo ВводКнопка(bool interpcet)
        {
            return Console.ReadKey(interpcet);
        }

        public static void Пауза()
        {
            Console.ReadKey();
        }

        public static void Звук()
        {
            Console.Beep();
        }

        public static void Звук(int частота,int длительность)
        {
            Console.Beep(частота,длительность);
        }
    }
}
