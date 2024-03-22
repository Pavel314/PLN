using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLNCompiler.Syntax.Analysis
{
    public delegate bool ScanErrorCallback(object sender, ScanErrorEventArgs e);

    public class ScanErrorEventArgs : EventArgs
    {
        public string Text { get; private set; }
        public Location Location { get; private set; }

        public ScanErrorEventArgs(string text, Location location)
        {
            Location = location;
            Text = text;
        }
    }
}
