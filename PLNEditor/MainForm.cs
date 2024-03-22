using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using FastColoredTextBoxNS;
using System.Xml;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;
using PLNEditor.SyntaxHighlighter;
using System.Diagnostics;
using PLNCompiler;
using PLNCompiler.Compile;

namespace PLNEditor
{
    public partial class MainForm : Controls.IconForm
    {
        public const string HighlighterName = "Highlighter.xml";

        private FindForm findForm;
        private HighlighterStyle PLNTextBoxStyle;


        public MainForm()
        {
            InitializeComponent();

            if (File.Exists(HighlighterName))
                PLNTextBoxStyle = HighlighterStyle.Load(HighlighterName);
            else
            {
                PLNTextBoxStyle = PLNHighlighterStyle.Get();
                HighlighterStyle.Save(PLNTextBoxStyle, HighlighterName);
            }
        //    errorViewer1.Items.Add(new Controls.VisualError(new PLN.Syntax.Location(1,1,1,1), "asd", @"D:\Fl.pln"));
       

            AddTabPage().TextBox.Select();
            binaryLinkLabel1.State = true;
        }

        private PLNTabPage getCurrentTabPge()
        {
            return ((PLNTabPage)tabControl1.SelectedTab);
        }

        private PLNTextBox getCurrentTextBox()
        {
            return getCurrentTabPge().TextBox;
        }

        private void UpdateTabPage(PLNTabPage page)
        {
            if (page == null) return;
            SetEnableUndoButton(true);
            SetEnableRedoButton(page.LastEditIsUndoRedo);
            var hasText = !string.IsNullOrEmpty(page.TextBox.Text);
            выделитьвсеToolStripMenuItem.Enabled = hasText;
            печатьToolStripMenuItem.Enabled = hasText;
        }

        private void SetEnableUndoButton(bool enabled)
        {
            undoFlatButton.Enabled = enabled;
            undoFlatButton.Invalidate();
            отменитьToolStripMenuItem.Enabled = enabled;
        }



        private void SetEnableRedoButton(bool enabled)
        {
            redoFlatButton.Enabled = enabled;
            восстановитьToolStripMenuItem1.Enabled = enabled;
        }

        private PLNTabPage AddTabPage(string text = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                int index = 1;
                text = PLNTabPage.GenNewName(index);
                for (int i = 0; i < tabControl1.TabPages.Count; i++)
                {
                    if (StringComparer.OrdinalIgnoreCase.Equals(tabControl1.TabPages[i].Text, text))
                    {
                        i = -1;
                        text = PLNTabPage.GenNewName(++index);
                    }
                }
            }
            var page = new PLNTabPage(text, PLNTextBoxStyle);
            page.TextBox.TextChanged += (sender, e) =>
            {
                UpdateTabPage(page);
            };
            tabControl1.TabPages.Add(page);
            tabControl1.SelectedTab = page;
            thisLastPage();
            return page;
        }

        #region Close
        private void CloseTabPage(int index)
        {
            var page = (PLNTabPage)tabControl1.TabPages[index];
            if (page.NeedSave)
            {
                var result = MessageBox.Show(string.Format("Выполнить сохранение {0}?", page.Text), Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Yes: SaveFile(); break;
                    case DialogResult.Cancel: return;

                }
            }
            tabControl1.TabPages[index].Dispose();
            thisLastPage();
        }

        private void CloseTabPage()
        {
            CloseTabPage(tabControl1.SelectedIndex);
        }

        private void CloseAllTabPagesWithoutCurrent()
        {
            for (int i = tabControl1.TabPages.Count - 1; i >= 0; i--)
            {
                if (tabControl1.TabPages[i] != tabControl1.SelectedTab)
                    CloseTabPage(i);
            }
        }

        private void CloseAllTabPages()
        {
            for (int i = tabControl1.TabPages.Count - 1; i >= 0; i--)
                CloseTabPage(i);
        }

        private bool ShutDown()
        {
            if (findForm != null) findForm.Dispose();
            CloseAllTabPages();

            return tabControl1.TabPages.Count > 0;
        }
        #endregion

        #region Open
        public void OpenFile(string path)
        {
            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                var cur = (PLNTabPage)tabControl1.TabPages[i];
                if (!cur.IsFirstSave && Utils.EqualsPath(cur.SavePath, path))
                {
                    tabControl1.SelectedTab = cur;
                    return;
                }

            }
            var page = getCurrentTabPge();

            if (page.IsEmpty())
                page.Load(path);
            else
                AddTabPage(Path.GetFileName(path)).Load(path);
        }

        public void OpenFileFromDialog()
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                OpenFile(openFileDialog.FileName);
        }
        #endregion

        #region Save
        private void SaveAsFile(PLNTabPage page)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                page.Save(saveFileDialog.FileName);
        }

        private void SaveAsFile()
        {
            SaveAsFile(getCurrentTabPge());
        }

        private void SaveFile()
        {
            var page = getCurrentTabPge();
            if (page.IsFirstSave)
                SaveAsFile();
            else
                page.Save(page.SavePath);

        }
        #endregion

        private void thisLastPage()
        {
            bool isLast = tabControl1.TabPages.Count > 1;
            закрытьToolStripMenuItem.Enabled = isLast;
            закрытьвсеToolStripMenuItem.Enabled = isLast;
            toolStripMenuItem2.Enabled = isLast;
            toolStripMenuItem3.Enabled = isLast;
        }
        private void undo()
        {
            var pg = getCurrentTabPge();
            pg.LastEditIsUndoRedo = true;
            var textbox = getCurrentTextBox();
            textbox.Undo();
            SetEnableRedoButton(true);
            if (!textbox.UndoEnabled)
                SetEnableUndoButton(false);
            pg.LastEditIsUndoRedo = false;
        }
        private void redo()
        {
            var pg = getCurrentTabPge();
            pg.LastEditIsUndoRedo = true;
            var textbox = getCurrentTextBox();
            textbox.Redo();
            SetEnableUndoButton(true);
            if (!textbox.RedoEnabled)
                SetEnableRedoButton(false);
            pg.LastEditIsUndoRedo = false;
        }

        private void selectAll()
        {
            getCurrentTextBox().SelectAll();
        }

        private void paste()
        {
            getCurrentTextBox().Paste();
        }

        private void cut()
        {
            getCurrentTextBox().Cut();
        }

        private void copy()
        {
            getCurrentTextBox().Copy();
        }

        private void newTabPage_Click(object sender, EventArgs e)
        {
            AddTabPage();
        }










        private void closeTabePage_Click(object sender, EventArgs e)
        {
            CloseTabPage();
        }

        private void closeAllTabPagesWithoutCurrent_Click(object sender, EventArgs e)
        {
            CloseAllTabPagesWithoutCurrent();
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileFromDialog();
        }

        private void undo_Click(object sender, EventArgs e)
        {
            undo();
        }

        private void redo_Click(object sender, EventArgs e)
        {
            redo();
        }


        private void выделитьвсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectAll();
        }

        private void paste_Click(object sender, EventArgs e)
        {
            paste();
        }

        private void cut_Click(object sender, EventArgs e)
        {
            cut();
        }

        private void copy_Click(object sender, EventArgs e)
        {
            copy();
        }

        private void find_Click(object sender, EventArgs e)
        {
            if (findForm == null)
                findForm = new FindForm(getCurrentTextBox());
            else
                findForm.TextBox = getCurrentTextBox();
            findNextToolStripMenuItem.Enabled = true;
            findForm.Show(this);
        }

        private void findAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (findForm == null) return;
            findForm.FindNext();
        }


        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void save_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void сохранитькакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAsFile();
        }



        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = ShutDown();
        }

        private void печатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentTabPge().PrintDiaolg();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTabPage(getCurrentTabPge());
        }

        private void runFlatButton_Click(object sender, EventArgs e)
        {
            errorListViewer1.Items.Clear();

            var curPage = getCurrentTabPge();
            var compiler = new PLNCompiler.Compile.PLNCompiler();

            string runPath = null;
            string viewPath = null;

            if (curPage.IsFirstSave)
            {
                runPath =Path.ChangeExtension(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),curPage.Text),"exe");
                viewPath = curPage.Text;
            } else
            {
                runPath = viewPath = Path.ChangeExtension(curPage.SavePath, "exe");
            }
            CompilationResult result = null;

            using (var memoryStream = new MemoryStream(StringHelper.Encoding.GetBytes(curPage.TextBox.Text)))
            {
                result = compiler.CompileFromStream(new ComilerSettings(false,runPath), memoryStream, StringHelper.Encoding.CodePage);
            }

            if (!result.IsSuccessful)
            {
                if (!result.Errors.LexicalErrors.IsNullOrEmpty())
                    foreach (var error in result.Errors.LexicalErrors)
                        errorListViewer1.Items.Add(new PLNEditor.Controls.VisualError(error.Location, error.Message, viewPath,curPage));

                if (!result.Errors.SyntaxErrors.IsNullOrEmpty())
                    foreach (var error in result.Errors.SyntaxErrors)
                        errorListViewer1.Items.Add(new PLNEditor.Controls.VisualError(error.Location, error.Message, viewPath, curPage));

                if (!result.Errors.SemanticErrors.IsNullOrEmpty())
                    foreach (var error in result.Errors.SemanticErrors)
                        errorListViewer1.Items.Add(new PLNEditor.Controls.VisualError(error.Location, error.Message, viewPath, curPage));
                errorListViewer1.NavigateToFirstError();
            }
            if (result.IsSuccessful)
                Process.Start(runPath);
            else
                binaryLinkLabel1.State = false;
            //AddErrorAndRun(result, df);


            //  MessageBox.Show(.IsFirstSave.ToString());
            //var compiler = new PLNCompiler(false);


            //var curTabPage = getCurrentTabPge();

            //var bytes = PLN.StringHelper.Encoding.GetBytes(curTabPage.TextBox.Text);

            //var name=curTabPage.Text;
            //using (var stream = new FileStream(name, FileMode.Create))
            //{
            //    stream.Write(bytes, 0, bytes.Length);
            //}
            //var proc = new Process();
            //proc.StartInfo = new ProcessStartInfo("PLNCompiler.exe", name);
            //proc.Start();
            //proc.WaitForExit();
            //File.Delete(name);
            //var exeFile = Path.GetFileNameWithoutExtension(name) + ".exe";
            //if (File.Exists(exeFile))
            //{
            //    Process.Start(exeFile);
            //}
            //if (curTabPage.IsFirstSave)
            //{
            //    PLN.Program.CompileWithConsoleIOMemory(curTabPage.TextBox.Text, PROGRAM_MEMORY_NAME);
            //    System.Diagnostics.Process.Start(PROGRAM_MEMORY_NAME);
            //}

            // PLN.Program.CompileWithConsoleIO()
        }

        private void AddErrorAndRun(CompilationResult result,string path)
        {
           
        }

        private const string PROGRAM_MEMORY_NAME = "PLNProgram";

        private void errorViewer1_NeedHide(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = true;
        }

        private void binaryLinkLabel1_StateChanged(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = binaryLinkLabel1.State;
        }

        private void errorListViewer1_NeedNavigate(object sender, Controls.VisualError e)
        {
            var page = (PLNTabPage)e.Parent;

            if (page == null || page.IsDisposed) { errorListViewer1.Items.Clear(); return; }

            tabControl1.SelectedTab = page;


            var textbox = getCurrentTextBox();
            textbox.Focus();


            textbox.SetCursorPosition(new Point(e.Location.StartColumn, e.Location.StartLine));
            textbox.DoCaretVisible();
           
        }

        private void содержаниеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "PLNСправка.chm");
        }

        private void опрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var f = new AboutBox())
                f.ShowDialog();
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
          if (MessageBox.Show("Все настройки редактора и подсветки доступны в файле Highlighter.xml. Запустить файл?","Настройки",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start("Highlighter.xml");
            }


        }
    }

    public class PLNTabPage : TabPage
    {

        public bool NeedSave { get; private set; }
        public string SavePath { get; private set; }
        public bool IsFirstSave { get; private set; }
        public bool LastEditIsUndoRedo = false;

        private string lastText;
        public Encoding Encoding { get; private set; }
        public PrintDialogSettings PrintSettings { get; private set; }


        public PLNTabPage(string tabPageName, HighlighterStyle style, Encoding encoding)
        {
            Encoding = encoding;
            TextBox = new PLNTextBox(new FastColoredTextBoxApplicant(style));
            TextBox.TextChanged += TextBox_TextChanged;
            Text = tabPageName;
            TextBox.Dock = DockStyle.Fill;
            NeedSave = false;
            IsFirstSave = true;
            PrintSettings = new PrintDialogSettings();
            PrintSettings.ShowPrintDialog = true;
            PrintSettings.ShowPageSetupDialog = true;
            this.Controls.Add(TextBox);
        }


        public PLNTabPage(string tabPageText, HighlighterStyle style) : this(tabPageText, style, Encoding.UTF8)
        {

        }

        public void Save(string path)
        {
            SavePath = path;
            IsFirstSave = false;
            TextBox.SaveToFile(path, Encoding);
            NeedSave = false;
            Text = Path.GetFileName(path);
        }

        public void Load(string path)
        {
            TextBox.OpenFile(path);
            lastText = TextBox.Text;
            Text = Path.GetFileName(path);
            SavePath = path;
            IsFirstSave = false;
            NeedSave = false;
        }

        public void PrintDiaolg()
        {
            TextBox.Print(PrintSettings);
        }

        public void PrintWithoutDiaolg()
        {
            TextBox.Print();
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(TextBox.Text) && IsFirstSave && !TextBox.RedoEnabled;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBox.Text))
            {
                NeedSave = lastText != TextBox.Text;
                lastText = TextBox.Text;
            }
            else
                NeedSave = false;

            NeedSave |= TextBox.UndoEnabled || TextBox.RedoEnabled;
        }


        public PLNTextBox TextBox { get; private set; }

        public static string GenNewName(int index) => string.Format("Программа{0}.pln", index);

    }

    public class PLNTextBox : FastColoredTextBox
    {
        public PLNTextBox(IHighlighterStyleApplicant<FastColoredTextBox, Range> applicant)
        {
            Applicant = applicant;
            Style = applicant.Style;
        }

        protected override void OnTextChanged(TextChangedEventArgs args)
        {
            base.OnTextChanged(args);
            Applicant.Update(this, args.ChangedRange);
        }

        public void FirstUpdate()
        {
            Applicant.FirstUpdate(this);
        }

        public void SetCursorPosition(Point pt)
        {
            var y = pt.Y - 1;
            if (y < 0) y = 0;

            var x = pt.X;
            if (x <0) x = 0;

            Selection = new Range(this,x,y,x,y);
        }

        public HighlighterStyle Style { get { return Applicant.Style; } set { Applicant.Style = value; Applicant.FirstUpdate(this); } }
        private IHighlighterStyleApplicant<FastColoredTextBox, Range> Applicant;
    }
}
