using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PLNCompiler.Syntax
{
    public sealed class Location:QUT.Gppg.IMerge<Location>
    {
        public int StartLine { get; private set; }
        public int StartColumn { get; private set; }
        public int EndLine { get; private set; }
        public int EndColumn { get; private set; }

        public Location(int startLine, int startColumn, int endLine, int endColumn)
        {
            StartLine = startLine;
            StartColumn = startColumn;
            EndLine = endLine;
            EndColumn = endColumn;
        }

        public Location()
        {

        }

     //   public Location(QUT.Gppg.LexLocation location) : this(location.StartLine, location.StartColumn, location.EndLine, location.EndColumn) { }

        public override string ToString()
        {
            return string.Format("{0},{1}", StartLine, StartColumn);
        }

        public Location Merge(Location last)
        {
             return new Location(this.StartLine, this.StartColumn, last.EndLine, last.EndColumn); 
        }

        //public static implicit operator Location (QUT.Gppg.LexLocation lexLocation)
        //{
        //    return new Location(lexLocation);
        //}

    }
}
