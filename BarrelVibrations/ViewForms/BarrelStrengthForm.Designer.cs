namespace BarrelVibrations.ViewForms
{
    partial class BarrelStrengthForm
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
            label1 = new Label();
            StrengthPcntNumericUpDown = new NumericUpDown();
            ThicknessFormsPlot = new ScottPlot.FormsPlot();
            comboBox = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)StrengthPcntNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(239, 14);
            label1.Name = "label1";
            label1.Size = new Size(148, 19);
            label1.TabIndex = 0;
            label1.Text = "Запас прочности, %:";
            // 
            // StrengthPcntNumericUpDown
            // 
            StrengthPcntNumericUpDown.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            StrengthPcntNumericUpDown.Location = new Point(393, 12);
            StrengthPcntNumericUpDown.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            StrengthPcntNumericUpDown.Name = "StrengthPcntNumericUpDown";
            StrengthPcntNumericUpDown.Size = new Size(61, 26);
            StrengthPcntNumericUpDown.TabIndex = 1;
            StrengthPcntNumericUpDown.Value = new decimal(new int[] { 10, 0, 0, 0 });
            StrengthPcntNumericUpDown.ValueChanged += StrengthPcntNumericUpDown_ValueChanged;
            // 
            // ThicknessFormsPlot
            // 
            ThicknessFormsPlot.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ThicknessFormsPlot.Location = new Point(13, 39);
            ThicknessFormsPlot.Margin = new Padding(4, 3, 4, 3);
            ThicknessFormsPlot.Name = "ThicknessFormsPlot";
            ThicknessFormsPlot.Size = new Size(1002, 399);
            ThicknessFormsPlot.TabIndex = 3;
            // 
            // comboBox
            // 
            comboBox.FormattingEnabled = true;
            comboBox.Items.AddRange(new object[] { "Критерий наибольших деформаций", "Критерий Сен-Венана" });
            comboBox.Location = new Point(13, 12);
            comboBox.Name = "comboBox";
            comboBox.Size = new Size(220, 23);
            comboBox.TabIndex = 4;
            comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;
            // 
            // BarrelStrengthForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1028, 450);
            Controls.Add(comboBox);
            Controls.Add(ThicknessFormsPlot);
            Controls.Add(StrengthPcntNumericUpDown);
            Controls.Add(label1);
            Name = "BarrelStrengthForm";
            Text = "Прочность ствола";
            ((System.ComponentModel.ISupportInitialize)StrengthPcntNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private NumericUpDown StrengthPcntNumericUpDown;
        private ScottPlot.FormsPlot ThicknessFormsPlot;
        private ComboBox comboBox;
    }
}