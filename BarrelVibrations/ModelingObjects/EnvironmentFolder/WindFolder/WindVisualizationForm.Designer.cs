using CustomControls;

namespace BarrelVibrations.ModelingObjects.EnvironmentFolder
{
    partial class WindVisualizationForm
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
            label10 = new Label();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            pictureBox = new PictureBox();
            parameterComboBox = new ComboBox();
            label1 = new Label();
            yNumericUpDown = new NumericUpDown();
            heatBar = new HeatBar();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)yNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // label10
            // 
            label10.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label10.AutoSize = true;
            label10.Location = new Point(477, 611);
            label10.Name = "label10";
            label10.Size = new Size(78, 15);
            label10.TabIndex = 21;
            label10.Text = "(xMax, zMax)";
            // 
            // label9
            // 
            label9.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label9.AutoSize = true;
            label9.Location = new Point(479, 38);
            label9.Name = "label9";
            label9.Size = new Size(76, 15);
            label9.TabIndex = 20;
            label9.Text = "(xMax, zMin)";
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label8.AutoSize = true;
            label8.Location = new Point(12, 611);
            label8.Name = "label8";
            label8.Size = new Size(76, 15);
            label8.TabIndex = 19;
            label8.Text = "(xMin, zMax)";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(12, 38);
            label7.Name = "label7";
            label7.Size = new Size(74, 15);
            label7.TabIndex = 18;
            label7.Text = "(xMin, zMin)";
            // 
            // pictureBox
            // 
            pictureBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox.Location = new Point(12, 56);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(543, 552);
            pictureBox.TabIndex = 22;
            pictureBox.TabStop = false;
            // 
            // parameterComboBox
            // 
            parameterComboBox.FormattingEnabled = true;
            parameterComboBox.Items.AddRange(new object[] { "Скорость ветра, м/с", "Угол ветра, град", "Скорость восходящего ветра, м/с" });
            parameterComboBox.Location = new Point(12, 12);
            parameterComboBox.Name = "parameterComboBox";
            parameterComboBox.Size = new Size(150, 23);
            parameterComboBox.TabIndex = 26;
            parameterComboBox.SelectedIndexChanged += parameterComboBox_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(168, 15);
            label1.Name = "label1";
            label1.Size = new Size(50, 15);
            label1.TabIndex = 27;
            label1.Text = "Высота:";
            // 
            // yNumericUpDown
            // 
            yNumericUpDown.Location = new Point(224, 12);
            yNumericUpDown.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            yNumericUpDown.Name = "yNumericUpDown";
            yNumericUpDown.Size = new Size(120, 23);
            yNumericUpDown.TabIndex = 28;
            yNumericUpDown.ValueChanged += yNumericUpDown_ValueChanged;
            // 
            // heatBar
            // 
            heatBar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            heatBar.BackColor = SystemColors.ControlLight;
            heatBar.BorderStyle = BorderStyle.FixedSingle;
            heatBar.Location = new Point(561, 9);
            heatBar.Max = 1D;
            heatBar.MeasureUnits = "";
            heatBar.Min = 0D;
            heatBar.Name = "heatBar";
            heatBar.Size = new Size(160, 617);
            heatBar.TabIndex = 23;
            // 
            // WindVisualizationForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(733, 635);
            Controls.Add(heatBar);
            Controls.Add(yNumericUpDown);
            Controls.Add(label1);
            Controls.Add(parameterComboBox);
            Controls.Add(pictureBox);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Name = "WindVisualizationForm";
            Text = "Визуализация карты высот";
            Load += TerrainVisualizationForm_Load;
            ResizeEnd += TerrainVisualizationForm_ResizeEnd;
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)yNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label10;
        private Label label9;
        private Label label8;
        private Label label7;
        private PictureBox pictureBox;
        private ComboBox parameterComboBox;
        private Label label1;
        private NumericUpDown yNumericUpDown;
        private CustomControls.HeatBar heatBar;
    }
}