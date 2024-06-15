using BarrelVibrations.Solvers.Solutions;
using BarrelVibrations.ViewForms.Common;
using BarrelVibrations.ViewForms.DataTables;
using BasicLibraryWinForm;
using ScottPlot;
using Visualization;

namespace BarrelVibrations.ViewForms
{
    public class DeflectionView : BasicView
    {

        public DeflectionView(string resultGroupBoxName, double[] barrelX, Deflection deflection, FormsPlot plot, VisualizationProperties visualizationProperties) : base(resultGroupBoxName, plot, visualizationProperties)
        {
            base.Text = "Начальный прогиб";

            data.Add(new SingleArrayTable(
                "Продольный прогиб",
                "x, м",
                barrelX,
                "u, мм",
                deflection.DeflectionX.Mult(1e3)));

            data.Add(new SingleArrayTable(
                "Вертикальный прогиб",
                "x, м",
                barrelX,
                "v, мм",
                deflection.DeflectionY.Mult(1e3)));

            data.Add(new SingleArrayTable(
                "Горизонтальный прогиб",
                "x, м",
                barrelX,
                "w, мм",
                deflection.DeflectionZ.Mult(1e3)));

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
