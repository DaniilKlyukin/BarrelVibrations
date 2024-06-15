namespace BarrelVibrations.Solvers.OutletBallisticFolder
{
    partial class MeteoTableForm
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
            this.AltitudeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PressureColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TemperatureColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DensityColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SoundSpeedColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.formsPlot = new ScottPlot.FormsPlot();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AltitudeColumn,
            this.PressureColumn,
            this.TemperatureColumn,
            this.DensityColumn,
            this.SoundSpeedColumn});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowTemplate.Height = 25;
            this.dataGridView.Size = new System.Drawing.Size(571, 703);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_ColumnHeaderMouseClick);
            this.dataGridView.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_ColumnHeaderMouseDoubleClick);
            this.dataGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridView_DefaultValuesNeeded);
            this.dataGridView.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dataGridView_PreviewKeyDown);
            // 
            // AltitudeColumn
            // 
            this.AltitudeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AltitudeColumn.HeaderText = "Высота, м";
            this.AltitudeColumn.MinimumWidth = 75;
            this.AltitudeColumn.Name = "AltitudeColumn";
            this.AltitudeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PressureColumn
            // 
            this.PressureColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.PressureColumn.HeaderText = "Давление, Па";
            this.PressureColumn.Name = "PressureColumn";
            this.PressureColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.PressureColumn.Width = 97;
            // 
            // TemperatureColumn
            // 
            this.TemperatureColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.TemperatureColumn.HeaderText = "Температура, К";
            this.TemperatureColumn.Name = "TemperatureColumn";
            this.TemperatureColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.TemperatureColumn.Width = 106;
            // 
            // DensityColumn
            // 
            this.DensityColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DensityColumn.HeaderText = "Плотность, кг/м^3";
            this.DensityColumn.Name = "DensityColumn";
            this.DensityColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.DensityColumn.Width = 124;
            // 
            // SoundSpeedColumn
            // 
            this.SoundSpeedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.SoundSpeedColumn.HeaderText = "Скорость звука, м/с";
            this.SoundSpeedColumn.Name = "SoundSpeedColumn";
            this.SoundSpeedColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.SoundSpeedColumn.Width = 112;
            // 
            // formsPlot
            // 
            this.formsPlot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlot.Location = new System.Drawing.Point(571, 0);
            this.formsPlot.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.formsPlot.Name = "formsPlot";
            this.formsPlot.Size = new System.Drawing.Size(854, 703);
            this.formsPlot.TabIndex = 1;
            // 
            // MeteoTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1425, 703);
            this.Controls.Add(this.formsPlot);
            this.Controls.Add(this.dataGridView);
            this.Name = "MeteoTableForm";
            this.Text = "Метеоданные";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView dataGridView;
        private DataGridViewTextBoxColumn AltitudeColumn;
        private DataGridViewTextBoxColumn PressureColumn;
        private DataGridViewTextBoxColumn TemperatureColumn;
        private DataGridViewTextBoxColumn DensityColumn;
        private DataGridViewTextBoxColumn SoundSpeedColumn;
        private ScottPlot.FormsPlot formsPlot;
    }
}