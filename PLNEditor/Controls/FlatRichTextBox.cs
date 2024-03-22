using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PLNEditor
{
    public partial class FlatRichTextBox : RichTextBox
    {
        public FlatRichTextBox()
        {
         //   RichTextBoxStyle.Default.ApplyStyle(this);
            InitializeComponent();

        }
        [Browsable(false)]
        public new Color BackColor { get { return base.BackColor; } set { base.BackColor = value; } }
        [Browsable(false)]
        public new Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; } }
        [Browsable(false)]
        public new Font Font { get { return base.Font; } set { base.Font = value; } }
        [Browsable(false)]
        public new BorderStyle BorderStyle { get { return base.BorderStyle; } set { base.BorderStyle = value; } }
     
    }
}
