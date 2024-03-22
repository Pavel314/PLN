using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.IO;
using PLNCompiler.Syntax;

namespace PLNEditor.Controls
{
   public class ErrorListViewer:UserControl
    {
       protected ListView ListView;
        public event EventHandler<VisualError> NeedNavigate;

        public ErrorViewerErrorCollection Items { get; private set; }

        public ErrorListViewer()
        {
            ListView = new ListView();
            ListView.View = View.Details;
            ListView.GridLines = true;
            ListView.BorderStyle = BorderStyle.FixedSingle;
            ListView.Columns.Add("Строка");
            ListView.Columns.Add("Описание");
            ListView.Columns.Add("Файл");
            ListView.Dock = DockStyle.Fill;
            ListView.FullRowSelect = true;
            ListView.ItemActivate += ListView_ItemActivate;

            Items = new ErrorViewerErrorCollection(ListView.Items);

            ContextMenuStrip = new ContextMenuStrip();
            ContextMenuStrip.Items.Add("Скопировать",null,copy_Click);
            ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            this.Controls.Add(ListView);
        }

        private VisualError GetSelectedItem()
        {
            return Items[ListView.SelectedIndices[0]];
        }

        private void copy_Click(object sender, EventArgs e)
        {
            var item = GetSelectedItem();
            Clipboard.SetText(string.Format("{0} {1} {2}", item.Location.StartColumn, item.Description, item.FileName));
        }

        protected virtual void OnNeedNavigate(VisualError error) => NeedNavigate?.Invoke(this, error);

        private void ListView_ItemActivate(object sender, EventArgs e)
        {
            OnNeedNavigate(GetSelectedItem());
        }

        public void NavigateToFirstError()
        {
            if (Items.IsNullOrEmpty()) return;
            OnNeedNavigate(Items[0]);
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
                ContextMenuStrip.Items[0].Enabled = ListView.SelectedItems != null && ListView.SelectedItems.Count > 0;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            var width = ListView.ClientSize.Width;
            ListView.Columns[0].Width = 60;
            ListView.Columns[1].Width = Convert.ToInt32(Math.Round(width / 100.0 * 70.0));
            ListView.Columns[2].Width = width - ListView.Columns[0].Width - ListView.Columns[1].Width;
        }

        public class ErrorViewerErrorCollection : IList<VisualError>, ICollection<VisualError>, IEnumerable<VisualError>
        {
            private ListView.ListViewItemCollection Parent;
            private List<VisualError> List;

            public ErrorViewerErrorCollection(ListView.ListViewItemCollection parent)
            {
                List = new List<VisualError>();
                Parent = parent;
            }

            public VisualError this[int index] { set { List[index] = value; Parent[index] = ToItem(value); } get { return List[index]; } }

            public int Count => ((ICollection<VisualError>)List).Count;

            public bool IsReadOnly => Parent.IsReadOnly;


            public void Add(VisualError item)
            {
                List.Add(item);
                Parent.Add(ToItem(item));
            }

            public void Clear()
            {
                Parent.Clear();
                List.Clear();
            }

            public bool Contains(VisualError item)
            {
                return ((ICollection<VisualError>)List).Contains(item);
            }

            public void CopyTo(VisualError[] array, int arrayIndex)
            {
                ((ICollection<VisualError>)List).CopyTo(array, arrayIndex);
            }

            public IEnumerator<VisualError> GetEnumerator()
            {
                return ((IEnumerable<VisualError>)List).GetEnumerator();
            }

            public int IndexOf(VisualError item)
            {
                return ((IList<VisualError>)List).IndexOf(item);
            }

            public void Insert(int index, VisualError item)
            {
                List.Insert(index, item);
                Parent.Insert(0, ToItem(item));
            }

            public bool Remove(VisualError item)
            {
                var ind = List.IndexOf(item);
                if (ind == -1) return false;
                Parent.RemoveAt(ind);
                List.RemoveAt(ind);
                return true;
            }

            public void RemoveAt(int index)
            {
                List.RemoveAt(index);
                Parent.RemoveAt(index);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable<VisualError>)List).GetEnumerator();
            }

            protected ListViewItem ToItem(VisualError item)
            {
                return new ListViewItem(new string[3] { item.Location.StartLine.ToString(), item.Description, item.FileName});
            }
        }

    }

    public class VisualError
    {
        public Location Location { get; private set; }
        public string Description { get; private set; }
        public string FileName { get; private set; }
        public object Parent { get; private set; }

        public VisualError(Location location, string description, string fileName,object parent)
        {
            Location = location;
            Description = description;
            FileName = fileName;
            Parent = parent;
        }
    }


}
