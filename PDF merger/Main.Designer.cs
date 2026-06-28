namespace PDF_merger
{
    partial class Main
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
            button2 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = SystemColors.GradientInactiveCaption;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Unispace", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 238);
            button1.Location = new Point(14, 90);
            button1.Margin = new Padding(4, 3, 4, 3);
            button1.Name = "button1";
            button1.Size = new Size(365, 74);
            button1.TabIndex = 0;
            button1.Text = "Image To PDF";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = SystemColors.GradientInactiveCaption;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Unispace", 19.8000011F, FontStyle.Bold);
            button2.Location = new Point(14, 11);
            button2.Margin = new Padding(4, 3, 4, 3);
            button2.Name = "button2";
            button2.Size = new Size(365, 74);
            button2.TabIndex = 1;
            button2.Text = "PDF Merger";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(9F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GrayText;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(395, 174);
            Controls.Add(button2);
            Controls.Add(button1);
            Font = new Font("Unispace", 9F, FontStyle.Bold, GraphicsUnit.Point, 238);
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "Main";
            ShowIcon = false;
            Text = "PDF Tool";
            FormClosing += Main_FormClosing;
            FormClosed += Main_FormClosed;
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
    }
}