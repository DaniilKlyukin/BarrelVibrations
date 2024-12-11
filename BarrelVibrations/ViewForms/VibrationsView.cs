using BarrelVibrations.Solvers.Solutions;
using BarrelVibrations.ViewForms.Common;
using BarrelVibrations.ViewForms.DataTables;
using BasicLibraryWinForm;
using ScottPlot;
using Visualization;

namespace BarrelVibrations.ViewForms
{
    public class VibrationsView : BasicView
    {
        public VibrationsView(
            string resultGroupBoxName,
            double[] xs,
            double[] timeMoments,
            Vibrations vibrations,
            double[] shotsTimeMoments,
            FormsPlot plot, VisualizationProperties visualizationProperties) : base(resultGroupBoxName, plot, visualizationProperties)
        {
            base.Text = "Колебания ствола";

            var drawShotsMethod = (double t, FormsPlot plot) =>
            {
                if (!visualizationProperties.PlotShots)
                    return;

                for (int i = 0; i < shotsTimeMoments.Length; i++)
                {
                    plot.Plot.AddVerticalLine(shotsTimeMoments[i] * 1000, label: $"выстрел {i + 1}");
                }

                plot.Plot.Legend();
            };

            data.Add(new SingleArrayTable(
                "Перемещения дульного среза по Ox",
                "t, мс",
                timeMoments.Mult(1e3),
                "Ux, мм",
                vibrations.BarrelEndXs.Mult(1e3))
            {
                AdditionalDrawMethod = drawShotsMethod
            });

            var v0 = vibrations.BarrelEndYs.First();

            data.Add(new SingleArrayTable(
                "Перемещения дульного среза по Oy",
                "t, мс",
                timeMoments.Mult(1e3),
                "Uy, мкм",
                vibrations.BarrelEndYs.Select(v => (v - v0) * 1e6))
            {
                AdditionalDrawMethod = drawShotsMethod
            });

            data.Add(new SingleArrayTable(
                "Перемещения дульного среза по Oz",
                 "t, мс",
                timeMoments.Mult(1e3),
                "Uz, мм",
                vibrations.BarrelEndZs.Mult(1e3))
            {
                AdditionalDrawMethod = drawShotsMethod
            });

            data.Add(new SingleArrayTable(
                "Скорость дульного среза по Ox",
                "t, мс",
                timeMoments.Mult(1e3),
                "Vx, м/с",
                vibrations.BarrelEndVelocityXs)
            {
                AdditionalDrawMethod = drawShotsMethod
            });

            data.Add(new SingleArrayTable(
                "Скорость дульного среза по Oy",
                "t, мс",
                timeMoments.Mult(1e3),
                "Vy, м/с",
                vibrations.BarrelEndVelocityYs)
            {
                AdditionalDrawMethod = drawShotsMethod
            });

            data.Add(new SingleArrayTable(
                "Скорость дульного среза по Oz",
                "t, мс",
                timeMoments.Mult(1e3),
                "Vz, м/с",
                vibrations.BarrelEndVelocityZs)
            {
                AdditionalDrawMethod = drawShotsMethod
            });

            data.Add(new SingleArrayTable(
                "Углы наклона дульного среза по горизонтали",
                "t, мс",
                timeMoments.Mult(1e3),
                "Углы наклона дульного среза по горизонтали, град",
                vibrations.BarrelHorizontalAngles)
            {
                AdditionalDrawMethod = drawShotsMethod
            });

            data.Add(new SingleArrayTable(
                "Углы наклона дульного среза по вертикали",
                "t, мс",
                timeMoments.Mult(1e3),
                "Углы наклона дульного среза по вертикали, град",
                vibrations.BarrelVerticalAngles)
            {
                AdditionalDrawMethod = drawShotsMethod
            });

            data.Add(new SingleArrayTable(
                "Положение ствола",
                "t, мс",
                timeMoments.Mult(1e3),
                "Положение ствола, мм",
                vibrations.BarrelXLocations.Mult(1e3)));

            data.Add(new SingleArrayTable(
                "Положение установки",
                "t, мс",
                timeMoments.Mult(1e3),
                "Положение установки, мм",
                vibrations.SystemXLocations.Mult(1e3)));

            data.Add(new SingleArrayTable(
                "Радиальные колебания в точке",
                "t, мс",
                timeMoments.Mult(1e3),
                "Радиальные колебания в точке, мм",
                vibrations.PointRVibrations.Mult(1e3)));

            data.Add(new SingleArrayTable(
                "Эпюра радиальных колебаний",
                "x, м",
                xs,
                "Эпюра радиальных колебаний, мм",
                vibrations.EpureInnerRVibrations.Mult(1e3)));

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
