using BarrelVibrations.ViewForms.Common;
using BarrelVibrations.ViewForms.DataTables;
using ScottPlot;
using Visualization;

namespace BarrelVibrations.ViewForms
{
    public class TemperatureDistributionsView : TrackBarFileView
    {

        public TemperatureDistributionsView(
            string resultName,
            double[] timeMoments,
            double[] barrelX,
            FormsPlot plot,
            VisualizationProperties visualizationProperties)
            : base(resultName, timeMoments, barrelX, plot, visualizationProperties)
        {
            base.Text = "Тепловые распределения";

            var fileDatas = new[]
            {
                new FileData(
                Path.Combine(Resource.CalculationFilesFolder, Resource.TemperatureBarrelInner),
                "Температура внутренней поверхности", "x, м", "T, К")
            };

            SetData(fileDatas);

            listBox.Items.Clear();

            foreach (var data in fileDatas)
            {
                listBox.Items.Add(data.Header);
            }

            timeTrackBar.Maximum = timeMoments.Length - 1;
            groupBox.Text = @$"{resultName}";
            timeLabel.Text = "t: 0 мс";
            listBox.SelectedIndex = 0;
        }
    }
}
