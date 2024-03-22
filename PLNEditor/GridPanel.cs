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
    public partial class GridPanel : TableLayoutPanel
    {
        public GridPanel()
        {
            InitializeComponent();
        }
        //protected override void OnCellPaint(TableLayoutCellPaintEventArgs e)
        //{
        //    Graphics g = e.Graphics;
        //    e.Graphics.SmoothingMode =System.Drawing.Drawing2D. SmoothingMode.HighQuality;
        //    Rectangle r = e.CellBounds;
    

        //    using (Pen pen = new Pen(Color.Black, 1 /*1px width despite of page scale, dpi, page units*/ ))
        //    {
        //        pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
        //        // define border style
        //        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

        //        // decrease border rectangle height/width by pen's width for last row/column cell
        //        if (e.Row == (RowCount - 1))
        //        {
        //            r.Height -= 1;
        //        }

        //        if (e.Column == (ColumnCount - 1))
        //        {
        //            r.Width -= 1;
        //        }

        //        // use graphics mehtods to draw cell's border
        //        e.Graphics.DrawRectangle(pen, r);
        //    }
        //}

    }
}
