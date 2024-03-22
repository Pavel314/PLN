using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace PLNEditor.Controls.Style
{
   public abstract class StyleBase : ICloneable
    {
        public Font Font { get;  set; }
        public Color BackColor { get;  set; }
        public Color ForeColor { get; set; }

        protected StyleBase(Font font, Color backColor, Color foreColor)
        {
            Font = font;
            BackColor = backColor;
            ForeColor = foreColor;
        }
        public abstract object Clone();
    }
    public abstract class DisableStyleBase:ICloneable
    {
        public static float AttenuationBackColorFactor = 0.7F;
        public static float AttenuationForeColorFactor = 0.7F;

        public Color BackColor { get; set; }
        public Color ForeColor { get; set; }
        protected DisableStyleBase(Color backColor, Color foreColor,bool shouldMultiply=false)
        {
            if (!shouldMultiply)
            {
                BackColor = backColor;
                ForeColor = foreColor;
            } else
            {
                BackColor = MultyplyColor(backColor, AttenuationBackColorFactor);
               ForeColor= MultyplyColor(foreColor, AttenuationForeColorFactor);
            }

        }
        protected DisableStyleBase(StyleBase style):this
            (
            MultyplyColor( style.BackColor, AttenuationBackColorFactor),
             MultyplyColor(style.ForeColor, AttenuationForeColorFactor)
            )
        {}
        public abstract object Clone();

        public static Color MultyplyColor(Color col, float fac)
        {
            return Color.FromArgb(col.A, (byte)(col.R * fac), (byte)(col.G * fac), (byte)(col.B * fac));
        }
    }





    public class BorderSetting:ICloneable
    {
        public int BorderWidth { get;  set; }
        public Color BorderColor { get; set; }
        public BorderSetting(int borderWidth, Color borderColor)
        {
            BorderWidth = borderWidth;
            BorderColor = borderColor;
        }
        public BorderSetting(BorderSetting borderSetting):this(borderSetting.BorderWidth,borderSetting.BorderColor)
        {}
        public object Clone()
        {return new BorderSetting(this);}
    }
    public class DisableStyle : DisableStyleBase
    {
        public DisableStyle(Color backColor, Color foreColor, bool shouldMultiply = false) : base(backColor, foreColor,shouldMultiply) { }
        public DisableStyle(DisableStyle style) : this(style.BackColor, style.ForeColor) { }
        public override object Clone()
        {
            return new DisableStyle(this);
        }

    }



    public abstract class StyleBorderDisable : StyleBase
    {
        public BorderSetting BorderSetting { get; set; }
        public DisableStyle DisableStyle { get; set; }

        protected StyleBorderDisable(Font font, Color backColor, Color foreColor, BorderSetting borderSetting, DisableStyle disableStyle):
            base(font,backColor,foreColor)
        {
            BorderSetting = borderSetting;
            DisableStyle = disableStyle;
        }
    }




    public interface IStyleControl
    {
        int DefaultStyleIndex { get; set; }
    }



 



    }
