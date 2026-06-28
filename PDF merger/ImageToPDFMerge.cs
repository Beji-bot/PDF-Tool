using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace PDF_merger
{
    public partial class ImageToPDFMerge : Form
    {
        private readonly ImageListManager _manager = new ImageListManager();
        private readonly PdfImageCreator _pdfCreator = new PdfImageCreator();
        private ContextMenuStrip _menu;
        private bool _isInternalDrag;
        private List<int> _draggedIndices = new List<int>();

        public ImageToPDFMerge()
        {
            InitializeComponent();
            InitializeUi();
        }

        private void ImageToPDFMerge_Load(object sender, EventArgs e)
        {
            label2.Text = "0%";
            RefreshListView();
        }

        private void InitializeUi()
        {
            InitializeContextMenu();
            typeof(ListView)
    .GetProperty("DoubleBuffered",
        System.Reflection.BindingFlags.NonPublic |
        System.Reflection.BindingFlags.Instance)
    ?.SetValue(listView1, true);
            // ListView setup
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.MultiSelect = true;
            listView1.GridLines = true;
            listView1.HideSelection = false;
            listView1.AllowDrop = true;

            if (listView1.Columns.Count == 0)
            {
                listView1.Columns.Add("No.", 60);
                listView1.Columns.Add("File name", 300);
                listView1.Columns.Add("Path", 500);
                listView1.Columns.Add("Format", 80);
            }

            // Events
            listView1.ItemDrag += listView1_ItemDrag;
            listView1.DragEnter += listView1_DragEnter;
            listView1.DragOver += listView1_DragOver;
            listView1.DragDrop += listView1_DragDrop;
            listView1.KeyDown += listView1_KeyDown;
        }
        private void OpenFolderItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            string file =
                listView1.SelectedItems[0].SubItems[2].Text;

            if (File.Exists(file))
            {
                Process.Start("explorer.exe",
                    "/select,\"" + file + "\"");
            }
        }
        private void OpenItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            string file =
                listView1.SelectedItems[0].SubItems[2].Text;

            if (File.Exists(file))
                Process.Start(file);
        }

        private void EnsureSelectedVisible()
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            listView1.BeginUpdate();

            listView1.SelectedItems[0].EnsureVisible();

            listView1.EndUpdate();
        }
        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                foreach (ListViewItem item in listView1.Items)
                    item.Selected = true;

                e.SuppressKeyPress = true;
                return;
            }

            if (e.KeyCode == Keys.Delete)
            {
                RemoveSelectedItems();
                e.SuppressKeyPress = true;
            }
        }
        private void InitializeContextMenu()
        {
            _menu = new ContextMenuStrip();

            ToolStripMenuItem openItem =
                new ToolStripMenuItem("Open File");

            ToolStripMenuItem openFolderItem =
                new ToolStripMenuItem("Open Folder");

            ToolStripMenuItem selectAllItem =
                new ToolStripMenuItem("Select All");

            openItem.Click += OpenItem_Click;
            openFolderItem.Click += OpenFolderItem_Click;
            selectAllItem.Click += SelectAllItem_Click;

            _menu.Items.Add(openItem);
            _menu.Items.Add(openFolderItem);
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(selectAllItem);

            listView1.ContextMenuStrip = _menu;
        }
        private void SelectAllItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
                item.Selected = true;
        }
        private void RefreshListView(IEnumerable<string> selectedPaths = null)
        {
            listView1.BeginUpdate();

            try
            {
                listView1.Items.Clear();

                var selected =
                    selectedPaths != null
                    ? new HashSet<string>(selectedPaths)
                    : new HashSet<string>();

                for (int i = 0; i < _manager.Items.Count; i++)
                {
                    var item = _manager.Items[i];

                    ListViewItem lvi =
                        new ListViewItem((i + 1).ToString());

                    lvi.SubItems.Add(item.FileName);
                    lvi.SubItems.Add(item.FilePath);
                    lvi.SubItems.Add(item.FileExtension);

                    if (selected.Contains(item.FilePath))
                        lvi.Selected = true;

                    listView1.Items.Add(lvi);
                }
            }
            finally
            {
                listView1.EndUpdate();
            }
        }

        private List<string> GetSelectedPaths()
        {
            return listView1.SelectedItems
                .Cast<ListViewItem>()
                .Select(x => x.SubItems[2].Text)
                .ToList();
        }

        private List<int> GetSelectedIndices()
        {
            return listView1.SelectedIndices
                .Cast<int>()
                .OrderBy(x => x)
                .ToList();
        }


        private void AddFilesToList(IEnumerable<string> filePaths)
        {
            var selectedBefore = GetSelectedPaths();
            var added = new List<string>();
            var invalid = new List<string>();
            int totalProcessed = 0;  // count all files dropped (even duplicates)

            foreach (var file in filePaths)
            {
                totalProcessed++;
                var result = _manager.TryAddImage(file);
                switch (result)
                {
                    case ImageListManager.AddResult.Success:
                        added.Add(file);
                        break;
                    case ImageListManager.AddResult.Duplicate:
                        // Silently skip duplicates – no message
                        break;
                    case ImageListManager.AddResult.InvalidImage:
                    case ImageListManager.AddResult.Error:
                        invalid.Add(file);
                        break;
                }
            }

            RefreshListView(selectedBefore);

            // Show warning ONLY for truly invalid images
            if (invalid.Count > 0)
            {
                MessageBox.Show(
                    "These files were skipped because they are not valid images:\n\n" +
                    string.Join("\n", invalid.Select(Path.GetFileName)),
                    "Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            // Only show "No files were added" if the drop was completely empty
            // (practically never happens, but just in case)
            if (totalProcessed == 0)
            {
                MessageBox.Show("No files were added.", "Info");
            }
            // Otherwise, if nothing was added and there were no invalid files,
            // it means only duplicates were dropped – we say nothing.
        }

        private void RemoveSelectedItems()
        {
            var selectedIndices = GetSelectedIndices();
            if (selectedIndices.Count == 0)
                return;

            var selectedPaths = GetSelectedPaths();
            _manager.RemoveAtIndices(selectedIndices);
            RefreshListView(selectedPaths);
        }

        private void MoveSelectedUp()
        {
            var selectedIndices = GetSelectedIndices();
            if (!selectedIndices.Any() || selectedIndices.First() == 0)
                return;

            var selectedPaths = GetSelectedPaths();
            _manager.MoveBlockUp(selectedIndices);
            RefreshListView(selectedPaths);
            EnsureSelectedVisible();
        }

        private void MoveSelectedDown()
        {
            var selectedIndices = GetSelectedIndices();
            if (!selectedIndices.Any() || selectedIndices.Last() == _manager.Items.Count - 1)
                return;

            var selectedPaths = GetSelectedPaths();
            _manager.MoveBlockDown(selectedIndices);
            RefreshListView(selectedPaths);
            EnsureSelectedVisible();
        }

        private void MoveSelectedFirst()
        {
            var selectedPaths = GetSelectedPaths();

            _manager.MoveBlockToTop(GetSelectedIndices());

            RefreshListView(selectedPaths);

            if (listView1.Items.Count > 0)
                listView1.Items[0].EnsureVisible();
        }

        private void MoveSelectedLast()
        {
            var selectedPaths = GetSelectedPaths();

            _manager.MoveBlockToBottom(GetSelectedIndices());

            RefreshListView(selectedPaths);

            if (listView1.Items.Count > 0)
                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
        }

        private void MoveSelectedToPosition()
        {
            var selectedIndices = GetSelectedIndices();
            if (!selectedIndices.Any())
                return;

            using (var dialog = new MoveToPositionForm(_manager.Items.Count))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                var selectedPaths = GetSelectedPaths();
                _manager.MoveBlockToPosition(selectedIndices, dialog.SelectedPosition);
                RefreshListView(selectedPaths);
            }
        }

        private void SortAscending()
        {
            var selectedPaths = GetSelectedPaths();
            _manager.SortAscending();
            RefreshListView(selectedPaths);
        }

        private void SortDescending()
        {
            var selectedPaths = GetSelectedPaths();
            _manager.SortDescending();
            RefreshListView(selectedPaths);
        }

        private void CreatePdf()
        {
            if (_manager.Items.Count == 0)
            {
                MessageBox.Show("List is empty.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "PDF file (*.pdf)|*.pdf";
                saveDialog.Title = "Save PDF";
                saveDialog.FileName = "Images.pdf";

                if (saveDialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    Cursor = Cursors.WaitCursor;
                    label2.Text = "0%";
                    Application.DoEvents();

                    _pdfCreator.CreatePdf(
                        _manager.Items,
                        saveDialog.FileName,
                        (current, total) =>
                        {
                            int percent = (int)Math.Round((current * 100.0) / total);
                            label2.Text = percent + "%";
                            Application.DoEvents();
                        });

                    label2.Text = "Done";
                    MessageBox.Show("PDF created successfully.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    label2.Text = "Error";
                    MessageBox.Show(ex.Message, "PDF error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            _isInternalDrag = true;
            _draggedIndices = GetSelectedIndices();
            listView1.DoDragDrop(_draggedIndices, DragDropEffects.Move);
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
                return;
            }

            if (e.Data.GetDataPresent(typeof(List<int>)))
            {
                e.Effect = DragDropEffects.Move;
                return;
            }

            e.Effect = DragDropEffects.None;
        }

        private void listView1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
                return;
            }

            if (e.Data.GetDataPresent(typeof(List<int>)))
            {
                e.Effect = DragDropEffects.Move;
                return;
            }

            e.Effect = DragDropEffects.None;
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            Point clientPoint = listView1.PointToClient(new Point(e.X, e.Y));
            ListViewItem targetItem = listView1.GetItemAt(clientPoint.X, clientPoint.Y);
            int targetIndex = targetItem != null ? targetItem.Index : listView1.Items.Count;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                AddFilesToList(files);
                return;
            }

            if (e.Data.GetDataPresent(typeof(List<int>)))
            {
                var draggedIndices = (List<int>)e.Data.GetData(typeof(List<int>));
                if (draggedIndices == null || draggedIndices.Count == 0)
                    return;

                draggedIndices = draggedIndices.Distinct().OrderBy(x => x).ToList();

                if (draggedIndices.Contains(targetIndex) || draggedIndices.Contains(Math.Max(0, targetIndex - 1)))
                    return;

                var selectedPaths = GetSelectedPaths();
                _manager.MoveBlockBeforeOriginalIndex(draggedIndices, targetIndex);
                RefreshListView(selectedPaths);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreatePdf();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var openDialog = new OpenFileDialog())
            {
                openDialog.Title = "Select images";
                openDialog.Filter = "Image files|*.bmp;*.dib;*.rle;*.jpg;*.jpeg;*.jpe;*.png;*.gif;*.tif;*.tiff|All files|*.*";
                openDialog.Multiselect = true;

                if (openDialog.ShowDialog(this) != DialogResult.OK)
                    return;

                AddFilesToList(openDialog.FileNames);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RemoveSelectedItems();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MoveSelectedUp();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MoveSelectedToPosition();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MoveSelectedDown();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SortAscending();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MoveSelectedLast();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SortDescending();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MoveSelectedFirst();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void ImageToPDFMerge_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(1);
        }
    }    

    

    

    

    
    
    
}
