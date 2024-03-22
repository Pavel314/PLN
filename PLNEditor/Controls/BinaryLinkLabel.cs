using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace PLNEditor.Controls
{
  public  class BinaryLinkLabel:LinkLabel
    {
        [Browsable(true)]
        public event EventHandler StateChanged;

        //[EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Description("This is text for true state"), Category("Content")]
        public string TrueText { get { return trueText; } set { trueText = value; if (state) Text = trueText; } }
        private string trueText;

       // [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Description("This is text for false state"), Category("Content")]
        public string FalseText { get { return falseText; } set { falseText = value;  if (!state) Text = falseText; } }
        private string falseText;

        [Browsable(true)]
        [Description("This is text for false state"), Category("State")]
        public bool State { get { return state; } set { state = value;UpdateText();OnStateChanged(EventArgs.Empty); } }
        private bool state;

        public BinaryLinkLabel(string trueText,string falseText,bool state)
        {
            this.state = state;
            this.trueText = trueText;
            this.falseText = falseText;
          //  UpdateText();
            //OnStateChanged(EventArgs.Empty);
        }


        public BinaryLinkLabel() : this("true", "false", true)
        {

        }

        protected virtual void OnStateChanged(EventArgs e) => StateChanged?.Invoke(this, e);

        protected override void OnClick(EventArgs e)
        {
            State = !State;
            OnStateChanged(EventArgs.Empty);
            base.OnClick(e);
        }


        public virtual void UpdateText()
        {
            if (state)
                Text = TrueText;
            else
                Text = FalseText;
        }
    }
}
