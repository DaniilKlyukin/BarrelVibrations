namespace BarrelVibrations.ModelingObjects.EnvironmentFolder
{
    partial class TerrainVisualizationForm
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
            heatBar = new CustomControls.HeatBar();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
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
            label9.Location = new Point(479, 9);
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
            label7.Location = new Point(12, 9);
            label7.Name = "label7";
            label7.Size = new Size(74, 15);
            label7.TabIndex = 18;
            label7.Text = "(xMin, zMin)";
            // 
            // pictureBox
            // 
            pictureBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox.Location = new Point(12, 27);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(543, 581);
            pictureBox.TabIndex = 22;
            pictureBox.TabStop = false;
            // 
            // heatBar
            // 
            heatBar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            heatBar.BackColor = SystemColors.ControlLight;
            heatBar.BorderStyle = BorderStyle.FixedSingle;
            heatBar.Location = new Point(561, 9);
            heatBar.Max = 1D;
            heatBar.MeasureUnits = "Высота, м";
            heatBar.Min = 0D;
            heatBar.Name = "heatBar";
            heatBar.Size = new Size(160, 617);
            heatBar.TabIndex = 23;
            // 
            // TerrainVisualizationForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(733, 635);
            Controls.Add(heatBar);
            Controls.Add(pictureBox);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Name = "TerrainVisualizationForm";
            Text = "Визуализация карты высот";
            Load += new EventHandler(TerrainVisualizationForm_Load);
            ResizeEnd += new EventHandler(TerrainVisualizationForm_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label10;
        private Label label9;
        private Label label8;
        private Label label7;
        private PictureBox pictureBox;
        private CustomControls.HeatBar heatBar;
    }
}