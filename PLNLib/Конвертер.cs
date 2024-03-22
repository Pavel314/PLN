using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNLib
{
    public static class Конвертер
    {
        public static Int32 КЦелому(object value)
        {
            return Convert.ToInt32(value);
        }
        public static Double КДробному(object value)
        {
            return Convert.ToDouble(value);
        }
        public static string КСтроке(object value)
        {
            return Convert.ToString(value);
        }

        public static bool КБулеву(object value)
        {
            return Convert.ToBoolean(value);
        }

        public static Массив<char> КМассиву(string value)
        {
            return new Массив<char>(value.ToCharArray());
        }
    }
}
