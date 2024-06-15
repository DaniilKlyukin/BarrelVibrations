using BarrelVibrations.Solvers.Solutions.InletBallistic;
using BarrelVibrations.ViewForms.Common;
using BarrelVibrations.ViewForms.DataTables;
using BasicLibraryWinForm;
using ScottPlot;
using Visualization;

namespace BarrelVibrations.ViewForms
{
    public class InletBallisticView : BasicView
    {

        public InletBallisticView(string resultGroupBoxName, InletBallistic ballistic, FormsPlot plot, VisualizationProperties visualizationProperties) : base(resultGroupBoxName, plot, visualizationProperties)
        {
            base.Text = "Внутренняя баллистика";
            groupBox.Text = resultGroupBoxName;

            data.Add(new SingleArrayTable(
                "Координата снаряда",
                "t, мс",
                ballistic.TimeMoments.Mult(1e3),
                 "Xсн, м",
                ballistic.Xsn.ToArray()));

            data.Add(new SingleArrayTable(
                "Скорость снаряда",
                "t, мс",
                ballistic.TimeMoments.Mult(1e3),
                "Vсн, м/с",
                ballistic.Vsn.ToArray()));

            data.Add(new SingleArrayTable(
                "Среднее давление",
                "t, мс",
                ballistic.TimeMoments.Mult(1e3),
                "p, МПа",
                 ballistic.P.Mult(1e-6)));

            data.Add(new SingleArrayTable(
                "Давление на дно снаряда",
                "t, мс",
                ballistic.TimeMoments.Mult(1e3),
                 "p_сн, МПа",
                 ballistic.Psn.Mult(1e-6)));

            data.Add(new SingleArrayTable(
                "Давление на дно канала",
                "t, мс",
                ballistic.TimeMoments.Mult(1e3),
                "p_кн, МПа",
                 ballistic.Pkn.Mult(1e-6)));

            data.Add(new SingleArrayTable(
                "Противодавление",
                "t, мс",
                ballistic.TimeMoments.Mult(1e3),
                "p_пр, МПа",
                 ballistic.Ppr.Mult(1e-6)));

            data.Add(new SingleArrayTable(
                "Плотность газопороховой смеси",
                "t, мс",
                ballistic.TimeMoments.Mult(1e3),
                "ρ, кг/м³",
                 ballistic.Density));

            data.Add(new SingleArrayTable(
                "Средняя температура",
                "t, мс",
                ballistic.TimeMoments.Mult(1e3),
                "T, К",
                ballistic.T.ToArray()));

            data.Add(new SingleArrayTable(
                "Заснарядный объем",
                "t, мс",
                ballistic.TimeMoments.Mult(1e3),
                "Wсн, м³",
                ballistic.W.ToArray()));

            data.Add(new SingleArrayTable(
                "J0",
                "t, мс",
                ballistic.TimeMoments.Mult(1e3),
                "J0",
                ballistic.J0.ToArray()));

            data.Add(new SingleArrayTable(
                "J1",
                "t, мс",
                ballistic.TimeMoments.Mult(1e3),
                "J1",
                ballistic.J1.ToArray()));

            data.Add(new SingleArrayTable(
                "J2", 
                "t, мс",
                ballistic.TimeMoments.Mult(1e3),
                "J2",
                ballistic.J2.ToArray()));

            data.Add(new MultiArrayTable(
                "Доля сгоревшего пороха",
                "t, мс",
                ballistic.TimeMoments.Mult(1e3),
                "ψ",
                ballistic.Psi.ToArray().Convert()));

            data.Add(new MultiArrayTable(
                "Относительная толщина сгоревшего свода", 
                "t, мс",
                ballistic.TimeMoments.Mult(1e3),
                "Z",
                ballistic.Z.ToArray().Convert()));

            data.Add(new MultiArrayTable(
                "Относительная площадь поверхности горения", 
                "t, мс",
                ballistic.TimeMoments.Mult(1e3),
                "Относительная площадь поверхности горения",
                ballistic.Sigma.ToArray().Convert()));

            data.Add(new MultiArrayTable(
                "Скорость горения",
                "t, мс",
                ballistic.TimeMoments.Mult(1e3),
                "U, м³/(Н с)",
                ballistic.U.ToArray().Convert()));

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
