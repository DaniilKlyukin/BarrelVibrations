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
            groupBox = new GroupBox();
            hitpointsDataGridView = new DataGridView();
            ShotColumn = new DataGridViewTextBoxColumn();
            XColumn = new DataGridViewTextBoxColumn();
            YColumn = new DataGridViewTextBoxColumn();
            ZColumn = new DataGridViewTextBoxColumn();
            distanceNumericUpDown = new NumericUpDown();
            label2 = new Label();
            shotComboBox = new ComboBox();
            listBox = new ListBox();
            SaveButton = new Button();
            CopyButton = new Button();
            groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)hitpointsDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)distanceNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // groupBox
            // 
            groupBox.Controls.Add(hitpointsDataGridView);
            groupBox.Controls.Add(distanceNumericUpDown);
            groupBox.Controls.Add(label2);
            groupBox.Controls.Add(shotComboBox);
            groupBox.Controls.Add(listBox);
            groupBox.Controls.Add(SaveButton);
            groupBox.Controls.Add(CopyButton);
            groupBox.Dock = DockStyle.Fill;
            groupBox.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            groupBox.Location = new Point(0, 0);
            groupBox.Name = "groupBox";
            groupBox.Size = new Size(752, 785);
            groupBox.TabIndex = 5;
            groupBox.TabStop = false;
            groupBox.Text = "Название";
            // 
            // hitpointsDataGridView
            // 
            hitpointsDataGridView.AllowUserToAddRows = false;
            hitpointsDataGridView.AllowUserToDeleteRows = false;
            hitpointsDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            hitpointsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            hitpointsDataGridView.ColumnHeadersVisible = false;
            hitpointsDataGridView.Columns.AddRange(new DataGridViewColumn[] { ShotColumn, XColumn, YColumn, ZColumn });
            hitpointsDataGridView.Location = new Point(6, 339);
            hitpointsDataGridView.Name = "hitpointsDataGridView";
            hitpointsDataGridView.ReadOnly = true;
            hitpointsDataGridView.RowHeadersVisible = false;
            hitpointsDataGridView.RowTemplate.Height = 25;
            hitpointsDataGridView.Size = new Size(740, 443);
            hitpointsDataGridView.TabIndex = 15;
            // 
            // ShotColumn
            // 
            ShotColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            ShotColumn.HeaderText = "Выстрел";
            ShotColumn.MinimumWidth = 100;
            ShotColumn.Name = "ShotColumn";
            ShotColumn.ReadOnly = true;
            ShotColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // XColumn
            // 
            XColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            XColumn.HeaderText = "X, м";
            XColumn.Name = "XColumn";
            XColumn.ReadOnly = true;
            XColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            XColumn.Width = 5;
            // 
            // YColumn
            // 
            YColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            YColumn.HeaderText = "Y, м";
            YColumn.Name = "YColumn";
            YColumn.ReadOnly = true;
            YColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            YColumn.Width = 5;
            // 
            // ZColumn
            // 
            ZColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            ZColumn.HeaderText = "Z, м";
            ZColumn.Name = "ZColumn";
            ZColumn.ReadOnly = true;
            ZColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            ZColumn.Width = 5;
            // 
            // distanceNumericUpDown
            // 
            distanceNumericUpDown.DecimalPlaces = 2;
            distanceNumericUpDown.Location = new Point(181, 307);
            distanceNumericUpDown.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            distanceNumericUpDown.Name = "distanceNumericUpDown";
            distanceNumericUpDown.Size = new Size(117, 26);
            distanceNumericUpDown.TabIndex = 8;
            distanceNumericUpDown.Value = new decimal(new int[] { 1500, 0, 0, 0 });
            distanceNumericUpDown.ValueChanged += distanceNumericUpDown_ValueChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(11, 307);
            label2.Name = "label2";
            label2.Size = new Size(164, 19);
            label2.TabIndex = 7;
            label2.Text = "Расстояние до цели, м:";
            // 
            // shotComboBox
            // 
            shotComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            shotComboBox.FormattingEnabled = true;
            shotComboBox.Location = new Point(634, 105);
            shotComboBox.Name = "shotComboBox";
            shotComboBox.Size = new Size(112, 27);
            shotComboBox.TabIndex = 4;
            shotComboBox.SelectedIndexChanged += shotComboBox_SelectedIndexChanged;
            // 
            // listBox
            // 
            listBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            listBox.FormattingEnabled = true;
            listBox.ItemHeight = 19;
            listBox.Location = new Point(6, 25);
            listBox.Name = "listBox";
            listBox.Size = new Size(620, 270);
            listBox.TabIndex = 1;
            listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;
            // 
            // SaveButton
            // 
            SaveButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            SaveButton.Location = new Point(634, 65);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(112, 34);
            SaveButton.TabIndex = 3;
            SaveButton.Text = "Сохранить";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // CopyButton
            // 
            CopyButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            CopyButton.Location = new Point(634, 25);
            CopyButton.Name = "CopyButton";
            CopyButton.Size = new Size(112, 34);
            CopyButton.TabIndex = 2;
            CopyButton.Text = "Скопировать";
            CopyButton.UseVisualStyleBackColor = true;
            CopyButton.Click += CopyButton_Click;
            // 
            // OutletBallisticView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(752, 785);
            Controls.Add(groupBox);
            Name = "OutletBallisticView";
            Text = "Внешняя баллистика";
            groupBox.ResumeLayout(false);
            groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)hitpointsDataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)distanceNumericUpDown).EndInit();
            ResumeLayout(false);
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