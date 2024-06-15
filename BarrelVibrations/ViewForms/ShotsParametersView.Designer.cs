namespace BarrelVibrations.ViewForms
{
    partial class ShotsParametersView
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
            this.ShotNumberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MissilePressureColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AveragePressureColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CamoraPressureColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MissileSpeedColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HorizontalAngleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VerticalAngleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HorizontalRotationAngleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VerticalRotationAngleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.ColumnHeadersVisible = false;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ShotNumberColumn,
            this.TimeColumn,
            this.MissilePressureColumn,
            this.AveragePressureColumn,
            this.CamoraPressureColumn,
            this.MissileSpeedColumn,
            this.HorizontalAngleColumn,
            this.VerticalAngleColumn,
            this.HorizontalRotationAngleColumn,
            this.VerticalRotationAngleColumn});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowTemplate.Height = 25;
            this.dataGridView.Size = new System.Drawing.Size(1008, 388);
            this.dataGridView.TabIndex = 0;
            // 
            // ShotNumberColumn
            // 
            this.ShotNumberColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ShotNumberColumn.HeaderText = "Выстрел";
            this.ShotNumberColumn.MinimumWidth = 100;
            this.ShotNumberColumn.Name = "ShotNumberColumn";
            this.ShotNumberColumn.ReadOnly = true;
            this.ShotNumberColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TimeColumn
            // 
            this.TimeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.TimeColumn.HeaderText = "Время, мс";
            this.TimeColumn.Name = "TimeColumn";
            this.TimeColumn.ReadOnly = true;
            this.TimeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TimeColumn.Width = 5;
            // 
            // MissilePressureColumn
            // 
            this.MissilePressureColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.MissilePressureColumn.HeaderText = "Pсн, МПа";
            this.MissilePressureColumn.Name = "MissilePressureColumn";
            this.MissilePressureColumn.ReadOnly = true;
            this.MissilePressureColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.MissilePressureColumn.Width = 5;
            // 
            // AveragePressureColumn
            // 
            this.AveragePressureColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.AveragePressureColumn.HeaderText = "Pсред, МПа";
            this.AveragePressureColumn.Name = "AveragePressureColumn";
            this.AveragePressureColumn.ReadOnly = true;
            this.AveragePressureColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.AveragePressureColumn.Width = 5;
            // 
            // CamoraPressureColumn
            // 
            this.CamoraPressureColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CamoraPressureColumn.HeaderText = "Pкн, МПа";
            this.CamoraPressureColumn.Name = "CamoraPressureColumn";
            this.CamoraPressureColumn.ReadOnly = true;
            this.CamoraPressureColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CamoraPressureColumn.Width = 5;
            // 
            // MissileSpeedColumn
            // 
            this.MissileSpeedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.MissileSpeedColumn.HeaderText = "Vсн, м/с";
            this.MissileSpeedColumn.Name = "MissileSpeedColumn";
            this.MissileSpeedColumn.ReadOnly = true;
            this.MissileSpeedColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.MissileSpeedColumn.Width = 5;
            // 
            // HorizontalAngleColumn
            // 
            this.HorizontalAngleColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.HorizontalAngleColumn.HeaderText = "Горизонтальный угол наклона, град";
            this.HorizontalAngleColumn.Name = "HorizontalAngleColumn";
            this.HorizontalAngleColumn.ReadOnly = true;
            this.HorizontalAngleColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.HorizontalAngleColumn.Width = 5;
            // 
            // VerticalAngleColumn
            // 
            this.VerticalAngleColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.VerticalAngleColumn.HeaderText = "Вертикальный угол наклона, град";
            this.VerticalAngleColumn.Name = "VerticalAngleColumn";
            this.VerticalAngleColumn.ReadOnly = true;
            this.VerticalAngleColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.VerticalAngleColumn.Width = 5;
            // 
            // HorizontalRotationAngleColumn
            // 
            this.HorizontalRotationAngleColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.HorizontalRotationAngleColumn.HeaderText = "Горизонтальный угол нутации, град";
            this.HorizontalRotationAngleColumn.Name = "HorizontalRotationAngleColumn";
            this.HorizontalRotationAngleColumn.ReadOnly = true;
            this.HorizontalRotationAngleColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.HorizontalRotationAngleColumn.Width = 5;
            // 
            // VerticalRotationAngleColumn
            // 
            this.VerticalRotationAngleColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.VerticalRotationAngleColumn.HeaderText = "Вертикальный угол нутации, град";
            this.VerticalRotationAngleColumn.Name = "VerticalRotationAngleColumn";
            this.VerticalRotationAngleColumn.ReadOnly = true;
            this.VerticalRotationAngleColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.VerticalRotationAngleColumn.Width = 5;
            // 
            // ShotsParametersView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 388);
            this.Controls.Add(this.dataGridView);
            this.Name = "ShotsParametersView";
            this.Text = "Дульные параметры";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView dataGridView;
        private DataGridViewTextBoxColumn ShotNumberColumn;
        private DataGridViewTextBoxColumn TimeColumn;
        private DataGridViewTextBoxColumn MissilePressureColumn;
        private DataGridViewTextBoxColumn AveragePressureColumn;
        private DataGridViewTextBoxColumn CamoraPressureColumn;
        private DataGridViewTextBoxColumn MissileSpeedColumn;
        private DataGridViewTextBoxColumn HorizontalAngleColumn;
        private DataGridViewTextBoxColumn VerticalAngleColumn;
        private DataGridViewTextBoxColumn HorizontalRotationAngleColumn;
        private DataGridViewTextBoxColumn VerticalRotationAngleColumn;
    }
}