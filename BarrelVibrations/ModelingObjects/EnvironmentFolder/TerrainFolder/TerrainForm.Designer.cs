namespace BarrelVibrations.ModelingObjects.EnvironmentFolder
{
    partial class TerrainForm
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
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.rowsNumeric = new System.Windows.Forms.NumericUpDown();
            this.columnsNumeric = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.LoadButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.VisualizeButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnsNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.ColumnHeadersVisible = false;
            this.dataGridView.Location = new System.Drawing.Point(151, 32);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowTemplate.Height = 25;
            this.dataGridView.Size = new System.Drawing.Size(530, 418);
            this.dataGridView.TabIndex = 8;
            this.dataGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridView_DefaultValuesNeeded);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "Строк:";
            // 
            // rowsNumeric
            // 
            this.rowsNumeric.Location = new System.Drawing.Point(83, 12);
            this.rowsNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.rowsNumeric.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.rowsNumeric.Name = "rowsNumeric";
            this.rowsNumeric.Size = new System.Drawing.Size(62, 23);
            this.rowsNumeric.TabIndex = 9;
            this.rowsNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.rowsNumeric.ValueChanged += new System.EventHandler(this.rowsNumeric_ValueChanged);
            // 
            // columnsNumeric
            // 
            this.columnsNumeric.Location = new System.Drawing.Point(83, 43);
            this.columnsNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.columnsNumeric.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.columnsNumeric.Name = "columnsNumeric";
            this.columnsNumeric.Size = new System.Drawing.Size(62, 23);
            this.columnsNumeric.TabIndex = 11;
            this.columnsNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.columnsNumeric.ValueChanged += new System.EventHandler(this.columnsNumeric_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 15);
            this.label6.TabIndex = 12;
            this.label6.Text = "Столбцов:";
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(12, 72);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(133, 23);
            this.LoadButton.TabIndex = 13;
            this.LoadButton.Text = "Загрузить";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(151, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 15);
            this.label7.TabIndex = 14;
            this.label7.Text = "(xMin, zMin)";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(151, 453);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 15);
            this.label8.TabIndex = 15;
            this.label8.Text = "(xMin, zMax)";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(605, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 15);
            this.label9.TabIndex = 16;
            this.label9.Text = "(xMax, zMin)";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(603, 453);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(78, 15);
            this.label10.TabIndex = 17;
            this.label10.Text = "(xMax, zMax)";
            // 
            // VisualizeButton
            // 
            this.VisualizeButton.Location = new System.Drawing.Point(12, 101);
            this.VisualizeButton.Name = "VisualizeButton";
            this.VisualizeButton.Size = new System.Drawing.Size(133, 23);
            this.VisualizeButton.TabIndex = 18;
            this.VisualizeButton.Text = "Визуализировать";
            this.VisualizeButton.UseVisualStyleBackColor = true;
            this.VisualizeButton.Click += new System.EventHandler(this.VisualizeButton_Click);
            // 
            // TerrainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 477);
            this.Controls.Add(this.VisualizeButton);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.columnsNumeric);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.rowsNumeric);
            this.Controls.Add(this.dataGridView);
            this.Name = "TerrainForm";
            this.Text = "Карта высот";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TerrainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnsNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DataGridView dataGridView;
        private Label label5;
        private NumericUpDown rowsNumeric;
        private NumericUpDown columnsNumeric;
        private Label label6;
        private Button LoadButton;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Button VisualizeButton;
    }
}