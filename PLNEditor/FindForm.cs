using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using FastColoredTextBoxNS;

namespace PLNEditor
{
    public partial class FindForm : Form
    {
        public FindForm(FastColoredTextBox textBox)
        {
            InitializeComponent();
            TextBox = textBox;
            isFirst = true;
            autoComplete = new AutoCompleteStringCollection();
            targetTextBox.AutoCompleteCustomSource = autoComplete;
            targetTextBox_TextChanged(targetTextBox, EventArgs.Empty);
        }

        public void Show(Form parent)
        {
            if (Visible) return;
            Location = new Point((parent.Location.X)+(parent.Width-Width)/2, (parent.Location.Y) + (parent.Height - Height) / 2);
            ActiveControl = targetTextBox;
            targetTextBox.SelectAll();
            Reset();
            base.Show(parent);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void findNextButton_Click(object sender, EventArgs e)
        {
            FindNext();
            autoComplete.Add(targetTextBox.Text);
        }
        private AutoCompleteStringCollection autoComplete;

        public void FindNext()
        {
            findNext(targetTextBox.Text, useCaseCheckBox.Checked, wholeWordCheckBox.Checked, regexСheckBox.Checked);
        }

        protected virtual void findNext(string pattern,bool useCase, bool wholeWords, bool useRegex)
        {
            if (!useRegex)
                pattern = Regex.Escape(pattern);

            if (wholeWords)
                pattern = "\\b" + pattern + "\\b";

            var range = textBox.Selection.Clone();
            range.Normalize();

            if (isFirst)
            {
                startPlace = range.Start;
                isFirst = false;
            }
            //
            range.Start = range.End;
            if (range.Start >= startPlace)
                range.End = new Place(textBox.GetLineLength(textBox.LinesCount - 1), textBox.LinesCount - 1);
            else
                range.End = startPlace;
            label2.Text = string.Empty;
            foreach (var findRange in range.GetRangesByLines(pattern,useCase? RegexOptions.None: RegexOptions.IgnoreCase))
            {
                textBox.Selection = findRange;
                textBox.DoSelectionVisible();
                textBox.Invalidate();
                return;
            }

            if (range.Start >= startPlace && startPlace > Place.Empty)
            {
                textBox.Selection.Start = new Place(0, 0);
                findNext(pattern, useCaseCheckBox.Checked, wholeWordCheckBox.Checked, regexСheckBox.Checked);
                return;
            }
            label2.Text = "Совпадения не найдены";
        }

        private void Reset()
        {
            isFirst = true;
            label2.Text = string.Empty;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
            TextBox.Focus();
            base.OnFormClosing(e);
        }

        public FastColoredTextBox TextBox { get { return textBox; } set { textBox = value;startPlace = Place.Empty; Reset(); } }
        private FastColoredTextBox textBox;

        private Place startPlace;
        private bool isFirst;

        private void wholeWordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Reset();
        }

        private void targetTextBox_TextChanged(object sender, EventArgs e)
        {
            findNextButton.Enabled = !string.IsNullOrEmpty(targetTextBox.Text);

        }
    }
}
