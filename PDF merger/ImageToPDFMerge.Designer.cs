namespace PDF_merger
{
    partial class ImageToPDFMerge
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            listView1 = new ListView();
            label1 = new Label();
            button2 = new Button();
            button3 = new Button();
            button9 = new Button();
            button7 = new Button();
            button4 = new Button();
            button6 = new Button();
            button5 = new Button();
            button8 = new Button();
            button10 = new Button();
            label2 = new Label();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 551);
            button1.Name = "button1";
            button1.Size = new Size(998, 40);
            button1.TabIndex = 0;
            button1.Text = "Create PDF";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // listView1
            // 
            listView1.Location = new Point(12, 39);
            listView1.Name = "listView1";
            listView1.Size = new Size(686, 461);
            listView1.TabIndex = 1;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.ItemDrag += listView1_ItemDrag;
            listView1.DragDrop += listView1_DragDrop;
            listView1.DragEnter += listView1_DragEnter;
            listView1.DragOver += listView1_DragOver;
            listView1.KeyDown += listView1_KeyDown;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 518);
            label1.Name = "label1";
            label1.Size = new Size(49, 20);
            label1.TabIndex = 2;
            label1.Text = "Status";
            // 
            // button2
            // 
            button2.Location = new Point(704, 39);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 3;
            button2.Text = "Add file";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(704, 74);
            button3.Name = "button3";
            button3.Size = new Size(94, 29);
            button3.TabIndex = 4;
            button3.Text = "Remove file";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button9
            // 
            button9.BackColor = Color.GhostWhite;
            button9.FlatStyle = FlatStyle.Flat;
            button9.Font = new Font("Meiryo", 9F, FontStyle.Bold);
            button9.Location = new Point(808, 119);
            button9.Name = "button9";
            button9.Size = new Size(90, 80);
            button9.TabIndex = 20;
            button9.Text = "DSC";
            button9.UseVisualStyleBackColor = false;
            button9.Click += button9_Click;
            // 
            // button7
            // 
            button7.BackColor = Color.GhostWhite;
            button7.FlatStyle = FlatStyle.Flat;
            button7.Font = new Font("Meiryo", 9F, FontStyle.Bold);
            button7.Location = new Point(712, 119);
            button7.Name = "button7";
            button7.Size = new Size(90, 80);
            button7.TabIndex = 19;
            button7.Text = "ASC";
            button7.UseVisualStyleBackColor = false;
            button7.Click += button7_Click;
            // 
            // button4
            // 
            button4.BackColor = Color.GhostWhite;
            button4.FlatStyle = FlatStyle.Flat;
            button4.Font = new Font("Meiryo", 9F, FontStyle.Bold);
            button4.Location = new Point(712, 205);
            button4.Name = "button4";
            button4.Size = new Size(90, 80);
            button4.TabIndex = 15;
            button4.Text = "Move UP";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // button6
            // 
            button6.BackColor = Color.GhostWhite;
            button6.FlatStyle = FlatStyle.Flat;
            button6.Font = new Font("Meiryo", 9F, FontStyle.Bold);
            button6.Location = new Point(712, 291);
            button6.Name = "button6";
            button6.Size = new Size(90, 80);
            button6.TabIndex = 16;
            button6.Text = "Move DOWN";
            button6.UseVisualStyleBackColor = false;
            button6.Click += button6_Click;
            // 
            // button5
            // 
            button5.BackColor = Color.GhostWhite;
            button5.FlatStyle = FlatStyle.Flat;
            button5.Font = new Font("Meiryo", 9F, FontStyle.Bold);
            button5.Location = new Point(712, 377);
            button5.Name = "button5";
            button5.Size = new Size(186, 80);
            button5.TabIndex = 14;
            button5.Text = "Move to position (number)";
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // button8
            // 
            button8.BackColor = Color.GhostWhite;
            button8.FlatStyle = FlatStyle.Flat;
            button8.Font = new Font("Meiryo", 9F, FontStyle.Bold);
            button8.Location = new Point(808, 291);
            button8.Name = "button8";
            button8.Size = new Size(90, 80);
            button8.TabIndex = 18;
            button8.Text = "Last";
            button8.UseVisualStyleBackColor = false;
            button8.Click += button8_Click;
            // 
            // button10
            // 
            button10.BackColor = Color.GhostWhite;
            button10.FlatStyle = FlatStyle.Flat;
            button10.Font = new Font("Meiryo", 9F, FontStyle.Bold);
            button10.Location = new Point(808, 205);
            button10.Name = "button10";
            button10.Size = new Size(90, 80);
            button10.TabIndex = 17;
            button10.Text = "First";
            button10.UseVisualStyleBackColor = false;
            button10.Click += button10_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(67, 518);
            label2.Name = "label2";
            label2.Size = new Size(12, 20);
            label2.TabIndex = 21;
            label2.Text = ".";
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1022, 28);
            menuStrip1.TabIndex = 22;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(224, 26);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // ImageToPDFMerge
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1022, 612);
            Controls.Add(label2);
            Controls.Add(button9);
            Controls.Add(button7);
            Controls.Add(button4);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button8);
            Controls.Add(button10);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(label1);
            Controls.Add(listView1);
            Controls.Add(button1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "ImageToPDFMerge";
            Text = "ImageToPDFMerge";
            FormClosing += ImageToPDFMerge_FormClosing;
            Load += ImageToPDFMerge_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private ListView listView1;
        private Label label1;
        private Button button2;
        private Button button3;
        private Button button9;
        private Button button7;
        private Button button4;
        private Button button6;
        private Button button5;
        private Button button8;
        private Button button10;
        private Label label2;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
    }
}