using BarrelVibrations.ModelingObjects.BarrelFolder;
using BarrelVibrations.ViewForms.Common;
using BarrelVibrations.ViewForms.DataTables;
using BasicLibraryWinForm;
using ScottPlot;
using Visualization;

namespace BarrelVibrations.ViewForms
{
    public class BarrelGeometryView : BasicView
    {
        public BarrelGeometryView(string barrelGroupBoxName, double density, Barrel barrelGeometry, FormsPlot plot, VisualizationProperties visualizationProperties)
            : base(barrelGroupBoxName, plot, visualizationProperties)
        {
            base.Text = "Геометрия ствола";

            data.Add(new SingleArrayTable(
                "Внутренний диаметр",
                "x, м",
                barrelGeometry.X,
                "d, мм",
                barrelGeometry.InnerD.Mult(1e3)));

            data.Add(new SingleArrayTable(
                "Внешний диаметр",
                "x, м",
                barrelGeometry.X,
                "D, мм",
                barrelGeometry.OuterD.Mult(1e3)));

            if (barrelGeometry.StiffenersType != StiffenersType.None)
            {
                data.Add(new SingleArrayTable(
                    "Диаметр ребер жесткости",
                    "x, м",
                    barrelGeometry.X,
                    "D_жест, мм",
                    barrelGeometry.StiffenersDiameters.Mult(1e3)));

                data.Add(new SingleArrayTable(
                    "Расстояние до ребер жесткости",
                    "x, м",
                    barrelGeometry.X,
                    "d_жест, мм",
                    barrelGeometry.StiffenersDistances.Mult(1e3)));
            }

            data.Add(new SingleArrayTable(
                "Неровности по Oy",
                "x, м",
                barrelGeometry.X,
                "v00, мм",
                barrelGeometry.Wy.Mult(1e3)));

            data.Add(new SingleArrayTable(
                "Неровности по Oz",
                "x, м",
                barrelGeometry.X,
                "w00, мм",
                barrelGeometry.Wz.Mult(1e3)));

            data.Add(new SingleArrayTable(
                "Разность внешнего и внутреннего радиусов",
                "x, м",
                barrelGeometry.X,
                "h, мм",
                barrelGeometry.RadiiDifference.Mult(1e3)));

            data.Add(new SingleArrayTable(
                "Площадь канала",
                "x, м",
                barrelGeometry.X,
                "S, м²",
                barrelGeometry.S));

            data.Add(new SingleArrayTable(
                "Площадь сечения",
                "x, м",
                barrelGeometry.X,
                "F, м²",
                barrelGeometry.F));

            data.Add(new SingleArrayTable(
                "Момент инерции Jy сечения",
                "x, м",
                barrelGeometry.X,
                "Jy, м⁴",
                barrelGeometry.Jy));

            data.Add(new SingleArrayTable(
                "Момент инерции Jz сечения",
                "x, м",
                barrelGeometry.X,
                "Jz, м⁴",
                barrelGeometry.Jz));

            data.Add(new SingleArrayTable(
                "Продольная сила в сечениях",
                "x, м",
                barrelGeometry.X,
                "Fx, Н",
                barrelGeometry.LongitudinalUnitForce));

            data.Add(new SingleArrayTable(
                "Момент силы по Oy в сечениях",
                "x, м",
                barrelGeometry.X,
                "My, Н⋅м",
                barrelGeometry.UnitTorqueY));

            data.Add(new SingleArrayTable(
                "Момент силы по Oz в сечениях",
                "x, м",
                barrelGeometry.X,
                "Mz, Н⋅м",
                barrelGeometry.UnitTorqueZ));

            listBox.Items.Clear();

            foreach (var table in data)
            {
                listBox.Items.Add(table.Name);
            }

            groupBox.Text = @$"{barrelGroupBoxName}, m={barrelGeometry.GetMass(density):0.000} кг";
            listBox.SelectedIndex = 0;
        }
    }
}
