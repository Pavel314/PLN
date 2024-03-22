using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace PLNEditor.Controls.Style
{
    //public class RichTextBoxStyle : StyleBase
    //{
    //    public BorderStyle BorderStyle { get; set; }

    //    public RichTextBoxStyle(Font font, Color backColor, Color foreColor, BorderStyle borderStyle)
    //    {
    //        Font = font;
    //        BackColor = backColor;
    //        ForeColor = foreColor;
    //        BorderStyle = borderStyle;
    //    }
    //    public RichTextBoxStyle(RichTextBox rtb)
    //    {
    //        BorderStyle = rtb.BorderStyle;
    //        Font = rtb.Font;
    //        BackColor = rtb.BackColor;
    //        ForeColor = rtb.ForeColor;
    //    }

    //    public override void ApplyStyle(Control ctr)
    //    {
    //        var rtb = (RichTextBox)ctr;
    //        rtb.BorderStyle = BorderStyle;
    //        base.ApplyStyle(ctr);
    //    }
    //    public static RichTextBoxStyle Default =
    //        new RichTextBoxStyle(FontProvider.Consolas, Color.White, Color.Black, BorderStyle.None);

    //}



    public class ButtonStyle :StyleBorderDisable
    {
        public Color OverColor { get; set; }
        public Color DownColor { get; set; }

        public ButtonStyle():base(FontProvider.Arial,Color.Orange,Color.LightGray,null,new DisableStyle(Color.DarkGray, Color.LightGray)) { }

        public ButtonStyle(Font font, Color backColor, Color foreColor, BorderSetting borderSetting,DisableStyle disableStyle,Color overColor,Color downColor):base(font,backColor,foreColor,borderSetting,disableStyle)
        {
            OverColor = overColor;
            DownColor = downColor;
        }
    
        public ButtonStyle(Font font, Color backColor, Color foreColor, BorderSetting borderSetting, DisableStyle disableStyle, float overColorFactor= 1.2F, float downColorFactor= 1.3F) :
            this(font, backColor, foreColor, borderSetting,disableStyle, 
                DisableStyleBase.MultyplyColor(backColor, overColorFactor),
                DisableStyleBase.MultyplyColor(backColor, downColorFactor))
        {}


        public ButtonStyle(Font font, Color backColor, Color foreColor, BorderSetting borderSetting) :
    this(font, backColor, foreColor, borderSetting,new DisableStyle(backColor,foreColor,true))
        {
       
        }

        public ButtonStyle(ButtonStyle style) : this(style.Font, style.BackColor, style.ForeColor, style.BorderSetting, style.DisableStyle, style.OverColor, style.DownColor)
        {
        }

        public override object Clone()
        {
            return new ButtonStyle(this);
        }




        //new BorderSetting(4,Color.Black)
        public static ButtonStyle DefaultStyle1 = 
            new ButtonStyle(FontProvider.Arial, Color.FromArgb(192,0,192), Color.White,null);

        public static ButtonStyle DefaultStyle2 = //Color.FromArgb(102, 180, 19)
            new ButtonStyle(FontProvider.Arial, Color.FromArgb(102, 180, 19), Color.White, null);

        public static ButtonStyle 
            DefaultStyle3 = new ButtonStyle(FontProvider.Arial, Color.FromArgb(0, 0, 192), Color.White, null);

        public static List<ButtonStyle> Styles=new List<ButtonStyle>();

        static ButtonStyle()
        {
            Styles.Add(DefaultStyle1);
            Styles.Add(DefaultStyle2);
            Styles.Add(DefaultStyle3);
        }

    }
}
