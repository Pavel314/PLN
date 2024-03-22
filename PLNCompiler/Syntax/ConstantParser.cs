using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax
{
    public class ConstantParser
    {
        public ConstantParser(ConstantParserSettings settings)
        {
            Settings = settings;
        }

        public ParseConstantResult Parse(Constant constant)
        {
            switch (constant.Kind)
            {
                case ConstKind.IntNumber:
                    {
                        if (UInt64.TryParse(constant.Value, Settings.IntStyle, Settings.CultureInfo, out UInt64 result))
                        {
                            return new ParseConstantResult(constant, ParseConstantResults.OK, new ParsableConstant(result));
                        }
                        return new ParseConstantResult(constant, ParseConstantResults.IntError, null);
                    }
                case ConstKind.FloatNumber:
                    {
                        if (Double.TryParse(constant.Value, Settings.FloatStyle, Settings.CultureInfo, out Double result))
                        {
                            return new ParseConstantResult(constant, ParseConstantResults.OK, new ParsableConstant(result));
                        }
                        return new ParseConstantResult(constant, ParseConstantResults.FloatError, null);
                    }

                    
                default:
                    throw new PresentVariantNotImplementedException(typeof(ConstKind));
            }
        }

        public ConstantParserSettings Settings { get; private set; }
    }
}
