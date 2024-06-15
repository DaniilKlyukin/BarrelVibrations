namespace BarrelVibrations.Optimization
{
    partial class BarrelOptimizationConstraintForm
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
            XColumn = new DataGridViewTextBoxColumn();
            MinThColumn = new DataGridViewTextBoxColumn();
            maxMassNumericUpDown = new NumericUpDown();
            maxMassLabel = new Label();
            outerDNeedDecreaseCheckBox = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)maxMassNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // dataGridView
            // 
            dataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Columns.AddRange(new DataGridViewColumn[] { XColumn, MinThColumn });
            dataGridView.Location = new Point(0, 0);
            dataGridView.Name = "dataGridView";
            dataGridView.RowTemplate.Height = 25;
            dataGridView.Size = new Size(344, 393);
            dataGridView.TabIndex = 0;
            // 
            // XColumn
            // 
            XColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            XColumn.HeaderText = "x сечения, м";
            XColumn.MinimumWidth = 100;
            XColumn.Name = "XColumn";
            XColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // MinThColumn
            // 
            MinThColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            MinThColumn.HeaderText = "Минимальная толщина ствола, мм";
            MinThColumn.Name = "MinThColumn";
            MinThColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // maxMassNumericUpDown
            // 
            maxMassNumericUpDown.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            maxMassNumericUpDown.DecimalPlaces = 1;
            maxMassNumericUpDown.Location = new Point(212, 399);
            maxMassNumericUpDown.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            maxMassNumericUpDown.Name = "maxMassNumericUpDown";
            maxMassNumericUpDown.Size = new Size(120, 23);
            maxMassNumericUpDown.TabIndex = 2;
            // 
            // maxMassLabel
            // 
            maxMassLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            maxMassLabel.AutoSize = true;
            maxMassLabel.Location = new Point(20, 401);
            maxMassLabel.Name = "maxMassLabel";
            maxMassLabel.Size = new Size(186, 15);
            maxMassLabel.TabIndex = 4;
            maxMassLabel.Text = "Максимальная масса ствола, кг:";
            // 
            // outerDNeedDecreaseCheckBox
            // 
            outerDNeedDecreaseCheckBox.AutoSize = true;
            outerDNeedDecreaseCheckBox.Location = new Point(20, 430);
            outerDNeedDecreaseCheckBox.Name = "outerDNeedDecreaseCheckBox";
            outerDNeedDecreaseCheckBox.Size = new Size(220, 19);
            outerDNeedDecreaseCheckBox.TabIndex = 5;
            outerDNeedDecreaseCheckBox.Text = "Внешний диаметр должен убывать";
            outerDNeedDecreaseCheckBox.UseVisualStyleBackColor = true;
            // 
            // BarrelOptimizationConstraintForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(344, 461);
            Controls.Add(outerDNeedDecreaseCheckBox);
            Controls.Add(maxMassLabel);
            Controls.Add(maxMassNumericUpDown);
            Controls.Add(dataGridView);
            Name = "BarrelOptimizationConstraintForm";
            Text = "Ограничения оптимизации формы ствола";
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)maxMassNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView;
        private NumericUpDown maxMassNumericUpDown;
        private Label maxMassLabel;
        private DataGridViewTextBoxColumn XColumn;
        private DataGridViewTextBoxColumn MinThColumn;
        private CheckBox outerDNeedDecreaseCheckBox;
    }
}