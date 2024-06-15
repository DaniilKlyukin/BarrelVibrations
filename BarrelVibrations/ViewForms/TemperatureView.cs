using BarrelVibrations.Solvers.Solutions;
using BarrelVibrations.ViewForms.Common;
using BarrelVibrations.ViewForms.DataTables;
using BasicLibraryWinForm;
using ScottPlot;
using Visualization;

namespace BarrelVibrations.ViewForms
{
    public class TemperatureView : BasicView
    {
        public TemperatureView(string resultGroupBoxName, double[] timeMoments, TemperatureField temperatureField, FormsPlot plot, VisualizationProperties visualizationProperties)
            : base(resultGroupBoxName, plot, visualizationProperties)
        {
            base.Text = "Тепловое поле";

            data.Add(new SingleArrayTable(
                "Продольная эпюра температуры внутренней поверхности",
                "x, м",
                temperatureField.LongitudinalEpureXs,
                "T, К",
                temperatureField.LongitudinalEpureInnerSurface));

            data.Add(new SingleArrayTable(
                "Продольная эпюра температуры внешней поверхности",
                "x, м",
                temperatureField.LongitudinalEpureXs,
                "T, К",
                temperatureField.LongitudinalEpureOuterSurface));

            data.Add(new SingleArrayTable(
                "Поперечная эпюра температур",
                "r, мм",
                temperatureField.RadialEpureRadiuses.Mult(1e3),
                "T, К",
                temperatureField.RadialEpure));

            data.Add(new SingleArrayTable(
                "Динамика максимальной температуры",
                "t, мс",
                timeMoments.Mult(1e3),
                "Tmax, К",
                temperatureField.MaxTemperate.ToArray()));

            data.Add(new SingleArrayTable(
                "Динамика температуры в точке",
                "t, мс",
                timeMoments.Mult(1e3),
                "T, К",
                temperatureField.PointTemperate.ToArray()));

            data.Add(new SingleArrayTable(
                "Средняя температура внутренней поверхности",
                "t, мс",
                timeMoments.Mult(1e3),
                "T, К",
                temperatureField.AverageTemperatureInnerSurface.ToArray()));

            data.Add(new SingleArrayTable(
                "Средняя температура внешней поверхности",
                "t, мс",
                timeMoments.Mult(1e3),
                "T, К",
                temperatureField.AverageTemperatureOuterSurface.ToArray()));

            listBox.Items.Clear();

            foreach (var table in data)
            {
                listBox.Items.Add(table.Name);
            }

            groupBox.Text = @$"{resultGroupBoxName}";
            listBox.SelectedIndex = 0;
        }
    }
}
