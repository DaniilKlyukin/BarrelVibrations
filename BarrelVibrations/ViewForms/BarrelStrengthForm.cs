using BarrelVibrations.ModelingObjects.BarrelFolder;
using BarrelVibrations.Solvers;
using BasicLibraryWinForm;

namespace BarrelVibrations.ViewForms
{
    public partial class BarrelStrengthForm : Form
    {
        /// <summary>
        /// Предел пропорциональности материала, МПа
        /// </summary>
        private const double sigma_e = 784e6;

        private readonly Barrel barrel;
        private readonly double[] maxPressures;
        private readonly double[] snPressures;
        private readonly double[] snX;

        public BarrelStrengthForm()
        {
            InitializeComponent();
        }

        public BarrelStrengthForm(
            Barrel barrel,
            double[] maxPressures,
            double[] snPressures,
            double[] snX) : this()
        {
            this.barrel = barrel;
            this.maxPressures = maxPressures;
            this.snPressures = snPressures;
            this.snX = snX;

            comboBox.SelectedIndex = 0;
            DrawThickness(comboBox.SelectedIndex);
        }

        private void StrengthPcntNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            DrawThickness(comboBox.SelectedIndex);
        }

        private void DrawThickness(int methodId)
        {
            ThicknessFormsPlot.Plot.Clear();

            ThicknessFormsPlot.Plot.AddScatter(
                barrel.X, barrel.RadiiDifference.Mult(1e3).ToArray(),
                Color.Black, 2, markerShape: ScottPlot.MarkerShape.none, label: "Расчетная толщина");

            double[] strengthThickness;
            switch (methodId)
            {
                default:
                case 0:
                    {
                        var n = 1 + (double)StrengthPcntNumericUpDown.Value / 100;

                        strengthThickness = StrengthCalculator.MinThicknesses(
                            barrel.InnerD, maxPressures, Algebra.Ones(barrel.X.Length, n), sigma_e);
                    }
                    break;
                case 1:
                    {
                        strengthThickness = StrengthCalculator.MinThicknesses(
                            barrel.X, barrel.InnerD, maxPressures, snPressures, snX, barrel.CamoraLength, sigma_e);
                    }
                    break;
            }

            ThicknessFormsPlot.Plot.AddScatter(
                barrel.X, strengthThickness.Mult(1e3).ToArray(),
                Color.Red, 2, markerShape: ScottPlot.MarkerShape.none, label: "Допустимая толщина");

            ThicknessFormsPlot.Plot.Title("Толщина");

            ThicknessFormsPlot.Plot.XLabel("x, м");
            ThicknessFormsPlot.Plot.YLabel("h, мм");

            ThicknessFormsPlot.Plot.Legend(location: ScottPlot.Alignment.UpperRight);

            ThicknessFormsPlot.Refresh();
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox.SelectedIndex == 0)
            {
                StrengthPcntNumericUpDown.Enabled = true;
            }
            else
            {
                StrengthPcntNumericUpDown.Enabled = false;
            }
            DrawThickness(comboBox.SelectedIndex);
        }
    }
}
