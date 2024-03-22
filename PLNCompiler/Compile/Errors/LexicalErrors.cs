using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.Errors
{
    public static class LexicalErrors
    {
        public class IllicitChar : LexicalError
        {
            public string DetectedChar { get; private set; }
            public IllicitChar(string detectedChar,Syntax.Location location) : base(0,location,string.Format("Запрещённый символ {0}", detectedChar))
            {
                DetectedChar = detectedChar;
            }
        }
    }
}
