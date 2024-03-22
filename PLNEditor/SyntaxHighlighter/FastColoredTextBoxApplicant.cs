using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using FastColoredTextBoxNS;

namespace PLNEditor.SyntaxHighlighter
{
    public class FastColoredTextBoxApplicant : IHighlighterStyleApplicant<FastColoredTextBox,Range>,IDisposable
    {
        private HighlighterStyle style;
        public HighlighterStyle Style { get { return style; } set { setStyle(value); } }

        public FastColoredTextBoxApplicant(HighlighterStyle style)
        {
            Style = style;
        }

        public void SetStyle(HighlighterStyle style,FastColoredTextBox textBox)
        {
            Style = style;
            FirstUpdate(textBox);
        }

        private void setStyle(HighlighterStyle style)
        {
            BracketStyle = null;
            BracketStyle2 = null;
            BracketCount = 0;
            TextStyles = null;

            this.style = style;
            if (!style.Brackets.IsNullOrEmpty())
            {
                BracketCount = style.Brackets.Count;
                if (style.Brackets.Count > 2)
                    throw new ArgumentException("can not use brackets(if >2) for fastcoloredtextbox");

                BracketStyle = new MarkerStyle(new SolidBrush(style.Brackets[0].Color));
                if (style.Brackets.Count > 1)
                    BracketStyle2 = new MarkerStyle(new SolidBrush(style.Brackets[1].Color));
            }

            if (!style.ColorStyles.IsNullOrEmpty())
            {
                TextStyles = new TextStyle[style.ColorStyles.Count];

                for (int i = 0; i < style.ColorStyles.Count; i++)
                {
                    var colorStyle = style.ColorStyles[i];
                    TextStyles[i] = new TextStyle(new SolidBrush(colorStyle.ForeColor),
                        new SolidBrush(colorStyle.BackgroundColor),
                        colorStyle.FontStyle)
                    { stringFormat = colorStyle.StringFormat };

                }
            }
        }


        public void FirstUpdate(FastColoredTextBox textBox)
        {
            textBox.ClearStylesBuffer();
            textBox.Range.ClearStyle(StyleIndex.All);

            if (Style.GlobalSettings.Font != null)
                textBox.Font = Style.GlobalSettings.Font;

            textBox.BackColor = Style.GlobalSettings.BackgroundColor;
            textBox.ForeColor = Style.GlobalSettings.ForeColor;

            textBox.CommentPrefix = Style.CommentPrefix;
        }

        public void Update(FastColoredTextBox textBox,Range range)
        {
            if (BracketCount > 0)
            {
                textBox.LeftBracket = Style.Brackets[0].Open;
                textBox.RightBracket = Style.Brackets[0].Close;
                textBox.BracketsStyle = BracketStyle;

                if (BracketCount > 1)
                {
                    textBox.LeftBracket2 = Style.Brackets[1].Open;
                    textBox.RightBracket2 = Style.Brackets[1].Close;
                    textBox.BracketsStyle2 = BracketStyle2;
                }
            }

            range.ClearStyle(TextStyles);

            if (Style.HighlighterElements != null)
            {
                foreach (var element in Style.HighlighterElements)
                {
                    range.SetStyle(TextStyles[element.StyleIndex], element.Regex);
                }
            }
            range.ClearFoldingMarkers();

            foreach (var market in Style.FoldingMarkers)
            {
                range.SetFoldingMarkers(market.StartFoldingMarker, market.EndFoldingMarker, market.RegexOptions);
            }

        }

        private MarkerStyle BracketStyle;
        private MarkerStyle BracketStyle2;
        private int BracketCount=0;

        private TextStyle[] TextStyles;



        #region IDisposable Support
        private bool disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue) return;
            if (disposing)
            {
                if (BracketStyle != null)
                    BracketStyle.Dispose();
                if (BracketStyle2 != null)
                    BracketStyle2.Dispose();
                if (!TextStyles.IsNullOrEmpty())
                {
                    foreach (var style in TextStyles)
                    {
                        style.Dispose();
                    }
                    TextStyles = null;
                }
            }
            disposedValue = true;
        }

         ~FastColoredTextBoxApplicant()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


    }
}
