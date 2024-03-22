
namespace PLNEditor
{
    partial class MainForm:Controls.IconForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.создатьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитькакToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.закрытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.закрытьвсеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.печатьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.правкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отменитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.восстановитьToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.вырезатьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.копироватьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.вставкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findNextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.выделитьвсеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сервисToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.содержаниеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.опрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.errorListViewer1 = new PLNEditor.Controls.ErrorListViewer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.binaryLinkLabel1 = new PLNEditor.Controls.BinaryLinkLabel();
            this.gridPanel1 = new PLNEditor.GridPanel();
            this.pastleFlatButton = new PLNEditor.Controls.FlatButton();
            this.copyFlatButton = new PLNEditor.Controls.FlatButton();
            this.cutFlatButton = new PLNEditor.Controls.FlatButton();
            this.runFlatButton = new PLNEditor.Controls.FlatButton();
            this.newFlatButton = new PLNEditor.Controls.FlatButton();
            this.openFlatButton = new PLNEditor.Controls.FlatButton();
            this.saveFlatButton = new PLNEditor.Controls.FlatButton();
            this.gridPanel2 = new PLNEditor.GridPanel();
            this.redoFlatButton = new PLNEditor.Controls.FlatButton();
            this.undoFlatButton = new PLNEditor.Controls.FlatButton();
            this.mainMenuStrip.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gridPanel1.SuspendLayout();
            this.gridPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.правкаToolStripMenuItem,
            this.сервисToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(624, 24);
            this.mainMenuStrip.TabIndex = 9;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.создатьToolStripMenuItem,
            this.открытьToolStripMenuItem,
            this.toolStripSeparator,
            this.сохранитьToolStripMenuItem,
            this.сохранитькакToolStripMenuItem,
            this.toolStripSeparator1,
            this.закрытьToolStripMenuItem,
            this.закрытьвсеToolStripMenuItem,
            this.toolStripSeparator6,
            this.печатьToolStripMenuItem,
            this.toolStripSeparator2,
            this.выходToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "&Файл";
            // 
            // создатьToolStripMenuItem
            // 
            this.создатьToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("создатьToolStripMenuItem.Image")));
            this.создатьToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.создатьToolStripMenuItem.Name = "создатьToolStripMenuItem";
            this.создатьToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.создатьToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.создатьToolStripMenuItem.Text = "&Создать";
            this.создатьToolStripMenuItem.Click += new System.EventHandler(this.newTabPage_Click);
            // 
            // открытьToolStripMenuItem
            // 
            this.открытьToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("открытьToolStripMenuItem.Image")));
            this.открытьToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            this.открытьToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.открытьToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.открытьToolStripMenuItem.Text = "&Открыть";
            this.открытьToolStripMenuItem.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(234, 6);
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("сохранитьToolStripMenuItem.Image")));
            this.сохранитьToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.сохранитьToolStripMenuItem.Text = "&Сохранить";
            this.сохранитьToolStripMenuItem.Click += new System.EventHandler(this.save_Click);
            // 
            // сохранитькакToolStripMenuItem
            // 
            this.сохранитькакToolStripMenuItem.Image = global::PLNEditor.Properties.Resources.save_all;
            this.сохранитькакToolStripMenuItem.Name = "сохранитькакToolStripMenuItem";
            this.сохранитькакToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.сохранитькакToolStripMenuItem.Text = "Сохранить &как";
            this.сохранитькакToolStripMenuItem.Click += new System.EventHandler(this.сохранитькакToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(234, 6);
            // 
            // закрытьToolStripMenuItem
            // 
            this.закрытьToolStripMenuItem.Image = global::PLNEditor.Properties.Resources.close_small;
            this.закрытьToolStripMenuItem.Name = "закрытьToolStripMenuItem";
            this.закрытьToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.закрытьToolStripMenuItem.Text = "Закрыть";
            this.закрытьToolStripMenuItem.Click += new System.EventHandler(this.closeTabePage_Click);
            // 
            // закрытьвсеToolStripMenuItem
            // 
            this.закрытьвсеToolStripMenuItem.Name = "закрытьвсеToolStripMenuItem";
            this.закрытьвсеToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.закрытьвсеToolStripMenuItem.Text = "Закрыть все, кроме текущего";
            this.закрытьвсеToolStripMenuItem.Click += new System.EventHandler(this.closeAllTabPagesWithoutCurrent_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(234, 6);
            // 
            // печатьToolStripMenuItem
            // 
            this.печатьToolStripMenuItem.Enabled = false;
            this.печатьToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("печатьToolStripMenuItem.Image")));
            this.печатьToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.печатьToolStripMenuItem.Name = "печатьToolStripMenuItem";
            this.печатьToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.печатьToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.печатьToolStripMenuItem.Text = "&Печать";
            this.печатьToolStripMenuItem.Click += new System.EventHandler(this.печатьToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(234, 6);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Image = global::PLNEditor.Properties.Resources.exit_small;
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.выходToolStripMenuItem.Text = "Вы&ход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
            // 
            // правкаToolStripMenuItem
            // 
            this.правкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.отменитьToolStripMenuItem,
            this.восстановитьToolStripMenuItem1,
            this.toolStripSeparator3,
            this.вырезатьToolStripMenuItem,
            this.копироватьToolStripMenuItem,
            this.вставкаToolStripMenuItem,
            this.toolStripSeparator4,
            this.findToolStripMenuItem,
            this.findNextToolStripMenuItem,
            this.toolStripSeparator7,
            this.выделитьвсеToolStripMenuItem});
            this.правкаToolStripMenuItem.Name = "правкаToolStripMenuItem";
            this.правкаToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.правкаToolStripMenuItem.Text = "&Правка";
            // 
            // отменитьToolStripMenuItem
            // 
            this.отменитьToolStripMenuItem.Image = global::PLNEditor.Properties.Resources.undo_small;
            this.отменитьToolStripMenuItem.Name = "отменитьToolStripMenuItem";
            this.отменитьToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.отменитьToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.отменитьToolStripMenuItem.Text = "&Отменить";
            this.отменитьToolStripMenuItem.Click += new System.EventHandler(this.undo_Click);
            // 
            // восстановитьToolStripMenuItem1
            // 
            this.восстановитьToolStripMenuItem1.Image = global::PLNEditor.Properties.Resources.redo_small;
            this.восстановитьToolStripMenuItem1.Name = "восстановитьToolStripMenuItem1";
            this.восстановитьToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.восстановитьToolStripMenuItem1.Size = new System.Drawing.Size(191, 22);
            this.восстановитьToolStripMenuItem1.Text = "&Восстановить";
            this.восстановитьToolStripMenuItem1.Click += new System.EventHandler(this.redo_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(188, 6);
            // 
            // вырезатьToolStripMenuItem
            // 
            this.вырезатьToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("вырезатьToolStripMenuItem.Image")));
            this.вырезатьToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.вырезатьToolStripMenuItem.Name = "вырезатьToolStripMenuItem";
            this.вырезатьToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.вырезатьToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.вырезатьToolStripMenuItem.Text = "Вырезат&ь";
            this.вырезатьToolStripMenuItem.Click += new System.EventHandler(this.cut_Click);
            // 
            // копироватьToolStripMenuItem
            // 
            this.копироватьToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("копироватьToolStripMenuItem.Image")));
            this.копироватьToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.копироватьToolStripMenuItem.Name = "копироватьToolStripMenuItem";
            this.копироватьToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.копироватьToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.копироватьToolStripMenuItem.Text = "&Копировать";
            this.копироватьToolStripMenuItem.Click += new System.EventHandler(this.copy_Click);
            // 
            // вставкаToolStripMenuItem
            // 
            this.вставкаToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("вставкаToolStripMenuItem.Image")));
            this.вставкаToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.вставкаToolStripMenuItem.Name = "вставкаToolStripMenuItem";
            this.вставкаToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.вставкаToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.вставкаToolStripMenuItem.Text = "Вст&авить";
            this.вставкаToolStripMenuItem.Click += new System.EventHandler(this.paste_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(188, 6);
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Image = global::PLNEditor.Properties.Resources.find;
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.findToolStripMenuItem.Text = "Найти";
            this.findToolStripMenuItem.Click += new System.EventHandler(this.find_Click);
            // 
            // findNextToolStripMenuItem
            // 
            this.findNextToolStripMenuItem.Enabled = false;
            this.findNextToolStripMenuItem.Image = global::PLNEditor.Properties.Resources.FindNext;
            this.findNextToolStripMenuItem.Name = "findNextToolStripMenuItem";
            this.findNextToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.findNextToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.findNextToolStripMenuItem.Text = "Найти далее...";
            this.findNextToolStripMenuItem.Click += new System.EventHandler(this.findAllToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(188, 6);
            // 
            // выделитьвсеToolStripMenuItem
            // 
            this.выделитьвсеToolStripMenuItem.Enabled = false;
            this.выделитьвсеToolStripMenuItem.Name = "выделитьвсеToolStripMenuItem";
            this.выделитьвсеToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.выделитьвсеToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.выделитьвсеToolStripMenuItem.Text = "Выделить &всё";
            this.выделитьвсеToolStripMenuItem.Click += new System.EventHandler(this.выделитьвсеToolStripMenuItem_Click);
            // 
            // сервисToolStripMenuItem
            // 
            this.сервисToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.настройкиToolStripMenuItem});
            this.сервисToolStripMenuItem.Name = "сервисToolStripMenuItem";
            this.сервисToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.сервисToolStripMenuItem.Text = "&Сервис";
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.Image = global::PLNEditor.Properties.Resources.options_small;
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.настройкиToolStripMenuItem.Text = "&Настройки";
            this.настройкиToolStripMenuItem.Click += new System.EventHandler(this.настройкиToolStripMenuItem_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.содержаниеToolStripMenuItem,
            this.toolStripSeparator5,
            this.опрограммеToolStripMenuItem});
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.справкаToolStripMenuItem.Text = "Помо&щь";
            // 
            // содержаниеToolStripMenuItem
            // 
            this.содержаниеToolStripMenuItem.Name = "содержаниеToolStripMenuItem";
            this.содержаниеToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.содержаниеToolStripMenuItem.Text = "&Справка";
            this.содержаниеToolStripMenuItem.Click += new System.EventHandler(this.содержаниеToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(155, 6);
            // 
            // опрограммеToolStripMenuItem
            // 
            this.опрограммеToolStripMenuItem.Name = "опрограммеToolStripMenuItem";
            this.опрограммеToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.опрограммеToolStripMenuItem.Text = "&О программе...";
            this.опрограммеToolStripMenuItem.Click += new System.EventHandler(this.опрограммеToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(238, 70);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(237, 22);
            this.toolStripMenuItem2.Text = "Закрыть";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.closeTabePage_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(237, 22);
            this.toolStripMenuItem3.Text = "Закрыть все, кроме текущего";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.closeAllTabPagesWithoutCurrent_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem1.Image")));
            this.toolStripMenuItem1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.toolStripMenuItem1.Size = new System.Drawing.Size(237, 22);
            this.toolStripMenuItem1.Text = "&Сохранить";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.save_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "PLN файлы|*.pln|Все файлы|*.*";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "PLN файлы|*.pln|Все файлы|*.*";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 75);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.errorListViewer1);
            this.splitContainer1.Size = new System.Drawing.Size(624, 346);
            this.splitContainer1.SplitterDistance = 243;
            this.splitContainer1.TabIndex = 11;
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.ContextMenuStrip = this.contextMenuStrip1;
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(624, 243);
            this.tabControl1.TabIndex = 9;
            // 
            // errorListViewer1
            // 
            this.errorListViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorListViewer1.Location = new System.Drawing.Point(0, 0);
            this.errorListViewer1.Name = "errorListViewer1";
            this.errorListViewer1.Size = new System.Drawing.Size(624, 99);
            this.errorListViewer1.TabIndex = 0;
            this.errorListViewer1.NeedNavigate += new System.EventHandler<PLNEditor.Controls.VisualError>(this.errorListViewer1_NeedNavigate);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.binaryLinkLabel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 421);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(624, 20);
            this.panel1.TabIndex = 12;
            // 
            // binaryLinkLabel1
            // 
            this.binaryLinkLabel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.binaryLinkLabel1.FalseText = "Спрятать список ошибок";
            this.binaryLinkLabel1.Location = new System.Drawing.Point(0, 0);
            this.binaryLinkLabel1.Name = "binaryLinkLabel1";
            this.binaryLinkLabel1.Size = new System.Drawing.Size(136, 18);
            this.binaryLinkLabel1.State = true;
            this.binaryLinkLabel1.TabIndex = 0;
            this.binaryLinkLabel1.TabStop = true;
            this.binaryLinkLabel1.Text = "Показать список ошибок";
            this.binaryLinkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.binaryLinkLabel1.TrueText = "Показать список ошибок";
            this.binaryLinkLabel1.StateChanged += new System.EventHandler(this.binaryLinkLabel1_StateChanged);
            // 
            // gridPanel1
            // 
            this.gridPanel1.ColumnCount = 8;
            this.gridPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.gridPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.gridPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.gridPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.gridPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.gridPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.gridPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.gridPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.gridPanel1.Controls.Add(this.pastleFlatButton, 6, 0);
            this.gridPanel1.Controls.Add(this.copyFlatButton, 5, 0);
            this.gridPanel1.Controls.Add(this.cutFlatButton, 4, 0);
            this.gridPanel1.Controls.Add(this.runFlatButton, 3, 0);
            this.gridPanel1.Controls.Add(this.newFlatButton, 0, 0);
            this.gridPanel1.Controls.Add(this.openFlatButton, 1, 0);
            this.gridPanel1.Controls.Add(this.saveFlatButton, 2, 0);
            this.gridPanel1.Controls.Add(this.gridPanel2, 7, 0);
            this.gridPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.gridPanel1.Location = new System.Drawing.Point(0, 24);
            this.gridPanel1.Name = "gridPanel1";
            this.gridPanel1.RowCount = 1;
            this.gridPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            this.gridPanel1.Size = new System.Drawing.Size(624, 51);
            this.gridPanel1.TabIndex = 5;
            // 
            // pastleFlatButton
            // 
            this.pastleFlatButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pastleFlatButton.BackgroundImage")));
            this.pastleFlatButton.DefaultStyleIndex = 0;
            this.pastleFlatButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pastleFlatButton.Location = new System.Drawing.Point(444, 0);
            this.pastleFlatButton.Margin = new System.Windows.Forms.Padding(0);
            this.pastleFlatButton.Name = "pastleFlatButton";
            this.pastleFlatButton.Size = new System.Drawing.Size(74, 51);
            this.pastleFlatButton.TabIndex = 22;
            this.pastleFlatButton.Text = "Вставить";
            this.pastleFlatButton.Click += new System.EventHandler(this.paste_Click);
            // 
            // copyFlatButton
            // 
            this.copyFlatButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("copyFlatButton.BackgroundImage")));
            this.copyFlatButton.DefaultStyleIndex = 0;
            this.copyFlatButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.copyFlatButton.Location = new System.Drawing.Point(370, 0);
            this.copyFlatButton.Margin = new System.Windows.Forms.Padding(0);
            this.copyFlatButton.Name = "copyFlatButton";
            this.copyFlatButton.Size = new System.Drawing.Size(74, 51);
            this.copyFlatButton.TabIndex = 21;
            this.copyFlatButton.Text = "Копировать";
            this.copyFlatButton.Click += new System.EventHandler(this.copy_Click);
            // 
            // cutFlatButton
            // 
            this.cutFlatButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cutFlatButton.BackgroundImage")));
            this.cutFlatButton.DefaultStyleIndex = 0;
            this.cutFlatButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cutFlatButton.Location = new System.Drawing.Point(296, 0);
            this.cutFlatButton.Margin = new System.Windows.Forms.Padding(0);
            this.cutFlatButton.Name = "cutFlatButton";
            this.cutFlatButton.Size = new System.Drawing.Size(74, 51);
            this.cutFlatButton.TabIndex = 20;
            this.cutFlatButton.Text = "Вырезать";
            this.cutFlatButton.Click += new System.EventHandler(this.cut_Click);
            // 
            // runFlatButton
            // 
            this.runFlatButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("runFlatButton.BackgroundImage")));
            this.runFlatButton.DefaultStyleIndex = 0;
            this.runFlatButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runFlatButton.Location = new System.Drawing.Point(222, 0);
            this.runFlatButton.Margin = new System.Windows.Forms.Padding(0);
            this.runFlatButton.Name = "runFlatButton";
            this.runFlatButton.Size = new System.Drawing.Size(74, 51);
            this.runFlatButton.TabIndex = 19;
            this.runFlatButton.Text = "Запустить";
            this.runFlatButton.Click += new System.EventHandler(this.runFlatButton_Click);
            // 
            // newFlatButton
            // 
            this.newFlatButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("newFlatButton.BackgroundImage")));
            this.newFlatButton.DefaultStyleIndex = 0;
            this.newFlatButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newFlatButton.Location = new System.Drawing.Point(0, 0);
            this.newFlatButton.Margin = new System.Windows.Forms.Padding(0);
            this.newFlatButton.Name = "newFlatButton";
            this.newFlatButton.Size = new System.Drawing.Size(74, 51);
            this.newFlatButton.TabIndex = 17;
            this.newFlatButton.Text = "Новый";
            this.newFlatButton.Click += new System.EventHandler(this.newTabPage_Click);
            // 
            // openFlatButton
            // 
            this.openFlatButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("openFlatButton.BackgroundImage")));
            this.openFlatButton.DefaultStyleIndex = 0;
            this.openFlatButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.openFlatButton.Location = new System.Drawing.Point(74, 0);
            this.openFlatButton.Margin = new System.Windows.Forms.Padding(0);
            this.openFlatButton.Name = "openFlatButton";
            this.openFlatButton.Size = new System.Drawing.Size(74, 51);
            this.openFlatButton.TabIndex = 16;
            this.openFlatButton.Text = "Открыть";
            this.openFlatButton.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // saveFlatButton
            // 
            this.saveFlatButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("saveFlatButton.BackgroundImage")));
            this.saveFlatButton.DefaultStyleIndex = 0;
            this.saveFlatButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveFlatButton.Location = new System.Drawing.Point(148, 0);
            this.saveFlatButton.Margin = new System.Windows.Forms.Padding(0);
            this.saveFlatButton.Name = "saveFlatButton";
            this.saveFlatButton.Size = new System.Drawing.Size(74, 51);
            this.saveFlatButton.TabIndex = 15;
            this.saveFlatButton.Text = "Сохранить";
            this.saveFlatButton.Click += new System.EventHandler(this.save_Click);
            // 
            // gridPanel2
            // 
            this.gridPanel2.ColumnCount = 1;
            this.gridPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.gridPanel2.Controls.Add(this.redoFlatButton, 0, 1);
            this.gridPanel2.Controls.Add(this.undoFlatButton, 0, 0);
            this.gridPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPanel2.Location = new System.Drawing.Point(518, 0);
            this.gridPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.gridPanel2.Name = "gridPanel2";
            this.gridPanel2.RowCount = 2;
            this.gridPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.gridPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.gridPanel2.Size = new System.Drawing.Size(106, 51);
            this.gridPanel2.TabIndex = 23;
            // 
            // redoFlatButton
            // 
            this.redoFlatButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("redoFlatButton.BackgroundImage")));
            this.redoFlatButton.DefaultStyleIndex = 0;
            this.redoFlatButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.redoFlatButton.Enabled = false;
            this.redoFlatButton.Location = new System.Drawing.Point(0, 25);
            this.redoFlatButton.Margin = new System.Windows.Forms.Padding(0);
            this.redoFlatButton.Name = "redoFlatButton";
            this.redoFlatButton.Size = new System.Drawing.Size(106, 26);
            this.redoFlatButton.TabIndex = 24;
            this.redoFlatButton.Click += new System.EventHandler(this.redo_Click);
            // 
            // undoFlatButton
            // 
            this.undoFlatButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("undoFlatButton.BackgroundImage")));
            this.undoFlatButton.DefaultStyleIndex = 0;
            this.undoFlatButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.undoFlatButton.Enabled = false;
            this.undoFlatButton.Location = new System.Drawing.Point(0, 0);
            this.undoFlatButton.Margin = new System.Windows.Forms.Padding(0);
            this.undoFlatButton.Name = "undoFlatButton";
            this.undoFlatButton.Size = new System.Drawing.Size(106, 25);
            this.undoFlatButton.TabIndex = 23;
            this.undoFlatButton.Click += new System.EventHandler(this.undo_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.gridPanel1);
            this.Controls.Add(this.mainMenuStrip);
            this.Controls.Add(this.panel1);
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PLNКод";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.gridPanel1.ResumeLayout(false);
            this.gridPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private GridPanel gridPanel1;
        private Controls.FlatButton saveFlatButton;
        private Controls.FlatButton runFlatButton;
        private Controls.FlatButton newFlatButton;
        private Controls.FlatButton openFlatButton;
        private Controls.FlatButton pastleFlatButton;
        private Controls.FlatButton copyFlatButton;
        private Controls.FlatButton cutFlatButton;
        private GridPanel gridPanel2;
        private Controls.FlatButton redoFlatButton;
        private Controls.FlatButton undoFlatButton;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem создатьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитькакToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem печатьToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem правкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отменитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem восстановитьToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem вырезатьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem копироватьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem вставкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem выделитьвсеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сервисToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem содержаниеToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem опрограммеToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem закрытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem закрытьвсеToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findNextToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private Controls.ErrorListViewer errorListViewer1;
        private System.Windows.Forms.Panel panel1;
        private Controls.BinaryLinkLabel binaryLinkLabel1;
    }
}

