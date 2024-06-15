namespace BarrelVibrations.ModelingObjects.EnvironmentFolder
{
    partial class WindForm
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
            this.xsNumeric = new System.Windows.Forms.NumericUpDown();
            this.ysNumeric = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.VisualizeButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.zsNumeric = new System.Windows.Forms.NumericUpDown();
            this.ysDataGridView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ysNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ysDataGridView)).BeginInit();
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
            this.dataGridView.Location = new System.Drawing.Point(261, 32);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowTemplate.Height = 25;
            this.dataGridView.Size = new System.Drawing.Size(681, 401);
            this.dataGridView.TabIndex = 8;
            this.dataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellEndEdit);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(55, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "Xs:";
            // 
            // xsNumeric
            // 
            this.xsNumeric.Location = new System.Drawing.Point(83, 12);
            this.xsNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.xsNumeric.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.xsNumeric.Name = "xsNumeric";
            this.xsNumeric.Size = new System.Drawing.Size(62, 23);
            this.xsNumeric.TabIndex = 9;
            this.xsNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.xsNumeric.ValueChanged += new System.EventHandler(this.xsNumeric_ValueChanged);
            // 
            // ysNumeric
            // 
            this.ysNumeric.Location = new System.Drawing.Point(83, 43);
            this.ysNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.ysNumeric.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.ysNumeric.Name = "ysNumeric";
            this.ysNumeric.Size = new System.Drawing.Size(62, 23);
            this.ysNumeric.TabIndex = 11;
            this.ysNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ysNumeric.ValueChanged += new System.EventHandler(this.ysNumeric_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(55, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 15);
            this.label6.TabIndex = 12;
            this.label6.Text = "Ys:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(261, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 15);
            this.label7.TabIndex = 14;
            this.label7.Text = "(xMin, zMin)";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(261, 436);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 15);
            this.label8.TabIndex = 15;
            this.label8.Text = "(xMin, zMax)";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(866, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 15);
            this.label9.TabIndex = 16;
            this.label9.Text = "(xMax, zMin)";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(864, 436);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(78, 15);
            this.label10.TabIndex = 17;
            this.label10.Text = "(xMax, zMax)";
            // 
            // VisualizeButton
            // 
            this.VisualizeButton.Location = new System.Drawing.Point(12, 104);
            this.VisualizeButton.Name = "VisualizeButton";
            this.VisualizeButton.Size = new System.Drawing.Size(133, 23);
            this.VisualizeButton.TabIndex = 18;
            this.VisualizeButton.Text = "Визуализировать";
            this.VisualizeButton.UseVisualStyleBackColor = true;
            this.VisualizeButton.Click += new System.EventHandler(this.VisualizeButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 15);
            this.label1.TabIndex = 19;
            this.label1.Text = "Zs:";
            // 
            // zsNumeric
            // 
            this.zsNumeric.Location = new System.Drawing.Point(83, 72);
            this.zsNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.zsNumeric.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.zsNumeric.Name = "zsNumeric";
            this.zsNumeric.Size = new System.Drawing.Size(62, 23);
            this.zsNumeric.TabIndex = 20;
            this.zsNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.zsNumeric.ValueChanged += new System.EventHandler(this.zsNumeric_ValueChanged);
            // 
            // ysDataGridView
            // 
            this.ysDataGridView.AllowUserToAddRows = false;
            this.ysDataGridView.AllowUserToDeleteRows = false;
            this.ysDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ysDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ysDataGridView.ColumnHeadersVisible = false;
            this.ysDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.ysDataGridView.Location = new System.Drawing.Point(151, 32);
            this.ysDataGridView.Name = "ysDataGridView";
            this.ysDataGridView.RowHeadersVisible = false;
            this.ysDataGridView.RowTemplate.Height = 25;
            this.ysDataGridView.Size = new System.Drawing.Size(104, 401);
            this.ysDataGridView.TabIndex = 21;
            this.ysDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ysDataGridView_CellClick);
            this.ysDataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.ysDataGridView_CellEndEdit);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(151, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 15);
            this.label2.TabIndex = 22;
            this.label2.Text = "yMin";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(151, 436);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 15);
            this.label3.TabIndex = 23;
            this.label3.Text = "yMax";
            // 
            // comboBox
            // 
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Items.AddRange(new object[] {
            "Скорость ветра, м/с",
            "Угол ветра, град",
            "Скорость восходящего ветра, м/с"});
            this.comboBox.Location = new System.Drawing.Point(12, 133);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(133, 23);
            this.comboBox.TabIndex = 25;
            this.comboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // WindForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 460);
            this.Controls.Add(this.comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ysDataGridView);
            this.Controls.Add(this.zsNumeric);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.VisualizeButton);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ysNumeric);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.xsNumeric);
            this.Controls.Add(this.dataGridView);
            this.Name = "WindForm";
            this.Text = "Карта ветра";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WindForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ysNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ysDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DataGridView dataGridView;
        private Label label5;
        private NumericUpDown xsNumeric;
        private NumericUpDown ysNumeric;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Button VisualizeButton;
        private Label label1;
        private NumericUpDown zsNumeric;
        private DataGridView ysDataGridView;
        private Label label2;
        private Label label3;
        private ComboBox comboBox;
        private DataGridViewTextBoxColumn Column1;
    }
}