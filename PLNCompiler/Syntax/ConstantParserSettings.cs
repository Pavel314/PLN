using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace PLNCompiler.Syntax
{
    public class ConstantParserSettings
    {
        public NumberStyles IntStyle { get; private set; }
        public NumberStyles FloatStyle { get; private  set; }
        public CultureInfo CultureInfo { get; private set; }

        public ConstantParserSettings(NumberStyles intStyle, NumberStyles floatStyle, CultureInfo cultureInfo)
        {
            IntStyle = IntStyle;
            FloatStyle = floatStyle;
            CultureInfo = cultureInfo;
        }
    }
}
