using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDF_merger
{
    public partial class MergePDF : Form
    {
        // Central list to store file paths. The ListView is rebuilt from this list.
        private List<string> filePaths = new List<string>();

        // Native Windows API for Natural Sorting (1, 2, 3, 10, 11 instead of 1, 10, 11, 2, 3)
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string psz1, string psz2);
        public MergePDF()
        {
            InitializeComponent();
            // Fix for PdfSharp encoding issues in newer .NET versions
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            // 2. Enable Double Buffering here for the Form
            this.DoubleBuffered = true;

            // 3. Apply the specific optimization for the ListView control
            typeof(ListView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null, listView1, new object[] { true });
            SetupListView();
            SetupLayout();
        }

        private void MergePDF_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            this.KeyDown += MergePDF_KeyDown;
        }
        private void SetupLayout()
        {
            // --- Anchor Settings ---
            // Make ListView stretch with the window
            listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Anchor the Status labels to the bottom-left
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            // Anchor buttons on the right side
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button8.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button9.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button10.Anchor = AnchorStyles.Bottom | AnchorStyles.Right; // Merge button at bottom right
        }
        private void SetupListView()
        {
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.AllowDrop = true;

            // Adjusted order: No, Name, Format, Path
            listView1.Columns.Add("No.", 50);
            listView1.Columns.Add("File Name", 250);
            listView1.Columns.Add("Format", 70);
            listView1.Columns.Add("Path", 300);

            // Right-Click Context Menu
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem openItem = new ToolStripMenuItem("Open File");
            openItem.Click += (s, e) =>
            {
                if (listView1.SelectedItems.Count > 0)
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(listView1.SelectedItems[0].Tag.ToString()) { UseShellExecute = true });
            };
            contextMenu.Items.Add(openItem);
            listView1.ContextMenuStrip = contextMenu;
        }

        private int? ShowMoveToDialog()
        {
            // Use explicit namespaces to resolve CS0104
            System.Windows.Forms.Form prompt = new System.Windows.Forms.Form();
            prompt.Width = 380;
            prompt.Height = 220;
            prompt.Text = "Move to Position";
            prompt.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            prompt.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;

            prompt.MaximizeBox = false;
            prompt.MinimizeBox = false;

            Font boldFont = new Font("Segoe UI", 12f, FontStyle.Bold);

            // Explicitly use System.Windows.Forms.Label
            System.Windows.Forms.Label label = new System.Windows.Forms.Label
            {
                Left = 20,
                Top = 20,
                Text = $"Enter position (1 - {filePaths.Count}):",
                AutoSize = true,
                Font = boldFont
            };

            // Explicitly use System.Windows.Forms.NumericUpDown
            System.Windows.Forms.NumericUpDown numeric = new System.Windows.Forms.NumericUpDown
            {
                Left = 20,
                Top = 70,
                Minimum = 1,
                Maximum = filePaths.Count,
                Value = 1,
                Width = 150,
                Font = boldFont
            };

            // Explicitly use System.Windows.Forms.Button
            System.Windows.Forms.Button confirmation = new System.Windows.Forms.Button
            {
                Text = "Move",
                Left = 200,
                Top = 65,
                Width = 120,
                Height = 40,
                DialogResult = System.Windows.Forms.DialogResult.OK,
                Font = boldFont
            };

            prompt.Controls.Add(label);
            prompt.Controls.Add(numeric);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            int? result = null;
            if (prompt.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                result = (int)numeric.Value - 1;
            }

            prompt.Dispose();
            boldFont.Dispose();
            return result;
        }

        // Helper to keep items selected after moving
        private void RefreshAndReselect(List<string> selectedPaths)
        {
            listView1.BeginUpdate();
            RefreshListView();

            ListViewItem lastItem = null;
            foreach (ListViewItem item in listView1.Items)
            {
                if (selectedPaths.Contains(item.Tag.ToString()))
                {
                    item.Selected = true;
                    lastItem = item;
                }
            }

            // Auto-scroll to ensure visibility
            if (lastItem != null) lastItem.EnsureVisible();

            listView1.EndUpdate();
            listView1.Focus();
        }

        private void RefreshListView()
        {
            listView1.Items.Clear();
            for (int i = 0; i < filePaths.Count; i++)
            {
                string path = filePaths[i];
                ListViewItem item = new ListViewItem((i + 1).ToString());
                item.SubItems.Add(Path.GetFileName(path));
                item.SubItems.Add(Path.GetExtension(path));
                item.SubItems.Add(path);
                item.Tag = path;
                listView1.Items.Add(item);
            }

            // Adjust column widths automatically
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            /*
            listView1.Items.Clear();

            // Determine padding based on count (e.g., if 100 items, pad to 001)
            int count = filePaths.Count;
            string format = count >= 1000 ? "D4" : (count >= 100 ? "D3" : (count >= 10 ? "D2" : "D1"));

            for (int i = 0; i < filePaths.Count; i++)
            {
                string path = filePaths[i];
                string fileName = Path.GetFileNameWithoutExtension(path);
                string extension = Path.GetExtension(path);

                ListViewItem item = new ListViewItem((i + 1).ToString(format));
                item.SubItems.Add(fileName);
                item.SubItems.Add(path);
                item.SubItems.Add(extension);

                // Keep track of the actual list index in the Tag for easy reference
                item.Tag = path;
                listView1.Items.Add(item);
            }*/
        }
        // Delete key integration
        private void MergePDF_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && listView1.SelectedItems.Count > 0)
            {
                button2_Click(sender, e);
            }
        }

        // Button 1 - Add file(s)
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = true;
                ofd.Filter = "PDF Files|*.pdf";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in ofd.FileNames)
                    {
                        if (!filePaths.Contains(file))
                            filePaths.Add(file);
                    }
                    RefreshListView();
                }
            }
        }

        // Button 2 - Remove file(s)
        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            // Iterate backwards to safely remove from list
            for (int i = listView1.SelectedIndices.Count - 1; i >= 0; i--)
            {
                int index = listView1.SelectedIndices[i];
                filePaths.RemoveAt(index);
            }
            RefreshListView();
        }

        // Button 3 - Move to specific number
        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            // Call the helper to show the dialog
            int? targetIndex = ShowMoveToDialog();

            if (targetIndex.HasValue)
            {
                MoveSelectedItems(targetIndex.Value);
            }

            /*if (listView1.SelectedItems.Count == 0) return;

            using (Form prompt = new Form())
            {
                prompt.Width = 300;
                prompt.Height = 150;
                prompt.Text = "Move to Position";
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.MaximizeBox = false;

                Label label = new Label() { Left = 20, Top = 20, Text = "Enter new position (1 - " + filePaths.Count + "):", Width = 200 };
                NumericUpDown numeric = new NumericUpDown() { Left = 20, Top = 50, Minimum = 1, Maximum = filePaths.Count, Width = 100 };
                Button confirmation = new Button() { Text = "Move", Left = 150, Top = 50, DialogResult = DialogResult.OK };

                prompt.Controls.Add(label);
                prompt.Controls.Add(numeric);
                prompt.Controls.Add(confirmation);
                prompt.AcceptButton = confirmation;

                if (prompt.ShowDialog() == DialogResult.OK)
                {
                    int targetIndex = (int)numeric.Value - 1;
                    MoveSelectedItems(targetIndex);
                }
            }*/
        }

        // Button 4 - Move file(s) up
        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            // Get the paths of selected items to re-select them after the move
            List<string> selectedPaths = listView1.SelectedItems.Cast<ListViewItem>().Select(i => i.Tag.ToString()).ToList();

            // Check if the first selected item is already at the top
            if (listView1.SelectedIndices[0] == 0) return;

            // Loop through selected items (top to bottom) and swap with the one above
            for (int i = 0; i < listView1.SelectedIndices.Count; i++)
            {
                int index = listView1.SelectedIndices[i];
                string temp = filePaths[index];
                filePaths[index] = filePaths[index - 1];
                filePaths[index - 1] = temp;
            }

            RefreshAndReselect(selectedPaths);
        }

        // Button 5 - Move file(s) down
        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            List<string> selectedPaths = listView1.SelectedItems.Cast<ListViewItem>().Select(i => i.Tag.ToString()).ToList();

            // Remove selected items from current positions
            foreach (var path in selectedPaths) filePaths.Remove(path);
            // Insert them at the start
            filePaths.InsertRange(0, selectedPaths);

            RefreshAndReselect(selectedPaths);
        }

        // Button 6 - Move to first position
        private void button6_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            List<string> selectedPaths = listView1.SelectedItems.Cast<ListViewItem>().Select(i => i.Tag.ToString()).ToList();

            // Check if the last selected item is already at the bottom
            if (listView1.SelectedIndices[listView1.SelectedIndices.Count - 1] == filePaths.Count - 1) return;

            // Loop backwards (bottom to top) to prevent index shifting errors during swap
            for (int i = listView1.SelectedIndices.Count - 1; i >= 0; i--)
            {
                int index = listView1.SelectedIndices[i];
                string temp = filePaths[index];
                filePaths[index] = filePaths[index + 1];
                filePaths[index + 1] = temp;
            }

            RefreshAndReselect(selectedPaths);
        }

        // Button 7 - Move to last position
        private void button7_Click(object sender, EventArgs e)
        {
            // Sort ASC
            filePaths.Sort((a, b) => StrCmpLogicalW(Path.GetFileName(a), Path.GetFileName(b)));
            RefreshListView(); // No need to reselect here as the order has changed globally
        }

        // Button 8 - Order ASC (Natural Sort)
        private void button8_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            List<string> selectedPaths = listView1.SelectedItems.Cast<ListViewItem>().Select(i => i.Tag.ToString()).ToList();

            // Remove and add to the end of the list
            foreach (var path in selectedPaths) filePaths.Remove(path);
            filePaths.AddRange(selectedPaths);

            RefreshAndReselect(selectedPaths);
        }

        // Button 9 - Order DESC (Natural Sort)
        private void button9_Click(object sender, EventArgs e)
        {
            // Sort DESC
            filePaths.Sort((a, b) => StrCmpLogicalW(Path.GetFileName(b), Path.GetFileName(a)));
            RefreshListView();
        }

        private string statusMessage = "Waiting for action...";

        private void label1_Paint(object sender, PaintEventArgs e)
        {
            // Ensure smooth rendering
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            using (Font boldFont = new Font("Segoe UI", 10, FontStyle.Bold))
            using (Font regularFont = new Font("Segoe UI", 10, FontStyle.Regular))
            {
                // Draw "Status: " in Bold
                e.Graphics.DrawString("Status:", boldFont, Brushes.Black, 0, 0);

                // Measure the width of "Status: " to place the message next to it
                SizeF boldSize = e.Graphics.MeasureString("Status:", boldFont);

                // Draw the message in Regular font, starting right after "Status:"
                // We use a small offset (e.g., 2 pixels) for natural spacing
                e.Graphics.DrawString(statusMessage, regularFont, Brushes.Black, boldSize.Width + 2, 0);
            }
        }

        // Call this method whenever you need to update the text
        private void UpdateStatus(string message)
        {
            label2.Text = message;
            label2.Refresh(); // Ensures text updates immediately during loops
        }

        // Button 10 - Merge PDFs
        private void button10_Click(object sender, EventArgs e)
        {
            if (filePaths.Count < 2)
            {
                UpdateStatus("Error: Need at least 2 files.");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "PDF Files|*.pdf" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (PdfDocument outputDocument = new PdfDocument())
                        {
                            int totalFiles = filePaths.Count;
                            for (int i = 0; i < totalFiles; i++)
                            {
                                // Calculate percentage
                                int percent = (int)((double)(i + 1) / totalFiles * 100);
                                UpdateStatus($"Merging... {percent}%");

                                using (PdfDocument inputDocument = PdfReader.Open(filePaths[i], PdfDocumentOpenMode.Import))
                                {
                                    foreach (PdfPage page in inputDocument.Pages)
                                    {
                                        outputDocument.AddPage(page);
                                    }
                                }
                            }
                            outputDocument.Save(sfd.FileName);
                        }

                        // Final Success Status
                        UpdateStatus("DONE - Saved successfully.");
                    }
                    catch (Exception ex)
                    {
                        UpdateStatus("Error: " + ex.Message);
                    }
                }
            }
        }

        // --- HELPER METHODS FOR MOVEMENT AND DRAG&DROP ---

        private void SwapItems(int indexA, int indexB)
        {
            string temp = filePaths[indexA];
            filePaths[indexA] = filePaths[indexB];
            filePaths[indexB] = temp;
        }

        private void MoveSelectedItems(int targetIndex)
        {
            // 1. Capture selected paths before moving
            List<string> selectedPaths = listView1.SelectedItems.Cast<ListViewItem>()
                                                  .Select(i => i.Tag.ToString())
                                                  .ToList();

            // 2. Remove items from current positions
            foreach (string path in selectedPaths)
            {
                filePaths.Remove(path);
            }

            // 3. Ensure target is within bounds
            if (targetIndex > filePaths.Count) targetIndex = filePaths.Count;
            if (targetIndex < 0) targetIndex = 0;

            // 4. Insert at the new position
            filePaths.InsertRange(targetIndex, selectedPaths);

            // 5. Refresh UI and reselect items
            RefreshAndReselect(selectedPaths);
        }

        private void ReselectItemsAfterMove()
        {
            // Used after step-by-step up/down moves to keep selection alive
            listView1.Focus();
        }

        // --- DRAG AND DROP EVENTS ---

        private void ListView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            listView1.DoDragDrop(listView1.SelectedItems, DragDropEffects.Move);
        }

        private void ListView1_DragEnter(object sender, DragEventArgs e)
        {
            // Accept files dragged from Windows Explorer OR internal ListView items
            if (e.Data.GetDataPresent(DataFormats.FileDrop) || e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)))
            {
                e.Effect = DragDropEffects.Move; // Or Copy for files
            }
        }

        private void ListView1_DragDrop(object sender, DragEventArgs e)
        {
            // Handle External Files Drop
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    if (Path.GetExtension(file).ToLower() == ".pdf" && !filePaths.Contains(file))
                    {
                        filePaths.Add(file);
                    }
                }
                RefreshListView();
                return;
            }

            // Handle Internal Reordering Drop
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)))
            {
                Point cp = listView1.PointToClient(new Point(e.X, e.Y));
                ListViewItem dragToItem = listView1.GetItemAt(cp.X, cp.Y);

                int targetIndex = dragToItem != null ? dragToItem.Index : filePaths.Count;
                MoveSelectedItems(targetIndex);
            }
        }

        private void MergePDF_Resize(object sender, EventArgs e)
        {
            // Example: Dynamically resize the ListView width to fill 80% of form
            // and keep buttons aligned to the remaining 20%
            listView1.Width = (int)(this.ClientSize.Width * 0.8);
            listView1.Height = (int)(this.ClientSize.Height * 0.9);

            // Refresh column widths so text is always visible
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void MergePDF_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(1);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}