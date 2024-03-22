using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLNCompiler.Syntax;
namespace PLNCompiler.Compile.Errors
{
    public static class SyntaxErrors
    {
        public class UnknownConstruction : SyntaxError
        {
            public IEnumerable<string> CanBeFound { get; private set; }
            public string Found { get; private set; }

            public UnknownConstruction(string found, IEnumerable<string> canBeFound, Location location) :
                base(0, location, string.Format("Неизвестная синтаксическая конструкция, встречно:{0} а ожидалось: {1}", found, canBeFound.Interlock(f=>f,", ")))
            {
                CanBeFound = canBeFound;
                Found = found;
            }

            public static UnknownConstruction CreateForYYERROR(string format,  object[] args, Location location)
            {
                var argsStr = args.Cast<string>();
                return new UnknownConstruction(argsStr.First(),argsStr.Skip(1), location);
            }

        }
    }
}
