using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PLNEditor.Controls.Style;

namespace PLNEditor.Controls
{
    public partial class FlatButton : NotBlinkControl,IStyleControl
    {
        private ButtonStyle _Style;
        protected virtual ButtonStyle GetStyle()
        {
            return _Style;
        }
        protected virtual void SetStyle(ButtonStyle value)
        {
            _Style=value;
            Invalidate();
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ButtonStyle Style{get{return GetStyle(); } set { if (DefaultStyleIndex >= 0) throw new ArgumentException("Index should be negative"); SetStyle(value); } }
        //
        private int _DefaultStyleIndex;

      [Browsable(true)]
      [Description("The index for default style"), Category("Style")]
        public int DefaultStyleIndex { get {return _DefaultStyleIndex; }  set { if (value >= 0) SetStyle(ButtonStyle.Styles[value]); _DefaultStyleIndex = value; } }
        // 

        public FlatButton()
        {
            DefaultStyleIndex = 0;
            InitializeComponent();
        }

        protected override void OnPaintBackground(PaintEventArgs pevent){}

        protected virtual void DrawDown(PaintEventArgs e)
        {
            e.Graphics.Clear(Style.DownColor);
            if (Style.BorderSetting != null)
            {
                var pen = new Pen(Style.BorderSetting.BorderColor, Style.BorderSetting.BorderWidth)
                {
                    Alignment = PenAlignment.Center
                };
                e.Graphics.DrawRectangle(pen, DisplayRectangle);
                pen.Dispose();
            }
            // e.Graphics.TranslateTransform(-0.5F, -0.5F);
        }

 
        protected virtual void DrawOver(PaintEventArgs e)
        {
            e.Graphics.Clear(Style.OverColor);
        }
        protected virtual void DrawLeave(PaintEventArgs e)
        {
            e.Graphics.Clear(Style.BackColor);
        }
        protected virtual void DrawDisable(PaintEventArgs e)
        {
            DrawLeave(e);
            //       e.Graphics.Clear(Style.DisableStyle.BackColor);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            SetQuality(e.Graphics);
            Color textColor = Style.ForeColor;
            if (Enabled)
            {
                switch (MouseState)
                {
                    case MouseState.Down: DrawDown(e); break;
                    case MouseState.Over: DrawOver(e); break;
                    case MouseState.Leave: DrawLeave(e); break;
                }
            }
            else
            {
                if (Style.DisableStyle != null)
                {
                    DrawDisable(e);
                    textColor = Style.DisableStyle.ForeColor;
                }
            }
               

            using (var br = new SolidBrush(textColor))
            {
                e.Graphics.DrawString(Text, Style.Font, br, new PointF((Width - _TextSize.Width) / 2, Height - _TextSize.Height - 1));
            }
            if (base.BackgroundImage != null)
            {
                var sz = base.BackgroundImage.Size;
                e.Graphics.DrawImage(base.BackgroundImage, (this.Width - sz.Width) / 2, 0);
            }

            base.OnPaint(e);
        }
        protected override void OnEnabledChanged(EventArgs e)
        {
            var pos = PointToClient(Control.MousePosition);
            if (ClientRectangle.Contains(pos))
                OnMouseEnter(EventArgs.Empty);
            else
                Invalidate();

            base.OnEnabledChanged(e);

        }



        protected Size _TextSize;
        protected virtual void ProcessImg(Image img)
        {
            Graphics g = Graphics.FromHwnd(this.Handle);
            _TextSize = TextRenderer.MeasureText(g, this.Text, Style.Font);
            int sz = Convert.ToInt32((Math.Min(this.Width, this.Height) - _TextSize.Height));/// 1.2F
            if (sz <= 0) base.BackgroundImage = null;
            else
                base.BackgroundImage = ImageResize.Resize(img, sz, sz);
        }
        protected Image _OriginalImage;
        protected virtual void SetImage(Image img)
        {
            _OriginalImage = (Image)img.Clone();
            ProcessImg(_OriginalImage);
           Invalidate();
        }
        protected virtual Image GetImage()
        {
            return _OriginalImage;

        }
        public new Image BackgroundImage { get { return GetImage(); } set { SetImage(value); } }

        protected override void OnSizeChanged(EventArgs e)
        {
            ProcessImg(_OriginalImage);
            base.OnSizeChanged(e);  
            Invalidate();
        }
        protected override void OnTextChanged(EventArgs e)
        {
            ProcessImg(_OriginalImage);
            base.OnTextChanged(e);
            this.Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            MouseState = MouseState.Over;
            Invalidate();
            base.OnMouseEnter(e);
        }
        private bool _canMouseUp = true;
        protected override void OnMouseLeave(EventArgs e)
        {
            MouseState = MouseState.Leave;
            Invalidate();
            _canMouseUp = false;
            base.OnMouseLeave(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            MouseState = MouseState.Down;
            Invalidate();
            _canMouseUp = true;
            base.OnMouseDown(e);
            
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_canMouseUp)
            {
                MouseState = MouseState.Over;
                Invalidate();
            }
            base.OnMouseUp(e);
        }
        
       

        public MouseState MouseState = MouseState.Leave;

        protected virtual void SetQuality(Graphics gr)
        {

            gr.CompositingQuality = CompositingQuality.HighQuality;
            gr.InterpolationMode =InterpolationMode.HighQualityBicubic;
            gr.SmoothingMode = SmoothingMode.HighQuality;
        }
        
       public void ApplyStyle()
        {
            Invalidate();
        }

        [Browsable(false)]
        [Bindable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property has been deprecated.Please use are Style", true)]
        public new Color BackColor { get { return base.BackColor; } set { base.BackColor = value; } }
        [Browsable(false)]
        [Bindable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property has been deprecated.Please use are Style", true)]
        public new Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; } }
        [Browsable(false)]
        [Bindable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property has been deprecated.Please use are Style", true)]
        public new Font Font { get { return base.Font; } set { base.Font=value; } }


    }
}
