using BasicLibraryWinForm;
using BasicLibraryWinForm.PropertiesTemplates.TypeEditors;

namespace BarrelVibrations.ModelingObjects.MaterialFolder
{
    public partial class MaterialTableForm : Form, IEditable<MaterialTable>
    {
        public MaterialTableForm()
        {
            InitializeComponent();
        }

        public MaterialTable GetValues()
        {
            var materialTable = new MaterialTable();

            for (var i = 0; i < dataGridView.RowCount; i++)
            {
                var cells = dataGridView.Rows[i].Cells;

                var isValidRow = true;

                for (int j = 0; j < cells.Count; j++)
                {
                    if (cells[j].Value == null)
                    {
                        isValidRow = false;
                        break;
                    }
                }

                if (cells.Count == 0 || !isValidRow)
                    continue;

                var temperature = TextWorker.Parse(cells[0].Value.ToString());

                if (i > 0 && materialTable.MaterialDatas[i - 1].Temperature <= temperature)
                    continue;

                materialTable.MaterialDatas.Add(
                    new MaterialData(
                        temperature,
                        TextWorker.Parse(cells[1].Value.ToString()),
                        TextWorker.Parse(cells[2].Value.ToString()),
                        TextWorker.Parse(cells[3].Value.ToString()),
                        TextWorker.Parse(cells[4].Value.ToString()),
                        TextWorker.Parse(cells[5].Value.ToString()),
                        1e9 * TextWorker.Parse(cells[6].Value.ToString())));
            }

            materialTable.UpdateFunctions();
            return materialTable;
        }

        public void SetValues(MaterialTable values)
        {
            dataGridView.Rows.Clear();

            foreach (var materialData in values.MaterialDatas)
            {
                dataGridView.Rows.Add(
                    materialData.Temperature,
                    materialData.Density,
                    materialData.HeatCapacity,
                    materialData.HeatConductivity,
                    materialData.LinearThermalExpansion,
                    materialData.PoissonRatio,
                    materialData.YoungModulus * 1e-9);
            }
        }

        private void Plot(double[] xs, double[] ys, string xLabel, string yLabel, ScottPlot.MarkerShape markerShape)
        {
            formsPlot.Plot.XLabel(xLabel);
            formsPlot.Plot.YLabel(yLabel);

            formsPlot.Plot.Clear();

            formsPlot.Plot.AddScatter(xs, ys, markerShape: markerShape);

            formsPlot.Render();
        }

        private void dataGridView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                var data = Clipboard.GetText().Split('\n').Select(row => row.Split('\t').ToArray()).SkipLast(1).ToArray();

                dataGridView.Rows.Clear();

                foreach (var row in data)
                {
                    dataGridView.Rows.Add(row.Cast<object>().ToArray());
                }
            }
        }

        private void GetSelectedColumnPlotData(int columnIndex, out double[] xs, out double[] ys)
        {
            xs = new double[dataGridView.RowCount - 1];
            ys = new double[dataGridView.RowCount - 1];

            for (var i = 0; i < dataGridView.RowCount - 1; i++)
            {
                xs[i] = TextWorker.Parse(dataGridView.Rows[i].Cells[0].Value.ToString());
                ys[i] = TextWorker.Parse(dataGridView.Rows[i].Cells[columnIndex].Value.ToString());
            }
        }

        private void dataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView.RowCount <= 1)
                return;

            GetSelectedColumnPlotData(e.ColumnIndex, out var xs, out var ys);

            var func = Algebra.GetFunc(xs, ys, false);

            var altitudes = Algebra.Linspace(200, 2000, 1000);
            var values = altitudes.Select(func).ToArray();

            Plot(altitudes, values,
                dataGridView.Columns[0].HeaderText,
                dataGridView.Columns[e.ColumnIndex].HeaderText,
                ScottPlot.MarkerShape.none);
        }

        private void dataGridView_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView.RowCount <= 1)
                return;

            GetSelectedColumnPlotData(e.ColumnIndex, out var xs, out var ys);

            Plot(xs, ys,
                dataGridView.Columns[0].HeaderText,
                dataGridView.Columns[e.ColumnIndex].HeaderText,
                ScottPlot.MarkerShape.filledCircle);
        }

        private void dataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {            
            e.Row.Cells["TemperatureColumn"].Value = 300;
            e.Row.Cells["DensityColumn"].Value = 7850;
            e.Row.Cells["HeatCapacityColumn"].Value = 567;
            e.Row.Cells["HeatConductivityColumn"].Value = 32;
            e.Row.Cells["LinearThermalExpansionColumn"].Value = 12.5e-6;
            e.Row.Cells["PoissonRatioColumn"].Value = 0.3;
            e.Row.Cells["YoungModulusColumn"].Value = 200;
        }
    }
}
