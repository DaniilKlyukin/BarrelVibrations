using BarrelVibrations.ViewForms.Common;
using BarrelVibrations.ViewForms.DataTables;
using ScottPlot;
using Visualization;

namespace BarrelVibrations.ViewForms
{
    public class VibrationsDistributionsView : TrackBarFileView
    {
        public VibrationsDistributionsView(
            string resultName,
            double[] timeMoments,
            double[] barrelX,
            FormsPlot plot, VisualizationProperties visualizationProperties)
            : base(resultName, timeMoments, barrelX, plot, visualizationProperties)
        {
            base.Text = "Колебания ствола";

            var fileDatas = new[]
            {
                new FileData(
                    Path.Combine(Resource.CalculationFilesFolder, Resource.VibrationsRInner),
                    "Радиальные перемещения", "x, м", "Δr, мм", dataMultiplier: 1e3),
                new FileData(
                    Path.Combine(Resource.CalculationFilesFolder, Resource.VibrationsXFile),
                    "Перемещения по Ox", "x, м", "u, мм", dataMultiplier: 1e3),
                new FileData(
                    Path.Combine(Resource.CalculationFilesFolder, Resource.VibrationsYFile),
                    "Перемещения по Oy", "x, м", "v, мм", dataMultiplier: 1e3),
                new FileData(
                    Path.Combine(Resource.CalculationFilesFolder, Resource.VibrationsZFile),
                    "Перемещения по Oz", "x, м", "w, мм", dataMultiplier: 1e3)
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
