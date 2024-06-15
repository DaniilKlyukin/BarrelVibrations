using BarrelVibrations.ModelingObjects.EnvironmentFolder.TerrainFolder;
using BasicLibraryWinForm;
using CustomControls;
using ScottPlot.Plottable;
using System.Data;
using System.Windows.Forms;

namespace BarrelVibrations.ModelingObjects.EnvironmentFolder
{
    public partial class WindVisualizationForm : Form
    {
        private readonly Wind wind = new();

        public WindVisualizationForm()
        {
            InitializeComponent();
        }

        public WindVisualizationForm(Wind wind)
        {
            InitializeComponent();
            this.wind = wind;
            yNumericUpDown.Minimum = (decimal)wind.Ys.Min();
            yNumericUpDown.Maximum = (decimal)wind.Ys.Max();
            yNumericUpDown.Increment = (yNumericUpDown.Maximum - yNumericUpDown.Minimum) / 100;
            parameterComboBox.SelectedIndex = 0;
        }

        private void TerrainVisualizationForm_Load(object sender, EventArgs e)
        {
            Draw();
        }

        private void Draw()
        {
            var y = (double)yNumericUpDown.Value;
            heatBar.MeasureUnits = parameterComboBox.SelectedItem.ToString();

            switch (parameterComboBox.SelectedIndex)
            {
                case 0:
                    {
                        var min = wind.WindVelocityMap.Min();
                        var max = wind.WindVelocityMap.Max();

                        Draw(wind.GetWindVelocity, y, min, max);
                        heatBar.SetLimits(min, max);
                    }
                    break;
                case 1:
                    {
                        var min = Algebra.ConvertRadToGrad(wind.WindAngleMap.Min());
                        var max = Algebra.ConvertRadToGrad(wind.WindAngleMap.Max());

                        Draw((x, y, z) => Algebra.ConvertRadToGrad(wind.GetWindAngle(x, y, z)), y, min, max);
                        heatBar.SetLimits(min, max);
                    }
                    break;
                case 2:
                    {
                        var min = wind.RisingWindVelocityMap.Min();
                        var max = wind.RisingWindVelocityMap.Max();

                        Draw(wind.GetRisingWindVelocity, y, min, max);
                        heatBar.SetLimits(min, max);
                    }
                    break;
            }
        }

        private void Draw(Func<double, double, double, double> interpolation, double y, double min, double max)
        {
            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);

            var bmp = (Bitmap)pictureBox.Image;

            for (var x = 0; x < bmp.Width; x++)
            {
                for (var z = 0; z < bmp.Height; z++)
                {
                    var xTerrain = wind.Xs[0] + x * (wind.Xs[^1] - wind.Xs[0]) / (bmp.Width - 1);
                    var zTerrain = wind.Zs[0] + z * (wind.Zs[^1] - wind.Zs[0]) / (bmp.Height - 1);

                    var value = interpolation(xTerrain, y, zTerrain);

                    bmp.SetPixel(x, z, Algebra.GetHeatColor(value, min, max));
                }
            }
        }

        private void TerrainVisualizationForm_ResizeEnd(object sender, EventArgs e)
        {
            Draw();
        }

        private void parameterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Draw();
        }

        private void yNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Draw();
        }
    }
}
