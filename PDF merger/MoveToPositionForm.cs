using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDF_merger
{
    public class MoveToPositionForm : Form
    {
        private readonly NumericUpDown numericUpDown1;
        private readonly Button buttonOk;
        private readonly Button buttonCancel;

        public int SelectedPosition { get; private set; }

        public MoveToPositionForm(int maxPosition)
        {
            Text = "Move to position";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            Width = 280;
            Height = 160;
            MaximizeBox = false;
            MinimizeBox = false;

            Label label = new Label
            {
                Left = 15,
                Top = 15,
                Width = 220,
                Text = "Enter target position:"
            };

            numericUpDown1 = new NumericUpDown
            {
                Left = 15,
                Top = 40,
                Width = 230,
                Minimum = 1,
                Maximum = Math.Max(1, maxPosition),
                Value = 1
            };

            buttonOk = new Button
            {
                Text = "OK",
                Left = 45,
                Top = 75,
                Width = 75,
                DialogResult = DialogResult.OK
            };

            buttonCancel = new Button
            {
                Text = "Cancel",
                Left = 135,
                Top = 75,
                Width = 75,
                DialogResult = DialogResult.Cancel
            };

            buttonOk.Click += ButtonOk_Click;

            Controls.Add(label);
            Controls.Add(numericUpDown1);
            Controls.Add(buttonOk);
            Controls.Add(buttonCancel);

            AcceptButton = buttonOk;
            CancelButton = buttonCancel;
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            SelectedPosition = (int)numericUpDown1.Value;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
