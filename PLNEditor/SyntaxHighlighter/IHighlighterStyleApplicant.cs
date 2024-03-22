using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNEditor.SyntaxHighlighter
{
    public interface IHighlighterStyleApplicant<TMainObject,TUpdateData>
    {
        HighlighterStyle Style { get; set; }
        void FirstUpdate(TMainObject mainObject );
        void Update(TMainObject mainObject, TUpdateData updateData);
    }
}
