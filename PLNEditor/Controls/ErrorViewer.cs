using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace PLNEditor.Controls
{
   public class ErrorViewer:UserControl
    {
        public event EventHandler NeedHide;
        public ErrorListViewer ErrorListViewer { get; private set; }
        protected Panel BottomPanel;
        protected Label LeftLabel;
        protected LinkLabel RightLabel;

        public ErrorViewer()
        {
            BottomPanel = new Panel();
            BottomPanel.Dock = DockStyle.Bottom;

            LeftLabel = new Label();
            LeftLabel.Text = "Список ошибок";
            LeftLabel.Dock = DockStyle.Left;
            LeftLabel.AutoSize = false;
            LeftLabel.TextAlign = ContentAlignment.MiddleLeft;


            RightLabel= new LinkLabel();
            RightLabel.Text = "Спрятать";
            RightLabel.Dock = DockStyle.Right;
            RightLabel.AutoSize = false;
            RightLabel.TextAlign = ContentAlignment.MiddleRight;
            RightLabel.Click += RightLabel_Click;

            BottomPanel.Controls.Add(LeftLabel);
            BottomPanel.Controls.Add(RightLabel);
            BottomPanel.Height = 20;

            ErrorListViewer = new ErrorListViewer();
            ErrorListViewer.Dock = DockStyle.Fill;

            Controls.Add(ErrorListViewer);
            Controls.Add(BottomPanel);


        }

        protected virtual void OnNeedHide(EventArgs e) => NeedHide?.Invoke(this, null);

        private void RightLabel_Click(object sender, EventArgs e)
        {
            OnNeedHide(EventArgs.Empty);
        }
    }
}
