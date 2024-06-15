namespace BarrelVibrations.ModelingObjects.ShotFolder
{
    partial class ShotForm
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
            dataGridView = new DataGridView();
            TimeColumn = new DataGridViewTextBoxColumn();
            AmmoColumn = new DataGridViewComboBoxColumn();
            SetShotsButton = new Button();
            shotsCountNumericUpDown = new NumericUpDown();
            label1 = new Label();
            shotsIntervalNumericUpDown = new NumericUpDown();
            label2 = new Label();
            label3 = new Label();
            shotTimeSpreadNumericUpDown = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)shotsCountNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)shotsIntervalNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)shotTimeSpreadNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // dataGridView
            // 
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Columns.AddRange(new DataGridViewColumn[] { TimeColumn, AmmoColumn });
            dataGridView.Location = new Point(12, 12);
            dataGridView.Name = "dataGridView";
            dataGridView.RowTemplate.Height = 25;
            dataGridView.Size = new Size(337, 547);
            dataGridView.TabIndex = 0;
            // 
            // TimeColumn
            // 
            TimeColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            TimeColumn.HeaderText = "Момент времени, сек";
            TimeColumn.Name = "TimeColumn";
            TimeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // AmmoColumn
            // 
            AmmoColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            AmmoColumn.HeaderText = "Боеприпас";
            AmmoColumn.Name = "AmmoColumn";
            // 
            // SetShotsButton
            // 
            SetShotsButton.Location = new Point(355, 105);
            SetShotsButton.Name = "SetShotsButton";
            SetShotsButton.Size = new Size(267, 28);
            SetShotsButton.TabIndex = 1;
            SetShotsButton.Text = "Добавить";
            SetShotsButton.UseVisualStyleBackColor = true;
            SetShotsButton.Click += SetShotsButton_Click;
            // 
            // shotsCountNumericUpDown
            // 
            shotsCountNumericUpDown.Location = new Point(558, 12);
            shotsCountNumericUpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            shotsCountNumericUpDown.Name = "shotsCountNumericUpDown";
            shotsCountNumericUpDown.Size = new Size(64, 23);
            shotsCountNumericUpDown.TabIndex = 2;
            shotsCountNumericUpDown.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(355, 14);
            label1.Name = "label1";
            label1.Size = new Size(137, 15);
            label1.TabIndex = 3;
            label1.Text = "Количество выстрелов:";
            // 
            // shotsIntervalNumericUpDown
            // 
            shotsIntervalNumericUpDown.DecimalPlaces = 3;
            shotsIntervalNumericUpDown.Increment = new decimal(new int[] { 1, 0, 0, 196608 });
            shotsIntervalNumericUpDown.Location = new Point(558, 41);
            shotsIntervalNumericUpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            shotsIntervalNumericUpDown.Name = "shotsIntervalNumericUpDown";
            shotsIntervalNumericUpDown.Size = new Size(64, 23);
            shotsIntervalNumericUpDown.TabIndex = 4;
            shotsIntervalNumericUpDown.Value = new decimal(new int[] { 182, 0, 0, 196608 });
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(355, 43);
            label2.Name = "label2";
            label2.Size = new Size(197, 15);
            label2.TabIndex = 5;
            label2.Text = "Интервал между выстрелами, сек:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(355, 71);
            label3.Name = "label3";
            label3.Size = new Size(122, 15);
            label3.TabIndex = 6;
            label3.Text = "Разброс времени, %:";
            // 
            // shotTimeSpreadNumericUpDown
            // 
            shotTimeSpreadNumericUpDown.DecimalPlaces = 1;
            shotTimeSpreadNumericUpDown.Location = new Point(558, 69);
            shotTimeSpreadNumericUpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            shotTimeSpreadNumericUpDown.Name = "shotTimeSpreadNumericUpDown";
            shotTimeSpreadNumericUpDown.Size = new Size(64, 23);
            shotTimeSpreadNumericUpDown.TabIndex = 7;
            // 
            // ShotForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(634, 571);
            Controls.Add(shotTimeSpreadNumericUpDown);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(shotsIntervalNumericUpDown);
            Controls.Add(label1);
            Controls.Add(shotsCountNumericUpDown);
            Controls.Add(SetShotsButton);
            Controls.Add(dataGridView);
            Name = "ShotForm";
            Text = "Выстрелы";
            Load += ShotForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)shotsCountNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)shotsIntervalNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)shotTimeSpreadNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView;
        private DataGridViewTextBoxColumn TimeColumn;
        private DataGridViewComboBoxColumn AmmoColumn;
        private Button SetShotsButton;
        private NumericUpDown shotsCountNumericUpDown;
        private Label label1;
        private NumericUpDown shotsIntervalNumericUpDown;
        private Label label2;
        private Label label3;
        private NumericUpDown shotTimeSpreadNumericUpDown;
    }
}