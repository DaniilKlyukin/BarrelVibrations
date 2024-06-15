using BarrelVibrations.ViewForms.Common;
using BarrelVibrations.ViewForms.DataTables;
using ScottPlot;
using Visualization;

namespace BarrelVibrations.ViewForms
{
    public class GasDistributionsView : TrackBarFileView
    {
        public GasDistributionsView(
            string groupBoxName, double[] timeMoments, double[] barrelX, FormsPlot plot, VisualizationProperties visualizationProperties)
            : base(groupBoxName, timeMoments, barrelX, plot, visualizationProperties)
        {
            base.Text = "Распределение параметров газа";

            var fileDatas = new[]
            {
                new FileData(
                    Path.Combine(Resource.CalculationFilesFolder, Resource.GasPressuresFile),
                    "Давление", "x, м", "P, МПа", dataMultiplier: 1e-6),
                new FileData(
                    Path.Combine(Resource.CalculationFilesFolder, Resource.GasTemperaturesFile),
                    "Температура", "x, м", "T, K"),
                new FileData(
                    Path.Combine(Resource.CalculationFilesFolder, Resource.GasDensitiesFile),
                    "Плотность", "x, м", "ρ, кг/м³"),
                new FileData(
                    Path.Combine(Resource.CalculationFilesFolder, Resource.GasVelocitiesFile),
                    "Скорость газа", "x, м", "v, м/с"),
                new FileData(
                    Path.Combine(Resource.CalculationFilesFolder, Resource.GasHeatTransfersFile),
                    "Коэффициент теплопередачи", "x, м", "α, КВт/(м²K)", dataMultiplier: 1e-3)
            };

            SetData(fileDatas);

            listBox.Items.Clear();

            foreach (var data in fileDatas)
            {
                listBox.Items.Add(data.Header);
            }

            groupBox.Text = @$"{groupBoxName}";
            timeTrackBar.Maximum = timeMoments.Length - 1;
            timeLabel.Text = "t: 0 мс";
            listBox.SelectedIndex = 0;
        }
    }
}
