using BarrelVibrations.ModelingObjects.MaterialFolder;
using BasicLibraryWinForm;
using BasicLibraryWinForm.PropertiesTemplates.TypeEditors;
using System.Data;

namespace BarrelVibrations.Solvers.OutletBallisticFolder
{
    public partial class MeteoTableForm : Form, IEditable<MeteoTable>
    {
        public MeteoTableForm()
        {
            InitializeComponent();
        }

        public MeteoTable GetValues()
        {
            var meteoTable = new MeteoTable();

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

                var altitude = TextWorker.Parse(cells[0].Value.ToString());

                if (i > 0 && meteoTable.MeteoDatas[i - 1].Altitude <= altitude)
                    continue;

                meteoTable.MeteoDatas.Add(
                    new MeteoData(
                        TextWorker.Parse(cells[0].Value.ToString()),
                        TextWorker.Parse(cells[1].Value.ToString()),
                        TextWorker.Parse(cells[2].Value.ToString()),
                        TextWorker.Parse(cells[3].Value.ToString()),
                        TextWorker.Parse(cells[4].Value.ToString())));
            }

            meteoTable.UpdateFunctions();
            return meteoTable;
        }

        public void SetValues(MeteoTable meteoTable)
        {
            dataGridView.Rows.Clear();

            foreach (var meteoData in meteoTable.MeteoDatas)
            {
                dataGridView.Rows.Add(
                    meteoData.Altitude,
                    meteoData.Pressure,
                    meteoData.Temperature,
                    meteoData.Density,
                    meteoData.SoundSpeed);
            }
        }

        private void dataGridView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                var data = Clipboard.GetText().Split('\n').Select(row => row.Split('\t').ToArray()).SkipLast(1)
                    .ToArray();

                dataGridView.Rows.Clear();

                foreach (var row in data)
                {
                    dataGridView.Rows.Add(row.Cast<object>().ToArray());
                }
            }
        }

        private void dataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView.RowCount <= 1)
                return;

            GetSelectedColumnPlotData(e.ColumnIndex, out var xs, out var ys);

            var func = Algebra.GetFunc(xs, ys, false);

            var altitudes = Algebra.Linspace(-2e3, 1e5, 1000);
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

        private void Plot(double[] xs, double[] ys, string xLabel, string yLabel, ScottPlot.MarkerShape markerShape)
        {
            formsPlot.Plot.XLabel(xLabel);
            formsPlot.Plot.YLabel(yLabel);

            formsPlot.Plot.Clear();

            formsPlot.Plot.AddScatter(xs, ys, markerShape: markerShape);

            formsPlot.Render();
        }

        private void dataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["AltitudeColumn"].Value = 0;
            e.Row.Cells["PressureColumn"].Value = 101325;
            e.Row.Cells["TemperatureColumn"].Value = 300;
            e.Row.Cells["DensityColumn"].Value = 1.2255;
            e.Row.Cells["SoundSpeedColumn"].Value = 340.3;
        }
    }
}