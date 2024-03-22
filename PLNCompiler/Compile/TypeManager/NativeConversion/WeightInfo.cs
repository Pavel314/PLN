using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Compile.TypeManager.NativeConversion
{
    public enum WeightKind { Native, Class, Interface, CopmpConv, Equivalent }

    public struct WeightInfo
    {
        public readonly WeightKind Kind;
        public readonly int Weight;

        public WeightInfo(WeightKind kind, int weight)
        {
            Weight = weight;
            Kind = kind;
        }

        public static WeightInfo Native(int weight)
        {
            return new WeightInfo(WeightKind.Native, weight);
        }

        public static WeightInfo Equivalent() { return new WeightInfo(WeightKind.Equivalent,0); }

        public override string ToString()
        {
            return string.Format("WeightInfo:[Kind={0} Weight={1}]", Kind, Weight);
        }
    }
}
