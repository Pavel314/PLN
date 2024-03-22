using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace PLNCompiler.Semantic
{
  public  class LabelWrapper
    {
        public string Name { get; private set; }
        public Label? Label { get; private set; }

        public void SetLabel(Label label)
        {
            if (Label!=null)
                throw new InvalidOperationException("Label arleady setted");
            Label = label;
        }

        public LabelWrapper(string name)
        {
            Name = name;
            Label = null;
        }
    }
}
