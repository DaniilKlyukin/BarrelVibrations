using BarrelVibrations.ModelingObjects.EnvironmentFolder.TerrainFolder;
using BasicLibraryWinForm;
using BasicLibraryWinForm.PropertiesTemplates.TypeEditors;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;

namespace BarrelVibrations.ModelingObjects.EnvironmentFolder
{

    public partial class WindForm : Form, IEditable<Wind>
    {
        Wind wind = new Wind();

        private int selectedYIndex = 0;

        public WindForm()
        {
            InitializeComponent();
            comboBox.SelectedIndex = 0;
        }

        public Wind GetValues()
        {
            wind.Update();

            return wind;
        }

        public void SetValues(Wind values)
        {
            xsNumeric.Value = values.WindVelocityMap.GetLength(0);
            dataGridView.RowCount = values.WindVelocityMap.GetLength(0) + 1;

            ysNumeric.Value = values.WindVelocityMap.GetLength(1);
            ysDataGridView.RowCount = values.WindVelocityMap.GetLength(1);

            zsNumeric.Value = values.WindVelocityMap.GetLength(2);
            dataGridView.ColumnCount = values.WindVelocityMap.GetLength(2) + 1;

            wind = values;

            WriteWind();
        }

        private void VisualizeButton_Click(object sender, EventArgs e)
        {
            if (!IsValidData(out var message))
            {
                MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var wf = new WindVisualizationForm(GetValues());

            wf.ShowDialog();
        }

        private void UpdateSize()
        {
            var xn = (int)xsNumeric.Value;
            var yn = (int)ysNumeric.Value;
            var zn = (int)zsNumeric.Value;

            dataGridView.ColumnCount = xn + 1;
            ysDataGridView.RowCount = yn;
            dataGridView.RowCount = zn + 1;

            wind = new Wind(
                wind.Xs.Length >= xn ? wind.Xs.Take(xn).ToArray() : wind.Xs.Concat(Enumerable.Repeat(0.0, xn - wind.Xs.Length)).ToArray(),
                wind.Ys.Length >= yn ? wind.Ys.Take(yn).ToArray() : wind.Ys.Concat(Enumerable.Repeat(0.0, yn - wind.Ys.Length)).ToArray(),
                wind.Zs.Length >= zn ? wind.Zs.Take(zn).ToArray() : wind.Zs.Concat(Enumerable.Repeat(0.0, zn - wind.Zs.Length)).ToArray(),
                ResizeArray(wind.WindVelocityMap, xn, yn, zn),
                ResizeArray(wind.WindAngleMap, xn, yn, zn),
                ResizeArray(wind.RisingWindVelocityMap, xn, yn, zn));

            wind.Update();
        }

        private void xsNumeric_ValueChanged(object sender, EventArgs e)
        {
            UpdateSize();
        }

        private void ysNumeric_ValueChanged(object sender, EventArgs e)
        {
            UpdateSize();
        }

        private void zsNumeric_ValueChanged(object sender, EventArgs e)
        {
            UpdateSize();
        }

        private double[,,] ResizeArray(double[,,] values, int length1, int length2, int length3)
        {
            var result = new double[length1, length2, length3];

            for (int i = 0; i < Math.Min(result.GetLength(0), values.GetLength(0)); i++)
            {
                for (int j = 0; j < Math.Min(result.GetLength(1), values.GetLength(1)); j++)
                {
                    for (int k = 0; k < Math.Min(result.GetLength(2), values.GetLength(2)); k++)
                    {
                        result[i, j, k] = values[i, j, k];
                    }
                }
            }

            return result;
        }

        private void SaveDataGrids()
        {
            for (var x = 0; x < wind.Xs.Length; x++)
            {
                var value = 0.0;

                if (dataGridView.Rows[0].Cells[x + 1].Value != null)
                    value = TextWorker.Parse(dataGridView.Rows[0].Cells[x + 1].Value.ToString());

                wind.Xs[x] = value;
            }

            for (var y = 0; y < wind.Ys.Length; y++)
            {
                var value = 0.0;

                if (ysDataGridView.Rows[y].Cells[0].Value != null)
                    value = TextWorker.Parse(ysDataGridView.Rows[y].Cells[0].Value.ToString());

                wind.Ys[y] = value;
            }

            for (var z = 0; z < wind.Zs.Length; z++)
            {
                var value = 0.0;

                if (dataGridView.Rows[z + 1].Cells[0].Value != null)
                    value = TextWorker.Parse(dataGridView.Rows[z + 1].Cells[0].Value.ToString());

                wind.Zs[z] = value;
            }

            for (var x = 0; x < wind.Xs.Length; x++)
            {
                for (var z = 0; z < wind.Zs.Length; z++)
                {
                    var value = 0.0;

                    if (dataGridView.Rows[z + 1].Cells[x + 1].Value != null)
                        value = TextWorker.Parse(dataGridView.Rows[z + 1].Cells[x + 1].Value.ToString());

                    switch (comboBox.SelectedIndex)
                    {
                        case 0: wind.WindVelocityMap[x, selectedYIndex, z] = value; break;
                        case 1: wind.WindAngleMap[x, selectedYIndex, z] = Algebra.ConvertGradToRad(value); break;
                        case 2: wind.RisingWindVelocityMap[x, selectedYIndex, z] = value; break;
                    }
                }
            }
        }

        private void WriteWind()
        {
            if (wind.Xs.Length != dataGridView.ColumnCount - 1
                || wind.Zs.Length != dataGridView.RowCount - 1
                || wind.Ys.Length != ysDataGridView.RowCount)
                return;

            for (var x = 0; x < wind.Xs.Length; x++)
            {
                dataGridView.Rows[0].Cells[x + 1].Value = wind.Xs[x];
            }

            for (var y = 0; y < wind.Ys.Length; y++)
            {
                ysDataGridView.Rows[y].Cells[0].Value = wind.Ys[y];
            }

            for (var z = 0; z < wind.Zs.Length; z++)
            {
                dataGridView.Rows[z + 1].Cells[0].Value = wind.Zs[z];
            }

            for (var x = 0; x < wind.Xs.Length; x++)
            {
                for (var z = 0; z < wind.Zs.Length; z++)
                {
                    switch (comboBox.SelectedIndex)
                    {
                        case 0: dataGridView.Rows[z + 1].Cells[x + 1].Value = wind.WindVelocityMap[x, selectedYIndex, z]; break;
                        case 1: dataGridView.Rows[z + 1].Cells[x + 1].Value = Algebra.ConvertRadToGrad(wind.WindAngleMap[x, selectedYIndex, z]); break;
                        case 2: dataGridView.Rows[z + 1].Cells[x + 1].Value = wind.RisingWindVelocityMap[x, selectedYIndex, z]; break;
                    }
                }
            }
        }

        private bool IsValidData(out string message)
        {
            var xs = new double[dataGridView.ColumnCount - 1];
            var ys = new double[ysDataGridView.RowCount];
            var zs = new double[dataGridView.RowCount - 1];

            for (var y = 0; y < ysDataGridView.RowCount; y++)
            {
                if (ysDataGridView.Rows[y].Cells[0].Value == null)
                {
                    message = "Недопустимое значение Y в таблице высот";
                    return false;
                }

                ys[y] = TextWorker.Parse(ysDataGridView.Rows[y].Cells[0].Value.ToString());
            }

            for (var y = 0; y < ys.Length - 1; y++)
            {
                if (ys[y] >= ys[y + 1])
                {
                    message = "Значения Y в таблице высот должны возрастать сверху вниз";
                    return false;
                }
            }

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
                    message = "Значения X в таблице должны возрастать слева направо";
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
                    message = "Значения Z в таблице должны возрастать сверху вниз";
                    return false;
                }
            }

            message = "";
            return true;
        }

        private void ysDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedYIndex = e.RowIndex;

            WriteWind();
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            WriteWind();
        }

        private void ysDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            SaveDataGrids();
            wind.Update();
        }

        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            SaveDataGrids();
            wind.Update();
        }

        private void WindForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsValidData(out var message))
            {
                MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel= true;
                return;
            }
        }
    }
}
