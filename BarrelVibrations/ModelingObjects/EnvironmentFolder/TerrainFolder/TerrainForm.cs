using BarrelVibrations.ModelingObjects.EnvironmentFolder.TerrainFolder;
using BasicLibraryWinForm;
using BasicLibraryWinForm.PropertiesTemplates.TypeEditors;
using ScottPlot.Renderable;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace BarrelVibrations.ModelingObjects.EnvironmentFolder
{
    public partial class TerrainForm : Form, IEditable<Terrain>
    {
        public TerrainForm()
        {
            InitializeComponent();
        }

        public Terrain GetValues()
        {
            var altitudes = new double[dataGridView.ColumnCount - 1, dataGridView.RowCount - 1];
            var xs = new double[dataGridView.ColumnCount - 1];
            var zs = new double[dataGridView.RowCount - 1];

            for (var j = 0; j < altitudes.GetLength(1); j++)
            {
                zs[j] = TextWorker.Parse(dataGridView.Rows[j + 1].Cells[0].Value.ToString());
            }

            for (var i = 0; i < altitudes.GetLength(0); i++)
            {
                xs[i] = TextWorker.Parse(dataGridView.Rows[0].Cells[i + 1].Value.ToString());
            }

            for (var i = 0; i < altitudes.GetLength(0); i++)
            {
                for (var j = 0; j < altitudes.GetLength(1); j++)
                {
                    if (dataGridView.Rows[j + 1].Cells[i + 1].Value != null)
                        altitudes[i, j] = TextWorker.Parse(dataGridView.Rows[j + 1].Cells[i + 1].Value.ToString());
                }
            }

            return new Terrain(
                xs,
                zs,
                altitudes);
        }

        public void SetValues(Terrain values)
        {
            rowsNumeric.Value = values.AltitudeMap.GetLength(1);
            dataGridView.RowCount = values.AltitudeMap.GetLength(1) + 1;

            columnsNumeric.Value = values.AltitudeMap.GetLength(0);
            dataGridView.ColumnCount = values.AltitudeMap.GetLength(0) + 1;

            for (var j = 0; j < values.AltitudeMap.GetLength(1); j++)
            {
                dataGridView.Rows[j + 1].Cells[0].Value = values.Zs[j].ToString();
            }

            for (var i = 0; i < values.AltitudeMap.GetLength(0); i++)
            {
                dataGridView.Rows[0].Cells[i + 1].Value = values.Xs[i].ToString();
            }

            for (var i = 0; i < values.AltitudeMap.GetLength(0); i++)
            {
                for (var j = 0; j < values.AltitudeMap.GetLength(1); j++)
                {
                    dataGridView.Rows[j + 1].Cells[i + 1].Value = values.AltitudeMap[i, j].ToString();
                }
            }
        }

        private void rowsNumeric_ValueChanged(object sender, EventArgs e)
        {
            dataGridView.RowCount = (int)rowsNumeric.Value + 1;
        }

        private void columnsNumeric_ValueChanged(object sender, EventArgs e)
        {
            dataGridView.ColumnCount = (int)columnsNumeric.Value + 1;
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            var str = File.ReadAllLines(ofd.FileName);

            var altitudes = str
                .Select(s => s.Split("\t")
                    .Select(w => TextWorker.Parse(w))
                    .ToArray())
                .ToArray()
                .Convert();

            rowsNumeric.Value = dataGridView.RowCount = altitudes.GetLength(0);
            columnsNumeric.Value = dataGridView.ColumnCount = altitudes.GetLength(1);

            for (var i = 0; i < dataGridView.RowCount; i++)
            {
                for (var j = 0; j < dataGridView.ColumnCount; j++)
                {
                    dataGridView.Rows[i].Cells[j].Value = altitudes[i, j].ToString();
                }
            }
        }

        private void dataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            for (var i = 0; i < dataGridView.ColumnCount; i++)
            {
                e.Row.Cells[i].Value = 0;
            }
        }

        private void VisualizeButton_Click(object sender, EventArgs e)
        {
            var tf = new TerrainVisualizationForm(GetValues());

            tf.ShowDialog();
        }

        private void TerrainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsValidData(out var message))
            {
                e.Cancel = true;
                MessageBox.Show(message, "Неверные данные", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsValidData(out string message)
        {
            var xs = new double[dataGridView.ColumnCount - 1];
            var zs = new double[dataGridView.RowCount - 1];

            for (var x = 0; x < dataGridView.ColumnCount - 1; x++)
            {
                if (dataGridView.Rows[0].Cells[x + 1].Value == null)
                {
                    message = "Недопустимое значение X в таблице";
                    return false;
                }

                xs[x] = TextWorker.Parse(dataGridView.Rows[0].Cells[x + 1].Value.ToString());
            }

            for (var x = 0; x < xs.Length - 1; x++)
            {
                if (xs[x] >= xs[x + 1])
                {
                    message = "Значения в таблице должны возрастать слева направо";
                    return false;
                }
            }

            for (var z = 0; z < dataGridView.RowCount - 1; z++)
            {
                if (dataGridView.Rows[z + 1].Cells[0].Value == null)
                {
                    message = "Недопустимое значение Z в таблице";
                    return false;
                }

                zs[z] = TextWorker.Parse(dataGridView.Rows[z + 1].Cells[0].Value.ToString());
            }

            for (var z = 0; z < zs.Length - 1; z++)
            {
                if (zs[z] >= zs[z + 1])
                {
                    message = "Значения в таблице должны возрастать сверху вниз";
                    return false;
                }
            }

            message = "";
            return true;
        }
    }
}
