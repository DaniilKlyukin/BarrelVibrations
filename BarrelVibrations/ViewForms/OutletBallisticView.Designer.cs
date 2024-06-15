namespace BarrelVibrations.ViewForms
{
    partial class OutletBallisticView
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
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.hitpointsDataGridView = new System.Windows.Forms.DataGridView();
            this.distanceNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.shotComboBox = new System.Windows.Forms.ComboBox();
            this.listBox = new System.Windows.Forms.ListBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.CopyButton = new System.Windows.Forms.Button();
            this.ShotColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.XColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hitpointsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.distanceNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.hitpointsDataGridView);
            this.groupBox.Controls.Add(this.distanceNumericUpDown);
            this.groupBox.Controls.Add(this.label2);
            this.groupBox.Controls.Add(this.shotComboBox);
            this.groupBox.Controls.Add(this.listBox);
            this.groupBox.Controls.Add(this.SaveButton);
            this.groupBox.Controls.Add(this.CopyButton);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(752, 785);
            this.groupBox.TabIndex = 5;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Название";
            // 
            // hitpointsDataGridView
            // 
            this.hitpointsDataGridView.AllowUserToAddRows = false;
            this.hitpointsDataGridView.AllowUserToDeleteRows = false;
            this.hitpointsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hitpointsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.hitpointsDataGridView.ColumnHeadersVisible = false;
            this.hitpointsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ShotColumn,
            this.XColumn,
            this.YColumn,
            this.ZColumn});
            this.hitpointsDataGridView.Location = new System.Drawing.Point(6, 339);
            this.hitpointsDataGridView.Name = "hitpointsDataGridView";
            this.hitpointsDataGridView.ReadOnly = true;
            this.hitpointsDataGridView.RowHeadersVisible = false;
            this.hitpointsDataGridView.RowTemplate.Height = 25;
            this.hitpointsDataGridView.Size = new System.Drawing.Size(740, 443);
            this.hitpointsDataGridView.TabIndex = 15;
            // 
            // distanceNumericUpDown
            // 
            this.distanceNumericUpDown.DecimalPlaces = 2;
            this.distanceNumericUpDown.Location = new System.Drawing.Point(181, 307);
            this.distanceNumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.distanceNumericUpDown.Name = "distanceNumericUpDown";
            this.distanceNumericUpDown.Size = new System.Drawing.Size(117, 26);
            this.distanceNumericUpDown.TabIndex = 8;
            this.distanceNumericUpDown.Value = new decimal(new int[] {
            1500,
            0,
            0,
            0});
            this.distanceNumericUpDown.ValueChanged += new System.EventHandler(this.distanceNumericUpDown_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 307);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(164, 19);
            this.label2.TabIndex = 7;
            this.label2.Text = "Расстояние до цели, м:";
            // 
            // shotComboBox
            // 
            this.shotComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.shotComboBox.FormattingEnabled = true;
            this.shotComboBox.Location = new System.Drawing.Point(634, 105);
            this.shotComboBox.Name = "shotComboBox";
            this.shotComboBox.Size = new System.Drawing.Size(112, 27);
            this.shotComboBox.TabIndex = 4;
            this.shotComboBox.SelectedIndexChanged += new System.EventHandler(this.shotComboBox_SelectedIndexChanged);
            // 
            // listBox
            // 
            this.listBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox.FormattingEnabled = true;
            this.listBox.ItemHeight = 19;
            this.listBox.Location = new System.Drawing.Point(6, 25);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(620, 270);
            this.listBox.TabIndex = 1;
            this.listBox.SelectedIndexChanged += new System.EventHandler(this.listBox_SelectedIndexChanged);
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Location = new System.Drawing.Point(634, 65);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(112, 34);
            this.SaveButton.TabIndex = 3;
            this.SaveButton.Text = "Сохранить";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // CopyButton
            // 
            this.CopyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CopyButton.Location = new System.Drawing.Point(634, 25);
            this.CopyButton.Name = "CopyButton";
            this.CopyButton.Size = new System.Drawing.Size(112, 34);
            this.CopyButton.TabIndex = 2;
            this.CopyButton.Text = "Скопировать";
            this.CopyButton.UseVisualStyleBackColor = true;
            this.CopyButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // ShotColumn
            // 
            this.ShotColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ShotColumn.HeaderText = "Выстрел";
            this.ShotColumn.MinimumWidth = 100;
            this.ShotColumn.Name = "ShotColumn";
            this.ShotColumn.ReadOnly = true;
            this.ShotColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // XColumn
            // 
            this.XColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.XColumn.HeaderText = "X, м";
            this.XColumn.Name = "XColumn";
            this.XColumn.ReadOnly = true;
            this.XColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.XColumn.Width = 5;
            // 
            // YColumn
            // 
            this.YColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.YColumn.HeaderText = "Y, м";
            this.YColumn.Name = "YColumn";
            this.YColumn.ReadOnly = true;
            this.YColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.YColumn.Width = 5;
            // 
            // ZColumn
            // 
            this.ZColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ZColumn.HeaderText = "Z, м";
            this.ZColumn.Name = "ZColumn";
            this.ZColumn.ReadOnly = true;
            this.ZColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ZColumn.Width = 5;
            // 
            // OutletBallisticView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 785);
            this.Controls.Add(this.groupBox);
            this.Name = "OutletBallisticView";
            this.Text = "Внешняя баллистика";
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hitpointsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.distanceNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox;
        private ComboBox shotComboBox;
        private ListBox listBox;
        private Button SaveButton;
        private Button CopyButton;
        private NumericUpDown distanceNumericUpDown;
        private Label label2;
        private DataGridView hitpointsDataGridView;
        private DataGridViewTextBoxColumn ShotColumn;
        private DataGridViewTextBoxColumn XColumn;
        private DataGridViewTextBoxColumn YColumn;
        private DataGridViewTextBoxColumn ZColumn;
    }
}