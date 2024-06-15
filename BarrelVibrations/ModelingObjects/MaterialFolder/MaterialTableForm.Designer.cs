namespace BarrelVibrations.ModelingObjects.MaterialFolder
{
    partial class MaterialTableForm
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
            this.formsPlot = new ScottPlot.FormsPlot();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.TemperatureColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DensityColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HeatCapacityColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HeatConductivityColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LinearThermalExpansionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PoissonRatioColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YoungModulusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // formsPlot
            // 
            this.formsPlot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlot.Location = new System.Drawing.Point(885, 0);
            this.formsPlot.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.formsPlot.Name = "formsPlot";
            this.formsPlot.Size = new System.Drawing.Size(736, 657);
            this.formsPlot.TabIndex = 3;
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TemperatureColumn,
            this.DensityColumn,
            this.HeatCapacityColumn,
            this.HeatConductivityColumn,
            this.LinearThermalExpansionColumn,
            this.PoissonRatioColumn,
            this.YoungModulusColumn});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowTemplate.Height = 25;
            this.dataGridView.Size = new System.Drawing.Size(885, 657);
            this.dataGridView.TabIndex = 2;
            this.dataGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_ColumnHeaderMouseClick);
            this.dataGridView.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_ColumnHeaderMouseDoubleClick);
            this.dataGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridView_DefaultValuesNeeded);
            this.dataGridView.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dataGridView_PreviewKeyDown);
            // 
            // TemperatureColumn
            // 
            this.TemperatureColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TemperatureColumn.HeaderText = "Температура, К";
            this.TemperatureColumn.MinimumWidth = 75;
            this.TemperatureColumn.Name = "TemperatureColumn";
            this.TemperatureColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DensityColumn
            // 
            this.DensityColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DensityColumn.HeaderText = "Плотность, кг/м^3";
            this.DensityColumn.Name = "DensityColumn";
            this.DensityColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.DensityColumn.Width = 124;
            // 
            // HeatCapacityColumn
            // 
            this.HeatCapacityColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.HeatCapacityColumn.HeaderText = "Теплоемкость, Дж/(Кг·К)";
            this.HeatCapacityColumn.Name = "HeatCapacityColumn";
            this.HeatCapacityColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.HeatCapacityColumn.Width = 153;
            // 
            // HeatConductivityColumn
            // 
            this.HeatConductivityColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.HeatConductivityColumn.HeaderText = "Теплопроводность, Вт/(м·К)";
            this.HeatConductivityColumn.Name = "HeatConductivityColumn";
            this.HeatConductivityColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.HeatConductivityColumn.Width = 169;
            // 
            // LinearThermalExpansionColumn
            // 
            this.LinearThermalExpansionColumn.HeaderText = "Коэффициент линейного теплового расширения, 1/К";
            this.LinearThermalExpansionColumn.Name = "LinearThermalExpansionColumn";
            // 
            // PoissonRatioColumn
            // 
            this.PoissonRatioColumn.HeaderText = "Коэффициент Пуассона";
            this.PoissonRatioColumn.Name = "PoissonRatioColumn";
            // 
            // YoungModulusColumn
            // 
            this.YoungModulusColumn.HeaderText = "Модуль Юнга, ГПа";
            this.YoungModulusColumn.Name = "YoungModulusColumn";
            // 
            // MaterialTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1621, 657);
            this.Controls.Add(this.formsPlot);
            this.Controls.Add(this.dataGridView);
            this.Name = "MaterialTableForm";
            this.Text = "Материал";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ScottPlot.FormsPlot formsPlot;
        private DataGridView dataGridView;
        private DataGridViewTextBoxColumn TemperatureColumn;
        private DataGridViewTextBoxColumn DensityColumn;
        private DataGridViewTextBoxColumn HeatCapacityColumn;
        private DataGridViewTextBoxColumn HeatConductivityColumn;
        private DataGridViewTextBoxColumn LinearThermalExpansionColumn;
        private DataGridViewTextBoxColumn PoissonRatioColumn;
        private DataGridViewTextBoxColumn YoungModulusColumn;
    }
}