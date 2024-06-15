using BarrelVibrations.Solvers;
using BasicLibraryWinForm;
using BasicLibraryWinForm.PointFolder;
using GeneticSharp;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Linq;
using Point = BasicLibraryWinForm.PointFolder.Point;

namespace BarrelVibrations.Optimization.TargetCalculators
{
    [Serializable]
    [DataContract(Name = "Метод расчета целевой функции")]
    public class ShotsWeightedOptimizationTarget : OptimizationTargetCalculator
    {
        public override OptimizationTarget OptimizationTarget => OptimizationTarget.ShotsWeightedCriterion;

        /// <summary>
        ///     Вес разброса колебаний дульного среза
        /// </summary>
        [Browsable(true)]
        [Category("Весовые коэффициенты")]
        [Description("")]
        [DisplayName("Вес разброса колебаний дульного среза")]
        [DataMember(Name = "Вес разброса колебаний дульного среза")]
        public double VibrationsSpreadWeight { get; set; } = 0.01;

        /// <summary>
        ///     Вес разброса наклона дульного среза
        /// </summary>
        [Browsable(true)]
        [Category("Весовые коэффициенты")]
        [Description("")]
        [DisplayName("Вес разброса наклона дульного среза")]
        [DataMember(Name = "Вес разброса наклона дульного среза")]
        public double AnglesSpreadWeight { get; set; } = 150;

        /// <summary>
        ///     Вес разброса угла нутации
        /// </summary>
        [Browsable(true)]
        [Category("Весовые коэффициенты")]
        [Description("")]
        [DisplayName("Вес разброса угла нутации")]
        [DataMember(Name = "Вес разброса угла нутации")]
        public double ProjectileAnglesSpreadWeight { get; set; } = 150;

        /// <summary>
        ///     Вес разброса скорости дульного среза
        /// </summary>
        [Browsable(true)]
        [Category("Весовые коэффициенты")]
        [Description("")]
        [DisplayName("Вес разброса скорости дульного среза")]
        [DataMember(Name = "Вес разброса скорости дульного среза")]
        public double BarrelVelocitiesSpreadWeight { get; set; } = 40;

        public override double Calculate(MainSolver mainSolver)
        {
            var pointsBarrelEnd = new List<Point>();
            var pointsAngles = new List<Point>();
            var pointsProjectilesAngles = new List<Point>();

            for (var i = 0; i < mainSolver.ShotsParameters.Count; i++)
            {
                var y = mainSolver.ShotsParameters[i].BarrelEndY;
                var z = mainSolver.ShotsParameters[i].BarrelEndZ;

                var ay = mainSolver.ShotsParameters[i].VerticalAngle;
                var az = mainSolver.ShotsParameters[i].HorizontalAngle;

                var ray = mainSolver.ShotsParameters[i].VerticalRotationAngle;
                var raz = mainSolver.ShotsParameters[i].HorizontalRotationAngle;

                pointsBarrelEnd.Add(new Point(z, y));
                pointsAngles.Add(new Point(az, ay));
                pointsProjectilesAngles.Add(new Point(raz, ray));
            }

            var circleBarrelEnd = SmallestEnclosingCircle.MakeCircle(pointsBarrelEnd);
            var circleAngles = SmallestEnclosingCircle.MakeCircle(pointsAngles);
            var circleProjectilesAngles = SmallestEnclosingCircle.MakeCircle(pointsProjectilesAngles);

            var velocities = mainSolver.ShotsParameters.Select(x => x.BarrelEndVelocity).ToList();
            var velocitySpread = velocities.Max() - velocities.Min();

            return
                VibrationsSpreadWeight * circleBarrelEnd.Radius
                + AnglesSpreadWeight * circleAngles.Radius
                + ProjectileAnglesSpreadWeight * circleProjectilesAngles.Radius
                + BarrelVelocitiesSpreadWeight * velocitySpread;
        }

        public override string GetTargetText()
        {
            return "Взвешенный критерий выстрелов";
        }

        public override string ToString()
        {
            return "Взвешенный критерий выстрелов";
        }
    }
}
