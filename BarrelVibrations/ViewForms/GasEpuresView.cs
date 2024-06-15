using BarrelVibrations.Solvers.Solutions.InletBallistic;
using BarrelVibrations.ViewForms.Common;
using BarrelVibrations.ViewForms.DataTables;
using BasicLibraryWinForm;
using ScottPlot;
using Visualization;

namespace BarrelVibrations.ViewForms
{
    public class GasEpuresView : BasicView
    {
        public GasEpuresView(string resultGroupBoxName, double[] barrelX, GasEpures ballisticEpures, FormsPlot plot, VisualizationProperties visualizationProperties) : base(resultGroupBoxName,plot, visualizationProperties)
        {
            base.Text = "Эпюры газопороховых параметров";

            data.Add(new SingleArrayTable(
                "Давление",
                "x, м",
                barrelX,
                "p, МПа",
                ballisticEpures.Pressures.Mult(1e-6)));

            data.Add(new SingleArrayTable(
                "Температура",
                "x, м",
                barrelX,
                "T, К",
                ballisticEpures.Temperatures));

            data.Add(new SingleArrayTable(
                "Коэффициент теплопередачи",
                "x, м",
                barrelX,
                "α, Вт/(м²K)",
                ballisticEpures.HeatTransfers));

            data.Add(new SingleArrayTable(
                "Плотность",
                "x, м",
                barrelX,
                "ρ, кг/м³",
                ballisticEpures.Densities));

            data.Add(new SingleArrayTable(
                "Скорость газа",
                "x, м",
                barrelX,
                "U, м/с",
                ballisticEpures.GasVelocities));

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
