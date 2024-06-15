using BarrelVibrations.Solvers;
using BasicLibraryWinForm;
using BasicLibraryWinForm.PointFolder;
using GeneticSharp;
using System.Runtime.Serialization;
using System.Xml.Linq;
using Point = BasicLibraryWinForm.PointFolder.Point;

namespace BarrelVibrations.Optimization.TargetCalculators
{
    [Serializable]
    [DataContract(Name = "Метод расчета целевой функции")]
    public class ShotsVibrationsSpreadOptimizationTarget : OptimizationTargetCalculator
    {
        public override OptimizationTarget OptimizationTarget => OptimizationTarget.ShotsVibrationsSpread;

        public override double Calculate(MainSolver mainSolver)
        {
            var points = new List<Point>();

            for (var i = 0; i < mainSolver.ShotsParameters.Count; i++)
            {
                var vy = mainSolver.ShotsParameters[i].BarrelEndY;
                var vz = mainSolver.ShotsParameters[i].BarrelEndZ;

                points.Add(new Point(vz, vy));
            }

            var circle = SmallestEnclosingCircle.MakeCircle(points);

            return circle.Radius;
        }

        public override string GetTargetText()
        {
            return "Разброс колебаний ствола выстрелов, мкм";
        }

        public override string ToString()
        {
            return "Разброс колебаний ствола выстрелов";
        }
    }
}
